using ERPServices.Identity.Domain;

namespace ERPServices.Identity.Repository
{
    public interface IUserRepository
    {
        public UserEntity? GetByEmail(string email);
    }
}
