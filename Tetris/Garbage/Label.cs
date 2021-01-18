using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Label
    {
        public string Text;
        public SpriteFont Font;
        public Color Color;
        public Vector2 Location;
        TimeSpan time;
        TimeSpan tick;
        public bool fade = false;
        public Color originalcolor;
        Vector2 origin;
        float rotation;
        float scale;
        float depth;
        SpriteEffects effect;

        public Label(SpriteFont font, Color color, Vector2 location, string text, TimeSpan lifetime)
            : this(font, color, location, text, lifetime, new Vector2(0, 0), 0, SpriteEffects.None, 1, 1) { }
        public Label(SpriteFont font, Color color, Vector2 location, string text, TimeSpan lifetime, float Scale)
            : this(font, color, location, text, lifetime, new Vector2(0, 0), 0, SpriteEffects.None, Scale, 1) { }
        public Label(SpriteFont font, Color color, Vector2 location, string text, TimeSpan lifetime, Vector2 Origin, float Rotation, SpriteEffects Effect, float Scale, float Depth)
        {
            Text = text;
            Location = location;
            Color = color;
            Font = font;
            time = lifetime;
            originalcolor = color;
            origin = Origin;
            scale = Scale;
            depth = Depth;
            effect = Effect;
        }
        public void update(GameTime timetick)
        {
            tick += timetick.ElapsedGameTime;
            if(tick > time)
            {
                fade = true;
            }
        }
        public void write(SpriteBatch batch)
        {
            batch.DrawString(Font, Text, Location, Color, rotation, origin, scale, effect, depth);
        }
    }
}
