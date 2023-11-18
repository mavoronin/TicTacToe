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

        public GameCharacter? Winner { get; private set; }

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

        private bool _finished;
        public bool Finished
        {
            get
            {
                return _finished;
            }
            private set
            {
                if (value != _finished)
                {
                    _finished = value;
                    OnPropertyChanged(nameof(Finished));
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
                Finished = true;
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

            // full field
            bool allCellsFilled = true;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Field[i, j] == null)
                    {
                        allCellsFilled = false;
                        break;
                    }

                }
            }

            if (allCellsFilled) return true;

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
                if (Field[i, point.Y] != CurrentMoveCharacter) break;

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
            if (point.X == Size - 1 - point.Y)
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
