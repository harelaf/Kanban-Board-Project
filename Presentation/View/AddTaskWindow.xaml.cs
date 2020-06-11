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
        private ObservableCollection<ColumnModel> columns;

        public AddTaskWindow()
        {
            InitializeComponent();
            this.ATViewModel = new AddTaskViewModel();
            this.DataContext = ATViewModel;
            columns = new ObservableCollection<ColumnModel>();
        }

        internal AddTaskWindow(BackendController Controller, ObservableCollection<ColumnModel> columns)
        {
            InitializeComponent();
            this.ATViewModel = new AddTaskViewModel(Controller);
            this.DataContext = ATViewModel;
            this.columns = columns;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            ATViewModel.AddTask(columns);
        }
    }
}
