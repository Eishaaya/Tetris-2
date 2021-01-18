using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class GameScreen : Screen
    {
        Grid grid;
        Label score;
        Button pause;
        bool lost = false;
        public GameScreen(Grid newgrid, Label laby, Button pauser)
            : base()
        {
            pause = pauser;
            grid = newgrid;
            score = laby;
            lost = false;
        }
        public override void Draw(SpriteBatch batch)
        {
            grid.Draw(batch);
            score.write(batch);
            if (!lost)
            {
                pause.Draw(batch);
            }
        }
        public override void Reset()
        {
            grid.Reset();
        }
        public override void Update(GameTime time, Screenmanager manny)
        {            
            if (lost)
            {
                grid.Reset();
                lost = false;
            }
            if (grid.lose)
            {
                manny.next(4, false);
                manny.peek().Transfer(grid.score);
                lost = true;
                return;
            }
            base.Update(time, manny);
            grid.Update(time);
            score.Text = $"Score: \n {grid.score}";
            if (heldMouse)
            {
                return;
            }
            if (pause.check(mousy.Position.ToVector2(), nou))
            {
                manny.next(3, false);
                return;
            }
        }
    }
}
