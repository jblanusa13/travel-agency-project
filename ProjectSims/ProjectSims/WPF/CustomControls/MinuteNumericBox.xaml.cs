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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectSims.WPF.CustomControls
{
    /// <summary>
    /// Interaction logic for NumericBox.xaml
    /// </summary>
    public partial class MinuteNumericBox : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(int), typeof(MinuteNumericBox), new FrameworkPropertyMetadata(0));

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public MinuteNumericBox()
        {
            InitializeComponent();
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsNumeric(e.Text);
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Handle arrow keys to increment/decrement value
            if (e.Key == Key.Up)
            {
                Value = 30;
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                Value = 0;
                e.Handled = true;
            }
            else if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                Value = 0;
                e.Handled = true;
            }
        }

        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out var _);
        }

        private void IncrementButton_Click(object sender, RoutedEventArgs e)
        { 
                Value = 30;
        }

        private void DecrementButton_Click(object sender, RoutedEventArgs e)
        {
            Value = 0;
        }
    }
}
