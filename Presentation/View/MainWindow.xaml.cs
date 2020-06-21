using Presentation.View;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Presentation.ViewModel;
using Presentation.Model;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel MWViewModel;

        /// <summary>
        /// for new startup
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            MWViewModel = new MainWindowViewModel();
            this.DataContext = MWViewModel;
        }

        /// <summary>
        /// for after logout
        /// </summary>
        /// <param name="controller">which olds an instance of service</param>
        internal MainWindow(MainWindowViewModel MWVModel)
        {
            InitializeComponent();
            MWViewModel = MWVModel;
            this.DataContext = MWViewModel;
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            KanbanViewModel LoggedIn = MWViewModel.Login();
            if (LoggedIn != null)
            {
                KanbanWindow KanbanWindow = new KanbanWindow(LoggedIn);
                KanbanWindow.Show();
                this.Close();
            }
        }

        /// <summary>
        /// boots up a new register window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow w = new RegisterWindow(MWViewModel.RegisterPressed());
            w.ShowDialog();
        }

    }
}
