using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        public bool bindsChanged;
        public Screenmanager(List<Screen> screens)
        {
            activeScreens = new Stack<Screen>();
            allScreens = screens;
            activeScreens.Push(allScreens[0]);
            previousScreens = new Stack<Screen>();
            activeScreens.Peek().Start(-1);
        }
        public Screen peek()
        {
            return activeScreens.Peek();
        }

        public void assignBinds()
        {
            for (int i = 0; i < allScreens.Count; i++)
            {
                allScreens[i].changeBinds(allScreens[5].binds, allScreens[5].GetBools());
                bindsChanged = false;
            }
        }

        public void back()
        {
            if (bindsChanged)
            {
                for (int i = 0; i < allScreens.Count; i++)
                {
                    allScreens[i].changeBinds(activeScreens.Peek().binds, activeScreens.Peek().GetBools());
                    bindsChanged = false;
                }
            }
            var callingScreen = activeScreens.Pop();

            callingScreen.StopMusic();
            if (activeScreens.Count > 0)
            {
                if (activeScreens.Peek() != previousScreens.Peek())
                {
                    activeScreens.Pop().StopMusic();
                }
                else
                {
                    previousScreens.Pop();
                    activeScreens.Peek().heldMouse = true;
                    activeScreens.Peek().Start(callingScreen.ID);
                    return;
                }
            }
            activeScreens.Push(previousScreens.Pop());
            activeScreens.Peek().heldMouse = true;
            activeScreens.Peek().Start(callingScreen.ID);
        }
        public void next(int index, bool replace)
        {
            var callingScreen = activeScreens.Peek();
            if (replace)
            {
                activeScreens.Pop();

                callingScreen.StopMusic();
                previousScreens.Push(callingScreen);

                if (activeScreens.Count > 0)
                {
                    activeScreens.Peek().StopMusic();
                    activeScreens.Clear();
                }
                activeScreens.Push(allScreens[index]);
            }
            else
            {
                previousScreens.Push(activeScreens.Peek());
                activeScreens.Push(allScreens[index]);
            }
            activeScreens.Peek().heldMouse = true;
            activeScreens.Peek().Start(callingScreen.ID);
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
