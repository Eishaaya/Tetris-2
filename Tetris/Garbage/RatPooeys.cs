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
        public Vector2 pieceSize;
        List<Vector2> spots;
        int chonkValue;
        public float explosive;
        public bool speedUp;
        Random random;
        float biggerSide;
        public RatPooeys(RatPooeys old)
        {
            rotated = false;
            sideways = 0;
            scale = old.scale;
            downtime = old.downtime;
            locations = new List<Vector2>();
            symmetry = old.symmetry;
            image = new Sprite(old.image.Image, old.image.Location, old.image.Color, old.image.rotation, old.image.effect, old.image.Origin, old.image.Scale, old.image.Depth);
            var newBoxes = new List<Coordinate>();
            for (int i = 0; i < old.boxes.Count; i++)
            {
                if (old.boxes[i].chonkImage != null)
                {
                    newBoxes.Add(new Coordinate(new Sprite(old.boxes[i].image.Image, old.boxes[i].image.Location, old.boxes[i].image.Color, old.boxes[i].image.rotation, old.boxes[i].image.effect, old.boxes[i].image.Origin, old.boxes[i].image.Scale, old.boxes[i].image.Depth), old.boxes[i].place, old.boxes[i].score, old.boxes[i].chonker, old.boxes[i].explosive, old.boxes[i].speed, old.boxes[i].chonkImage.Image));
                }
                else
                {
                    newBoxes.Add(new Coordinate(new Sprite(old.boxes[i].image.Image, old.boxes[i].image.Location, old.boxes[i].image.Color, old.boxes[i].image.rotation, old.boxes[i].image.effect, old.boxes[i].image.Origin, old.boxes[i].image.Scale, old.boxes[i].image.Depth), old.boxes[i].place, old.boxes[i].score, old.boxes[i].chonker, old.boxes[i].explosive, old.boxes[i].speed));
                }
            }
            boxes = newBoxes;
            rotation = old.rotation;
            prepsize = old.prepsize;
            pieceSize = old.pieceSize;
            dimensions = old.dimensions;
            chonkValue = old.chonkValue;
            speedUp = old.speedUp;
            explosive = old.explosive;            
        }
        public RatPooeys(Sprite im, List<Vector2> spots, Vector2 widthHeight, Color c, int se, float sc = 1, bool s = false, int t = 650, int ch = 0, Texture2D chImage = null, bool sp = false, Texture2D spImage = null, float ex = 0, Texture2D exImage = null, int o = 6, int d1 = 10, int d2 = 26, int p = 6, float r = 0)
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
            if (ex > 0 || sp)
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
                float depthFactor = 0.01f;
                Color tempColor = image.Color;
                Texture2D tempImage = image.Image;                
                if (i == exSpot)
                {
                    if (ex > 0)
                    {
                        tempImage = exImage;
                        tempColor = Color.White;
                        explosive = ex;
                        depthFactor = .5f;
                    }
                    else if (sp)
                    {
                        tempImage = spImage;
                        speedUp = sp;
                    }
                }
                else
                {
                    explosive = 0;
                    speedUp = false;
                }
                locations.Add(spots[i] * (float)Math.Round(60 * scale));
                if (ch > 0)
                {
                    boxes.Add(new Coordinate(new Sprite(tempImage, image.Location, tempColor, image.rotation, image.effect, image.Origin, image.Scale, image.Depth + depthFactor), spots[i], se, ch, explosive, speedUp, chImage));
                }
                else
                {
                    boxes.Add(new Coordinate(new Sprite(tempImage, image.Location, tempColor, image.rotation, image.effect, image.Origin, image.Scale, image.Depth + depthFactor), spots[i], se, ch, explosive, speedUp));
                }
                Vector2 oragami = new Vector2(image.Origin.X * (float)scale, image.Origin.Y * (float)scale);
                boxes[i].image.Location = new Vector2(spots[i].X * (float)Math.Round(60 * scale), spots[i].Y * (float)Math.Round(60 * scale) - (o * (float)Math.Round(60 * scale))) + oragami;
            }
            score *= se * ((int)Math.Sqrt(boxes.Count) + 1);
            explosive = ex;
            speedUp = sp;
        }
        public void Revert()
        {
            var noxes = new List<Coordinate>(boxes);
            boxes.Clear();
            locations.Clear();
            for (int i = 0; i < spots.Count; i++)
            {
                locations.Add(spots[i] * (float)Math.Round(60 * scale));
                boxes.Add(new Coordinate(new Sprite(noxes[i].image.Image, image.Location, noxes[i].image.Color, image.rotation, image.effect, image.Origin, image.Scale, image.Depth), spots[i], (int)image.Scale, chonkValue, noxes[i].explosive, speedUp));
                Vector2 oragami = new Vector2(image.Origin.X * (float)scale, image.Origin.Y * (float)scale);
                boxes[i].image.Location = new Vector2(spots[i].X * (float)Math.Round(60 * scale), spots[i].Y * (float)Math.Round(60 * scale) - (6 * (float)Math.Round(60 * scale))) + oragami;
            }
            rotated = false;
            sideways = 0;
        }
        public void Display(Vector2 location, int size)
        {
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
                if (boxes[i].Chonker() && boxes[i].chonkImage != null)
                {
                    boxes[i].chonkImage.Scale = tempScale;
                    boxes[i].chonkImage.Location = new Vector2(boxes[i].place.X * (float)Math.Round(60 * tempScale), boxes[i].place.Y * (float)Math.Round(60 * tempScale)) + location + new Vector2(size / 2 - pieceSize.X / 2 * tempScale, size / 2 - pieceSize.Y / 2 * tempScale) + image.Origin * tempScale / 2;
                }
            }
        }
        public void Ready()
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].image.Scale = image.Scale;
                Vector2 oragami = new Vector2(image.Origin.X * (float)scale, image.Origin.Y * (float)scale);
                boxes[i].image.Location = new Vector2(boxes[i].place.X * (float)Math.Round(60 * scale), boxes[i].place.Y * (float)Math.Round(60 * scale) - (6 * (float)Math.Round(60 * scale))) + oragami;
                if (boxes[i].Chonker() && boxes[i].chonkImage != null)
                {
                    boxes[i].chonkImage.Scale = image.Scale;
                    boxes[i].chonkImage.Location = new Vector2(boxes[i].place.X * (float)Math.Round(60 * scale), boxes[i].place.Y * (float)Math.Round(60 * scale) - (6 * (float)Math.Round(60 * scale))) + oragami;
                }
            }
        }
        public void Update(GameTime gameTime)
        {
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
            Animate();
        }

        public bool rotate()
        {
            rotated = true;
            return rotate(1);
        }
        public bool rotate(int direction)
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                if (boxes[i].place.Y < biggerSide / 120)
                {
                    return false;
                }
            }
            rotation = MathHelper.ToRadians(90) * direction;
            if (!symmetry || explosive > 0 || speedUp)
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
            return true;
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
                boxes[i].image.Location = new Vector2(boxes[i].image.Location.X, boxes[i].image.Location.Y - (float)Math.Round(60 * scale));
                boxes[i].place.Y -= 1;
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
        public void Animate()
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].Animate();
            }
        }
        public void Draw(SpriteBatch JohannSebastianBach)
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].Draw(JohannSebastianBach);
            }
        }
    }
}