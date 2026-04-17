using MichelPage_TechnicalTest_Back.Dtos.TaskDtos;
using MichelPage_TechnicalTest_Back.Repositories.TaskRepository;
using Microsoft.AspNetCore.Mvc;

namespace MichelPage_TechnicalTest_Back.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskRepository taskRepository, ILogger<TasksController> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> TaskList()
        {
            try
            {
                var values = await _taskRepository.GetAllTasksAsync();
                return Ok(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el listado de tareas");
                return StatusCode(500, new { message = "No fue posible obtener las tareas." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskCreateDto taskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Los datos enviados no son válidos.", errors = ModelState });

            try
            {
                await _taskRepository.CreateTaskAsync(taskDto);
                return Ok(new { message = "Tarea creada exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la tarea");
                return StatusCode(500, new { message = "No fue posible crear la tarea." });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask(TaskUpdateDto taskDto)
        {
            if (taskDto.Id <= 0)
                return BadRequest(new { message = "El Id de la tarea no es válido." });

            if (!ModelState.IsValid)
                return BadRequest(new { message = "Los datos enviados no son válidos.", errors = ModelState });

            try
            {
                await _taskRepository.UpdateTaskAsync(taskDto);
                return Ok(new { message = "Tarea actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la tarea con Id {Id}", taskDto.Id);
                return StatusCode(500, new { message = "No fue posible actualizar la tarea." });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStatus(TaskUpdateStatusDto taskDto)
        {
            if (taskDto.Id <= 0)
                return BadRequest(new { message = "El Id de la tarea no es válido." });

            var validStatuses = new[] { "Pending", "InProgress", "Done" };
            if (string.IsNullOrWhiteSpace(taskDto.Status) || !validStatuses.Contains(taskDto.Status))
                return BadRequest(new { message = $"Estado no permitido. Los valores válidos son: {string.Join(", ", validStatuses)}." });

            try
            {
                await _taskRepository.UpdateStatusAsync(taskDto);
                return Ok(new { message = "Estado actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar el estado de la tarea con Id {Id}", taskDto.Id);
                return StatusCode(500, new { message = "No fue posible actualizar el estado." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id, [FromQuery] int usuarioMod)
        {
            if (id <= 0)
                return BadRequest(new { message = "El Id de la tarea no es válido." });

            try
            {
                await _taskRepository.DeleteTaskAsync(id, usuarioMod);
                return Ok(new { message = "Tarea eliminada exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la tarea con Id {Id}", id);
                return StatusCode(500, new { message = "No fue posible eliminar la tarea." });
            }
        }
    }
}
