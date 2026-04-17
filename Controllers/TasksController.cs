using MichelPage_TechnicalTest_Back.Dtos.TaskDtos;
using MichelPage_TechnicalTest_Back.Repositories.TaskRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MichelPage_TechnicalTest_Back.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            var values = await _taskRepository.GetAllTasksAsync();
            return Ok(values);
        }
        //[HttpGet("{id}")]
        //public async Task<IActionResult> CategoryByID(int id)
        //{
        //    var values = await _categoryRepository.GetByIdAsync(id);
        //    return Ok(values);
        //}
        [HttpPost]
        public async Task<IActionResult> CreateCategory(TaskCreateDto taskDto)
        {
            try
            {
               await _taskRepository.CreateTaskAsync(taskDto, 1);
                return Ok("Successfull");
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear el error (recomendado usar ILogger)
                // _logger.LogError(ex, "Error al crear la tarea");

                return StatusCode(500, new
                {
                    message = "Error interno del servidor",
                    Error=ex.Message
                });
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(TaskUpdateDto taskDto)
        {
            await _taskRepository.UpdateTaskAsync(taskDto,1);
            return Ok("Successfull");
        }
    }
}
