using MichelPage_TechnicalTest_Back.Dtos.TaskDtos;

namespace MichelPage_TechnicalTest_Back.Repositories.TaskRepository
{
    public interface ITaskRepository
    {

        Task<List<TaskResultDto>> GetAllTasksAsync();

        Task CreateTaskAsync(TaskCreateDto taskDto, int currentLoggedInUserId);

        Task<bool> UpdateTaskAsync(TaskUpdateDto taskDto, int currentUserId);
         //Task DeleteTaskAsync(int taskId, int currentUserId);
    }
}
