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
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Location, null, Color, Rotation, Origin, Scale, Effects, Depth);
        }
    }
}
