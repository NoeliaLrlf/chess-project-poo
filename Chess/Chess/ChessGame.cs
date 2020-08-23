using Chess.Pieces;
using System.Collections.Generic;

namespace Chess
{
    internal class ChessGame
    {
        public Board BoardGame { get; private set; }

        public Color ColorGame { get; private set; }

        public bool Check { get; private set; }

        private HashSet<Piece> Pieces;
        private HashSet<Piece> Captured;

        public ChessGame()
        {
            BoardGame = new Board(8, 8);

            ColorGame = Color.White;

            Pieces = new HashSet<Piece>();
            Captured = new HashSet<Piece>();
            MountBoard();
        }

        public void PutNewPiece(char column, int row, Piece piece)
        {
            BoardGame.PutPiece(piece, new ChessPosition(column, row).ToPosition());
            Pieces.Add(piece);
        }

        public HashSet<Piece> InGamePieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in Pieces)
            {
                if (x.Color == color)
                {
                    aux.Add(x);
                }
            }
          //  aux.ExceptWith(CapturedPieces(color));
            return aux;
        }

        private Color Opponent(Color color)
        {
            if (color == Color.White)
            {
                return Color.Black;
            }
            return Color.White;
        }

        private Piece King(Color color)
        {
            foreach (Piece p in InGamePieces(color))
            {
                if (p is King)
                {
                    return p;
                }
            }
            return null;
        }

        public bool IsItChecked(Color color)
        {
            Piece k = King(color);
            if (k == null)
            {
                throw new BoardException("There is no " + color + "King on the board!");
            }

            foreach (Piece p in InGamePieces(Opponent(color)))
            {
                bool[,] mat = p.PossibleMovements();
                if (mat[k.Position.Row, k.Position.Column])
                {
                    return true;
                }
            }
            return false;
        }

        public void MountBoard()
        {
            PutNewPiece('a', 1, new Rook(BoardGame, Color.White));
            PutNewPiece('b', 1, new Horse(BoardGame, Color.White));
            PutNewPiece('c', 1, new Bishop(BoardGame, Color.White));
            PutNewPiece('d', 1, new Queen(BoardGame, Color.White));
            PutNewPiece('e', 1, new King(BoardGame, Color.White, this));
            PutNewPiece('f', 1, new Bishop(BoardGame, Color.White));
            PutNewPiece('g', 1, new Horse(BoardGame, Color.White));
            PutNewPiece('h', 1, new Rook(BoardGame, Color.White));
            PutNewPiece('a', 2, new Pawn(BoardGame, Color.White, this));
            PutNewPiece('b', 2, new Pawn(BoardGame, Color.White, this));
            PutNewPiece('c', 2, new Pawn(BoardGame, Color.White, this));
            PutNewPiece('d', 2, new Pawn(BoardGame, Color.White, this));
            PutNewPiece('e', 2, new Pawn(BoardGame, Color.White, this));
            PutNewPiece('f', 2, new Pawn(BoardGame, Color.White, this));
            PutNewPiece('g', 2, new Pawn(BoardGame, Color.White, this));
            PutNewPiece('h', 2, new Pawn(BoardGame, Color.White, this));

            PutNewPiece('a', 8, new Rook(BoardGame, Color.Black));
            PutNewPiece('b', 8, new Horse(BoardGame, Color.Black));
            PutNewPiece('c', 8, new Bishop(BoardGame, Color.Black));
            PutNewPiece('d', 8, new Queen(BoardGame, Color.Black));
            PutNewPiece('e', 8, new King(BoardGame, Color.Black, this));
            PutNewPiece('f', 8, new Bishop(BoardGame, Color.Black));
            PutNewPiece('g', 8, new Horse(BoardGame, Color.Black));
            PutNewPiece('h', 8, new Rook(BoardGame, Color.Black));
            PutNewPiece('a', 7, new Pawn(BoardGame, Color.Black, this));
            PutNewPiece('b', 7, new Pawn(BoardGame, Color.Black, this));
            PutNewPiece('c', 7, new Pawn(BoardGame, Color.Black, this));
            PutNewPiece('d', 7, new Pawn(BoardGame, Color.Black, this));
            PutNewPiece('e', 7, new Pawn(BoardGame, Color.Black, this));
            PutNewPiece('f', 7, new Pawn(BoardGame, Color.Black, this));
            PutNewPiece('g', 7, new Pawn(BoardGame, Color.Black, this));
            PutNewPiece('h', 7, new Pawn(BoardGame, Color.Black, this));
        }
    }
}