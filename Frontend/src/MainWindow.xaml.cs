﻿using System;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CalcPrimesMultiThread.Prime.util;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Frontend
{
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancelToken;

        public MainWindow()
        {
            InitializeComponent();
            ThreadRadioThread.IsChecked = true;
            OutputTextField.Text = Environment.CurrentDirectory;
            Task.Run(Update);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
        }

        private void Update()
        {
            while (true)
            {
                Thread.Sleep(1000);
                ConsoleTextBlock.Dispatcher.Invoke(
                    new UpdateTextCallback(UpdateText),
                    CustomConsole.Output()
                );
            }
        }

        private void UpdateText(string message)
        {
            ConsoleTextBlock.Text = "" + message;
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            int? threadCount = null;
            BigInteger? maxNumber = null;
            if ((!MaxNumberCheckBox.IsChecked ?? false) && !IsLegitNumber(MaxNumberBox.Text))
            {
                MessageBox.Show("Maximum Number not Valid!", "ERROR", MessageBoxButton.OK);
                return;
            }

            if ((!MaxNumberCheckBox.IsChecked ?? false) &&
                (maxNumber = BigInteger.Parse(MaxNumberBox.Text)) < BigInteger.One * 2)
            {
                MessageBox.Show("Maximum Number should not be below 2!", "ERROR", MessageBoxButton.OK);
                return;
            }

            if ((ThreadRadioThread.IsChecked ?? false) && !IsLegitNumber(ThreadBox.Text))
            {
                MessageBox.Show("Threadcount is not Valid!", "ERROR", MessageBoxButton.OK);
                return;
            }

            if ((ThreadRadioThread.IsChecked ?? false) && (threadCount = int.Parse(MaxNumberBox.Text)) < 1)
            {
                MessageBox.Show("Threadcount should not be below 1!", "ERROR", MessageBoxButton.OK);
                return;
            }

            if (OverrideCheckBox.IsChecked ?? false)
            {
                if (MessageBox.Show("Are you sure that you want to override / delete all existing Files?", "WARNING",
                                    MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
                if (MessageBox.Show("Are you really sure?", "WARNING",
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
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            CustomConsole.WriteLine("Canceling...");
            _cancelToken?.Cancel();
            _cancelToken?.Dispose();
            StartButton.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Hidden;
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

        private void Window_OnClosed(object? sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private delegate void UpdateTextCallback(string message);
    }
}