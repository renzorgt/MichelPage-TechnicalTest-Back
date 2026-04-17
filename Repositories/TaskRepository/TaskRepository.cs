using Dapper;
using MichelPage_TechnicalTest_Back.DapperContext;
using MichelPage_TechnicalTest_Back.Dtos.TaskDtos;

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
            
            string query = @"
                    SELECT 
                        t.Id, 
                        t.Titulo, 
                        t.UserId, 
                        u.Nombre AS UserName,
                        t.Status, 
                        t.Informacion, 
                        t.FechaCreacion, 
                        t.FechaModificacion
                    FROM Tasks t
                    INNER JOIN Users u ON t.UserId = u.Id
                    WHERE t.Eliminado = 0;";

            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<TaskResultDto>(query);
                return values.ToList();
            }
        }

        public async Task CreateTaskAsync(TaskCreateDto taskDto, int currentLoggedInUserId)
        {
            try
            {
                string query = @"
        INSERT INTO Tasks (Titulo, UserId, Informacion, UserIdCrea) 
        VALUES (@Titulo, @UserId, @Informacion, @UserIdActual)";

                var parameters = new DynamicParameters();

                // Datos del DTO
                parameters.Add("@Titulo", taskDto.Titulo);
                parameters.Add("@UserId", taskDto.UserId);
                parameters.Add("@Informacion", taskDto.Informacion);
                parameters.Add("@UserIdActual", currentLoggedInUserId);

                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(query, parameters);
                }
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear el error (recomendado usar ILogger)
                // _logger.LogError(ex, "Error al crear la tarea");

                throw new ApplicationException(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateTaskAsync(TaskUpdateDto taskDto, int currentUserId)
        {
            // Solo actualizamos los campos permitidos y la auditoría.
            // Agregamos "AND Eliminado = 0" para asegurarnos de no modificar una tarea borrada lógicamente.
            string query = @"
            UPDATE Tasks 
            SET Titulo = @Titulo, 
                Status = @Status, 
                Informacion = @Informacion, 
                FechaModificacion = @FechaModificacion,
                UserIdMod = @UserIdMod
            WHERE Id = @Id AND Eliminado = 0;
        ";

            var parameters = new DynamicParameters();

            // 1. Identificador de la tarea
            parameters.Add("@Id", taskDto.Id);

            // 2. Datos modificables por el usuario (vienen del DTO)
            parameters.Add("@Titulo", taskDto.Titulo);
            parameters.Add("@Status", taskDto.Status);

            // Aseguramos que envíe un JSON válido o null
            parameters.Add("@Informacion", taskDto.Informacion);

            // 3. Datos de auditoría controlados por el backend
            parameters.Add("@FechaModificacion", DateTime.UtcNow); // Siempre usa UTC para servidores
            parameters.Add("@UserIdMod", currentUserId);

            using (var connection = _context.CreateConnection())
            {
                // ExecuteAsync devuelve el número de filas afectadas (int)
                var rowsAffected = await connection.ExecuteAsync(query, parameters);

                // Si rowsAffected es mayor a 0, significa que encontró el ID y lo actualizó
                return rowsAffected > 0;
            }
        }
    }
}
