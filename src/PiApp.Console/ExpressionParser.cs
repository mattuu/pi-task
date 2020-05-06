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

            var calculators = new List<Calculator>();

            var calculator = new Calculator();
            using (var stringReader = new StringReader(expression))
            {
                var tokenizer = new Tokenizer(stringReader);

                while (tokenizer.Token != Token.EOF)
                {
                    if (!calculator.LeftArgSet)
                    {
                        calculator.LeftArg = tokenizer.Number;
                    }
                    else if (!calculator.OperationSet)
                    {
                        calculator.Operation = tokenizer.Token;
                    }
                    else if (!calculator.RightArgSet)
                    {
                        calculator.RightArg = tokenizer.Number;
                    }

                    if (calculator.IsReady())
                    {
                        //calculator.Calculate();
                        calculators.Add(calculator);
                        calculator = new Calculator();
                    }

                    //System.Console.WriteLine($"{tokenizer.Number}, {tokenizer.Token == Token.Add}, {tokenizer.Token == Token.Subtract}");
                    tokenizer.NextToken();
                }
            }

            foreach (var calc in calculators)
            {
                calc.Calculate();
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
}