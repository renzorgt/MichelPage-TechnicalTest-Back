using MichelPage_TechnicalTest_Back.Dtos.UserDtos;
using MichelPage_TechnicalTest_Back.Repositories.UserRepository;
using Microsoft.AspNetCore.Mvc;

namespace MichelPage_TechnicalTest_Back.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el listado de usuarios");
                return StatusCode(500, new { message = "No fue posible obtener los usuarios." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateDto userCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Los datos enviados no son válidos.", errors = ModelState });

            if (string.IsNullOrWhiteSpace(userCreate.Nombre) || string.IsNullOrWhiteSpace(userCreate.Email))
                return BadRequest(new { message = "El nombre y el correo son requeridos." });

            try
            {
                _userRepository.CreateUser(userCreate);
                return Ok(new { message = "Usuario creado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el usuario con email {Email}", userCreate.Email);
                return StatusCode(500, new { message = "No fue posible crear el usuario." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin credentials)
        {
            if (string.IsNullOrWhiteSpace(credentials.Email) || string.IsNullOrWhiteSpace(credentials.Contraseña))
                return BadRequest(new { message = "El email y la contraseña son requeridos." });

            try
            {
                var user = await _userRepository.LoginAsync(credentials);

                if (user is null)
                    return Unauthorized(new { message = "Credenciales incorrectas." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el proceso de autenticación para {Email}", credentials.Email);
                return StatusCode(500, new { message = "Error interno durante la autenticación." });
            }
        }
    }
}
