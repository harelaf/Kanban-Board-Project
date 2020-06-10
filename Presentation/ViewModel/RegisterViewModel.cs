using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class RegisterViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        public RegisterViewModel()
        {
            this.Controller = new BackendController();
            isRegisterEnabled = true;
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        private string nickname;
        public string Nickname
        {
            get => nickname;
            set
            {
                nickname = value;
                RaisePropertyChanged("Nickname");
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        private string hostEmail;
        public string HostEmail
        {
            get => hostEmail;
            set
            {
                hostEmail = value;
                RaisePropertyChanged("HostEmail");
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                RaisePropertyChanged("ErrorMessage");
            }
        }

        private bool isRegisterEnabled;
        public bool IsRegisterEnabled
        {
            get => isRegisterEnabled;
            set
            {
                isRegisterEnabled = value;
                RaisePropertyChanged("isRegisterEnabled");
            }
        }

        public void Register()
        {
            ErrorMessage = "";
            try
            {
                Controller.Register(Email, Nickname, Password, HostEmail);
                IsRegisterEnabled = false;
                ErrorMessage = "Registered Successefully! You can now login to the system.";
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }
    }
}
