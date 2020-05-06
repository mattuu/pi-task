using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PiApp.Console
{
    public class TokenSetCalculator
    {
        private readonly ICollection<TokenValue> _tokens = new Collection<TokenValue>();
        private readonly IDictionary<Token, Func<decimal, decimal, decimal>> _operations;

        public TokenSetCalculator()
        {
            _operations = new Dictionary<Token, Func<decimal, decimal, decimal>>
            {
                {Token.Multiply, (x, y) => x * y},
                {Token.Divide, (x, y) => x / y},
                {Token.Add, (x, y) => x + y},
                {Token.Subtract, (x, y) => x - y}
            };
        }

        public Token OperationToken { get; set; }

        public Func<decimal, decimal, decimal> OperationFunc => _operations.ContainsKey(OperationToken) ? _operations[OperationToken] : null;

        public void AddToken(TokenValue tokenValue)
        {
            _tokens.Add(tokenValue);
        }

        public decimal Calculate()
        {
            var value = _tokens.First().Value ?? decimal.One;
            Func<decimal, decimal, decimal> op = null;

            foreach (var tokenValue in _tokens.Skip(1))
            {
                if (tokenValue.Token != Token.Number)
                {
                    op = _operations[tokenValue.Token];
                }

                if (op != null && tokenValue.Token == Token.Number)
                {
                    value = op(value, tokenValue.Value ?? decimal.One);
                    op = null;
                }
            }

            return value;
        }
    }
}