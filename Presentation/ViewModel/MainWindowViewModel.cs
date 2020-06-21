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
        
        /// <summary>
        /// displays messages to the window
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

        public MainWindowViewModel()
        {
            this.Controller = new BackendController();
        }

        /// <summary>
        /// in case of reopen the main window. get an old controller instead of making a new ona
        /// </summary>
        /// <param name="controller"></param>
        public MainWindowViewModel(BackendController controller)
        {
            this.Controller = controller;
        }

        /// <summary>
        /// order the controller to login a user and displays a proper message
        /// </summary>
        /// <returns></returns>
        public KanbanViewModel Login()
        {
            ErrorMessage = "";
            try
            {
                BoardModel loggedIn = Controller.Login(email, password);
                KanbanViewModel KVModel = new KanbanViewModel(Controller, loggedIn);
                return KVModel;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return null;
            }
        }

        public RegisterViewModel RegisterPressed()
        {
            return new RegisterViewModel(Controller);
        }

    }
}
