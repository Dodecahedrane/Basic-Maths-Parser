using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.CompilerServices;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Program prog = new Program();

            Console.WriteLine("Basic Maths Parser");
            Console.WriteLine("Examples: '3+9*9'   '(14/5)*(6/7+3)");
            Console.WriteLine("Input: ");
            string test = Console.ReadLine();
            Console.WriteLine("RPN: " + prog.convertRPN(test));
            Console.WriteLine("Answer: " + prog.calc(prog.convertRPN(test)));
        }

        double calc(string rpn)
        {

            List<string> read = new List<string>();
            read = rpn.Split(" ").ToList();

            double ans;
            double a;
            double b;

            for (int i = 0; i < rpn.Length; i++)
            {
                if (read.Count() == 1)
                {
                    break;
                }
                else if (oper(read[i]))
                {

                    switch (read[i])
                    {
                        case "*":
                            a = Convert.ToDouble(read[i - 2]);
                            b = Convert.ToDouble(read[i - 1]);
                            ans = a * b;
                            read[i] = Convert.ToString(ans);
                            read.RemoveAt(i - 2);
                            read.RemoveAt(i - 2);
                            i = 0;
                            break;

                        case "/":
                            a = Convert.ToDouble(read[i - 2]);
                            b = Convert.ToDouble(read[i - 1]);
                            ans = a / b;
                            read[i] = Convert.ToString(ans);
                            read.RemoveAt(i - 2);
                            read.RemoveAt(i - 2);
                            i = 0;
                            break;

                        case "+":
                            a = Convert.ToDouble(read[i - 2]);
                            b = Convert.ToDouble(read[i - 1]);
                            ans = a + b;
                            read[i] = Convert.ToString(ans);
                            read.RemoveAt(i - 2);
                            read.RemoveAt(i - 2);
                            i = 0;
                            break;

                        case "-":
                            a = Convert.ToDouble(read[i - 2]);
                            b = Convert.ToDouble(read[i - 1]);
                            ans = a - b;
                            read[i] = Convert.ToString(ans);
                            read.RemoveAt(i - 2);
                            read.RemoveAt(i - 2);
                            i = 0;
                            break;

                    }
                }
            }

            return Convert.ToDouble(read[0]);
        }

        string convertRPN(string input)
        {
            input = formatString(input);
            Console.WriteLine("RPN Conv Input:" + input);

            string[] tokens = input.Split(" ");
            var stack = new List<string>();
            var queue = new List<string>();


            //iterate through array of tokens
            for (int i = 0; i < tokens.Length; i++)
            {
                string read = tokens[i];

                //if read is a number, add to queue
                if (int.TryParse(read, out _))
                {
                    queue.Add(read);
                }

                //if read is an operator
                else if (oper(read))
                {
                    //while stack != 0 length
                    //while the top of stack has greater precendence
                    //pop off stack and onto queue
                    //push current operator (read) onto queue

                    while (stack.Any())
                    {
                        if (Precedence(read, stack[stack.Count() - 1]))
                        {
                            queue.Add(stack[stack.Count() - 1]);
                            stack.RemoveAt(stack.Count - 1);
                        }
                        else if (!Precedence(read, stack[stack.Count() - 1]))
                        {
                            stack.Add(read);
                            break;
                        }
                    }
                    if (!stack.Any())
                    {
                        stack.Add(read);
                    }
                }

                //if read is a left bracket
                else if (read == "(")
                {
                    //push onto stack
                    stack.Add("(");

                }

                //if read is a right bracket
                else if (read == ")")
                {
                    //while top of stack is not left bracket
                    //pop operators from stack onto output queue
                    //pop left bracket from stack and discard

                    while (stack[stack.Count() - 1] != "(")
                    {
                        queue.Add(stack[stack.Count() - 1]);
                        stack.RemoveAt(stack.Count - 1);
                    }

                    stack.RemoveAt(stack.Count - 1);
                }
            }

            //pop any remaining operators onto the queue

            while (stack.Any())
            {
                queue.Add(stack[stack.Count() - 1]);
                stack.RemoveAt(stack.Count - 1);
            }

            //generate RPN String from string list
            string strOutput = "";
            for (int j = 0; j < queue.Count(); j++)
            {
                strOutput = strOutput + " " + queue[j];
            }
            return strOutput.Trim();
        }

        bool oper(string toCheck)
        {
            if (toCheck == "+" | toCheck == "-" | toCheck == "*" | toCheck == "/")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool Precedence(string toCompare, string topOfStack)
        {
            switch (topOfStack)
            {
                case "+":
                    if (toCompare == "*" | toCompare == "/")
                    {
                        return false;
                    }
                    break;

                case "-":
                    if (toCompare == "*" | toCompare == "/")
                    {
                        return false;
                    }
                    break;

                case "*":
                    break;

                case "/":
                    if (toCompare == "*")
                    {
                        return false;
                    }
                    break;

                case "(":
                    return false;

                case ")":
                    //should be handled before precedence is called
                    break;
                default:
                    break;
            }

            return true;
        }

        string formatString(string input)
        {
            //i hate this implentation, pls refactor

            string formatted = String.Concat(input.Where(c => !Char.IsWhiteSpace(c)));
            string formattedWithSpaces = "";

            //need to add spaces between all parts
            for (int i = 0; i < formatted.Length; i++)
            {
                char c = formatted[i];
                if (c.ToString() == "+" | c.ToString() == "-" | c.ToString() == "*" | c.ToString() == "/" | c.ToString() == "(" | c.ToString() == ")")      //if not a number
                {
                    formattedWithSpaces = formattedWithSpaces + " " + c.ToString() + " ";
                }
                else
                {
                    formattedWithSpaces = formattedWithSpaces + c.ToString();
                }
            }
            formattedWithSpaces = Regex.Replace(formattedWithSpaces, @"\s+", " ");                //remove double spaces

            formattedWithSpaces = Regex.Replace(formattedWithSpaces, @"\+\s-\s", "+ -");          //Make negative numbers work 
            formattedWithSpaces = Regex.Replace(formattedWithSpaces, @"\-\s-\s", "- -");          //I hate how I have to use all this regex
            formattedWithSpaces = Regex.Replace(formattedWithSpaces, @"\*\s-\s", "* -");          //Need to refactor this to not use all this regex
            formattedWithSpaces = Regex.Replace(formattedWithSpaces, @"\/\s-\s", "/ -");          // :(

            return formattedWithSpaces.Trim();
        }
    }
}
