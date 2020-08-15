namespace Chess
{
    public abstract class Piece
    {
        protected string Color;

        protected abstract void Move();

        protected abstract bool CanMove();

        protected abstract void GetLocation();

        protected abstract void SetLocation();
    }
}