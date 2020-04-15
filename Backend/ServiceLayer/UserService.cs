using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;
namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {
        private readonly UserController userController;

        public Response Logout(string email)
        {
            Response response = new Response();
            try
            {
                userController.Logout(email);
            }
            catch (Exception e)
            {
                response = new Response(e.Message);
            }
            return response;
        }

        public Response LoadData()
        {
            throw new NotImplementedException();
        }

        public Response<User> Login(string email, string password)
        {
            Response<User> response;
            BusinessLayer.UserPackage.User user = null;
            try
            {
                user = userController.Login(email, password);
            }
            catch (Exception e)
            {
                return new Response<User>(e.Message);
            }

            User serviceUser = new User(user.GetEmail(),user.GetNickname());
            response = new Response<User>(serviceUser);
            return response;
        }

        public Response Register(string email, string password, string nickname)
        {
            Response response = new Response();
            try
            {
                userController.Register(email, password, nickname);
            }
            catch (Exception e)
            {
                response = new Response(e.Message);
            }
            return response;
        }

    }
}
