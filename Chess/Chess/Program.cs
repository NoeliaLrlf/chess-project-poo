namespace Chess
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ChessGame chessGame = new ChessGame();
            // Console.Clear();
            PrintBoard.Display(chessGame);
        }
    }
}