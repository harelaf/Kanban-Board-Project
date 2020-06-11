﻿using Presentation.View;
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

        public MainWindow()
        {
            InitializeComponent();
            MWViewModel = new MainWindowViewModel();
            this.DataContext = MWViewModel;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            UserModel LoggedIn = MWViewModel.Login();
            if (LoggedIn != null)
            {
                KanbanWindow KanbanWindow = new KanbanWindow(MWViewModel.Controller);
                KanbanWindow.Show();
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow w = new RegisterWindow(MWViewModel.Controller);
            w.Show();
        }

    }
}
