using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Presentation.ViewModel;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for RegisterWindowxaml.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private RegisterViewModel registerViewModel;

        internal RegisterWindow(RegisterViewModel registerViewModel)
        {
            InitializeComponent();
            this.registerViewModel = registerViewModel;
            this.DataContext = registerViewModel;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            registerViewModel.Register();
        }
    }
}
