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
        Random random = new Random();
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
        Label scoreX;
        int doubleTap;
        bool labelFade = true;
        Color moveColor;
        Keys pauseKey;
        bool isClassic;
        bool colorChanged;
        int[] progressionPoints = new int[] { 1, 10, 20, 30, 45, 60, 80, 100 };
        bool willClearBoard = true;


        public GameScreen(Grid newgrid, Label laby, Button pauser, Sprite box, AnimatingSprite down, Texture2D boxSprite, List<Vector2> boxLocations, SoundEffect mus, SoundEffect intro, int number, Keys pauseK = Keys.Escape)
            : base(mus, intro, number)
        {
            colorChanged = false;
            pauseKey = pauseK;
            boxes = new List<Button>();
            pause = pauser;
            grid = newgrid;
            score = laby;
            isClassic = grid.isClassic;
            lost = false;
            nextBox = box;
            bottom = down;
            baseSpeed = down.FrameTime.GetMillies();
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
                boxes.Add(new Button(boxSprite, boxLocations[i], Color.Black, 0, SpriteEffects.None, new Vector2(0, 0), 1, .079f, Color.Gray, Color.DarkGray));
            }
            doubleTap = boxes.Count - 1;
            moveLabel = new Label(laby.Font, Color.White, new Vector2(420, 850), "", TimeSpan.Zero, new Vector2(0, 0), 0, SpriteEffects.None, 1, .8f);
            scoreX = new Label(laby.Font, Color.White, score.Location + new Vector2(score.Font.MeasureString(score.Text).X + 0, 0), "x1", TimeSpan.Zero, new Vector2(0, 0), 0, SpriteEffects.None, 1, .8f);
            bottom.Color = colors[0];
        }
        public GameScreen(Grid newgrid, Label laby, Button pauser, Sprite box, AnimatingSprite down, SoundEffect mus, SoundEffect intro, int number, Keys pauseK = Keys.Escape)
            : base(mus, intro, number)
        {
            colorChanged = false;
            pauseKey = pauseK;
            boxes = new List<Button>();
            pause = pauser;
            grid = newgrid;
            score = laby;
            isClassic = grid.isClassic;
            lost = false;
            nextBox = box;
            bottom = down;
            baseSpeed = down.FrameTime.GetMillies();
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
        public override void changeBinds(List<Keys> newBinds, List<bool> bools)
        {
            base.changeBinds(newBinds, bools);
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
            grid.playSounds = bools[1];
            grid.holdTurn = bools[2];
            grid.holdDown = bools[3];
            grid.holdSide = bools[4];
            grid.willProject = bools[5];
            willClearBoard = bools[6];
        }
        public override void Reset()
        {
            colorChanged = false;
            if (willClearBoard)
            {
                grid.Reset();
            }
        }
        public override void Update(GameTime time, Screenmanager manny)
        {
            // grid.scale = (float)random.Next(0, 200) / 100;
            bottom.FrameTime.SetTime(new TimeSpan(0, 0, 0, 0, (int)(baseSpeed - (grid.progression * (50 / baseSpeed)))));
            if (!isClassic)
            {
                moveLabel.SetText($"{(int)grid.freeMoves}");
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
            }
            if (fade)
            {
                if (bottom.FadeTo(1))
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
            if (!colorChanged)
            {
                colorChanged = grid.ChangeBackColor(Color.Lerp(colors[stage], Color.White, .0f));
            }


            if (grid.progression == 0)
            {
                if (stage > 0)
                {
                    fade = true;
                    colorChanged = false;
                }
                stage = 0;
            }
            else if (grid.progression == progressionPoints[stage] && stage != progressionPoints.Length)
            {
                stage++;
                fade = true;
                colorChanged = false;
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
            scoreX.SetText($"x{Math.Round(grid.scoreFactor, 1)}");
            score.SetText($"Score: \n {grid.score}");
            if (heldMouse || keysDown)
            {
                return;
            }
            for (int i = 0; i < boxes.Count; i++)
            {
                if (boxes[i].check(mousy.Position.ToVector2(), isMouseClicked))
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
            if (pause.check(mousy.Position.ToVector2(), isMouseClicked) || Maryland.IsKeyDown(pauseKey))
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
            scoreX.Print(batch);
            score.Print(batch);
            if (!lost)
            {
                pause.Draw(batch);
            }
            if (!isClassic)
            {
                moveLabel.Print(batch);
            }
            grid.Draw(batch);
        }
    }
}
