using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Tetris
{
    class ParticleEffect
    {
        public enum EffectType
        {
            Explosion,
            Ray
        }
        bool fullFaded;
        List<Particle> particles;
        Random random = new Random();
        public ParticleEffect(EffectType type, Texture2D image, Vector2 origin, List<Color> colors, int amount, int time, List<double> speeds, List<int> scales, List<int> subsections = null, int zoneHeight = 0, int zoneWidth = 0, float directionX = 1, float directionY = 1, bool rando = true)
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
                if (type == EffectType.Explosion)
                {
                    float distance = (360 * (subsections.Count + 1)) / amount;
                    float angle = MathHelper.ToRadians(distance * i);
                    var scale = new Vector2(random.Next(scales[j]), random.Next(scales[j]));
                    if (!rando)
                    {
                        scale = new Vector2(scales[j], scales[j]);
                    }
                    particles.Add(new Particle(image, origin, colors[j], 0, SpriteEffects.None, new Vector2(image.Width / 2, image.Height / 2), new Vector2((float)(Math.Cos(angle) * speeds[j]), (float)(Math.Sin(angle) * speeds[j])), time, scale, 1f - (.1f / amount * i), scales[j]));
                }
                else if (type == EffectType.Ray)
                {
                    if (subsections.Count <= 2)
                    {
                        for (int e = 0; e < amount; e++)
                        {
                            int X = 0;
                            int Y = 0;
                            if (zoneWidth != 0)
                            {
                                X = random.Next(-zoneWidth, zoneWidth);
                            }
                            if (zoneHeight != 0)
                            {
                                Y = random.Next(-zoneHeight, zoneHeight);
                            }
                            var location = new Vector2(X, Y) + origin;
                            particles.Add(new Particle(image, location, colors[j], 0, SpriteEffects.None, new Vector2(image.Width / 2, image.Height / 2), new Vector2(directionX, directionY) * new Vector2((float)(speeds[j]), (float)speeds[j]), time, new Vector2(scales[j], scales[j]), 1, scales[j], e * 10, false));
                            if (e % 2 == 0)
                            {

                                particles.Add(new Particle(image, location, colors[j], 0, SpriteEffects.None, new Vector2(image.Width / 2, image.Height / 2), Vector2.Transform(new Vector2(directionX, directionY), Matrix.CreateRotationZ(MathHelper.Pi)) * new Vector2((float)(speeds[j]), (float)speeds[j]), time, new Vector2(scales[j], scales[j]), 1, scales[j], e * 10, false));
                            }
                        }
                    }
                    else if (subsections.Count >= 2)
                    {

                    }
                }
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
                if (particles[i].going)
                {
                    particles[i].Draw(batch);
                }
            }
        }
    }
}
