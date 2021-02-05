using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Sprite
    {
        public Color originalColor;
        public Texture2D Image { get; set; }
        public Color Color { get; set; }
        public Vector2 Location { get; set; }
        public float Rotation { get; set; }
        public SpriteEffects Effects { get; set; }
        public virtual Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Location.X, (int)Location.Y, Image.Width, Image.Height);
            }
        }
        public Vector2 Origin { get; set; }
        public float Scale {get; set;}
        public float Depth { get; set; }
        bool moved;
        bool bigger;
        Vector2 spotSet;
        float sizeSet;
        public Vector2 offset;
        float oldScale;
        Random random;

        public Sprite(Texture2D image, Vector2 location, Color color, float rotation, SpriteEffects effects, Vector2 origin, float scale, float depth)
        {
            Image = image;
            Color = color;
            Location = location;
            Rotation = rotation;
            Effects = effects;
            
            Origin = origin;
            Scale = scale;
            Depth = depth;

            originalColor = color;
            oldScale = scale;
            random = new Random();

            offset = Vector2.Zero;
            moved = false;
            bigger = false;
            sizeSet = float.NaN;
            spotSet = new Vector2(float.NaN, float.NaN);
        }        
        public void Vibrate(int distance, float sped)
        {            
            if(float.IsNaN(spotSet.X))
            {
                spotSet = new Vector2(random.Next(-distance, distance), random.Next(-distance, distance));
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
        public void Pulsate(int size, float sped)
        {
            if (float.IsNaN(sizeSet))
            {
                sizeSet = random.Next(size) + oldScale * 100;
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
        public bool Fade()
        {
            return Fade(originalColor);
        }
        public bool Fade(Color tint)
        {
            if (Color.A <= 0)
            {
                Color = tint;
                Color = Color.FromNonPremultiplied(Color.A, Color.G, Color.B, 0);
                return true;
            }
            Color = Color.FromNonPremultiplied(Color.A, Color.G, Color.B, Color.A - 3);
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
        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Location + offset, null, Color, Rotation, Origin, Scale, Effects, Depth);
        }
    }
}
