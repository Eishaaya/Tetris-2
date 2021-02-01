using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class RatPooeys
    {
        public bool goDown;
        public Timer downtime;
        public Sprite image;
        public float rotation;
        public List<Coordinate> boxes;
        List<Vector2> locations;
        bool symmetry;
        float scale;
        Vector2 dimensions;
        public int sideways;
        public bool rotated;
        int prepsize;
        public int score;
        Vector2 pieceSize;
        List<Vector2> spots;
        int chonkValue;
        float explosive;
        Random random;
        public RatPooeys(RatPooeys old)
        {
            rotated = false;
            sideways = 0;
            scale = old.scale;
            downtime = old.downtime;
            locations = new List<Vector2>();
            boxes = new List<Coordinate>();
            symmetry = old.symmetry;
            image = old.image;
            rotation = old.rotation;
            prepsize = old.prepsize;
            pieceSize = old.pieceSize;
            dimensions = old.dimensions;
            boxes = old.boxes;
        }
        public RatPooeys(Sprite im, List<Vector2> spots, Vector2 widthHeight, Color c, int se, float sc = 1, bool s = false, int t = 650, int ch = 0, float ex = 0, Texture2D exImage = null, int o = 6, int d1 = 10, int d2 = 26, int p = 6, float r = 0)
        {
            random = new Random();
            chonkValue = ch;
            rotated = false;
            sideways = 0;
            scale = sc;
            downtime = new Timer(new TimeSpan(0, 0, 0, 0, t));
            locations = new List<Vector2>();
            boxes = new List<Coordinate>();
            symmetry = s;
            image = im;
            rotation = r;
            prepsize = p;
            pieceSize = widthHeight;
            dimensions = new Vector2(d1, d2);
            image.Color = c;
            this.spots = spots;
            int exSpot = 0;
            if (ex > 0)
            {
                exSpot = random.Next(spots.Count); 
            }
            if (ch > 0)
            {
                score = ch;
            }
            else
            {
                score = 1;
            }            
            for (int i = 0; i < spots.Count; i++)
            {
                float depthFactor = 0;
                Color tempColor = image.Color;
                Texture2D tempImage = image.Image;
                if (i == exSpot && ex > 0)
                {
                    tempImage = exImage;
                    tempColor = Color.White;
                    explosive = ex;
                    depthFactor = .5f;
                }            
                else
                {
                    explosive = 0;
                }
                locations.Add(spots[i] * (float)Math.Round(60 * scale));
                boxes.Add(new Coordinate(new Sprite(tempImage, image.Location, tempColor, image.Rotation, image.Effects, image.Origin, image.Scale, image.Depth + depthFactor), spots[i], se, ch, explosive));
                if (ch > 0)
                {
                    boxes[i].image.Color = Color.Black;
                }
                Vector2 oragami = new Vector2(image.Origin.X * (float)scale, image.Origin.Y * (float)scale);
                boxes[i].image.Location = new Vector2(spots[i].X * (float)Math.Round(60 * scale), spots[i].Y * (float)Math.Round(60 * scale) - (o * (float)Math.Round(60 * scale))) + oragami;
            }
            score *= se * ((int)Math.Sqrt(boxes.Count) + 1);
            explosive = ex;
        }
        public void Revert()
        {
            boxes.Clear();
            locations.Clear();
            for (int i = 0; i < spots.Count; i++)
            {
                locations.Add(spots[i] * (float)Math.Round(60 * scale));
                boxes.Add(new Coordinate(new Sprite(image.Image, image.Location, image.Color, image.Rotation, image.Effects, image.Origin, image.Scale, image.Depth), spots[i], (int)image.Scale, chonkValue, explosive));
                Vector2 oragami = new Vector2(image.Origin.X * (float)scale, image.Origin.Y * (float)scale);
                boxes[i].image.Location = new Vector2(spots[i].X * (float)Math.Round(60 * scale), spots[i].Y * (float)Math.Round(60 * scale) - (6 * (float)Math.Round(60 * scale))) + oragami;
            }
            rotated = false;
            sideways = 0;
        }
        public void Display(Vector2 location, int size)
        {
            float biggerSide;
            if (pieceSize.X > pieceSize.Y)
            {
                biggerSide = pieceSize.X;
            }
            else
            {
                biggerSide = pieceSize.Y;
            }
            var tempScale = size / biggerSide;
            if (tempScale > 1)
            {
                tempScale = 1;
            }
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].image.Scale = tempScale;
                boxes[i].image.Location = new Vector2(boxes[i].place.X * (float)Math.Round(60 * tempScale), boxes[i].place.Y * (float)Math.Round(60 * tempScale)) + location + new Vector2(size / 2 - pieceSize.X / 2 * tempScale, size / 2 - pieceSize.Y / 2 * tempScale) + image.Origin * tempScale / 2;
            }
        }
        public void Ready()
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].image.Scale = image.Scale;
                Vector2 oragami = new Vector2(image.Origin.X * (float)scale, image.Origin.Y * (float)scale);
                boxes[i].image.Location = new Vector2(boxes[i].place.X * (float)Math.Round(60 * scale), boxes[i].place.Y * (float)Math.Round(60 * scale) - (6 * (float)Math.Round(60 * scale))) + oragami;
            }
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].Animate();
            }    
            if (goDown)
            {
                moveDown();
                goDown = false;
            }
            downtime.tick(gameTime);
            if (downtime.ready())
            {
                goDown = true;
            }
        }

        public void rotate()
        {
            rotate(1);
            rotated = true;
        }
        public void rotate(int direction)
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                if (boxes[i].place.Y < prepsize - 2)
                {
                    return;
                }
            }
            rotation = MathHelper.ToRadians(90) * direction;
            if (!symmetry)
            {
                for (int i = 0; i < boxes.Count; i++)
                {
                    var offset = Vector2.Transform(boxes[i].image.Location - boxes[0].image.Location, Matrix.CreateRotationZ(rotation));
                    var newPoint = offset;
                    offset = Vector2.Transform(boxes[i].place - boxes[0].place, Matrix.CreateRotationZ(rotation));
                    var smallPoint = offset;

                    newPoint = new Vector2((float)Math.Round(newPoint.X), (float)Math.Round(newPoint.Y));
                    smallPoint = new Vector2((float)Math.Round(smallPoint.X), (float)Math.Round(smallPoint.Y));

                    boxes[i].image.Location = newPoint + boxes[0].image.Location;
                    boxes[i].place = smallPoint + boxes[0].place;
                }

                bool good = true;
                do
                {
                    int bigDumb = 0;
                    good = true;
                    for (int i = 0; i < boxes.Count; i++)
                    {
                        if (boxes[i].place.X >= dimensions.X)
                        {
                            good = false;
                            bigDumb = -1;
                            break;
                        }
                        if (boxes[i].place.X < 0)
                        {
                            good = false;
                            bigDumb = 1;
                            break;
                        }
                    }
                    if (good)
                    {
                        break;
                    }
                    forceSide(bigDumb);
                }
                while (true);
            }
        }

        #region later
        // EXPANSION CODE, MAY BE USEFUL LATER 

        //public void rotate()
        //{
        //    if (!symmetry)
        //    {
        //        for (int i = 0; i < boxes.Count; i++)
        //        {
        //            var offset = Vector2.Transform(boxes[i].image.Location - boxes[0].image.Location, Matrix.CreateRotationZ(rotation));
        //            var newPoint = offset;
        //            offset = Vector2.Transform(boxes[i].place - boxes[0].place, Matrix.CreateRotationZ(rotation));
        //            var smallPoint = offset;

        //            newPoint = new Vector2((float)Math.Round(newPoint.X), (float)Math.Round(newPoint.Y));
        //            boxes[i].image.Location = newPoint + boxes[i].image.Location;
        //            boxes[i].place = smallPoint + boxes[i].place;
        //        }
        //    }
        //}

        #endregion

        public void moveDown()
        {
            bool good = true;
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].image.Location = new Vector2(boxes[i].image.Location.X, boxes[i].image.Location.Y + (float)Math.Round(60 * scale));
                boxes[i].place.Y += 1;
                if (boxes[i].place.Y >= dimensions.Y)
                {
                    good = false;
                }
            }
            if (!good)
            {
                for (int i = 0; i < boxes.Count; i++)
                {
                    boxes[i].image.Location = new Vector2(boxes[i].image.Location.X, boxes[i].image.Location.Y - (float)Math.Round(60 * scale));
                    boxes[i].place.Y -= 1;
                }
            }
        }

        public void forceUp()
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].image.Location = new Vector2(boxes[i].image.Location.X, boxes[i].image.Location.Y + (float)Math.Round(60 * scale));
                boxes[i].place.Y += 1;
            }
        }

        public void forceSide(int power = 1)
        {
            sideways = power;
            bool good = true;
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].image.Location = new Vector2(boxes[i].image.Location.X + 60 * power * scale, boxes[i].image.Location.Y);
                boxes[i].place.X += power;
            }
        }

        public void moveSide(int power = 1)
        {
            sideways = power;
            bool good = true;
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].image.Location = new Vector2(boxes[i].image.Location.X + 60 * power * scale, boxes[i].image.Location.Y);
                boxes[i].place.X += power;
                if (boxes[i].place.X < 0 || boxes[i].place.X >= dimensions.X)
                {
                    good = false;
                }
            }
            if (!good)
            {
                for (int i = 0; i < boxes.Count; i++)
                {
                    boxes[i].image.Location = new Vector2(boxes[i].image.Location.X - 60 * power * scale, boxes[i].image.Location.Y);
                    boxes[i].place.X -= power;
                }
            }
        }
        public void Draw(SpriteBatch JohannSebastianBach)
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].image.Draw(JohannSebastianBach);
            }
        }
    }
}