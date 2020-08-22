using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    public class BoardException : Exception
    {
        public BoardException(string message) : base(message)
        {
        }
    }
}
