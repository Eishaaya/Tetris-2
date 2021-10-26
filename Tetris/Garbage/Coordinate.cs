using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class Coordinate
    {
        public enum DisplayStyle
        {
            DrawAbove,
            DrawBelow
        };
        public Sprite Image { get; set; }
        public Vector2 GridSpot
        {
            get;
            set;
        }
        public bool IsFull { get; set; }
        public int Score { get; private set; }

        public float Chonk;
        public float Explosive { get; private set; }
        public bool Speed { get; private set; }

        public float Reppellent { get; private set; }

        Color chonkColor;
        float totalChonk;

        public Sprite SecondaryImage { get; set; }
        public Coordinate(Sprite Image, Vector2 place, int value, float chonkLevel, float explosiveLevel, float repellingLevel, bool isSpeed, Texture2D sImage = null, DisplayStyle displayStyle = DisplayStyle.DrawAbove)
        {
            this.Image = Image;
            GridSpot = place;
            Score = value;
            Chonk = chonkLevel;
            IsFull = false;
            chonkColor = Image.Color;
            Explosive = explosiveLevel;
            Speed = isSpeed;
            totalChonk = chonkLevel;
            if (sImage != null)
            {
                var depthOffset = .01f;
                if (displayStyle == DisplayStyle.DrawBelow)
                {
                    depthOffset *= -1;
                }
                SecondaryImage = new Sprite(sImage, this.Image.Location, Color.White, 0, SpriteEffects.None, new Vector2(sImage.Width / 2, sImage.Height / 2), this.Image.Scale, this.Image.Depth + depthOffset);
            }
        }
        public bool Chonker()
        {
            if (Chonk > 0)
            {
                return true;
            }
            return false;
        }
        public void reduceChonk()
        {
            Chonk--;
            //if (SecondaryImage == null)
            //{
            //    Image.Color = Color.FromNonPremultiplied((int)(chonkColor.R * ((totalChonk - Chonk) / totalChonk)), (int)(chonkColor.G * ((totalChonk - Chonk) / totalChonk)), (int)(chonkColor.B * ((totalChonk - Chonk) / totalChonk)), 255);
            //}

            SecondaryImage.Color = Color.FromNonPremultiplied(SecondaryImage.Color.R, SecondaryImage.Color.G, SecondaryImage.Color.B, (int)(SecondaryImage.Color.A * ((float)Chonk) / (float)totalChonk));
        }
        public void fill(Coordinate pooey)
        {
            Image.Image = pooey.Image.Image;
            SecondaryImage = pooey.SecondaryImage;
            Image.Color = pooey.Image.Color;
            chonkColor = pooey.chonkColor;
            Score = pooey.Score;
            Image.Depth = pooey.Image.Depth;
            totalChonk = pooey.Chonk;
            Chonk = pooey.Chonk;
            Explosive = pooey.Explosive;
            Speed = pooey.Speed;
            Animate(Chonker());
            IsFull = true;
        }

        public void UpdateLinkedImage()
        {
            if (SecondaryImage != null)
            {
                SecondaryImage.Location = Image.Location;
            }
        }
        public List<Vector2> Explode(List<List<Coordinate>> coords)
        {
            float top = GridSpot.Y - Explosive - 1;
            if (top < 0)
            {
                top = 0;
            }
            float bottom = GridSpot.Y + Explosive + 1;
            if (bottom > coords[0].Count)
            {
                bottom = coords[0].Count;
            }
            float left = GridSpot.X - Explosive - 1;
            if (left < 0)
            {
                left = 0;
            }
            float right = GridSpot.X + Explosive + 1;
            if (right > coords.Count)
            {
                right = coords.Count;
            }
            List<Vector2> spots = new List<Vector2>();
            for (int i = (int)left; i < right; i++)
            {
                for (int j = (int)top; j < bottom; j++)
                {
                    if (Vector2.Distance(new Vector2(i, j), GridSpot) <= Explosive + .01f)
                    {
                        if (coords[i][j].Chonk > 0)
                        {
                            for (int e = 0; e < Vector2.Distance(new Vector2(i, j), GridSpot) + .01f; e++)
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
            if (Chonker() && SecondaryImage != null)
            {
                SecondaryImage.Location = Image.Location;
            }
            else if (Explosive == 2)
            {
                Image.Pulsate(20, .05f);
            }
            else if (Explosive == 3)
            {
                Image.Pulsate(35, .1f);
                Image.Vibrate(12, .1f);
            }
            else if (Reppellent == 1)
            {
                SecondaryImage.rotation += 3;
            }
            else if (Reppellent == 2)
            {
                SecondaryImage.Rotate(180, 3, false);
            }
            else if (Reppellent == 3)
            {
                SecondaryImage.Rotate(360, 10, false);
                SecondaryImage.Pulsate(150, 3, false);
            }
        }
        public void empty(Sprite empty)
        {
            Image = new Sprite(empty.Image, Image.Location, empty.Color, empty.rotation, empty.effect, empty.Origin, Image.Scale, empty.Depth);
            SecondaryImage = null;
            IsFull = false;
            Explosive = 0;
            Chonk = 0;
            Speed = false;
        }
        public void Draw(SpriteBatch bath)
        {
            Image.Draw(bath);
            if (Chonker() && SecondaryImage != null || Reppellent > 0)
            {
                SecondaryImage.Draw(bath);
            }
        }
    }
}
