using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class FrameObject
    {
        public Vector2[][] Origins { get; }
        AnimationFrame[][] frames;
        public static implicit operator AnimationFrame[][](FrameObject frame) => frame.frames;

        public int[] frameSpeed { get; }

        public FrameObject(AnimationFrame[][] animationFrames, int[] speed, Vector2[][] origins = null)
        {
            Origins = origins;
            frames = animationFrames;
            frameSpeed = speed;
        }
    }
}
