using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;
using log4net;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {
        private readonly UserController userController;
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserService()
        {
            userController = new UserController();
        }

        public Response Logout(string email)
        {
            Response response = new Response();
            try
            {
                userController.Logout(email.ToLower());
                log.Debug($"Logged out of {email} successfully");
            }
            catch (Exception e)
            {
                log.Warn("Failed to logout: " + e.Message);
                response = new Response(e.Message);
            }
            return response;
        }

        public Response LoadData()
        {
            Response response = new Response();
            try
            {
                userController.LoadData();
                log.Debug("Loaded the data successfully");
            }
            catch (Exception e)
            {
                log.Warn("Failed to load the data: " + e.Message);
                response = new Response(e.Message);
            }
            return response;
        }

        public Response<User> Login(string email, string password)
        {
            Response<User> response;
            BusinessLayer.UserPackage.User user = null;
            try
            {
                user = userController.Login(email.ToLower(), password);
                log.Debug($"Logged in to {email} successfully");
            }
            catch (Exception e)
            {
                log.Warn($"Failed to login to {email}: " + e.Message);
                return new Response<User>(e.Message);
            }

            User serviceUser = new User(user.GetEmail(), user.GetNickname(), user.GetBoard());
            response = new Response<User>(serviceUser);
            return response;
        }

        public Response Register(string email, string password, string nickname)
        {
            Response response = new Response();
            try
            {
                userController.Register(email, password, nickname);
                log.Debug($"User {email} registered successfully");
            }
            catch (Exception e)
            {
                log.Warn($"Failed to register user {email}: " + e.Message);
                response = new Response(e.Message);
            }
            return response;
        }

        public void Save()
        {
            userController.Save();
        }

    }
}
