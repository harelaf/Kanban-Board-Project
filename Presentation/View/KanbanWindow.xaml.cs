﻿using System;
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

        public KanbanWindow()
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
           // KVModel.printColumn();
            KVModel.MoveColumnRight();
        }

        private void MoveColumnLeft_Button_Click(object sender, RoutedEventArgs e)
        {
            KVModel.MoveColumnLeft();
        }

        private void DeleteData_Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddColumn_Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void DeleteColumn_Button_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
