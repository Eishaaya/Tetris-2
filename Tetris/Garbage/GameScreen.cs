using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class GameScreen : Screen
    {
        Song music;
        Grid grid;
        Label score;
        Button pause;
        Sprite nextBox;
        AnimatingSprite bottom;
        bool lost = false;
        float baseSpeed;
        bool fade = false;
        bool fill = false;
        int stage = 0;
        List<Color> colors;
        bool erase = false;
        List<Button> boxes;
        Label moveLabel;
        int doubleTap;
        bool labelFade = true;
        Color moveColor;
        Keys pauseKey;
        public GameScreen(Grid newgrid, Label laby, Button pauser, Sprite box, AnimatingSprite down, Texture2D boxSprite, List<Vector2> boxLocations, SoundEffect mus, SoundEffect intro, Keys pauseK = Keys.Escape)
            : base(mus, intro)
        {
            pauseKey = pauseK;
            boxes = new List<Button>();
            pause = pauser;
            grid = newgrid;
            score = laby;
            lost = false;
            nextBox = box;
            bottom = down;
            baseSpeed = down.frametime.Milliseconds;
            colors = new List<Color>
            {
                Color.White,
                Color.Purple,
                Color.BlueViolet,
                Color.Blue,
                Color.SeaGreen,
                Color.Green,
                Color.Gold,
                Color.Orange,
                Color.Red
            };
            for (int i = 0; i < boxLocations.Count; i++)
            {
                boxes.Add(new Button(boxSprite, boxLocations[i], Color.Black, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.DarkGray));
            }
            doubleTap = boxes.Count - 1;
            moveLabel = new Label(laby.Font, Color.White, new Vector2(420, 850), "", TimeSpan.Zero);
            bottom.Color = colors[0];
        }
        public GameScreen(Grid newgrid, Label laby, Button pauser, Sprite box, AnimatingSprite down, SoundEffect mus, SoundEffect intro, Keys pauseK = Keys.Escape)
            : base(mus, intro)
        {
            pauseKey = pauseK;
            boxes = new List<Button>();
            pause = pauser;
            grid = newgrid;
            score = laby;
            lost = false;
            nextBox = box;
            bottom = down;
            baseSpeed = down.frametime.Milliseconds;
            colors = new List<Color>
            {
                Color.White,
                Color.Purple,
                Color.Blue,
                Color.SkyBlue,
                Color.SeaGreen,
                Color.Green,
                Color.Gold,
                Color.Orange,
                Color.Red
            };
            bottom.Color = colors[0];
        }
        public override void changeBinds(List<Keys> newBinds)
        {
            grid.downKey = newBinds[0];
            grid.turnKey = newBinds[1];
            grid.leftKey = newBinds[2];
            grid.rightKey = newBinds[3];
            grid.TeleKey = newBinds[4];
            grid.switchKeys[0] = newBinds[5];
            grid.switchKeys[1] = newBinds[6];
            grid.switchKeys[2] = newBinds[7];
            grid.switchKeys[3] = newBinds[8];
            pauseKey = newBinds[9];
        }
        public override void Reset()
        {
            grid.Reset();
        }
        public override void Update(GameTime time, Screenmanager manny)
        {
            bottom.frametime = new TimeSpan(0, 0, 0, 0, (int)(baseSpeed - (grid.progression * (50 / baseSpeed))));
            moveLabel.Text = $"{(int)grid.freeMoves}";
            if (grid.overused)
            {
                if (!labelFade)
                {
                    if (moveLabel.Fade(Color.White))
                    {
                        labelFade = true;
                    }
                }
                else
                {
                    moveLabel.Fill(Color.Red);
                }
            }
            else if (grid.dangerUse)
            {
                if (!labelFade)
                {
                    if (moveLabel.Fade(moveColor))
                    {
                        labelFade = true;
                        if (moveColor == Color.Red)
                        {
                            moveColor = Color.White;
                        }
                        else
                        {
                            moveColor = Color.Red;
                        }
                    }
                }
                else
                {
                    if (moveLabel.Fill(moveColor))
                    {
                        labelFade = false;
                    }
                }
            }
            else
            {
                if (moveLabel.Color != Color.White)
                {
                    moveLabel.Fill(Color.White);
                    moveColor = Color.White;
                    labelFade = false;
                }
            }
            if (fade)
            {
                if (bottom.Fade(colors[stage]))
                {
                    fade = false;
                    fill = true;
                }
            }
            else if (fill)
            {
                if (bottom.Fill(colors[stage]))
                {
                    fill = false;
                }
            }
            #region Bottom
            if (grid.progression == 0)
            {
                if (stage > 0)
                {
                    fade = true;
                }
                stage = 0;
            }
            else if (grid.progression == 1)
            {
                if (stage == 0)
                {
                    fade = true;
                }
                stage = 1;
            }
            else if (grid.progression == 10)
            {
                if (stage == 1)
                {
                    fade = true;
                }
                stage = 2;
            }
            else if (grid.progression == 20)
            {
                if (stage == 2)
                {
                    fade = true;
                }
                stage = 3;
            }
            else if (grid.progression == 30)
            {
                if (stage == 3)
                {
                    fade = true;
                }
                stage = 4;
            }
            else if (grid.progression == 45)
            {
                if (stage == 4)
                {
                    fade = true;
                }
                stage = 5;
            }
            else if (grid.progression == 60)
            {
                if (stage == 5)
                {
                    fade = true;
                }
                stage = 6;
            }
            else if (grid.progression == 80)
            {
                if (stage == 6)
                {
                    fade = true;
                }
                stage = 7;
            }
            else if (grid.progression == 100)
            {
                if (stage == 7)
                {
                    fade = true;
                }
                stage = 8;
            }
            #endregion //The animated bar down low

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
            if (heldMouse || keysDown)
            {
                return;
            }
            for (int i = 0; i < boxes.Count; i++)
            {
                if (boxes[i].check(mousy.Position.ToVector2(), nou))
                {
                    grid.Switch(i);
                    break;
                }
                else if (doubleTap == i && boxes[i].check(mousy.Position.ToVector2(), uno))
                {
                    grid.Switch(i + 1);
                    break;
                }
            }
            if (pause.check(mousy.Position.ToVector2(), nou) || Maryland.IsKeyDown(pauseKey))
            {
                manny.next(3, false);
                return;
            }
        }
        public override void Play(GameTime time)
        {
            base.Play(time);
            bottom.Animate(time);
        }
        public override void Draw(SpriteBatch batch)
        {
            bottom.Draw(batch);
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].Draw(batch);
            }
            nextBox.Draw(batch);
            score.write(batch);
            if (!lost)
            {
                pause.Draw(batch);
            }
            moveLabel.write(batch);
            grid.Draw(batch);
        }
    }
}
