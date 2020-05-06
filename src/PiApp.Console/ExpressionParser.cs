using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace PiApp.Console
{
    public class ExpressionParser
    {
        private IDictionary<Token, Func<decimal, decimal, decimal>> _operations;

        public ExpressionParser()
        {
            _operations = new Dictionary<Token, Func<decimal, decimal, decimal>>
            {
                {Token.Multiply, (x, y) => x * y},
                {Token.Divide, (x, y) => x / y},
                {Token.Add, (x, y) => x + y},
                {Token.Subtract, (x, y) => x - y}
            };
        }

        public void Parse(string expression)
        {
            var tokenValues = new Collection<TokenValue>();

            using (var stringReader = new StringReader(expression))
            {
                var tokenizer = new Tokenizer(stringReader);

                while (tokenizer.Token != Token.EOF)
                {
                    tokenValues.Add(new TokenValue(tokenizer));
                    tokenizer.NextToken();
                }
            }

            var listOfSets = new List<TokenSetCalculator>();

            decimal value = 0;
            Func<decimal, decimal, decimal> op = null;

            var set = new TokenSetCalculator();
            foreach (var token in tokenValues)
            {
                set.AddToken(token);
                if (token.Token == Token.Subtract || token.Token == Token.Add)
                {
                    listOfSets.Add(set);

                    set = new TokenSetCalculator {OperationToken = token.Token};
                }
            }

            listOfSets.Add(set);


            var total = 0m;
            foreach (var tokenSet in listOfSets)
                if (tokenSet.OperationFunc != null)
                    total = tokenSet.OperationFunc(total, tokenSet.Calculate());
                else
                    total += tokenSet.Calculate();

            System.Console.WriteLine($"TOTAL VALUE: {total}");
        }
    }
}