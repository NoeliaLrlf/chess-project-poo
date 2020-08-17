namespace Chess.Pieces
{
    internal class Queen : Piece
    {
        public Queen(Board brd, Color color) : base(color, brd)
        {
        }

        public override bool[,] PossibleMovements()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return "Q";
        }
    }
}