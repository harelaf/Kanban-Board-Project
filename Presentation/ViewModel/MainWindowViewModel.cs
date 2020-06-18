using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Presentation.View;
using Presentation.Model;

namespace Presentation.ViewModel
{
    class MainWindowViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

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

        private bool isLoginEnabled;
        public bool IsLoginEnabled
        {
            get => isLoginEnabled;
            set
            {
                isLoginEnabled = value;
                RaisePropertyChanged("IsLoginEnabled");
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

        public MainWindowViewModel()
        {
            this.Controller = new BackendController();
        }

        public MainWindowViewModel(BackendController controller)
        {
            this.Controller = controller;
        }

        public UserModel Login()
        {
            ErrorMessage = "";
            try
            {
                UserModel loggedIn = Controller.Login(email, password);
                return loggedIn;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return null;
            }
        }


    }
}
