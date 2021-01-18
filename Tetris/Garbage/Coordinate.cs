using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class Coordinate
    {
        public Sprite image;
        public Vector2 place;
        public bool isfull;
        public int score;
        public Coordinate(Sprite I, Vector2 P, int s)
        {
            image = I;
            place = P;
            score = s;
            isfull = false;
        }
        public void fill(RatPooeys pooey)
        {
            image.Image = pooey.image.Image;
            image.Color = pooey.image.Color;
            score = pooey.score;
            isfull = true;
        }
        public void empty(Sprite empty)
        {
            image = new Sprite(empty.Image, empty.Location, empty.Color, empty.Rotation, empty.Effects, empty.Origin, empty.Scale, empty.Depth);
            isfull = false;
        }
    }
}
