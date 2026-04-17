using MichelPage_TechnicalTest_Back.Dtos.UserDtos;

namespace MichelPage_TechnicalTest_Back.Repositories.UserRepository
{
    public interface IUserRepository
    {

        Task<List<UserResultDto>> GetAllAsync();

       void CreateUser(UserCreateDto userDto);
    }
}
