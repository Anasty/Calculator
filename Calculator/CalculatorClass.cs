using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class CalculatorClass
    {
        static public double Calculate(string input)
        {
            string rpn = RPN(input);          

            return Compute(rpn);
        }

        static bool IsOperator(char c)
        {
            if (("+-/*^() ".IndexOf(c) != -1))
                return true;
            return false;
        }

        static byte Priority(char c)
        {
            switch (c)
            {
                case '+': return 1;
                case '-': return 1;
                case '*': return 2;
                case '/': return 2;              
                default: return 0;
            }
        }

        static string RPN(string input)
        {
            string output = string.Empty;
            Stack<char> operators = new Stack<char>();

            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]) || input[i]== ',')
                {
                    if (input[i] == ',')
                        output += "0";
                    while (!IsOperator(input[i]))
                    { 
                        output += input[i];                        
                        if (i+1 == input.Length || IsOperator(input[i+1]))
                            break;
                        i++;
                    }
                    output += " ";
                }
               

                if (IsOperator(input[i]))
                {
                    if (input[i] == '(')
                        operators.Push(input[i]);
                    else if (input[i] == ')')
                    {
                        try
                        {
                            char oper = operators.Pop();
                            while (oper != '(')
                            {
                                output += oper.ToString();
                                oper = operators.Pop();
                            }
                        }
                        catch(InvalidOperationException ex)
                        {
                            System.Windows.MessageBox.Show("Ошибка! Некорректный ввод", ex.Message);
                            return "0";
                        }
                    }
                    else
                    {
                        if(operators.Count > 0)
                            if (Priority(input[i]) <= Priority(operators.Peek()))
                            {
                                output += operators.Pop().ToString();
                            }
                        operators.Push(char.Parse(input[i].ToString()));
                    }
                }
                
            }
            while (operators.Count > 0)
            {
                output += operators.Pop();                
            }
            return output;
        }

        static double Compute(string parseInput)
        {
            double result = 0;

            Stack<double> tempStack = new Stack<double>();
            if (parseInput.Length > 0)
            { 

                for (int i = 0; i < parseInput.Length; i++)
                {
                    if (parseInput[i] == ' ')
                        continue;

                    if (Char.IsDigit(parseInput[i]))
                    {
                        string number = string.Empty;
                        while (!IsOperator(parseInput[i]))
                        {
                            number += parseInput[i];
                            if (i + 1 == parseInput.Length || IsOperator(parseInput[i + 1]))
                                break;
                            i++;
                        }
                        tempStack.Push(double.Parse(number));
                    }
                    else if (IsOperator(parseInput[i]))
                    {
                        try
                        {
                            double secondNumber;
                            double firstNumber = tempStack.Pop();

                            if (tempStack.Count == 0) 
                            {
                                secondNumber = 0;
                            }
                            else
                                secondNumber = tempStack.Pop();

                            switch (parseInput[i])
                            {
                                case '+': result = firstNumber + secondNumber; break;
                                case '-': result = secondNumber - firstNumber; break;
                                case '*': result = firstNumber * secondNumber; break;
                                case '/':
                                    if (firstNumber != 0)
                                    {
                                        result = secondNumber / firstNumber;
                                        break;
                                    }
                                    else
                                    {
                                        System.Windows.MessageBox.Show("Ошибка! Деление на 0");
                                        return 0;
                                    }
                                default:
                                    System.Windows.MessageBox.Show("Ошибка! Неизвестный оператор");
                                    break;

                            }
                            tempStack.Push(result);
                        }
                        catch (InvalidOperationException ex)
                        {
                            System.Windows.MessageBox.Show("Ошибка! Некорректный ввод", ex.Message);
                            return 0;
                        }
                    }
                }
                return tempStack.Peek();
            }
            return 0;
        }
    }
    
}
