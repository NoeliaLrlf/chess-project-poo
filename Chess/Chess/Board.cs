using Chess.Pieces;

namespace Chess
{
    internal class Board
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        private Piece[,] Pieces;

        public Board(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Pieces = new Piece[rows, columns];
        }

        public Piece Piece(int row, int column)
        {
            return Pieces[row, column];
        }

        public Piece Piece(Position pos)
        {
            return Pieces[pos.Row, pos.Column];
        }

        public void PutPiece(Piece p, Position pos)
        {
            Pieces[pos.Row, pos.Column] = p;
            p.Position = pos;
        }

        public bool IsValidPosition(Position pos)
        {
            if (pos.Row < 0 || pos.Row >= Rows || pos.Column < 0 || pos.Column >= Columns)
            {
                return false;
            }

            return true;
        }
        public Piece RemovePiece(Position pos)
        {
            if (Piece(pos) == null)
            {
                return null;
            }
            Piece aux = Piece(pos);
            aux.Position = null;
            Pieces[pos.Row, pos.Column] = null;
            return aux;
        }
        public bool IsTherePiece(Position pos)
        {
            ValidatePosition(pos);
            return Piece(pos) != null;
        }
        public void ValidatePosition(Position pos)
        {
            if (!IsValidPosition(pos))
            {
                throw new BoardException("Invalid position!");
            }
        }
    }
}