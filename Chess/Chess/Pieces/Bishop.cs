namespace Chess.Pieces
{
    internal class Bishop : Piece
    {
        public Bishop(Board board, Color color) : base(color, board)
        {
        }

        public override bool[,] PossibleMovements()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return "B";
        }
    }
}