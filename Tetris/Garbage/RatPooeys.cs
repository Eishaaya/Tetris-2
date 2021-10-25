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
                var oldCoord = old.boxes[i];
                var oldSecondary = oldCoord.SecondaryImage == null ? null : oldCoord.SecondaryImage.Image;
                newBoxes.Add(new Coordinate(new Sprite(oldCoord.Image.Image, oldCoord.Image.Location, oldCoord.Image.Color, oldCoord.Image.rotation, oldCoord.Image.effect, oldCoord.Image.Origin, oldCoord.Image.Scale, oldCoord.Image.Depth), oldCoord.GridSpot, oldCoord.Score, oldCoord.Chonk, oldCoord.Explosive, oldCoord.Reppellent, oldCoord.Speed, oldSecondary));
            }
            boxes = newBoxes;
            rotation = old.rotation;
            pieceSize = old.pieceSize;
            dimensions = old.dimensions;
            chonkValue = old.chonkValue;
            speedUp = old.speedUp;
            explosive = old.explosive;
        }
        public RatPooeys(Sprite coordImage, List<Vector2> spots, Vector2 widthHeight, Color color, int coordValue, float scale = 1, bool isSymmetrical = false, int fallTime = 650, int chonkAmount = 0, Texture2D chImage = null, bool willSpeed = false, Texture2D spImage = null, int explosiveAmount = 0, Texture2D exImage = null, int repellentAmount = 0, Texture2D repImage = null, int gridOffset = 6, int gridWidth = 10, int gridHeight = 26, float pieceRotation = 0)
        {
            random = new Random();
            chonkValue = chonkAmount;
            rotated = false;
            sideways = 0;
            this.scale = scale;
            downtime = new Timer(new TimeSpan(0, 0, 0, 0, fallTime));
            locations = new List<Vector2>();
            boxes = new List<Coordinate>();
            symmetry = isSymmetrical;
            image = coordImage;
            rotation = pieceRotation;
            pieceSize = widthHeight;
            dimensions = new Vector2(gridWidth, gridHeight);
            image.Color = color;
            this.spots = spots;
            int exSpot = 0;
            if (explosiveAmount > 0 || willSpeed)
            {
                exSpot = random.Next(spots.Count);
            }
            if (chonkAmount > 0)
            {
                score = chonkAmount;
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
                    if (explosiveAmount > 0)
                    {
                        tempImage = exImage;
                        tempColor = Color.White;
                        explosive = explosiveAmount;
                        depthFactor = .5f;
                    }
                    else if (willSpeed)
                    {
                        tempImage = spImage;
                        speedUp = willSpeed;
                    }
                }
                else
                {
                    explosive = 0;
                    speedUp = false;
                }
                locations.Add(spots[i] * (float)Math.Round(60 * this.scale));
                if (chonkAmount > 0)
                {
                    boxes.Add(new Coordinate(new Sprite(tempImage, image.Location, tempColor, image.rotation, image.effect, image.Origin, image.Scale, image.Depth + depthFactor), spots[i], coordValue, chonkAmount, explosive, repellentAmount, speedUp, chImage));
                }
                else
                {
                    boxes.Add(new Coordinate(new Sprite(tempImage, image.Location, tempColor, image.rotation, image.effect, image.Origin, image.Scale, image.Depth + depthFactor), spots[i], coordValue, chonkAmount, explosive, repellentAmount, speedUp, repImage));
                }
                Vector2 coordOrigin = new Vector2(image.Origin.X * this.scale, image.Origin.Y * this.scale);
                boxes[i].Image.Location = new Vector2(spots[i].X * (float)Math.Round(60 * this.scale), spots[i].Y * (float)Math.Round(60 * this.scale) - (gridOffset * (float)Math.Round(60 * this.scale))) + coordOrigin;
            }
            score *= coordValue * ((int)Math.Sqrt(boxes.Count) + 1);
            explosive = explosiveAmount;
            speedUp = willSpeed;
        }
        public void Revert()
        {
            var noxes = new List<Coordinate>(boxes);
            boxes.Clear();
            locations.Clear();
            for (int i = 0; i < spots.Count; i++)
            {
                locations.Add(spots[i] * (float)Math.Round(60 * scale));
                boxes.Add(new Coordinate(new Sprite(noxes[i].Image.Image, image.Location, noxes[i].Image.Color, image.rotation, image.effect, image.Origin, image.Scale, image.Depth), spots[i], (int)image.Scale, chonkValue, noxes[i].Explosive, noxes[i].Reppellent, noxes[i].Speed));
                Vector2 oragami = new Vector2(image.Origin.X * (float)scale, image.Origin.Y * (float)scale);
                boxes[i].Image.Location = new Vector2(spots[i].X * (float)Math.Round(60 * scale), spots[i].Y * (float)Math.Round(60 * scale) - (6 * (float)Math.Round(60 * scale))) + oragami;
                boxes[i].SecondaryImage = noxes[i].SecondaryImage;

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
                boxes[i].Image.Scale = tempScale;
                boxes[i].Image.Location = new Vector2(boxes[i].GridSpot.X * (float)Math.Round(60 * tempScale), boxes[i].GridSpot.Y * (float)Math.Round(60 * tempScale)) + location + new Vector2(size / 2 - pieceSize.X / 2 * tempScale, size / 2 - pieceSize.Y / 2 * tempScale) + image.Origin * tempScale / 2;
                if (boxes[i].Chonker())
                {
                    boxes[i].SecondaryImage.Scale = tempScale;
                    boxes[i].SecondaryImage.Location = new Vector2(boxes[i].GridSpot.X * (float)Math.Round(60 * tempScale), boxes[i].GridSpot.Y * (float)Math.Round(60 * tempScale)) + location + new Vector2(size / 2 - pieceSize.X / 2 * tempScale, size / 2 - pieceSize.Y / 2 * tempScale) + image.Origin * tempScale / 2;
                }
            }
        }
        public void Ready()
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].Image.Scale = image.Scale;
                Vector2 oragami = new Vector2(image.Origin.X * (float)scale, image.Origin.Y * (float)scale);
                boxes[i].Image.Location = new Vector2(boxes[i].GridSpot.X * (float)Math.Round(60 * scale), boxes[i].GridSpot.Y * (float)Math.Round(60 * scale) - (6 * (float)Math.Round(60 * scale))) + oragami;
                if (boxes[i].Chonker() && boxes[i].SecondaryImage != null)
                {
                    boxes[i].SecondaryImage.Scale = image.Scale;
                    boxes[i].SecondaryImage.Location = new Vector2(boxes[i].GridSpot.X * (float)Math.Round(60 * scale), boxes[i].GridSpot.Y * (float)Math.Round(60 * scale) - (6 * (float)Math.Round(60 * scale))) + oragami;
                }
            }
            ForceSide((int)Math.Round(5 - pieceSize.X / 120), true);
        }
        public void Update(GameTime gameTime)
        {
            if (goDown)
            {
                MoveDown();
                goDown = false;
            }
            downtime.Tick(gameTime);
            if (downtime.Ready())
            {
                goDown = true;
            }
            Animate();
        }

        public bool Rotate()
        {
            rotated = true;
            return Rotate(1);
        }
        public bool Rotate(int direction)
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                if (boxes[i].GridSpot.Y < biggerSide / 120)
                {
                    return false;
                }
            }
            rotation = MathHelper.ToRadians(90) * direction;
            if (!symmetry || explosive > 0 || speedUp)
            {
                for (int i = 0; i < boxes.Count; i++)
                {
                    var offset = Vector2.Transform(boxes[i].Image.Location - boxes[0].Image.Location, Matrix.CreateRotationZ(rotation));
                    var newPoint = offset;
                    offset = Vector2.Transform(boxes[i].GridSpot - boxes[0].GridSpot, Matrix.CreateRotationZ(rotation));
                    var smallPoint = offset;

                    newPoint = new Vector2((float)Math.Round(newPoint.X), (float)Math.Round(newPoint.Y));
                    smallPoint = new Vector2((float)Math.Round(smallPoint.X), (float)Math.Round(smallPoint.Y));

                    boxes[i].Image.Location = newPoint + boxes[0].Image.Location;
                    boxes[i].GridSpot = smallPoint + boxes[0].GridSpot;
                }

                bool good = true;
                do
                {
                    int bigDumb = 0;
                    good = true;
                    for (int i = 0; i < boxes.Count; i++)
                    {
                        if (boxes[i].GridSpot.X >= dimensions.X)
                        {
                            good = false;
                            bigDumb = -1;
                            break;
                        }
                        if (boxes[i].GridSpot.X < 0)
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
                    ForceSide(bigDumb);
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

        public void MoveDown()
        {
            var one = new Vector2(0, 1);
            bool good = true;
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].Image.Location = new Vector2(boxes[i].Image.Location.X, boxes[i].Image.Location.Y + (float)Math.Round(60 * scale));
                boxes[i].GridSpot += one;
                if (boxes[i].GridSpot.Y >= dimensions.Y)
                {
                    good = false;
                }
            }
            if (!good)
            {
                for (int i = 0; i < boxes.Count; i++)
                {
                    boxes[i].Image.Location = new Vector2(boxes[i].Image.Location.X, boxes[i].Image.Location.Y - (float)Math.Round(60 * scale));
                    boxes[i].GridSpot -= one;
                }
            }
        }

        public void ForceUp()
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].Image.Location = new Vector2(boxes[i].Image.Location.X, boxes[i].Image.Location.Y - (float)Math.Round(60 * scale));
                boxes[i].GridSpot -= new Vector2(0, 1);
            }
        }

        public void ForceSide(int power = 1, bool spawning = false)
        {
            if (!spawning)
            {
                sideways = power;
            }
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].Image.Location = new Vector2(boxes[i].Image.Location.X + 60 * power * scale, boxes[i].Image.Location.Y);
                boxes[i].GridSpot += new Vector2(power, 0);
                boxes[i].UpdateLinkedImage();
            }
        }

        public void MoveSide(int power = 1)
        {
            sideways = power;
            var powerVector = new Vector2(power, 0);

            bool good = true;
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].Image.Location = new Vector2(boxes[i].Image.Location.X + 60 * power * scale, boxes[i].Image.Location.Y);
                boxes[i].GridSpot += powerVector;
                boxes[i].UpdateLinkedImage();
                if (boxes[i].GridSpot.X < 0 || boxes[i].GridSpot.X >= dimensions.X)
                {
                    good = false;
                }
            }
            if (!good)
            {
                for (int i = 0; i < boxes.Count; i++)
                {
                    boxes[i].Image.Location = new Vector2(boxes[i].Image.Location.X - 60 * power * scale, boxes[i].Image.Location.Y);
                    boxes[i].GridSpot -= powerVector;
                    boxes[i].UpdateLinkedImage();
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