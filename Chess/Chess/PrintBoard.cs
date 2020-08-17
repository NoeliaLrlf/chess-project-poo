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
    }
}