using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public float explosive;
        float totalChonk;
        public bool speed;
        Color chonkColor;
        public Sprite chonkImage;
        public Coordinate(Sprite I, Vector2 P, int s, float c, float e, bool sp, Texture2D ci = null)
        {
            image = I;
            place = P;
            score = s;
            chonker = c;
            isfull = false;
            chonkColor = I.Color;
            explosive = e;
            speed = sp;
            totalChonk = c;
            if (ci != null)
            {
                chonkImage = new Sprite(ci, image.Location, Color.White, 0, SpriteEffects.None, new Vector2(ci.Width / 2, ci.Height / 2), image.Scale, image.Depth + .01f);
            }
            // - image.Origin * image.Scale - new Vector2(ci.Width, ci.Height) * image.Scale
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
            if (chonkImage == null)
            {
                image.Color = Color.FromNonPremultiplied((int)(chonkColor.R * ((totalChonk - chonker) / totalChonk)), (int)(chonkColor.G * ((totalChonk - chonker) / totalChonk)), (int)(chonkColor.B * ((totalChonk - chonker) / totalChonk)), 255);
            }
            else
            {
                chonkImage.Color = Color.FromNonPremultiplied(chonkImage.Color.R, chonkImage.Color.G, chonkImage.Color.B, (int)(chonkImage.Color.A * ((float)chonker) / (float)totalChonk));
            }
        }
        public void fill(Coordinate pooey)
        {
            image.Image = pooey.image.Image;
            chonkImage = pooey.chonkImage;
            image.Color = pooey.image.Color;
            chonkColor = pooey.chonkColor;
            score = pooey.score;
            image.Depth = pooey.image.Depth;
            totalChonk = pooey.chonker;
            chonker = pooey.chonker;
            explosive = pooey.explosive;
            speed = pooey.speed;
            Animate(Chonker());
            isfull = true;
        }

        public List<Vector2> Explode(List<List<Coordinate>> coords)
        {
            float top = place.Y - explosive - 1;
            if (top < 0)
            {
                top = 0;
            }
            float bottom = place.Y + explosive + 1;
            if (bottom > coords[0].Count)
            {
                bottom = coords[0].Count;
            }
            float left = place.X - explosive - 1;
            if (left < 0)
            {
                left = 0;
            }
            float right = place.X + explosive + 1;
            if (right > coords.Count)
            {
                right = coords.Count;
            }
            List<Vector2> spots = new List<Vector2>();
            for (int i = (int)left; i < right; i++)
            {
                for (int j = (int)top; j < bottom; j++)
                {
                    if (Vector2.Distance(new Vector2(i, j), place) <= explosive + .01f)
                    {
                        if (coords[i][j].chonker > 0)
                        {
                            for (int e = 0; e < Vector2.Distance(new Vector2(i, j), place) + .01f; e++)
                            {
                                spots.Add(new Vector2(i, j));
                            }
                        }  
                        else
                        {
                            spots.Add(new Vector2(i, j));
                        }
                    }
                }
            }
            return spots;
        }
        public void Animate(bool testBool = false)
        {
            if (Chonker() && chonkImage != null)
            {
                chonkImage.Location = image.Location;
            }
            if (explosive == 2)
            {
                image.Pulsate(20, .05f);
            }
            else if (explosive == 3)
            {
                image.Pulsate(35, .1f);
                image.Vibrate(12, .1f);
            }
        }
        public void empty(Sprite empty)
        {
            image = new Sprite(empty.Image, image.Location, empty.Color, empty.rotation, empty.effect, empty.Origin, image.Scale, empty.Depth);
            chonkImage = null;
            isfull = false;
            explosive = 0;
            chonker = 0;
            speed = false;
        }
        public void Draw(SpriteBatch bath)
        {
            image.Draw(bath);
            if (Chonker() && chonkImage != null)
            {
                chonkImage.Draw(bath);
            }
        }
    }
}
