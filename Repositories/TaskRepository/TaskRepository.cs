using Dapper;
using MichelPage_TechnicalTest_Back.DapperContext;
using MichelPage_TechnicalTest_Back.Dtos.TaskDtos;
using System.Text.Json;

namespace MichelPage_TechnicalTest_Back.Repositories.TaskRepository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly Context _context;
        public TaskRepository(Context context)
        {
            _context = context;
        }
        public async Task<List<TaskResultDto>> GetAllTasksAsync()
        {

            string query = @"exec Get_ListAllTask";

            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<TaskResultDto>(query);
                return values.ToList();
            }
        }

        public async Task<List<TaskResultDto>> GetTasksByFilterAsync(TaskFiltersDto taskFiltersDto)
        {
            var json = JsonSerializer.Serialize(taskFiltersDto);
                
            string query = @"exec Get_ListAllTask_byfilters @Filters = @json";

            var parameters = new DynamicParameters();
            parameters.Add("@json", json);
            //parameters.Add("@status", status);

            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<TaskResultDto>(query, parameters);
                return values.ToList();
            }
        }

        public async Task CreateTaskAsync(TaskCreateDto taskDto)
        {
            try
            {
                string query = @"
        INSERT INTO Tasks (Titulo, UserId, Informacion, UserIdCrea) 
        VALUES (@Titulo, @UserId, @Informacion, @UserIdActual)";

                var parameters = new DynamicParameters();

                parameters.Add("@Titulo", taskDto.Titulo);
                parameters.Add("@UserId", taskDto.UserId);
                parameters.Add("@Informacion", taskDto.Informacion);
                parameters.Add("@UserIdActual", taskDto.UserIdCrea);

                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(query, parameters);
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateTaskAsync(TaskUpdateDto taskDto)
        {

            string query = @"
            UPDATE Tasks 
            SET Titulo = @Titulo, 
                Status = @Status, 
                Informacion = @Informacion, 
                FechaModificacion = getdate(),
                UserIdMod = @UserIdMod
            WHERE Id = @Id AND Eliminado = 0;
        ";

            var parameters = new DynamicParameters();

            parameters.Add("@Id", taskDto.Id);

            parameters.Add("@Titulo", taskDto.Titulo);
            parameters.Add("@Status", taskDto.Status);

            parameters.Add("@Informacion", taskDto.Informacion);

            parameters.Add("@FechaModificacion", DateTime.UtcNow);
            parameters.Add("@UserIdMod", taskDto.UserIdMod);

            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, parameters);

                return rowsAffected > 0;
            }
        }

             public async Task<bool> UpdateStatusAsync(TaskUpdateStatusDto taskDto)
        {

            string query = @"
            UPDATE Tasks 
            SET 
                Status = @Status,               
                FechaModificacion = getdate(),
                UserIdMod = @UserIdMod
            WHERE Id = @Id AND Eliminado = 0;
        ";

            var parameters = new DynamicParameters();


            parameters.Add("@Id", taskDto.Id);
            parameters.Add("@Status", taskDto.Status);
            parameters.Add("@UserIdMod", taskDto.UserIdMod);

            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, parameters);

                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteTaskAsync(int taskId, int currentUserId)
        {

            string query = @"
            UPDATE Tasks 
            SET 
                Eliminado = 1,               
                FechaModificacion = getdate(),
                UserIdMod = @UserIdMod
            WHERE Id = @Id AND Eliminado = 0;
        ";

            var parameters = new DynamicParameters();


            parameters.Add("@Id", taskId);
            parameters.Add("@UserIdMod", currentUserId);

            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, parameters);

                return rowsAffected > 0;
            }
        }
    } 

}
