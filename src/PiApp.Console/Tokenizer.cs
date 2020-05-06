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

        // Read the next character from the input strem
        // and store it in _currentChar, or load '\0' if EOF
        private void NextChar()
        {
            var ch = _reader.Read();
            _currentChar = ch < 0 ? '\0' : (char) ch;
        }

        // Read the next token from the input stream
        public void NextToken()
        {
            // Skip whitespace
            while (char.IsWhiteSpace(_currentChar)) NextChar();

            // Special characters
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

            // Number?
            if (char.IsDigit(_currentChar) || _currentChar == '.')
            {
                // Capture digits/decimal point
                var sb = new StringBuilder();
                var haveDecimalPoint = false;
                while (char.IsDigit(_currentChar) || !haveDecimalPoint && _currentChar == '.')
                {
                    sb.Append(_currentChar);
                    haveDecimalPoint = _currentChar == '.';
                    NextChar();
                }

                // Parse it
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