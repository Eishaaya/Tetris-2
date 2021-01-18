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
        }
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Location, null, Color, Rotation, Origin, Scale, Effects, Depth);
        }
    }
}
