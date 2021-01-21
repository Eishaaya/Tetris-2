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
        public float chonker;
        float totalChonk;
        Color chonkColor;        
        //public Coordinate(Sprite I, Vector2 P, int s, Color cc)
        //{
        //    Coordinate(I, P, s);
        //    chonkColor = Color.Black;
        //}
        public Coordinate(Sprite I, Vector2 P, int s, int c)
        {
            image = I;
            place = P;
            score = s;
            chonker = c;
            isfull = false;
            chonkColor = I.Color;
            if (c > 0)
            {
                image.Color = Color.Black;
            }
            totalChonk = c;
        }
        public bool Chonker()
        {
            if (chonker > 0)
            {
                return true;
            }
            return false;
        }
        public void reduceChonk()
        {
            chonker--;
            image.Color = Color.FromNonPremultiplied((int)(chonkColor.R * ((totalChonk - chonker) / totalChonk)), (int)(chonkColor.G * ((totalChonk - chonker) / totalChonk)), (int)(chonkColor.B * ((totalChonk - chonker) / totalChonk)), 255);
        }
        public void fill(RatPooeys pooey)
        {
            image.Image = pooey.image.Image;
            image.Color = pooey.boxes[0].image.Color;
            chonkColor = pooey.image.Color;
            score = pooey.score;
            isfull = true;
            totalChonk = pooey.boxes[0].chonker;
            chonker = pooey.boxes[0].chonker;
            if (chonker > 0)
            {
                ;
            }    
        }
        public void empty(Sprite empty)
        {            
            image = new Sprite(empty.Image, empty.Location, empty.Color, empty.Rotation, empty.Effects, empty.Origin, empty.Scale, empty.Depth);
            isfull = false;
        }
    }
}
