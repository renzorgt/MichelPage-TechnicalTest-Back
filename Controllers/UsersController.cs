using MichelPage_TechnicalTest_Back.Dtos.UserDtos;
using MichelPage_TechnicalTest_Back.Repositories.UserRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MichelPage_TechnicalTest_Back.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);

        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateDto userCreate)
        {
            _userRepository.CreateUser(userCreate);
            return Ok("Usuario creado exitosamente");
        }
    }
}
