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
        public async Task<IActionResult> TaskList()
        {
            var values = await _taskRepository.GetAllTasksAsync();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskCreateDto taskDto)
        {
            try
            {
                await _taskRepository.CreateTaskAsync(taskDto, 1);
                return Ok("Successfull");
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new
                {
                    message = "Error interno del servidor",
                    Error = ex.Message
                });
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTask(TaskUpdateDto taskDto)
        {
            await _taskRepository.UpdateTaskAsync(taskDto, 1);
            return Ok("Successfull");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStatus(TaskUpdateStatusDto taskDto)
        {
            await _taskRepository.UpdateStatusAsync(taskDto, 1);
            return Ok("Successfull");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskRepository.DeleteTaskAsync(id, 1);
            return Ok("Successfull");
        }
    }
}
