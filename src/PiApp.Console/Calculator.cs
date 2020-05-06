using System;
using System.Collections.Generic;

namespace PiApp.Console
{
    public class Calculator
    {
        private readonly IDictionary<Token, Func<decimal, decimal, decimal>> _operations;
        private readonly IDictionary<Token, OperatorPrecedence> _operatorPrecedences;
        private decimal _leftArg;
        private Token _operation;
        private decimal _rightArg;

        public Calculator()
        {
            _operations = new Dictionary<Token, Func<decimal, decimal, decimal>>
            {
                {Token.Multiply, (x, y) => x * y},
                {Token.Divide, (x, y) => x / y},
                {Token.Add, (x, y) => x + y},
                {Token.Subtract, (x, y) => x - y}
            };

            _operatorPrecedences = new Dictionary<Token, OperatorPrecedence>
            {
                {Token.Multiply, OperatorPrecedence.High},
                {Token.Divide, OperatorPrecedence.High},
                {Token.Add, OperatorPrecedence.Low},
                {Token.Subtract, OperatorPrecedence.Low}
            };
        }

        public bool LeftArgSet { get; private set; }
        public bool OperationSet { get; private set; }
        public bool RightArgSet { get; private set; }

        public decimal LeftArg
        {
            get => _leftArg;
            set
            {
                _leftArg = value;
                LeftArgSet = true;
            }
        }

        public decimal RightArg
        {
            get => _rightArg;
            set
            {
                _rightArg = value;
                RightArgSet = true;
            }
        }

        public Token Operation
        {
            get => _operation;
            set
            {
                _operation = value;
                OperationSet = true;
            }
        }

        public OperatorPrecedence Precedence => _operatorPrecedences[Operation];

        public bool IsReady()
        {
            return LeftArgSet && RightArgSet && OperationSet && !(Operation is Token.Number);
        }

        public void Calculate()
        {
            if (IsReady())
            {
                var r = _operations[Operation](LeftArg, RightArg);
                System.Console.WriteLine($"Calculate: {r}");
            }
        }
    }
}