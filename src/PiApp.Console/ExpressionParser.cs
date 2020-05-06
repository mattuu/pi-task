using System;
using System.Collections.Generic;
using System.IO;

namespace PiApp.Console
{
    public class ExpressionParser
    {
        public static char[] OperatorSigns = {'+', '-', '*', '/'};

        private readonly Operator[] _operators;

        public ExpressionParser()
        {
            _operators = new[]
            {
                new Operator {Sign = '+', Precedence = OperatorPrecedence.Low, Calculation = (x, y) => x + y},
                new Operator {Sign = '-', Precedence = OperatorPrecedence.Low, Calculation = (x, y) => x - y},
                new Operator {Sign = '*', Precedence = OperatorPrecedence.High, Calculation = (x, y) => x * y},
                new Operator {Sign = '/', Precedence = OperatorPrecedence.High, Calculation = (x, y) => x / y}
            };
        }

        public void Parse(string expression)
        {
            var counter = expression.Length;
            var index = 0;
            var buffer = new List<Operator>();

            var calculator = new Calculator();
            using (var stringReader = new StringReader(expression))
            {
                var tokenizer = new Tokenizer(stringReader);

                while (tokenizer.Token != Token.EOF)
                {
                    if (calculator.LeftArg == null)
                    {
                        calculator.LeftArg = tokenizer;
                    }
                    else if (calculator.Operation == null)
                    {
                        calculator.Operation = tokenizer;
                    }
                    else if (calculator.RightArg == null)
                    {
                        calculator.RightArg = tokenizer;
                    }

                    if (calculator.IsReady())
                    {
                        calculator.Calculate();
                        calculator = new Calculator();
                    }

                    //System.Console.WriteLine($"{tokenizer.Number}, {tokenizer.Token == Token.Add}, {tokenizer.Token == Token.Subtract}");
                    tokenizer.NextToken();
                }
            }
        }
    }


    public enum OperatorPrecedence
    {
        Low,
        High
    }

    public class Operator
    {
        public char Sign { get; set; }

        public Func<decimal, decimal, decimal> Calculation { get; set; }

        public OperatorPrecedence Precedence { get; set; }
    }


    public class Calculator
    {
        public Tokenizer LeftArg { get; set; }

        public Tokenizer RightArg { get; set; }

        public Tokenizer Operation { get; set; }

        public bool IsReady()
        {
            return LeftArg != null && RightArg != null && Operation != null;
        }

        public void Calculate()
        {
            var r = LeftArg.Number * RightArg.Number;
            System.Console.WriteLine($"Calculate: {r}");
        }
    }
}