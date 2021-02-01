using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class ParticleEffect
    {
        bool fullFaded;
        List<Particle> particles;
        Random random = new Random();
        public ParticleEffect(Texture2D image, Vector2 origin, List<Color> colors, int amount, int time, List<double> speeds, List<int> scales, List<int> subsections = null)
        {
            fullFaded = false;
            particles = new List<Particle>();
            if (subsections == null)
            {
                subsections = new List<int>();
                for (int i = 1; i < colors.Count; i++)
                {
                    subsections.Add(amount / colors.Count * i);
                }
            }
            int j = 0;
            for (int i = 0; i < amount; i++)
            {
                if (j < subsections.Count && i >= subsections[j])
                {
                    j++;
                }
                particles.Add(new Particle(image, origin, colors[j], 0, SpriteEffects.None, new Vector2(0, 0), new Vector2(MathHelper.ToRadians((float)(Math.Sin(random.Next(360)) * speeds[j])), (float)(Math.Cos(random.Next(360)) * speeds[j])), time, new Vector2(random.Next(scales[j]), random.Next(scales[j])), 1, scales[j]));
                return;
            }
        }
        public void Update(GameTime time)
        {
            if (particles.Count == 0)
            {
                fullFaded = true;
                return;
            }
            bool blah = true;
            while (blah)
            {
                blah = false;
                for (int i = 0; i < particles.Count; i++)
                {
                    particles[i].Update(time);
                    if (particles[i].faded)
                    {
                        particles.RemoveAt(i);
                        blah = true;
                        break;
                    }
                }
            }
        }
        public void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Draw(batch);
            }
        }
    }
}
