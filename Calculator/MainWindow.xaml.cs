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

namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool equalPressed = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        string CheckOutput(string inputElement)
        {
            if (equalPressed && ("+-/*".IndexOf(inputElement) == -1))
                Output.Text = "";

            if(Output.Text.Length > 0) //нельзя ввести операторы подряд
            {
                char lastElement = Output.Text[Output.Text.Length - 1];
                if (("+-/*".IndexOf(lastElement) != -1) && (("+-/*".IndexOf(inputElement) != -1)))                
                    Output.Text = Output.Text.Substring(0, Output.Text.Length - 1);                               
            }

            if (Output.Text.Length > 1) 
            {
                char lastElement = Output.Text[Output.Text.Length - 1];
                if (("(".IndexOf(lastElement) != -1) && (("+/*()".IndexOf(inputElement) != -1)))
                {
                    Output.Text = Output.Text.Substring(0, Output.Text.Length - 1);
                    return "(";
                }
            }
            return inputElement;
        }

        private void ButtonWithValue_Click(object sender, RoutedEventArgs e)
        {
            string inputElement = (sender as Button).Content.ToString();

            inputElement = CheckOutput(inputElement);

            Output.Text += inputElement;                      
            equalPressed = false;
        }

        private void Button_removeOne_Click(object sender, RoutedEventArgs e)
        {
            if(Output.Text.Length > 0)
                Output.Text = Output.Text.Substring(0, Output.Text.Length - 1); 
        }

        private void Button_clear_Click(object sender, RoutedEventArgs e)
        {
            Output.Text = "";
            OperationsHistory.Text = "";
        }        

        private void Button_equal_Click(object sender, RoutedEventArgs e)
        {
            OperationsHistory.Text = Output.Text + "=";
            Output.Text = CalculatorClass.Calculate(Output.Text).ToString();

            equalPressed = true;
        }
      
        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {           
            string inputElement;
            if (Int32.TryParse(e.Text, out int val) || ("-+/*()".IndexOf(e.Text) != -1))
            { 
                inputElement = CheckOutput(e.Text);
                Output.Text += inputElement;
                equalPressed = false;
            }            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                OperationsHistory.Text = Output.Text + "=";
                Output.Text = CalculatorClass.Calculate(Output.Text).ToString();

                equalPressed = true;
            }

            if(e.Key == Key.Back)
            {
                if (Output.Text.Length > 0)
                    Output.Text = Output.Text.Substring(0, Output.Text.Length - 1);
            }
        }
    }
}
