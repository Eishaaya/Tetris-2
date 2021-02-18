using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Label : VisualObject
    {
        public string Text;
        public SpriteFont Font;
        TimeSpan time;
        TimeSpan tick;
        public bool fade = false;
        public Color originalcolor;

        public Label(SpriteFont font, Color color, Vector2 location, string text, TimeSpan lifetime)
            : this(font, color, location, text, lifetime, new Vector2(0, 0), 0, SpriteEffects.None, 1, 1) { }
        public Label(SpriteFont font, Color color, Vector2 location, string text, TimeSpan lifetime, float Scale)
            : this(font, color, location, text, lifetime, new Vector2(0, 0), 0, SpriteEffects.None, Scale, 1) { }
        public Label(SpriteFont font, Color color, Vector2 location, string text, TimeSpan lifetime, Vector2 Origin, float Rotation, SpriteEffects Effect, float Scale, float Depth)
        : base (location, color, Origin, Rotation, Effect, Scale, Depth)
        {
            Text = text;
            Font = font;
            time = lifetime;
            originalcolor = color;
        }
        public void update(GameTime timetick)
        {
            tick += timetick.ElapsedGameTime;
            if (tick > time)
            {
                fade = true;
            }
        }
        public void write(SpriteBatch batch)
        {
            batch.DrawString(Font, Text, Location + offset, Color, rotation, Origin, Scale, effect, Depth);
        }
    }
}
