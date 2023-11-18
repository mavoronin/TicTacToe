using System.Text;
using TicTacToe.Domain;
using TicTacToe.UI;

namespace TicTacToe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            var drawer = new GameDrawer();

            var game = new TicTacToeGame(3);
            game.PropertyChanged += drawer.DrawGame;

            game.Init();

            while (true)
            {
                var input = Console.ReadLine();
                if (input is null || !input.Contains(','))
                {
                    Console.WriteLine("Введите позицию клетки в формате \"X,Y\", чтобы сделать ход");
                    continue;
                }

                var splittedInput = input.Split(',');
                var xParsed = int.TryParse(splittedInput[0], out var x);
                if (!xParsed) Console.WriteLine("Введите позицию клетки в формате \"X,Y\", чтобы сделать ход");

                var yParsed = int.TryParse(splittedInput[1], out var y);
                if (!yParsed) Console.WriteLine("Введите позицию клетки в формате \"X,Y\", чтобы сделать ход");

                var validMove = game.MakeMove(new Point(x - 1, y - 1));
                if (!validMove) Console.WriteLine("Неверный ход");
            }
        }
    }
}
