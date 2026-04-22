using MichelPage_TechnicalTest_Back.Dtos.UserDtos;

namespace MichelPage_TechnicalTest_Back.Services.UserService
{
    public interface IUserService
    {
        Task<List<UserResultDto>> GetAllAsync();

        void CreateUser(UserCreateDto userDto);

        Task<UserResultDto?> LoginAsync(UserLogin credentials);
    }
}
