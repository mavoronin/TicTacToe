using System.ComponentModel;
using TicTacToe.Domain;

namespace TicTacToe.UI
{
    internal class GameDrawer
    {
        public void DrawGame(object data, PropertyChangedEventArgs e)
        {
            if (data is not TicTacToeGame game) throw new ArgumentException($"Неверный тип параметра для {nameof(DrawGame)}", nameof(data));
            switch (e.PropertyName)
            {
                case nameof(TicTacToeGame.Finished):
                    DrawGame(game);
                    if (game.Winner != null)
                    {
                        Console.WriteLine($"Победил {game.Winner}!");
                    }
                    else
                    {
                        Console.WriteLine($"Всё поле заполнено");
                    }
                    break;
                case nameof(TicTacToeGame.CurrentMoveCharacter):
                    DrawGame(game);
                    Console.WriteLine($"Сейчас ходит {game.CurrentMoveCharacter}. Введите позицию клетки в формате \"X,Y\", чтобы сделать ход");
                    break;
            }

        }

        private void DrawGame(TicTacToeGame game)
        {
            for (int i = 0; i < game.Size; i++)
            {
                for (int j = 0; j < game.Size; j++)
                {
                    DrawSymbol(game.Field[i, j]);
                }

                Console.WriteLine();
            }
        }

        private void DrawSymbol(GameCharacter? c)
        {
            Console.Write(c == null ? '_' : c);
        }

        private void DrawSeparator()
        {
            Console.Write('|');
        }
    }
}
