using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public class Timer
    {
        TimeSpan wait;
        TimeSpan Until;
        public Timer(TimeSpan until)
        {
            Until = until;
        }
        public void tick(GameTime time)
        {
            wait += time.ElapsedGameTime;
        }
        public bool ready()
        {
            if(wait >= Until)
            {
                wait = TimeSpan.Zero;
                return true;
            }
            return false;
        }
        public void reset()
        {
            wait = TimeSpan.Zero;
        }
    }
}
