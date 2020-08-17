﻿namespace Chess.Pieces
{
    internal class Horse : Piece
    {
        public Horse(Board brd, Color color) : base(color, brd)
        {
        }

        public override bool[,] PossibleMovements()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return "N";
        }
    }
}