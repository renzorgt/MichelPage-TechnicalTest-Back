using MichelPage_TechnicalTest_Back.Dtos.TaskDtos;

namespace MichelPage_TechnicalTest_Back.Services.TaskService
{
    public interface ITaskService
    {

        Task<List<TaskResultDto>> GetAllTasksAsync();

        Task<List<TaskResultDto>> GetTasksByFilterAsync(TaskFiltersDto taskFiltersDto);

        Task CreateTaskAsync(TaskCreateDto taskDto);

        Task<bool> UpdateTaskAsync(TaskUpdateDto taskDto);
        
        Task<bool> UpdateStatusAsync(TaskUpdateStatusDto taskDto);

        Task<bool> DeleteTaskAsync(int taskId, int currentUserId);
    }
}
