using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    public class VisualObject : IPoolable
    {
        public Color Color;
        public Vector2 Location;
        public float rotation;
        public float Scale;
        public float Depth;
        public SpriteEffects effect;
        public Vector2 Origin;
        protected bool moved;

        protected bool bigger;
        protected bool rotated;
        protected Vector2 spotSet;
        protected float sizeSet;
        protected float degreeSet;
        public Vector2 offset;
        protected float oldScale;
        protected Random random;
        public Color originalColor;
        public float oldRotation;

        public VisualObject(Vector2 location, Color color, Vector2 origin, float Rotation, SpriteEffects Effect, float scale, float depth)
        {
            Location = location;
            Color = color;
            Origin = origin;
            Scale = scale;
            Depth = depth;
            effect = Effect;
            rotation = Rotation;
            originalColor = color;
            oldScale = Scale;
            oldRotation = rotation;
            random = Extensions.random;

            offset = Vector2.Zero;
            moved = false;
            bigger = false;
            sizeSet = float.NaN;
            degreeSet = float.NaN;
            spotSet = new Vector2(float.NaN, float.NaN);
        }

        #region clone

        public VisualObject Clone()
        {
            var copy = new VisualObject(Location, Color, Origin, rotation, effect, Scale, Depth);
            CloneLogic(copy);
            return copy;
        }
        protected void CloneLogic<T>(T copy) where T:VisualObject
        {
            copy.bigger = bigger;
            copy.rotated = rotated;
            copy.spotSet = spotSet;
            copy.sizeSet = sizeSet;
            copy.degreeSet = degreeSet;
            copy.offset = offset;
            copy.oldScale = oldScale;
            copy.originalColor = originalColor;
            copy.oldRotation = oldRotation;
        }


        #endregion

        #region visualFunctions
        public void Vibrate(int distance, float sped, bool rando = true)
        {
            if (float.IsNaN(spotSet.X))
            {
                if (rando)
                {
                    spotSet = new Vector2(random.Next(-distance, distance), random.Next(-distance, distance));
                }
                else
                {
                    spotSet = new Vector2(distance, distance);
                }
                offset = Vector2.Zero;
            }
            if (!moved)
            {
                offset = Vector2.Lerp(offset, spotSet, sped);
                if (Vector2.Distance(offset, spotSet) <= .5f)
                {
                    offset = spotSet;
                    moved = true;
                }
            }
            else
            {
                offset = Vector2.Lerp(offset, Vector2.Zero, sped);
                if (Vector2.Distance(offset, Vector2.Zero) <= .5f)
                {
                    spotSet = new Vector2(float.NaN, float.NaN);
                    moved = false;
                    offset = Vector2.Zero;
                }
            }
        }
        public void Pulsate(int size, float sped, bool rando = true)
        {
            if (float.IsNaN(sizeSet))
            {
                if (rando)
                {
                    sizeSet = random.Next(size) + oldScale * 100;
                }
                else
                {
                    sizeSet = size;
                }
                sizeSet /= 100;
            }
            if (!bigger)
            {
                var temp = Vector2.Lerp(new Vector2(Scale, 0), new Vector2(sizeSet, 0), sped);
                if (Vector2.Distance(temp, new Vector2(sizeSet, 0)) <= .01f)
                {
                    Scale = sizeSet;
                    bigger = true;
                }
                else
                {
                    Scale = temp.X;
                }
            }
            else
            {
                var temp = Vector2.Lerp(new Vector2(Scale, 0), new Vector2(oldScale, 0), sped);
                if (Vector2.Distance(temp, new Vector2(oldScale, 0)) <= .01f)
                {
                    sizeSet = float.NaN;
                    Scale = oldScale;
                    bigger = false;
                }
                else
                {
                    Scale = temp.X;
                }
            }
        }
        public void Rotate(float target, float sped, bool rando = true)
        {
            if (float.IsNaN(degreeSet))
            {
                if (rando)
                {
                    degreeSet = random.Next((int)-target * 1000, (int)target * 1000);
                    degreeSet /= 1000;
                }
                else
                {
                    degreeSet = target;
                }
            }
            if (!rotated)
            {
                var temp = MathHelper.Lerp(rotation, oldRotation, sped);
                if (MathHelper.Distance(temp, oldRotation) <= .01f)
                {
                    rotation = degreeSet;
                    rotated = true;
                }
                else
                {
                    rotation = temp;
                }
            }
            else
            {
                var temp = Vector2.Lerp(new Vector2(rotation, 0), new Vector2(degreeSet, 0), sped);
                if (Vector2.Distance(temp, new Vector2(rotation, 0)) <= .01f)
                {
                    degreeSet = float.NaN;
                    rotation = temp.X;
                    rotated = false;
                }
                else
                {
                    rotation = temp.X;
                }
            }
        }
        public void ChangeColor(Color newColor, float sped = .1f)
        {
            var temp = Color;
            Color = Color.FromNonPremultiplied((int)MathHelper.LerpPrecise(Color.R, newColor.R, sped), (int)MathHelper.LerpPrecise(Color.G, newColor.G, sped), (int)MathHelper.LerpPrecise(Color.B, newColor.B, sped), (int)MathHelper.LerpPrecise(Color.A, newColor.A, sped));
            if (temp == Color)
            {
                Color = newColor;
            }
        }
        public bool Fade(int speed = 3)
        {
            return Fade(originalColor, speed);
        }
        public bool Fade(Color tint, int speed = 3)
        {
            if (Color.A <= 0)
            {
                Color = tint;
                Color = Color.FromNonPremultiplied(tint.R, tint.G, tint.B, 0);
                return true;
            }
            Color = Color.FromNonPremultiplied(tint.R, tint.G, tint.B, Color.A - speed);
            return false;
        }

        public bool FadeTo(int cChoice)
        {
            var tint = Color;
            if (cChoice == 0)
            {
                tint = Color.FromNonPremultiplied(Color.R - Color.A, Color.G - Color.A, Color.B - Color.A, Color.A);
            }
            else if (cChoice == 1)
            {
                tint = Color.FromNonPremultiplied(Color.A, Color.G - Color.A, Color.B - Color.A, Color.A);
            }
            else if (cChoice == 2)
            {
                tint = Color.FromNonPremultiplied(Color.R - Color.A, Color.A, Color.B - Color.A, Color.A);
            }
            else if (cChoice == 3)
            {
                tint = Color.FromNonPremultiplied(Color.R - Color.A, Color.G - Color.A, Color.A, Color.A);
            }
            if (Color.A <= 0)
            {
                Color = tint;
                Color = Color.FromNonPremultiplied(tint.R, tint.G, tint.B, 0);
                return true;
            }
            Color = Color.FromNonPremultiplied(tint.R, tint.G, tint.B, Color.A - 3);
            return false;
        }
        public bool Fill()
        {
            return Fill(originalColor);
        }
        public bool Fill(Color tint)
        {
            Color = Color.FromNonPremultiplied(tint.R, tint.G, tint.B, Color.A + 3);
            if (Color.A >= 255)
            {
                Color = Color.FromNonPremultiplied(tint.R, tint.G, tint.B, 255);
                return true;
            }
            return false;
        }


        #endregion
    }
}
