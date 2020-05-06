namespace PiApp.Console
{
    public class TokenValue
    {
        public TokenValue(Tokenizer tokenizer)
        {
            Token = tokenizer.Token;

            if (tokenizer.Token == Token.Number)
            {
                Value = tokenizer.Number;
            }
        }

        public Token Token { get; set; }
        
        public decimal? Value { get; set; }
    }
}