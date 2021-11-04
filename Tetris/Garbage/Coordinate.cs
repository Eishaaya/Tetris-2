using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    struct PushController
    {
        double angleFromPush;
        int distanceToPush;
        int distanceTravelled;
        public bool IsPushing { get; set; }

        public static PushController None()
        {
            return new PushController(0, 0, false);
        }

        public PushController(double pushAngle, int pushDistance, bool push = true)
        {
            angleFromPush = pushAngle;
            distanceToPush = pushDistance;
            distanceTravelled = 0;
            IsPushing = push;
        }

        public PushController(PushController hitter)
        {
            angleFromPush = hitter.angleFromPush;
            distanceToPush = 1;
            distanceTravelled = 0;
            IsPushing = true;
        }

        public bool CanMove(int x, int y, List<List<Coordinate>> coords)
        {
            bool emptySpace = false;
            while (emptySpace == false)
            {
                x += (int)Math.Round(Math.Cos(angleFromPush));
                y += (int)Math.Round(Math.Sin(angleFromPush));
                if (x < 0 || x >= coords.Count || y < 0 || y >= coords[x].Count)
                {
                    return false;
                }
                if (!coords[x][y].IsFull)
                {
                    emptySpace = true;
                }
            }
            return true;
        }

        public Tuple<int, int> GetNewSpot(int x, int y)
        {
            x += (int)Math.Round(Math.Cos(angleFromPush));
            y += (int)Math.Round(Math.Sin(angleFromPush));
            distanceTravelled++;

            if (distanceTravelled == distanceToPush)
            {
                IsPushing = false;
            }

            return new Tuple<int, int>(x, y);
        }
    }

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

        public PushController Pusher { get; set; }

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
            Reppellent = repellingLevel;
            if (sImage != null)
            {
                var depthOffset = .01f;
                if (displayStyle == DisplayStyle.DrawBelow)
                {
                    depthOffset *= -1;
                }
                SecondaryImage = new Sprite(sImage, this.Image.Location, Color.White, 0, SpriteEffects.None, new Vector2(sImage.Width / 2, sImage.Height / 2), this.Image.Scale, this.Image.Depth + depthOffset);
            }

            Pusher = PushController.None();
        }

        public static Coordinate Clone(Coordinate coord)
        {
            Coordinate newCoord = new Coordinate(coord.Image, coord.GridSpot, coord.Score, coord.totalChonk, coord.Explosive, coord.Reppellent, coord.Speed, null);
            newCoord.SecondaryImage = coord.SecondaryImage;
            newCoord.Chonk = coord.Chonk;
            newCoord.IsFull = coord.IsFull;

            return newCoord;
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
            Reppellent = pooey.Reppellent;
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
                    var distance = Vector2.Distance(new Vector2(i, j), GridSpot);
                    if (distance <= Explosive + .01f)
                    {
                        if (coords[i][j].Chonk > 0)
                        {
                            for (int e = 0; e < distance + 1.01f; e++)
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

        public void Repel(List<List<Coordinate>> coords)
        {
            float top = GridSpot.Y - Reppellent - 1;
            if (top < 0)
            {
                top = 0;
            }
            float bottom = GridSpot.Y + Reppellent + 1;
            if (bottom > coords[0].Count)
            {
                bottom = coords[0].Count;
            }
            float left = GridSpot.X - Reppellent - 1;
            if (left < 0)
            {
                left = 0;
            }
            float right = GridSpot.X + Reppellent + 1;
            if (right > coords.Count)
            {
                right = coords.Count;
            }


            for (int i = (int)left; i < right; i++)
            {
                for (int j = (int)top; j < bottom; j++)
                {
                    if (coords[i][j].IsFull)
                    {
                        var distance = Vector2.Distance(new Vector2(i, j), GridSpot);
                        if (distance <= Reppellent + .01f)
                        {
                            var angle = Math.Atan2(j - GridSpot.Y, i - GridSpot.X);
                            coords[i][j].Pusher = new PushController(angle, (int)Math.Round(distance));
                        }
                    }
                }
            }
        }
        public bool Animate(bool testBool = false)
        {
            if (SecondaryImage != null)
            {
                SecondaryImage.Location = Image.Location;
                if (Reppellent == 1)
                {
                    SecondaryImage.rotation += MathHelper.ToRadians(1);
                }
                else if (Reppellent == 2)
                {
                    SecondaryImage.Rotate(-180, .1f, false);
                }
                else if (Reppellent == 3)
                {
                    SecondaryImage.Rotate(720, .2f);
                    SecondaryImage.Pulsate(50, .1f, false);
                    // SecondaryImage.Depth = 1;
                }

                if (Extensions.random.Next(2, 100) <= Reppellent)
                {
                    return true;
                }
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

            return false;
        }
        public void empty(Sprite empty)
        {
            Image = new Sprite(empty.Image, Image.Location, empty.Color, empty.rotation, empty.effect, empty.Origin, Image.Scale, empty.Depth);
            SecondaryImage = null;
            IsFull = false;
            Explosive = 0;
            Chonk = 0;
            Speed = false;
            Reppellent = 0;
            Pusher = PushController.None();
        }
        public void Draw(SpriteBatch bath)
        {

            Image.Draw(bath);
            if (SecondaryImage != null)
            {
                SecondaryImage.Draw(bath);
            }
        }
    }
}
