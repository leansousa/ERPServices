using ERPServices.Identity.Domain;

namespace ERPServices.Identity.Repository
{
    public class UserRepository : IUserRepository
    {
        private static IList<UserEntity> _allUsers = new List<UserEntity>();
        public UserRepository()
        {
            _allUsers = new List<UserEntity>
            {

                new UserEntity
                {
                    Role = "ADMIN",
                    Email = "admin@erpservices.com",
                    Name = "Admin",
                    Password = "admin"
                },

                new UserEntity
                {
                    Role = "REPORT",
                    Email = "report@erpservices.com",
                    Name = "Report",
                    Password = "report"
                },
            };
        }


        public UserEntity? GetByEmail(string email)
        {
            return _allUsers.Where(x => x.Email?.ToLower() == email.ToLower()).FirstOrDefault();
        }
    }
}
