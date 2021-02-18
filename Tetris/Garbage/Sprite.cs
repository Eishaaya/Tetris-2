using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Sprite : VisualObject
    {
        public Texture2D Image { get; set; }
        public virtual Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Location.X, (int)Location.Y, Image.Width, Image.Height);
            }
        }

        public Sprite(Texture2D image, Vector2 location, Color color, float rotation, SpriteEffects effects, Vector2 origin, float scale, float depth)
        : base(location, color, origin, rotation, effects, scale, depth)
        {
            Image = image;
        }                
        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Location + offset, null, Color, rotation, Origin, Scale, effect, Depth);
        }
    }
}
