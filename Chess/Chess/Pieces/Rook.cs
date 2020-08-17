namespace Chess.Pieces
{
    internal class Rook : Piece
    {
        public Rook(Board board, Color color) : base(color, board)
        {
        }

        public override bool[,] PossibleMovements()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return "R";
        }
    }
}