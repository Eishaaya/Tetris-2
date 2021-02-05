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
                float distance = (360 * (subsections.Count + 1)) / amount;
                float angle = MathHelper.ToRadians(distance * i);
                particles.Add(new Particle(image, origin, colors[j], 0, SpriteEffects.None, new Vector2(0, 0), new Vector2((float)(Math.Cos(angle) * speeds[j]), (float)(Math.Sin(angle) * speeds[j])), time, new Vector2(random.Next(scales[j]), random.Next(scales[j])), 1, scales[j]));
            }
        }
        public void Update(GameTime time)
        {
            if (particles.Count == 0)
            {
                fullFaded = true;
                return;
            }
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Update(time);
                if (particles[i].faded)
                {
                    particles.RemoveAt(i);
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
