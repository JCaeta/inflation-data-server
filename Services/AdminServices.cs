using InflationDataServer.Models;
using InflationDataServer.Persistence;
using Microsoft.AspNetCore.Identity;

namespace InflationDataServer.Services
{
    public class AdminServices
    {
        public async Task<bool> SignIn(string username, string password)
        {
            Admin admin = new Admin();
            admin.username = "admin";
            admin.password = "admin";

            PostgreSQLUnitOfWork unitOfWork = new PostgreSQLUnitOfWork(DatabaseInformation.Information);
            unitOfWork.connect();   
            bool result = await unitOfWork.SignInAdmin(username, password);
            unitOfWork.disconnect();

            if (result)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ChangePassword(string username, string oldPassword, string newPassword)
        {
            Admin admin = new Admin();
            admin.username = username;
            admin.password = oldPassword;

            PostgreSQLUnitOfWork unitOfWork = new PostgreSQLUnitOfWork(DatabaseInformation.Information);
            unitOfWork.connect();
            if (await unitOfWork.ExistsAdmin(admin))
            {
                admin.password = newPassword;
                bool result = await unitOfWork.UpdateAdminPassword(admin);
                unitOfWork.disconnect();
                return result;
            }
            else
            {
                unitOfWork.disconnect();
                return false;
            }
        }
    }
}
