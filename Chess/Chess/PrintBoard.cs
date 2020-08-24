﻿using Chess.Pieces;
using System;
using System.Collections.Generic;

namespace Chess
{
    internal class PrintBoard
    {
        public static void Display(ChessGame chessGame)
        {
            Print(chessGame.BoardGame);
            Console.WriteLine("");
            PrintCapturedPieces(chessGame);
            Console.WriteLine("");
            Console.WriteLine("Turn: " + chessGame.Turn);

            if (!chessGame.Finished)
            {
                if (chessGame.Quit)
                {
                    Console.WriteLine("Thanks for playing Console Chess!");
                }
                else
                {
                    if (chessGame.ColorGamePlayer.Equals(chessGame.player1.ColorPlayer))
                        Console.WriteLine("Waiting play: " + chessGame.player1.NamePlayer);
                    else 
                        Console.WriteLine("Waiting play: " + chessGame.player2.NamePlayer);

                    if (chessGame.Check)
                    {
                        Console.WriteLine("CHECK!");
                    }
                }
            }
            else
            {
                Console.WriteLine("CHECKMATE!");
                Console.WriteLine("Winner: " + chessGame.ColorGamePlayer);
            }

        }

        public static void Print(Board brd)
        {
            Console.WriteLine("Welcome to the Game");
            Console.WriteLine("===================================");

            Console.WriteLine("     a    b    c    d    e    f    g    h");
            for (int i = 0; i < brd.Columns; i++)
            {
                Console.WriteLine("  -----------------------------------------");
                Console.Write(8 - i + " |");
                for (int j = 0; j < brd.Rows; j++)
                {
                    PrintPiece(brd.Piece(i, j));
                    Console.Write(" |");
                }
                Console.WriteLine();          
            }
            Console.WriteLine("  -----------------------------------------");
        }
        public static void Print(Board brd, bool[,] possiblePositions)
        {
            ConsoleColor OriginalBackground = Console.BackgroundColor;
            ConsoleColor ChangedBackground = ConsoleColor.DarkGray;
            Console.WriteLine("Welcome to the Game");
            Console.WriteLine("===================================");

            Console.WriteLine("     a    b    c    d    e    f    g    h");
            for (int i = 0; i < brd.Columns; i++)
            {
                Console.WriteLine("  -----------------------------------------");
                Console.Write(8 - i + " |");
                for (int j = 0; j < brd.Rows; j++)
                {
                    if (possiblePositions[i, j])
                    {
                        Console.BackgroundColor = ChangedBackground;
                    }
                    else
                    {
                        Console.BackgroundColor = OriginalBackground;
                    }
                    PrintPiece(brd.Piece(i, j));
                    Console.BackgroundColor = OriginalBackground;
                    Console.Write(" |");
                }

                Console.WriteLine();
            }
            Console.WriteLine("  -----------------------------------------");
            Console.BackgroundColor = OriginalBackground;
        }

        public static void PrintPiece(Piece piece)
        {
            if (piece == null)
            {
                Console.Write("   ");
            }
            else
            {
                if (piece.Color == Color.White)
                {
                    Console.Write(piece + "W");
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(piece + "B");
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }

        public static ChessPosition ReadChessPosition(string letter, ChessGame chessGame, (Player,Player) players)
        {
            

            if (string.IsNullOrEmpty(letter) || string.IsNullOrWhiteSpace(letter))
            {
                throw new BoardException("You need to type something!");
            }

            if (letter == "q")
            {

                throw new ApplicationException("Thanks for playing Console Chess!");
            }

            if (letter == "r")
            {
                chessGame.StartGame(players.Item1, players.Item2);
                throw new BoardException("You pressed the reset the game");
            }

            if (letter.Length != 2)
            {
                throw new BoardException("You must type a valid position!");
            }

            char column = letter[0];
            bool correctC = char.IsLetter(column);
            bool correctR = int.TryParse(letter[1] + "", out int row);

            if (letter.Length == 2 && !correctC || letter.Length == 2 && !correctR)
            {
                throw new BoardException("You must type a valid position!");
            }

            return new ChessPosition(column, row);

        }

        public static void PrintCapturedPieces(ChessGame match)
        {
            Console.WriteLine("Captured Pieces:");
           
            Console.Write(match.player1.NamePlayer+$"({match.player1.ColorPlayer}): ");
            ConsoleColor aux = Console.ForegroundColor;
            if (match.player1.ColorPlayer.ToString() == "Blue")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }

            PrintHashset(match.CapturedPieces(match.player1.ColorPlayer));

            Console.ForegroundColor = aux;
            Console.WriteLine("");           
            Console.Write(match.player2.NamePlayer + $"({match.player2.ColorPlayer}): ");
            if (match.player2.ColorPlayer.ToString() == "Blue")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            PrintHashset(match.CapturedPieces(match.player2.ColorPlayer));
            Console.ForegroundColor = aux;
            Console.WriteLine("");

        }       
        public static void PrintHashset(HashSet<Piece> set)
        {
            Console.Write("[ ");
            foreach (Piece p in set)
            {
                Console.Write(p + " ");
            }
            Console.Write("]");
        }

    }
}