using Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    public class Player
    {
        private string name;
        private Color color;

        public string NamePlayer { get => name; set => name = value; }
        public Color ColorPlayer { get => color; set => color = value; }
        public Player(string name, Color color) {
            this.name = name;
            this.color = color;
        }
    }
}
