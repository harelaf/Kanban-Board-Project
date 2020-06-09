using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class MainWindowViewModel : 
    {
        private BackendController controller;

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                rais
            }
        }

        public MainWindowViewModel()
        {
            this.controller = new BackendController();

        }
    }
}
