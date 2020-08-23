using System;
namespace Chess
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                ChessGame chessGame = new ChessGame();

                while (!chessGame.Finished)
                {
                    try
                    {
                        Console.Clear();
                        PrintBoard.Display(chessGame);

                        Console.WriteLine("");
                        Console.WriteLine("Write an Origin or press q to quit.");
                        Console.Write("Origin: ");
                        Position origin = PrintBoard.ReadChessPosition().ToPosition();
                        chessGame.ValidateOriginPosition(origin);

                        bool[,] possiblePositions = chessGame.BoardGame.Piece(origin).PossibleMovements();
                        Console.Clear();
                        PrintBoard.Print(chessGame.BoardGame, possiblePositions);

                        Console.WriteLine("");
                        Console.Write("Destination: ");
                        Position destination = PrintBoard.ReadChessPosition().ToPosition();
                        chessGame.ValidadeDestinationPosition(origin, destination);

                        chessGame.MakeThePlay(origin, destination);
                    }

                    catch (BoardException e)
                    {
                        Console.Write(e.Message);
                        Console.ReadLine();
                    }

                    catch (ApplicationException)
                    {
                        chessGame.Quit = true;
                        break;
                    }

                }

                Console.Clear();
                PrintBoard.Display(chessGame);

            }

            catch (BoardException e)
            {
                Console.Write(e.Message);
            }
        }
    }
}