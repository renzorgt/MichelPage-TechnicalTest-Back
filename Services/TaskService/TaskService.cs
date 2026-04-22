using Dapper;
using MichelPage_TechnicalTest_Back.DapperContext;
using MichelPage_TechnicalTest_Back.Dtos.TaskDtos;
using MichelPage_TechnicalTest_Back.Repositories.TaskRepository;
using System.Text.Json;

namespace MichelPage_TechnicalTest_Back.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly Context _context;
        private readonly ITaskRepository _taskRepository;
        
        public TaskService(Context context, ITaskRepository taskRepository)
        {
            _context = context;
            _taskRepository = taskRepository;
        }
        public async Task<List<TaskResultDto>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllTasksAsync();
        }

        public async Task<List<TaskResultDto>> GetTasksByFilterAsync(TaskFiltersDto taskFiltersDto)
        {
            return await _taskRepository.GetTasksByFilterAsync(taskFiltersDto);
        }

        public async Task CreateTaskAsync(TaskCreateDto taskDto)
        {
            if (taskDto == null)
                throw new ArgumentNullException(nameof(taskDto));

            if (string.IsNullOrWhiteSpace(taskDto.Titulo))
                throw new ArgumentException("El Título es obligatorio.");

                if (!IsValidJson(taskDto.Informacion))
                {
                    throw new ArgumentException("La Información debe ser un formato JSON válido.");
                }

            if (taskDto.UserId <= 0)
                throw new ArgumentException("El UserId debe ser mayor a 0.");

            if (taskDto.UserIdCrea <= 0)
                throw new ArgumentException("El UserIdCrea debe ser mayor a 0.");

            await _taskRepository.CreateTaskAsync(taskDto);
        }

        public async Task<bool> UpdateTaskAsync(TaskUpdateDto taskDto)
        {
            if (taskDto == null)
                throw new ArgumentNullException(nameof(taskDto));

            if (taskDto.Id <= 0)
                throw new ArgumentException("El Id de la tarea debe ser mayor a 0.");

            if (string.IsNullOrWhiteSpace(taskDto.Titulo))
                throw new ArgumentException("El Título es obligatorio.");

            if (!IsValidJson(taskDto.Informacion))
            {
                throw new ArgumentException("La Información debe ser un formato JSON válido.");
            }

            if (taskDto.UserIdMod <= 0)
                throw new ArgumentException("El UserIdMod debe ser mayor a 0.");

            return await _taskRepository.UpdateTaskAsync(taskDto);
        }

        public async Task<bool> UpdateStatusAsync(TaskUpdateStatusDto taskDto)
        {
            if (taskDto == null)
                throw new ArgumentNullException(nameof(taskDto));

            if (taskDto.Id <= 0)
                throw new ArgumentException("El Id de la tarea debe ser mayor a 0.");

            if (string.IsNullOrWhiteSpace(taskDto.Status))
                throw new ArgumentException("El Status es obligatorio.");

            if (taskDto.Status != "InProgress" && taskDto.Status != "Done")
                throw new ArgumentException("El Status solo puede ser 'InProgress' o 'Done'.");

            if (taskDto.UserIdMod <= 0)
                throw new ArgumentException("El UserIdMod debe ser mayor a 0.");

            return await _taskRepository.UpdateStatusAsync(taskDto);
        }

        public async Task<bool> DeleteTaskAsync(int taskId, int currentUserId)
        {
            if (taskId <= 0)
                throw new ArgumentException("El Id de la tarea debe ser mayor a 0.");

            if (currentUserId <= 0)
                throw new ArgumentException("El UserIdMod debe ser mayor a 0.");

            return await _taskRepository.DeleteTaskAsync(taskId, currentUserId);
        }

        public static bool IsValidJson(string json)
{
    if (string.IsNullOrWhiteSpace(json))
        return false;

    try
    {
        using (JsonDocument.Parse(json))
        {
            return true;
        }
    }
    catch (JsonException)
    {
        return false;
    }
}
    } 

}
