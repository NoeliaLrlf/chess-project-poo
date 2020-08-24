﻿using Chess.Pieces;
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

        public Player player1;
        public Player player2;

        private HashSet<Piece> Pieces;
        private HashSet<Piece> Captured;

        public ChessGame()
        {
            
        }

        public void StartGame(Player player1, Player player2)
        {
            BoardGame = new Board(8, 8);
            Turn = 1;
            this.player1= player1;
            this.player2 = player2;
            ColorGamePlayer = player1.ColorPlayer;
            Finished = false;
            Quit = false;
            Check = false;
            VulnerableEnPassant = null;
            Pieces = new HashSet<Piece>();
            Captured = new HashSet<Piece>();
            FillBoard();
        }
        public void InitPlay(Position origin, Position destination)
        {
            Piece CapturedPiece = RunMovement(origin, destination);

            if (IsInTheBoardChecked(ColorGamePlayer))
            {
                UndoMovement(origin, destination, CapturedPiece);
                throw new BoardException("You Can't put yourself in check!");
            }

            Piece p = BoardGame.Piece(destination);

            //#Special Play: Promotion
            if (p is Pawn)
            {
                if (p.Color == player1.ColorPlayer && destination.Row == 0 || p.Color == Color.Black && destination.Row == 7)
                {
                    p = BoardGame.RemovePiece(destination);
                    Pieces.Remove(p);
                    Piece queen = new Queen(BoardGame, p.Color);
                    BoardGame.FillPiece(queen, destination);
                    Pieces.Add(queen);
                }
            }


            if (IsInTheBoardChecked(Opponent(ColorGamePlayer)))
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
            if (!IsInTheBoardChecked(color))
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
                            bool checkTest = IsInTheBoardChecked(color);
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
            if (ColorGamePlayer == player1.ColorPlayer)
            {
                ColorGamePlayer = player2.ColorPlayer;
            }
            else
            {
                ColorGamePlayer = player1.ColorPlayer;
            }
        }
        public Piece RunMovement(Position origin, Position destination)
        {
            Piece P = BoardGame.RemovePiece(origin);
            P.IncreaseMovementsY();
            Piece CapturedPiece = BoardGame.RemovePiece(destination);
            BoardGame.FillPiece(P, destination);
            if (CapturedPiece != null)
            {
                Captured.Add(CapturedPiece);
            }





            if (P is King && destination.Column == origin.Column + 2)
            {
                Position originR = new Position(origin.Row, origin.Column + 3);
                Position destinationR = new Position(origin.Row, origin.Column + 1);
                Piece R = BoardGame.RemovePiece(originR);
                R.IncreaseMovementsY();
                BoardGame.FillPiece(R, destinationR);
            }





            if (P is King && destination.Column == origin.Column - 2)
            {
                Position originR = new Position(origin.Row, origin.Column - 4);
                Position destinationR = new Position(origin.Row, origin.Column - 1);
                Piece R = BoardGame.RemovePiece(originR);
                R.IncreaseMovementsY();
                BoardGame.FillPiece(R, destinationR);
            }

         


            if (P is Pawn)
            {
                if (origin.Column != destination.Column && CapturedPiece == null)
                {
                    Position posP;
                    if (P.Color == player1.ColorPlayer)
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
            P.DecreaseMovementsY();
            if (capturedPiece != null)
            {
                BoardGame.FillPiece(capturedPiece, destination);
                Captured.Remove(capturedPiece);
            }
            BoardGame.FillPiece(P, origin);

          


            if (P is King && destination.Column == origin.Column + 2)
            {
                Position originR = new Position(origin.Row, origin.Column + 3);
                Position destinationR = new Position(origin.Row, origin.Column + 1);
                Piece R = BoardGame.RemovePiece(destinationR);
                R.DecreaseMovementsY();
                BoardGame.FillPiece(R, originR);
            }




            if (P is King && destination.Column == origin.Column - 2)
            {
                Position originR = new Position(origin.Row, origin.Column - 4);
                Position destinationR = new Position(origin.Row, origin.Column - 1);
                Piece R = BoardGame.RemovePiece(destinationR);
                R.DecreaseMovementsY();
                BoardGame.FillPiece(R, originR);
            }




            if (P is Pawn)
            {
                if (origin.Column != destination.Column && capturedPiece == VulnerableEnPassant)
                {
                    Piece pawn = BoardGame.RemovePiece(destination);
                    Position posP;
                    if (P.Color == player1.ColorPlayer)
                    {
                        posP = new Position(3, destination.Column);
                    }
                    else
                    {
                        posP = new Position(4, destination.Column);
                    }
                    BoardGame.FillPiece(pawn, posP);
                }

            }

        }
        public void FillNewPiece(char column, int row, Piece piece)
        {
            BoardGame.FillPiece(piece, new ChessPosition(column, row).IniPosition());
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
            if (color == player1.ColorPlayer)
            {
                return player2.ColorPlayer;
            }
            return player1.ColorPlayer;
        }
        public bool IsInTheBoardChecked(Color color)
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

            if (!BoardGame.Piece(pos).CanMove())
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
        public void FillBoard()
        {
            FillNewPiece('a', 1, new Rook(BoardGame, player1.ColorPlayer));
            FillNewPiece('b', 1, new Horse(BoardGame, player1.ColorPlayer));
            FillNewPiece('c', 1, new Bishop(BoardGame, player1.ColorPlayer));
            FillNewPiece('d', 1, new Queen(BoardGame, player1.ColorPlayer));
            FillNewPiece('e', 1, new King(BoardGame, player1.ColorPlayer, this));
            FillNewPiece('f', 1, new Bishop(BoardGame, player1.ColorPlayer));
            FillNewPiece('g', 1, new Horse(BoardGame, player1.ColorPlayer));
            FillNewPiece('h', 1, new Rook(BoardGame, player1.ColorPlayer));
            FillNewPiece('a', 2, new Pawn(BoardGame, player1.ColorPlayer, this));
            FillNewPiece('b', 2, new Pawn(BoardGame, player1.ColorPlayer, this));
            FillNewPiece('c', 2, new Pawn(BoardGame, player1.ColorPlayer, this));
            FillNewPiece('d', 2, new Pawn(BoardGame, player1.ColorPlayer, this));
            FillNewPiece('e', 2, new Pawn(BoardGame, player1.ColorPlayer, this));
            FillNewPiece('f', 2, new Pawn(BoardGame, player1.ColorPlayer, this));
            FillNewPiece('g', 2, new Pawn(BoardGame, player1.ColorPlayer, this));
            FillNewPiece('h', 2, new Pawn(BoardGame, player1.ColorPlayer, this));

            FillNewPiece('a', 8, new Rook(BoardGame, player2.ColorPlayer));
            FillNewPiece('b', 8, new Horse(BoardGame, player2.ColorPlayer));
            FillNewPiece('c', 8, new Bishop(BoardGame, player2.ColorPlayer));
            FillNewPiece('d', 8, new Queen(BoardGame, player2.ColorPlayer));
            FillNewPiece('e', 8, new King(BoardGame, player2.ColorPlayer, this));
            FillNewPiece('f', 8, new Bishop(BoardGame, player2.ColorPlayer));
            FillNewPiece('g', 8, new Horse(BoardGame, player2.ColorPlayer));
            FillNewPiece('h', 8, new Rook(BoardGame, player2.ColorPlayer));
            FillNewPiece('a', 7, new Pawn(BoardGame, player2.ColorPlayer, this));
            FillNewPiece('b', 7, new Pawn(BoardGame, player2.ColorPlayer, this));
            FillNewPiece('c', 7, new Pawn(BoardGame, player2.ColorPlayer, this));
            FillNewPiece('d', 7, new Pawn(BoardGame, player2.ColorPlayer, this));
            FillNewPiece('e', 7, new Pawn(BoardGame, player2.ColorPlayer, this));
            FillNewPiece('f', 7, new Pawn(BoardGame, player2.ColorPlayer, this));
            FillNewPiece('g', 7, new Pawn(BoardGame, player2.ColorPlayer, this));
            FillNewPiece('h', 7, new Pawn(BoardGame, player2.ColorPlayer, this));
        }
    }
}