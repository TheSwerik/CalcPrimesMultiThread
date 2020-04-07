using System;
using System.Windows;

namespace Frontend
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ThreadRadioThread.IsChecked = true;
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Start");
        }

        private void CheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            MaxNumberLabel.Visibility = Visibility.Hidden;
            MaxNumberBox.Visibility = Visibility.Hidden;
        }

        private void CheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            MaxNumberLabel.Visibility = Visibility.Visible;
            MaxNumberBox.Visibility = Visibility.Visible;
        }

        private void ThreadRadioThread_OnChecked(object sender, RoutedEventArgs e)
        {
            ThreadLabel.Visibility = Visibility.Visible;
            ThreadBox.Visibility = Visibility.Visible;
        }

        private void ThreadRadioTask_OnChecked(object sender, RoutedEventArgs e)
        {
            ThreadLabel.Visibility = Visibility.Hidden;
            ThreadBox.Visibility = Visibility.Hidden;
        }
    }
}