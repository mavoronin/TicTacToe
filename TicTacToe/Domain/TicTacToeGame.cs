using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TicTacToe.Domain
{
    internal enum GameCharacter
    {
        X,
        O
    }

    internal class TicTacToeGame : INotifyPropertyChanged
    {
        public GameCharacter?[,] Field { get; private set; }

        private GameCharacter? _winner;
        public GameCharacter? Winner
        {
            get
            {
                return _winner;
            }
            private set
            {
                if (value != _winner)
                {
                    _winner = value;
                    OnPropertyChanged(nameof(Winner));
                }
            }
        }

        private GameCharacter? _currentMoveCharacter;
        public GameCharacter? CurrentMoveCharacter
        {
            get
            {
                return _currentMoveCharacter;
            }
            private set
            {
                if (value != _currentMoveCharacter)
                {
                    _currentMoveCharacter = value;
                    OnPropertyChanged(nameof(CurrentMoveCharacter));
                }
            }
        }
        public int Size { get; }

        public TicTacToeGame(int size)
        {
            Size = size;
            Field = new GameCharacter?[size, size];
        }

        public void Init()
        {
            CurrentMoveCharacter = (GameCharacter)RandomHelper.Rnd.Next(2);
        }

        public bool MakeMove(Point point)
        {
            if (!IsMoveValid(point)) return false;

            Field[point.X, point.Y] = CurrentMoveCharacter;

            var gameFinished = GameIsFinished(point, out var winner);
            if (gameFinished)
            {
                Winner = winner;
            }
            else
            {
                SwitchCharacter();
            }

            return true;
        }

        private bool IsMoveValid(Point point)
        {
            if (point.X < 0 || point.Y < 0) return false;
            if (point.X > Size - 1 || point.Y > Size - 1) return false;

            if (Field[point.X, point.Y] != null) return false;

            return true;
        }

        private void SwitchCharacter()
        {
            CurrentMoveCharacter = CurrentMoveCharacter == GameCharacter.X ? GameCharacter.O : GameCharacter.X;
        }

        private bool GameIsFinished(Point point, out GameCharacter? winner)
        {
            winner = default;

            // row
            for (int i = 0; i < Size; i++)
            {
                if (Field[point.X, i] != CurrentMoveCharacter) break;

                if (i == Size - 1)
                {
                    winner = CurrentMoveCharacter;
                    return true;
                }
            }

            // column
            for (int i = 0; i < Size; i++)
            {
                if (Field[i, point.Y] !=CurrentMoveCharacter) break;

                if (i == Size - 1)
                {
                    winner = CurrentMoveCharacter;
                    return true;
                }
            }

            // diagonal
            // main
            if (point.X == point.Y)
            {
                for (int i = 0; i < Size; i++)
                {
                    if (Field[i, i] != CurrentMoveCharacter) break;

                    if (i == Size - 1)
                    {
                        winner = CurrentMoveCharacter;
                        return true;
                    }
                }
            }

            // secondary
            if (point.X == Size - point.Y)
            {
                for (int i = 0; i < Size; i++)
                {
                    if (Field[i, Size - 1 - i] != CurrentMoveCharacter) break;

                    if (i == Size - 1)
                    {
                        winner = CurrentMoveCharacter;
                        return true;
                    }
                }
            }

            return false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
