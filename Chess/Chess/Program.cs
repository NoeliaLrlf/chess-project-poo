using Chess.Pieces;
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

        private static (Player, Player) menuPlayer() {
           
            Console.WriteLine(" Player 1");
            Console.WriteLine("==========");
            Console.WriteLine("Enter Name for the Player 1 ");
            string name = Console.ReadLine();
            Console.WriteLine("Choose the colur for the player1 ");
            Console.WriteLine("1. Blue");
            Console.WriteLine("2. White");
            int option = ReadInt();
            Color color =Color.Black;
            switch (option)
            {
                case 1:
                    color =Color.Blue;
                    break;
                case 2:
                    color = Color.White;
                    break;

                default:
                    Console.WriteLine("Choices again the option:");
                    break;
            }
            Player player1 = new Player(name, color);

            Console.WriteLine(" Player 2");
            Console.WriteLine("==========");
            Console.WriteLine("Enter Name for the Player 2 ");
            string name2 = Console.ReadLine();
            Console.WriteLine("Choose the colur for the player2");
            Console.WriteLine("1. Blue");
            Console.WriteLine("2. White");
            int option2 = ReadInt();
            Color color2 = Color.Black;
            switch (option2)
            {
                case 1:
                    color2 = Color.Blue;
                    break;
                case 2:
                    color2 = Color.White;
                    break;

                default:
                    Console.WriteLine("Choices again the option:");
                    break;
            }
            Player player2 = new Player(name2, color2);

            return (player1, player2);



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
                 (Player,Player) players= menuPlayer();
                ChessGame chessGame = new ChessGame();
                chessGame.StartGame(players.Item1,players.Item2) ;

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

                        Position origin = PrintBoard.ReadChessPosition(s, chessGame, players).IniPosition();
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
                        Position destination = PrintBoard.ReadChessPosition(s, chessGame, players).IniPosition();
                        chessGame.ValidadeDestinationPosition(origin, destination);

                        chessGame.InitPlay(origin, destination);

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