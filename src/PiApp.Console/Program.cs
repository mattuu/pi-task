using System.IO;

namespace PiApp.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parser = new ExpressionParser();

            System.Console.WriteLine("Type expression and press Enter");

            var expression = System.Console.ReadLine();
            
            parser.Parse(expression);
        }
    }
}