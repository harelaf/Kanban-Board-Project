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

        public RegisterViewModel(BackendController Controller)
        {
            this.Controller = Controller;
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

        /// <summary>
        /// a label to display the error messages
        /// </summary>
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

        /// <summary>
        /// order the controller to register a new user and display a proper message
        /// </summary>
        public void Register()
        {
            ErrorMessage = "";
            try
            {
                Controller.Register(Email, Nickname, Password, HostEmail);
                ErrorMessage = "Done! You may login or register another one.";
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }
    }
}
