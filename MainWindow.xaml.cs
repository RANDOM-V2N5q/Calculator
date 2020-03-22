using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StringBuilder inputBuffer;
        private Boolean isNegative;
        private Boolean isInteger;
        private Boolean isBlocked;

        private Double buffer;
        private Double memory;
        private Double solution;
        private Char operation;

        public MainWindow()
        {
            CultureInfo en = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = en;
            Thread.CurrentThread.CurrentUICulture = en;

            InitializeComponent();
            InitializeCalc();
        }

        private void ResetInputBuffer()
        {
            inputBuffer.Clear();
            isNegative = false;
            isInteger = true;
        }

        private void ConvertInputBuffer()
        {
            if (inputBuffer.ToString().Length > 0)
            {
                buffer = Double.Parse(inputBuffer.ToString());
                if (isNegative)
                {
                    buffer *= -1;
                }
                ResetInputBuffer();
            }
        }

        private void InitializeCalc()
        {
            inputBuffer = new StringBuilder();
            ResetInputBuffer();
            isBlocked = true;
            buffer = 0;
            memory = 0;
            solution = 0;
            operation = '=';
            Show("0");
        }

        #region Show

        private void Show(String str)
        {
            textBlockDisplay.Text = str;
        }

        private void ShowInputBuffer()
        {
            if (isNegative)
            {
                textBlockDisplay.Text = "-" + inputBuffer.ToString();
            }
            else
            {
                textBlockDisplay.Text = inputBuffer.ToString();
            }
        }

        #endregion

        #region NumericPanel

        private void buttonNumeric_Click(object sender, RoutedEventArgs e)
        {
            Button numericButton = sender as Button;
            inputBuffer.Append(numericButton.Content);
            isBlocked = false;
            ShowInputBuffer();
        }

        private void buttonPlusMinus_Click(object sender, RoutedEventArgs e)
        {
            if(inputBuffer.Length != 0)
            {
                isNegative = !isNegative;
                ShowInputBuffer();
            }
            else
            {
                buffer *= -1;
                solution *= -1;
                Show(buffer.ToString());
            }
        }

        private void buttonDot_Click(object sender, RoutedEventArgs e)
        {
            if(isInteger == true)
            {
                isInteger = false;
                if (inputBuffer.Length == 0)
                {
                    inputBuffer.Append("0.");
                }
                else
                {
                    inputBuffer.Append(".");
                }
                isBlocked = false;
                ShowInputBuffer();
            }
        }
        #endregion

        #region Operations

        private void Solve()
        {
                 if(operation == '=') { solution = buffer; }
            else if(operation == '-') { solution -= buffer; }
            else if(operation == '+') { solution += buffer; }
            else if(operation == '*') { solution *= buffer; }
            else if(operation == '/' && buffer == 0) { solution = 0; }
            else if(operation == '/' && buffer != 0) { solution /= buffer; }
        }

        private void buttonOperation_Click(object sender, RoutedEventArgs e)
        {
            ConvertInputBuffer();
            if (!isBlocked)
            {
                Solve();
                buffer = solution;
                Show(solution.ToString());
                isBlocked = true;
            }
            Button operationButton = sender as Button;
            operation = operationButton.Content.ToString()[0];
        }

        private void buttonSqrt_Click(object sender, RoutedEventArgs e)
        {
            ConvertInputBuffer();
            if (buffer >= 0)
            {
                buffer = Math.Sqrt(buffer);
                Show(buffer.ToString());
            }
            else
            {
                buffer = 0;
                Show(buffer.ToString());
            }
            isBlocked = false;
        }

        private void buttonSolve_Click(object sender, RoutedEventArgs e)
        {
            ConvertInputBuffer();
            if (!isBlocked)
            {
                Solve();
                buffer = solution;
                Show(solution.ToString());
                isBlocked = false;
                solution = 0;
            }
            operation = '=';
        }

        #endregion

        #region Clear
        private void buttonAC_Click(object sender, RoutedEventArgs e)
        {
            InitializeCalc();
        }

        private void buttonEC_Click(object sender, RoutedEventArgs e)
        {
            ResetInputBuffer();
            Show("0");
        }
        #endregion

        #region Memory
        private void buttonMemoryRecall_Click(object sender, RoutedEventArgs e)
        {
            ResetInputBuffer();
            buffer = memory;
            isBlocked = false;
            Show(buffer.ToString());
        }

        private void buttonMemoryPlus_Click(object sender, RoutedEventArgs e)
        {
            buttonSolve_Click(sender, e);
            memory += solution;
        }

        private void buttonMemoryMinus_Click(object sender, RoutedEventArgs e)
        {
            buttonSolve_Click(sender, e);
            memory -= solution;
        }

        private void buttonMemoryClear_Click(object sender, RoutedEventArgs e)
        {
            memory = 0;
        }
        #endregion
    }
}
