using MichelPage_TechnicalTest_Back.Dtos.TaskDtos;

namespace MichelPage_TechnicalTest_Back.Repositories.TaskRepository
{
    public interface ITaskRepository
    {

        Task<List<TaskResultDto>> GetAllTasksAsync();

        Task CreateTaskAsync(TaskCreateDto taskDto);

        Task<bool> UpdateTaskAsync(TaskUpdateDto taskDto);
        
        Task<bool> UpdateStatusAsync(TaskUpdateStatusDto taskDto);

        Task<bool> DeleteTaskAsync(int taskId, int currentUserId);
    }
}
