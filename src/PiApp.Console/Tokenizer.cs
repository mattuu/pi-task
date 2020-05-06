using System.Globalization;
using System.IO;
using System.Text;

namespace PiApp.Console
{
    public class Tokenizer
    {
        private char _currentChar;

        private readonly TextReader _reader;

        public Tokenizer(TextReader reader)
        {
            _reader = reader;
            NextChar();
            NextToken();
        }

        public Token Token { get; private set; }

        public decimal Number { get; private set; }

        private void NextChar()
        {
            var ch = _reader.Read();
            _currentChar = ch < 0 ? '\0' : (char) ch;
        }

        public void NextToken()
        {
            while (char.IsWhiteSpace(_currentChar)) NextChar();

            switch (_currentChar)
            {
                case '\0':
                    Token = Token.EOF;
                    return;

                case '+':
                    NextChar();
                    Token = Token.Add;
                    return;

                case '-':
                    NextChar();
                    Token = Token.Subtract;
                    return;

                case '*':
                    NextChar();
                    Token = Token.Multiply;
                    return;
                
                case '/':
                    NextChar();
                    Token = Token.Divide;
                    return;
            }

            if (char.IsDigit(_currentChar) || _currentChar == '.')
            {
                var sb = new StringBuilder();
                var haveDecimalPoint = false;
                while (char.IsDigit(_currentChar) || !haveDecimalPoint && _currentChar == '.')
                {
                    sb.Append(_currentChar);
                    haveDecimalPoint = _currentChar == '.';
                    NextChar();
                }

                Number = decimal.Parse(sb.ToString(), CultureInfo.InvariantCulture);
                Token = Token.Number;
                return;
            }

            throw new InvalidDataException($"Unexpected character: {_currentChar}");
        }
    }

    public enum Token
    {
        EOF,
        Add,
        Subtract,
        Multiply,
        Divide,
        Number
    }
}