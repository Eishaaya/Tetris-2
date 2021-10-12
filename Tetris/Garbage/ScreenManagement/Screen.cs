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
    class Screen
    {
        public int ID { get; }
        protected int caller;
        public SoundEffectInstance introMusic;
        public SoundEffectInstance music;
        protected MouseState mousy;
        public List<Keys> binds;
        protected bool isMouseClicked;
        protected bool uno;
        public bool heldMouse;
        bool introDone;
        protected bool keysDown = false;
        protected KeyboardState Maryland;
        protected bool playMusic;
        public Screen(int num)
            : this(null, null, num) { }
        public Screen(SoundEffect m, int num)
            : this(m, null, num) { }
        public Screen(SoundEffect m, SoundEffect im, int number)
        {
            ID = number;
            playMusic = true;
            Maryland = new KeyboardState();
            mousy = new MouseState();
            isMouseClicked = false;
            uno = false;
            music = null;
            introMusic = null;
            isMouseClicked = false;
            if (m != null)
            {
                music = m.CreateInstance();
                music.IsLooped = true;
                if (im != null)
                {
                    introMusic = im.CreateInstance();
                    introDone = false;
                }
                else
                {
                    introDone = true;
                }
            }
            else
            {
                introDone = true;
            }
        }
        public virtual void changeBinds(List<Keys> newBinds, List<bool> bools)
        {
            playMusic = bools[0];
            if (!playMusic)
            {
                StopMusic();
            }
        }
        public virtual List<bool> GetBools()
        {
            return new List<bool>();
        }
        public void StopMusic()
        {
            if (music != null)
            {
                introDone = false;
                music.Stop();
                if (introMusic != null)
                {
                    introMusic.Stop();
                }
            }
        }
        public virtual void Start(int caller)
        {
            this.caller = caller;
            keysDown = true;
            heldMouse = true;
            if (playMusic)
            {
                if (introMusic == null)
                {
                    if (music != null && music.State != SoundState.Playing)
                    {
                        music.Play();
                    }
                    return;
                }
                if (music.State == SoundState.Playing)
                {
                    return;
                }
                introMusic.Play();
            }
            introDone = false;
        }
        public virtual void Update(GameTime time, Screenmanager manny, bool isActiveWindow)
        {
            Play(time);
            Maryland = Keyboard.GetState();
            if (Maryland.GetPressedKeyCount() == 0)
            {
                keysDown = false;
            }
            if (mousy.LeftButton == ButtonState.Pressed || mousy.RightButton == ButtonState.Pressed)
            {
                heldMouse = true;
            }
            mousy = Mouse.GetState();
            isMouseClicked = false;
            if (mousy.LeftButton == ButtonState.Pressed)
            {
                isMouseClicked = true;
            }
            else
            {
                heldMouse = false;
            }
            uno = false;
            if (mousy.RightButton == ButtonState.Pressed)
            {
                uno = true;
            }
        }

        public void PlayMusic()
        {
            if (!introDone && playMusic && (introMusic == null || introMusic.State == SoundState.Stopped))
            {
                music.Play();
                introDone = true;
            }
        }
        public virtual void Play(GameTime time)
        {
            if (music != null)
            {
                PlayMusic();
            }
        }

        public virtual void Transfer(int transfer)
        {

        }
        public virtual void Reset()
        {

        }
        public virtual void Draw(SpriteBatch batch)
        {

        }
    }
}
