namespace Chess.Pieces
{
    internal abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; protected set; }
        public int MovementsY { get; protected set; }
        public Board Board { get; protected set; }

        public Piece(Color color, Board board)
        {
            Position = null;
            Color = color;
            Board = board;
            MovementsY = 0;
        }

        public abstract bool[,] PossibleMovements();

        public void IncreaseMovementsY()
        {
            MovementsY++;
        }

        public void DecreaseMovementsY()
        {
            MovementsY--;
        }

        public bool CanMove()
        {
            bool[,] mat = PossibleMovements();
            for (int i = 0; i < Board.Rows; i++)
            {
                for (int j = 0; j < Board.Columns; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool PossibleMovement(Position pos)
        {
            return PossibleMovements()[pos.Row, pos.Column];
        }
    }
}