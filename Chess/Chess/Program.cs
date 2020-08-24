using System;
namespace Chess
{

    internal class Program
    {
        private static string Action { get; set; } = "Start Game";
        private static void Main(string[] args)
        {
            ProgramGame();
        }
        private static void Menu()
        {
            Console.WriteLine("Welcome to the Game Press a number");
            Console.WriteLine("===================================");
            Console.WriteLine($"1. {Action}");
            Console.WriteLine("2. exit");

        }

        private static int ReadInt()
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Wrong input! Enter number again:");
            }
            return result;
        }
        public static void ProgramGame()
        {

            bool exit = false;
            while (!exit)
            {
                Menu();

                int option = ReadInt();

                switch (option)
                {
                    case 1:
                        StartGame();
                        break;
                    case 2:
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Choices again the option:");
                        break;
                }

            }

        }
        public static void StartGame()
        {
            try
            {
                ChessGame chessGame = new ChessGame();
                chessGame.StartGame();

                while (!chessGame.Finished)
                {
                    try
                    {
                        Console.Clear();
                        PrintBoard.Display(chessGame);

                        Console.WriteLine("");
                        Console.WriteLine($"Write an Origin or press:" +
                            $"---> m to Menu.");
                        Console.Write("Origin: ");
                        string s = Console.ReadLine();

                        if (s == "m")
                        {
                            Action = "Reset Game";
                            break;
                        }

                        Position origin = PrintBoard.ReadChessPosition(s, chessGame).ToPosition();
                        chessGame.ValidateOriginPosition(origin);

                        bool[,] possiblePositions = chessGame.BoardGame.Piece(origin).PossibleMovements();
                        Console.Clear();
                        PrintBoard.Print(chessGame.BoardGame, possiblePositions);

                        Console.WriteLine("");
                        Console.Write("Destination: ");
                        s = Console.ReadLine();

                        if (s == "m")
                        {
                            Action = "Reset Game";
                            break;
                        }
                        Position destination = PrintBoard.ReadChessPosition(s, chessGame).ToPosition();
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