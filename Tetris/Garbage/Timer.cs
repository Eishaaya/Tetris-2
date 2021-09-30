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
        TimeSpan until;

        public static implicit operator TimeSpan(Timer timer) => timer.until;
        public static implicit operator Timer(TimeSpan span) => new Timer(span);
        public static implicit operator Timer(int time) => new Timer(time);

        public Timer(TimeSpan until)
        {
            this.until = until;
            this.wait = TimeSpan.Zero;
        }

        public void SetTime(TimeSpan until)
        {
            this.until = until;
        }


        public Timer(int length)
            : this(TimeSpan.FromMilliseconds(length)) { }

        public void Tick(GameTime time)
        {
            wait += time.ElapsedGameTime;
        }

        public int GetMillies()
        {
            return (int)until.TotalMilliseconds;
        }

        public bool Ready(bool reset = true)
        {
            if (wait >= until)
            {
                if (reset)
                {
                    wait = TimeSpan.Zero;
                }
                return true;
            }
            return false;
        }

        public void Reset()
        {
            wait = TimeSpan.Zero;
        }
    }
}
