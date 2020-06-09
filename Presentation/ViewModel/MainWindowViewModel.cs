using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Presentation.View;

namespace Presentation.ViewModel
{
    class MainWindowViewModel : NotifiableObject
    {
        private BackendController controller;

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
                RaisePropertyChanged("errorMessage");
            }
        }

        public MainWindowViewModel()
        {
            this.controller = new BackendController();

        }

        public void Login()
        {
            ErrorMessage = "";
            try
            {
                controller.Login(email, password);
                KanbanWindow KBWindow = new KanbanWindow();
                KBWindow.Show();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }
    }
}
