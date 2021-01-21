using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class Screenmanager
    {
        Stack<Screen> activeScreens;
        List<Screen> allScreens;
        public Stack<Screen> previousScreens;
        public Screenmanager(List<Screen> screens)
        {
            activeScreens = new Stack<Screen>();
            allScreens = screens;
            activeScreens.Push(allScreens[0]);
            previousScreens = new Stack<Screen>();
            activeScreens.Peek().Start();
        }
        public Screen peek()
        {
            return activeScreens.Peek();
        }
        public void back()
        {
            activeScreens.Pop().StopMusic();
            if (activeScreens.Count > 0)
            {
                if (activeScreens.Peek() != previousScreens.Peek())
                {
                    activeScreens.Pop().StopMusic();
                }
                else
                {
                    previousScreens.Pop();
                    return;
                }
            }
            activeScreens.Push(previousScreens.Pop());
            activeScreens.Peek().heldMouse = true;
        }
        public void next(int index, bool replace)
        {
            if (replace)
            {
                    activeScreens.Peek().StopMusic();
                    previousScreens.Push(activeScreens.Pop());
                    if (activeScreens.Count > 0)
                    {
                        activeScreens.Peek().StopMusic();
                        activeScreens.Clear();
                    }
                    activeScreens.Push(allScreens[index]);
                    activeScreens.Peek().Start();
            }
            else
            {
                previousScreens.Push(activeScreens.Peek());
                activeScreens.Push(allScreens[index]);
            }
            activeScreens.Peek().heldMouse = true;
        }
        public void clearMemory()
        {
            previousScreens.Clear();
        }
        public void Update(GameTime time)
        {
            Stack<Screen> drawScreens = new Stack<Screen>();
            while (activeScreens.Count > 0)
            {
                drawScreens.Push(activeScreens.Pop());
            }
            drawScreens.Peek().Play(time);
            while (drawScreens.Count > 0)
            {
                activeScreens.Push(drawScreens.Pop());
            }
            activeScreens.Peek().Update(time, this);
        }

        public void Draw(SpriteBatch batch)
        {
            Stack<Screen> drawScreens = new Stack<Screen>();
            while (activeScreens.Count > 0)
            {
                drawScreens.Push(activeScreens.Pop());
            }
            while (drawScreens.Count > 0)
            {
                var current = drawScreens.Pop();
                current.Draw(batch);
                activeScreens.Push(current);
            }
        }
    }
}
