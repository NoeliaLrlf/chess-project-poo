using Chess.Pieces;
using System.Collections.Generic;

namespace Chess
{
    internal class ChessGame
    {
        public Board BoardGame { get; private set; }
        public Color ColorGamePlayer { get; private set; }
        public int Turn { get; private set; }
        public bool Check { get; private set; }
        public Piece VulnerableEnPassant { get; private set; }
        public bool Finished { get; private set; }
        public bool Quit { get; set; }

        private HashSet<Piece> Pieces;
        private HashSet<Piece> Captured;

        public ChessGame()
        {
            BoardGame = new Board(8, 8);
            Turn = 1;
            ColorGamePlayer = Color.White;
            Finished = false;
            Quit = false;
            Check = false;
            VulnerableEnPassant = null;
            Pieces = new HashSet<Piece>();
            Captured = new HashSet<Piece>();
            MountBoard();
        }
        public void MakeThePlay(Position origin, Position destination)
        {
            Piece CapturedPiece = RunMovement(origin, destination);

            if (IsItChecked(ColorGamePlayer))
            {
                UndoMovement(origin, destination, CapturedPiece);
                throw new BoardException("You Can't put yourself in check!");
            }

            Piece p = BoardGame.Piece(destination);

            //#Special Play: Promotion
            if (p is Pawn)
            {
                if (p.Color == Color.White && destination.Row == 0 || p.Color == Color.Black && destination.Row == 7)
                {
                    p = BoardGame.RemovePiece(destination);
                    Pieces.Remove(p);
                    Piece queen = new Queen(BoardGame, p.Color);
                    BoardGame.PutPiece(queen, destination);
                    Pieces.Add(queen);
                }
            }


            if (IsItChecked(Opponent(ColorGamePlayer)))
            {
                Check = true;
            }
            else
            {
                Check = false;
            }

            if (CheckmateTest(Opponent(ColorGamePlayer)))
            {
                Finished = true;
            }
            else
            {
                Turn++;
                ChangePlayer();
            }

            //#Special Play: En Passant
            if (p is Pawn && destination.Row == origin.Row - 2 || destination.Row == origin.Row + 2)
            {
                VulnerableEnPassant = p;
            }
            else
            {
                VulnerableEnPassant = null;
            }

        }
        public bool CheckmateTest(Color color)
        {
            if (!IsItChecked(color))
            {
                return false;
            }
            foreach (Piece p in InGamePieces(color))
            {
                bool[,] mat = p.PossibleMovements();
                for (int i = 0; i < BoardGame.Rows; i++)
                {
                    for (int j = 0; j < BoardGame.Columns; j++)
                    {
                        if (mat[i, j])
                        {
                            Position origin = p.Position;
                            Position destination = new Position(i, j);
                            Piece capturedPiece = RunMovement(origin, destination);
                            bool checkTest = IsItChecked(color);
                            UndoMovement(origin, destination, capturedPiece);
                            if (!checkTest)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public HashSet<Piece> CapturedPieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in Captured)
            {
                if (x.Color == color)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }
        private void ChangePlayer()
        {
            if (ColorGamePlayer == Color.White)
            {
                ColorGamePlayer = Color.Blue;
            }
            else
            {
                ColorGamePlayer = Color.White;
            }
        }
        public Piece RunMovement(Position origin, Position destination)
        {
            Piece P = BoardGame.RemovePiece(origin);
            P.IncreaseMovementsQty();
            Piece CapturedPiece = BoardGame.RemovePiece(destination);
            BoardGame.PutPiece(P, destination);
            if (CapturedPiece != null)
            {
                Captured.Add(CapturedPiece);
            }





            if (P is King && destination.Column == origin.Column + 2)
            {
                Position originR = new Position(origin.Row, origin.Column + 3);
                Position destinationR = new Position(origin.Row, origin.Column + 1);
                Piece R = BoardGame.RemovePiece(originR);
                R.IncreaseMovementsQty();
                BoardGame.PutPiece(R, destinationR);
            }





            if (P is King && destination.Column == origin.Column - 2)
            {
                Position originR = new Position(origin.Row, origin.Column - 4);
                Position destinationR = new Position(origin.Row, origin.Column - 1);
                Piece R = BoardGame.RemovePiece(originR);
                R.IncreaseMovementsQty();
                BoardGame.PutPiece(R, destinationR);
            }

         


            if (P is Pawn)
            {
                if (origin.Column != destination.Column && CapturedPiece == null)
                {
                    Position posP;
                    if (P.Color == Color.White)
                    {
                        posP = new Position(destination.Row + 1, destination.Column);
                    }
                    else
                    {
                        posP = new Position(destination.Row - 1, destination.Column);
                    }
                    CapturedPiece = BoardGame.RemovePiece(posP);
                    Captured.Add(CapturedPiece);
                }
            }

            return CapturedPiece;
        }
        public void UndoMovement(Position origin, Position destination, Piece capturedPiece)
        {
            Piece P = BoardGame.RemovePiece(destination);
            P.DecreaseMovementsQty();
            if (capturedPiece != null)
            {
                BoardGame.PutPiece(capturedPiece, destination);
                Captured.Remove(capturedPiece);
            }
            BoardGame.PutPiece(P, origin);

          


            if (P is King && destination.Column == origin.Column + 2)
            {
                Position originR = new Position(origin.Row, origin.Column + 3);
                Position destinationR = new Position(origin.Row, origin.Column + 1);
                Piece R = BoardGame.RemovePiece(destinationR);
                R.DecreaseMovementsQty();
                BoardGame.PutPiece(R, originR);
            }




            if (P is King && destination.Column == origin.Column - 2)
            {
                Position originR = new Position(origin.Row, origin.Column - 4);
                Position destinationR = new Position(origin.Row, origin.Column - 1);
                Piece R = BoardGame.RemovePiece(destinationR);
                R.DecreaseMovementsQty();
                BoardGame.PutPiece(R, originR);
            }




            if (P is Pawn)
            {
                if (origin.Column != destination.Column && capturedPiece == VulnerableEnPassant)
                {
                    Piece pawn = BoardGame.RemovePiece(destination);
                    Position posP;
                    if (P.Color == Color.White)
                    {
                        posP = new Position(3, destination.Column);
                    }
                    else
                    {
                        posP = new Position(4, destination.Column);
                    }
                    BoardGame.PutPiece(pawn, posP);
                }

            }

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
           aux.ExceptWith(CapturedPieces(color));
            return aux;
        }
        private Color Opponent(Color color)
        {
            if (color == Color.White)
            {
                return Color.Blue;
            }
            return Color.White;
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
        public void ValidateOriginPosition(Position pos)
        {
            if (BoardGame.Piece(pos) == null)
            {
                throw new BoardException("There is no piece in the chosen origin position!");
            }

            if (ColorGamePlayer != BoardGame.Piece(pos).Color)
            {
                throw new BoardException("The chosen piece is not yours!");
            }

            if (!BoardGame.Piece(pos).IsTherePossibleMovements())
            {
                throw new BoardException("There is no possible movements for the chosen piece!");
            }

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
        public void ValidadeDestinationPosition(Position origin, Position destination)
        {
            if (!BoardGame.Piece(origin).PossibleMovement(destination))
            {
                throw new BoardException("Invalid destination position!");
            }
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

            PutNewPiece('a', 8, new Rook(BoardGame, Color.Blue));
            PutNewPiece('b', 8, new Horse(BoardGame, Color.Blue));
            PutNewPiece('c', 8, new Bishop(BoardGame, Color.Blue));
            PutNewPiece('d', 8, new Queen(BoardGame, Color.Blue));
            PutNewPiece('e', 8, new King(BoardGame, Color.Blue, this));
            PutNewPiece('f', 8, new Bishop(BoardGame, Color.Blue));
            PutNewPiece('g', 8, new Horse(BoardGame, Color.Blue));
            PutNewPiece('h', 8, new Rook(BoardGame, Color.Blue));
            PutNewPiece('a', 7, new Pawn(BoardGame, Color.Blue, this));
            PutNewPiece('b', 7, new Pawn(BoardGame, Color.Blue, this));
            PutNewPiece('c', 7, new Pawn(BoardGame, Color.Blue, this));
            PutNewPiece('d', 7, new Pawn(BoardGame, Color.Blue, this));
            PutNewPiece('e', 7, new Pawn(BoardGame, Color.Blue, this));
            PutNewPiece('f', 7, new Pawn(BoardGame, Color.Blue, this));
            PutNewPiece('g', 7, new Pawn(BoardGame, Color.Blue, this));
            PutNewPiece('h', 7, new Pawn(BoardGame, Color.Blue, this));
        }
    }
}