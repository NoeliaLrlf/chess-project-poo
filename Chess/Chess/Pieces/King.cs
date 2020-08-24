namespace Chess.Pieces
{
    internal class King : Piece, ICastling , ICheck
    {
        private ChessGame chessGame;

        public King(Board brd, Color color, ChessGame match) : base(color, brd)
        {
            chessGame = match;
        }

        private bool CanMove(Position pos)
        {
            Piece p = Board.Piece(pos);
            return p == null || p.Color != Color;
        }

        private bool CastlingTest(Position pos)
        {
            Piece p = Board.Piece(pos);
            return p != null && p is Rook && p.Color == Color && p.MovementsY == 0;
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] mat = new bool[Board.Rows, Board.Columns];
            Position pos = new Position(0, 0);

            
            pos.SetValues(Position.Row - 1, Position.Column);
            if (Board.IsValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Row, pos.Column] = true;
            }

            
            pos.SetValues(Position.Row - 1, Position.Column + 1);
            if (Board.IsValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Row, pos.Column] = true;
            }

           
            pos.SetValues(Position.Row, Position.Column + 1);
            if (Board.IsValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Row, pos.Column] = true;
            }

            
            pos.SetValues(Position.Row + 1, Position.Column + 1);
            if (Board.IsValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Row, pos.Column] = true;
            }

            
            pos.SetValues(Position.Row + 1, Position.Column);
            if (Board.IsValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Row, pos.Column] = true;
            }

            
            pos.SetValues(Position.Row + 1, Position.Column - 1);
            if (Board.IsValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Row, pos.Column] = true;
            }

          
            pos.SetValues(Position.Row, Position.Column - 1);
            if (Board.IsValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Row, pos.Column] = true;
            }

            
            pos.SetValues(Position.Row - 1, Position.Column - 1);
            if (Board.IsValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Row, pos.Column] = true;
            }

            //#Special Castling
            Castling(mat);
            
            return mat;
        }
         public void Castling(bool[,] mat)
        {
                if (MovementsY == 0 && !chessGame.Check)
                {
                    //Castling
                    Position posR1 = new Position(Position.Row, Position.Column + 3);
                    if (CastlingTest(posR1))
                    {
                        Position p1 = new Position(Position.Row, Position.Column + 1);
                        Position p2 = new Position(Position.Row, Position.Column + 2);
                        if (Board.Piece(p1) == null && Board.Piece(p2) == null)
                        {
                            mat[Position.Row, Position.Column + 2] = true;
                        }
                    }

                    //Castling
                    Position posR2 = new Position(Position.Row, Position.Column - 4);
                    if (CastlingTest(posR2))
                    {
                        Position p1 = new Position(Position.Row, Position.Column - 1);
                        Position p2 = new Position(Position.Row, Position.Column - 2);
                        Position p3 = new Position(Position.Row, Position.Column - 3);
                        if (Board.Piece(p1) == null && Board.Piece(p2) == null && Board.Piece(p3) == null)
                        {
                            mat[Position.Row, Position.Column - 2] = true;
                        }
                    }
                }
            }
        public override string ToString()
        {
            return "K";
        }

        public void CheckKing()
        {
            throw new System.NotImplementedException();
        }
    }
}