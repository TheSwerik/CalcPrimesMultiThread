using System;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CalcPrimesMultiThread.Prime.util;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Frontend
{
    public partial class MainWindow : Window
    {
        public static bool Finished = true;
        private CancellationTokenSource _cancelToken;

        public MainWindow()
        {
            InitializeComponent();
            ThreadRadioThread.IsChecked = true;
            OutputTextField.Text = Environment.CurrentDirectory;
            Task.Run(Update);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            int? threadCount = null;
            BigInteger? maxNumber = null;
            if (!MaxNumberCheckBox.IsChecked ?? false)
            {
                if (!IsLegitNumber(MaxNumberBox.Text))
                {
                    MessageBox.Show("Maximum Number not Valid!", "ERROR", MessageBoxButton.OK);
                    return;
                }

                if ((maxNumber = BigInteger.Parse(MaxNumberBox.Text)) < BigInteger.One * 2)
                {
                    MessageBox.Show("Maximum Number should not be below 2!", "ERROR", MessageBoxButton.OK);
                    return;
                }
            }

            if (ThreadRadioThread.IsChecked ?? false)
            {
                if (!IsLegitNumber(ThreadBox.Text))
                {
                    MessageBox.Show("Threadcount is not Valid!", "ERROR", MessageBoxButton.OK);
                    return;
                }

                if ((threadCount = int.Parse(ThreadBox.Text)) < 1)
                {
                    MessageBox.Show("Threadcount should not be below 1!", "ERROR", MessageBoxButton.OK);
                    return;
                }
            }

            if (OverrideCheckBox.IsChecked ?? false)
            {
                if (MessageBox.Show("Are you sure that you want to override / delete all existing Files?", "WARNING",
                                    MessageBoxButton.YesNo) != MessageBoxResult.Yes ||
                    MessageBox.Show("Are you really sure?", "WARNING",
                                    MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;

                ConsoleTextBlock.Text = "";
                CustomConsole.Clear();

                _cancelToken = new CancellationTokenSource();
                Starter.Task = !ThreadRadioThread.IsChecked ?? false;
                Starter.ShouldOverride = true;
                Starter.ThreadCount = threadCount;
                Starter.MaxN = maxNumber;
                ThreadPool.QueueUserWorkItem(Starter.Start, _cancelToken.Token);
            }
            else
            {
                ConsoleTextBlock.Text = "";
                CustomConsole.Clear();

                _cancelToken = new CancellationTokenSource();
                Starter.Task = !ThreadRadioThread.IsChecked ?? false;
                Starter.ShouldOverride = false;
                Starter.ThreadCount = threadCount;
                Starter.MaxN = maxNumber;
                ThreadPool.QueueUserWorkItem(Starter.Start, _cancelToken.Token);
            }

            StartButton.Visibility = Visibility.Hidden;
            CancelButton.Visibility = Visibility.Visible;
            ProgressBar.Visibility = Visibility.Visible;
            Finished = false;
            Task.Run(() =>
                     {
                         while (!Finished) Thread.Sleep(1000);
                         StartButton.Dispatcher.Invoke(new UpdateStartButtonCallback(ActivateStartButton));
                         CancelButton.Dispatcher.Invoke(new UpdateCancelButtonCallback(DeactivateCancelButton));
                         ProgressBar.Dispatcher.Invoke(new UpdateProgressBarCallback(DeactivateProgressBar));
                     });
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            CustomConsole.WriteLine("Canceling...");
            _cancelToken?.Cancel();
            _cancelToken?.Dispose();
            CancelButton.IsEnabled = false;
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

        private static bool IsLegitNumber(string text) => text != null && Regex.IsMatch(text, "[0-9]+");

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var folderDialog = new CommonOpenFileDialog
                               {
                                   IsFolderPicker = true,
                                   Title = "Select Output Folder",
                                   DefaultDirectory = OutputTextField.Text,
                                   InitialDirectory = OutputTextField.Text
                               };
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok) OutputTextField.Text = folderDialog.FileName;

            folderDialog.Dispose();
        }

        private void OutputTextField_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Environment.CurrentDirectory = OutputTextField.Text;
        }


        private void Window_OnClosed(object? sender, EventArgs e) => Environment.Exit(Environment.ExitCode);


        private void CheckForNumbers(object sender, TextCompositionEventArgs e) => e.Handled = !IsLegitNumber(e.Text);

        private void ThreadBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (ThreadBox.Text.Equals("")) ThreadBox.Text = "1";
            var input = int.Parse(ThreadBox.Text);
            if (input > 64) ThreadBox.Text = "64";
            else if (input < 1) ThreadBox.Text = "1";
        }

        // Helper Methods:
        private void Update()
        {
            while (true)
            {
                Thread.Sleep(1000);
                ConsoleTextBlock.Dispatcher.Invoke(
                    new UpdateConsoleCallback(UpdateConsoleText),
                    CustomConsole.Output()
                );
            }
        }

        private void UpdateConsoleText(string message)
        {
            ConsoleTextBlock.Text = "" + message;
            ConsoleTextBlock.ScrollToEnd();
        }

        private void ActivateStartButton()
        {
            StartButton.Visibility = Visibility.Visible;
        }

        private void DeactivateProgressBar()
        {
            ProgressBar.Visibility = Visibility.Hidden;
        }

        private void DeactivateCancelButton()
        {
            CancelButton.Visibility = Visibility.Hidden;
            CancelButton.IsEnabled = true;
        }

        private delegate void UpdateConsoleCallback(string message);

        private delegate void UpdateStartButtonCallback();

        private delegate void UpdateCancelButtonCallback();

        private delegate void UpdateProgressBarCallback();
    }
}