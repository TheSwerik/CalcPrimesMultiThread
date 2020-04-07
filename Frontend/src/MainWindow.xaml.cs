using System;
using System.Numerics;
using System.Text.RegularExpressions;
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
            if ((!MaxNumberCheckBox.IsChecked ?? false) && !IsLegitNumber(MaxNumberBox.Text))
            {
                MessageBox.Show("Maximum Number not Valid!", "ERROR", MessageBoxButton.OK);
                return;
            }

            if ((!MaxNumberCheckBox.IsChecked ?? false) && BigInteger.Parse(MaxNumberBox.Text) < 2)
            {
                MessageBox.Show("Maximum Number should not be below 2!", "ERROR", MessageBoxButton.OK);
                return;
            }

            if ((ThreadRadioThread.IsChecked ?? false) && !IsLegitNumber(ThreadBox.Text))
            {
                MessageBox.Show("Threadcount is not Valid!", "ERROR", MessageBoxButton.OK);
                return;
            }

            if ((ThreadRadioThread.IsChecked ?? false) && int.Parse(MaxNumberBox.Text) < 1)
            {
                MessageBox.Show("Threadcount should not be below 1!", "ERROR", MessageBoxButton.OK);
                return;
            }

            if (MessageBox.Show("Are you sure that you want to override / delete all existing Files?", "WARNING",
                MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;

            if (MessageBox.Show("Are you really sure?", "WARNING", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Console.WriteLine("START");
            }
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

        private static bool IsLegitNumber(string text)
        {
            return text != null && Regex.IsMatch(text, "[0-9]+");
        }
    }
}