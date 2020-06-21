using Presentation.Model;
using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        private AddTaskViewModel ATViewModel;

        internal AddTaskWindow(AddTaskViewModel ATViewModel)
        {
            InitializeComponent();
            this.ATViewModel = ATViewModel;
            this.DataContext = ATViewModel;
        }

        /// <summary>
        /// event that triggers when the "add task" button is clicked
        /// adds a new task to the board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            ATViewModel.AddTask();
        }
    }
}
