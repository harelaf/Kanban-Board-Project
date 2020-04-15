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

        public UserService()
        {
            userController = new UserController();
        }

        public Response Logout(string email)
        {
            Response response=new Response();
            try {
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

        public Response<User> Login(string email,string password)
        {
            throw new NotImplementedException();
            //Response<User> res;
        }
            
        public Response Register(string email,string password,string nickname)
        {
            throw new NotImplementedException();
        }

    }
}
