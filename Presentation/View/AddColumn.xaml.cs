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
    /// Interaction logic for AddColumn.xaml
    /// </summary>
    public partial class AddColumn : Window
    {
        private AddColumnViewModel AddColumnModel;
        private ObservableCollection<ColumnModel> columns;

        /// <summary>
        /// add column window
        /// </summary>
        /// <param name="Controller"></param>
        /// <param name="columns"></param>
        internal AddColumn(BackendController Controller, ObservableCollection<ColumnModel> columns)
        {
            InitializeComponent();
            this.AddColumnModel = new AddColumnViewModel(Controller);
            this.DataContext = AddColumnModel;
            this.columns = columns;
        }

        /// <summary>
        /// event that triggers when the "add column" button is clicked
        /// adds a new column to the board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            AddColumnModel.AddColumn(columns);
        }

    }
}
