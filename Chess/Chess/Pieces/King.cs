namespace Chess.Pieces
{
    internal class King : Piece
    {
        private ChessGame chessGame;

        public King(Board brd, Color color, ChessGame match) : base(color, brd)
        {
            chessGame = match;
        }

        public override bool[,] PossibleMovements()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return "K";
        }
    }
}