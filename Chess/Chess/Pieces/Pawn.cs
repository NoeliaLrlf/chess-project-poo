namespace Chess.Pieces
{
    internal class Pawn : Piece

    {
        private ChessGame Match;

        public Pawn(Board brd, Color color, ChessGame match) : base(color, brd)
        {
            Match = match;
        }

        public override string ToString()
        {
            return "P";
        }

        public override bool[,] PossibleMovements()
        {
            throw new System.NotImplementedException();
        }
    }
}