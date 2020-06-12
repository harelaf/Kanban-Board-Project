using Presentation.ViewModel;
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

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for AddColumn.xaml
    /// </summary>
    public partial class AddColumn : Window
    {
        private AddColumnViewModel AddColumnModel;

        public AddColumn()
        {
            InitializeComponent();
        }


        internal AddColumn(KanbanViewModel kvModel)
        {
            InitializeComponent();
            this.AddColumnModel = new AddColumnViewModel(kvModel);
            this.DataContext = AddColumnModel;
        }

        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            AddColumnModel.AddColumn();
        }

    }
}
