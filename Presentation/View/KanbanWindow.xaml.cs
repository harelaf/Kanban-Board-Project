using System;
using System.Collections.Generic;
using System.Globalization;
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
using Presentation.Model;
using Presentation.ViewModel;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for KanbanWindow.xaml
    /// </summary>
    public partial class KanbanWindow : Window
    {
        private KanbanViewModel KVModel;

        internal KanbanWindow()
        {
            InitializeComponent();
            this.KVModel = new KanbanViewModel();
            this.DataContext = KVModel;
        }

        internal KanbanWindow(BackendController Controller)
        {
            InitializeComponent();
            KVModel = new KanbanViewModel(Controller);
            this.DataContext = KVModel;
        }

        private void AdvanceTask_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.AdvanceTask();

	    }
        private void AddTask_Button_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindow adw = new AddTaskWindow(KVModel.Controller, KVModel.ColumnList);
            adw.Show();
	    }

        private void DeleteTask_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.DeleteTask();
        }

        private void MoveColumnRight_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.MoveColumnRight();
        }

        private void MoveColumnLeft_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.MoveColumnLeft();
        }


        private void DeleteData_Button_Click(object sender, RoutedEventArgs e)
        {
            if (KVModel.DeleteData())
            {
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
        }

        private void RemoveColumn_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.RemoveColumn();
	    }
        private void AddColumn_Button_Click(object sender, RoutedEventArgs e)
        {
            AddColumn addColumn = new AddColumn(KVModel.Controller, KVModel.ColumnList);
            addColumn.Show();
        }

        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.logout();
            MainWindow mw = new MainWindow(KVModel.Controller);
            mw.Show();
            this.Close();
        }

        private void Sort_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.SortByDueDate();
        }

        private void Search_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.filterByString();
        }

        private void Instructions_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Instructions for using the board:\n\n1. In order to add a task/column, click the appropriate button and fill in the fields\n2. When a field requires a date enter the date using the syntax MM/DD/YYYY or MM/DD/YYYY hh:mm:ss AM/PM for more accuracy\n3. In order to advance a task/column, first select one from the list and then click the appropriate button\n4. In order to update a tasks field, select the desired field of the task, change the text and then click off the field, in order to register your update\n5. In order to filter out tasks, enter the text box near the top right the desired key words, and then click the question mark", "Instructions");
        }
    }

}
