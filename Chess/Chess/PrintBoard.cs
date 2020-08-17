using Chess.Pieces;
using System;

namespace Chess
{
    internal class PrintBoard
    {
        public static void Display(ChessGame chessGame)
        {
            Print(chessGame.BoardGame);
        }

        public static void Print(Board brd)
        {
            for (int i = 0; i < brd.Columns; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < brd.Rows; j++)
                {
                    PrintPiece(brd.Piece(i, j));
                }

                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void PrintPiece(Piece piece)
        {
            if (piece == null)
            {
                Console.Write("- ");
            }
            else
            {
                if (piece.Color == Color.White)
                {
                    Console.Write(piece);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(piece);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }
    }
}