using Dapper;
using MichelPage_TechnicalTest_Back.DapperContext;
using MichelPage_TechnicalTest_Back.Dtos.UserDtos;

namespace MichelPage_TechnicalTest_Back.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }
        public async Task<List<UserResultDto>> GetAllAsync()
        {
            string query = "SELECT * FROM Users";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<UserResultDto>(query);
                return values.ToList();
            }
        }
        public async void CreateUser(UserCreateDto userDto)
        {
            string query = @"INSERT INTO Users (Nombre, Email, Contraseña) 
                    VALUES (@Nombre, @Email, @Contraseña)";
            var parameters = new DynamicParameters();
            parameters.Add("@nombre", userDto.Nombre);
            parameters.Add("@email", userDto.Email);
            parameters.Add("@contraseña", userDto.Contraseña);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
