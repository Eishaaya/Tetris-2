using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    //10x20
    class Grid
    {
        Random random;
        Sprite empty;
        Vector2 size;
        List<List<Coordinate>> map;
        public double scale;
        RatPooeys pooey;
        public Keys downKey;
        public Keys leftKey;
        public Keys rightKey;
        public Keys turnKey;
        public Keys TeleKey;
        public List<Keys> switchKeys;
        KeyboardState California;
        Timer NevadaCheck;
        List<List<Vector2>> locations;
        List<bool> symmetry;
        List<Color> colors;
        List<int> spawnChances;
        List<int> values;
        List<float> difficulty;
        List<Vector2> sizes;
        List<ParticleEffect> effects;
        List<List<Vector2>> explosives;
        public int score;
        Sprite image;
        bool fullDown;
        bool downHeld;
        public RatPooeys nextPooey;
        public RatPooeys lastPooey;
        public RatPooeys savedPooey;
        RatPooeys shadowPooey;
        int nevadaready;
        public bool lose;
        public int progression;
        int hold;
        public bool isClassic;
        float tempScale;
        bool saveGo = false;
        TimeSpan lifitime;
        float badFactor;
        float scoreBonus;
        public float freeMoves = 10;
        public bool overused;
        public bool dangerUse;
        SoundEffect rotate;
        SoundEffect land;
        SoundEffect boom;
        SoundEffect zoom;
        SoundEffect boss;
        SoundEffectInstance bossInstance;
        Texture2D explosiveImage;
        Texture2D chonkImage;
        Texture2D speedImage;
        Texture2D particleImage;
        Texture2D speedParticle;
        Texture2D projectionImage;
        Texture2D ReppelentImage;
        Texture2D RepellentSubstance;
        public bool playSounds;
        public bool holdTurn;
        public bool holdDown;
        public bool holdSide;
        public int rowBonus;
        int speedAdd;
        public int SpeedTime { get; private set; }
        public float scoreFactor = 1;
        int changeDone;
        int finishedColors;
        public bool willProject = true;

        Vector2 topOffset = new Vector2(0, 6);
        int boxSize = 60;

        ParticleEffect leaks;

        float swapSpotCutOff
        {
            get
            {
                float tempProg;
                tempProg = (30 - progression) / 6;
                if (tempProg <= 0)
                {
                    tempProg = 0;
                }

                return tempProg;
            }
        }
        float swapTimeCutoff
        {
            get
            {
                float tempProg2;
                tempProg2 = progression / 30;
                if (tempProg2 >= 1)
                {
                    tempProg2 /= 5;
                }
                return tempProg2;
            }
        }


        List<int> explosiveScales = new List<int> { 15, 20, 25 };
        public Grid(Vector2 gridSize, Sprite emptyCoord, List<List<Vector2>> pieceStructures, List<bool> pieceSymmetry, List<Color> pieceColors, List<int> spawningChances, List<int> pieceValues, List<float> pieceDifficulty,
                    List<Vector2> pieceSizes, Sprite coordImage, SoundEffect rotationSound, SoundEffect landingSound, SoundEffect boomSound, SoundEffect speedSound, SoundEffect bossBuzz, Texture2D shadowImage, float totalScale = 1,
                    bool isclassic = false, Texture2D chonkImage = null, Texture2D bombImage = null, Texture2D speedImage = null, Texture2D repellentImage = null, Texture2D repellentGunk = null, Texture2D particle = null, Texture2D zoomParticle = null, int checkingTime = 100, Keys downKey = Keys.S,
                    Keys turnKey = Keys.W, Keys leftKey = Keys.A, Keys rightKey = Keys.D, Keys dropKey = Keys.Space, Keys sidebar1 = Keys.D1, Keys sidebar2 = Keys.D2, Keys sidebar3 = Keys.D3, Keys sidebar4 = Keys.D4)
        {
            SpeedTime = 0;
            finishedColors = -1;
            changeDone = -1;
            speedAdd = 1;
            rowBonus = 1;
            playSounds = true;
            holdDown = true;
            holdSide = true;
            holdTurn = true;
            particleImage = particle;
            speedParticle = zoomParticle;
            switchKeys = new List<Keys>();
            rotate = rotationSound;
            land = landingSound;
            if (!isclassic)
            {
                zoom = speedSound;
                boom = boomSound;
                boss = bossBuzz;
                bossInstance = boss.CreateInstance();
                bossInstance.IsLooped = true;
            }
            effects = new List<ParticleEffect>();
            lose = false;
            nevadaready = checkingTime;
            score = 0;
            random = new Random();
            NevadaCheck = new Timer(new TimeSpan(0, 0, 0, 0, nevadaready));
            this.downKey = downKey;
            image = coordImage;
            this.leftKey = leftKey;
            this.rightKey = rightKey;
            this.turnKey = turnKey;
            TeleKey = dropKey;
            switchKeys.Add(sidebar1);
            switchKeys.Add(sidebar2);
            switchKeys.Add(sidebar3);
            switchKeys.Add(sidebar4);
            scale = totalScale;
            size = new Vector2(gridSize.X, gridSize.Y + 6);
            empty = emptyCoord;
            locations = pieceStructures;
            symmetry = pieceSymmetry;
            colors = pieceColors;
            values = pieceValues;
            sizes = pieceSizes;
            spawnChances = spawningChances;
            difficulty = pieceDifficulty;
            isClassic = isclassic;
            badFactor = 0;
            explosiveImage = bombImage;
            ReppelentImage = repellentImage;
            RepellentSubstance = repellentGunk;
            this.chonkImage = chonkImage;
            this.speedImage = speedImage;
            projectionImage = shadowImage;
            map = new List<List<Coordinate>>();
            for (int i = 0; i < size.X; i++)
            {
                map.Add(new List<Coordinate>());
                for (int j = 0; j < size.Y; j++)
                {
                    map[i].Add(new Coordinate(new Sprite(empty.Image, empty.Location, Color.White, empty.rotation, empty.effect, empty.Origin, (float)scale, empty.Depth), new Vector2(i, j), 0, 0, 0, 0, false));
                    Vector2 oragami = new Vector2(map[i][j].Image.Origin.X * (float)scale, map[i][j].Image.Origin.Y * (float)scale);
                    map[i][j].Image.Location = new Vector2(i * (float)Math.Round(60 * scale), j * (float)Math.Round(60 * scale) - (float)Math.Round(360 * scale)) + oragami;
                }
            }
            generate(4);
            if (willProject)
            {
                shadowPlace();
            }
            leaks = new ParticleEffect();
            effects.Add(leaks);
        }

        public void Reset()
        {
            overused = false;
            SpeedTime = 0;
            finishedColors = -1;
            changeDone = -1;
            speedAdd = 1;
            rowBonus = 1;
            savedPooey = null;
            freeMoves = 10;
            scoreBonus = 0;
            badFactor = 0;
            hold = 0;
            progression = 0;
            lose = false;
            score = 0;
            map = new List<List<Coordinate>>();
            NevadaCheck = new Timer(new TimeSpan(0, 0, 0, 0, nevadaready));
            for (int i = 0; i < size.X; i++)
            {
                map.Add(new List<Coordinate>());
                for (int j = 0; j < size.Y; j++)
                {
                    map[i].Add(new Coordinate(new Sprite(empty.Image, empty.Location, Color.White, empty.rotation, empty.effect, empty.Origin, (float)scale, empty.Depth), new Vector2(i, j), 0, 0, 0, 0, false));
                    Vector2 oragami = new Vector2(map[i][j].Image.Origin.X * (float)scale, map[i][j].Image.Origin.Y * (float)scale);
                    map[i][j].Image.Location = new Vector2(i * (float)Math.Round(60 * scale), j * (float)Math.Round(60 * scale) - (float)Math.Round(360 * scale)) + oragami;
                }
            }
            generate(3);
            if (willProject)
            {
                shadowPlace();
            }
        }

        public bool CanSwap()
        {
            return lifitime.Seconds <= 2 - swapTimeCutoff && pooey.boxes[0].GridSpot.Y - 5 <= 5 + swapSpotCutOff && freeMoves >= 5 && !overused;
        }

        public void Switch(int switcher)
        {
            saveGo = false;
            if (overused || isClassic)
            {
                return;
            }
            badFactor++;
            var savedPieceState = savedPooey;
            if (switcher == 0)
            {
                savedPooey = nextPooey;
                if (savedPieceState == null)
                {
                    nextPooey = lastPooey;
                    lastPooey = gen();
                    lastPooey.Display(new Vector2(450, 540), 140);
                }
                else
                {
                    nextPooey = savedPieceState;
                }
                savedPooey.Display(new Vector2(450, 740), 140);
                nextPooey.Display(new Vector2(450, 340), 140);
                freeMoves--;
                return;
            }
            if (switcher == 1)
            {
                savedPooey = lastPooey;
                if (savedPieceState == null)
                {
                    lastPooey = gen();
                }
                else
                {
                    lastPooey = savedPieceState;
                }
                savedPooey.Display(new Vector2(450, 740), 140);
                lastPooey.Display(new Vector2(450, 540), 140);
                freeMoves--;
                badFactor += 4;
                return;
            }
            if (switcher == 2)
            {
                if (savedPooey == null)
                {
                    if (CanSwap())
                    {
                        freeMoves -= 5;
                        pooey.Revert();
                        savedPooey = pooey;
                        generate();
                        savedPooey.Display(new Vector2(450, 740), 140);
                        badFactor++;
                    }
                    return;
                }
                else if (CanSwap())
                {
                    bossInstance.Stop();
                    freeMoves -= 5;
                    pooey.Revert();
                    var temperPooey = pooey;
                    saveGo = true;
                    generate();
                    savedPooey = temperPooey;
                    savedPooey.Display(new Vector2(450, 740), 140);
                    badFactor++;
                    return;
                }
                saveGo = true;
                return;
            }
            if (switcher == 3)
            {
                if (savedPooey != null)
                {
                    saveGo = true;
                }
            }
        }

        public void generate()
        {
            lifitime = TimeSpan.Zero;
            fullDown = false;
            if (savedPooey == null)
            {
                saveGo = false;
            }
            if (saveGo)
            {
                pooey = savedPooey;
                pooey.Ready();
                if (willProject)
                {
                    shadowPooey = new RatPooeys(pooey);
                    shadowPlace();
                }
                savedPooey = null;
                saveGo = false;
                if (!isClassic)
                {
                    bossInstance.Pause();
                }
                if (pooey.boxes.Count > 10)
                {
                    bossInstance.Play();
                }
                return;
            }
            if (!isClassic)
            {
                badFactor--;
                if (badFactor <= 0)
                {
                    scoreBonus += 3;
                    if (scoreBonus > 30)
                    {
                        scoreBonus = 3;
                    }
                    badFactor = 0;
                }
                else
                {
                    scoreBonus -= 9;
                    if (scoreBonus <= 0)
                    {
                        scoreBonus = 0;
                    }
                }
                freeMoves++;
                if (freeMoves >= 15)
                {
                    overused = false;
                }
                if (freeMoves > 15)
                {
                    freeMoves = 15;
                }
            }
            generate(1);
            if (boss != null)
            {
                bossInstance.Pause();
                if (pooey.boxes.Count > 10)
                {
                    boss.Play();
                }
            }

        }
        public bool ChangeBackColor(Color newColor, int speed = 5)
        {
            var doneUP = false;
            var allTouched = true;
            if (changeDone < 0)
            {
                changeDone = 0;
                return false;
            }
            changeDone++;
            int xSize = changeDone / speed;
            int ySIze = changeDone / speed + 6;
            if (xSize > map.Count)
            {
                xSize = map.Count;
            }
            if (ySIze > map[0].Count)
            {
                ySIze = map[0].Count;
            }
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 6; j < ySIze; j++)
                {
                    if (!map[i][j].IsFull)
                    {
                        if (map[i][j].Image.Color == newColor || i <= finishedColors && j <= finishedColors + 6)
                        {
                            if (i > finishedColors || j > finishedColors + 6)
                            {
                                doneUP = true;
                                //Console.WriteLine($"{i}  {j - 6}");
                            }
                            map[i][j].Image.ChangeColor(empty.Color, .05f);
                        }
                        else
                        {
                            map[i][j].Image.ChangeColor(newColor, .05f);
                            allTouched = false;
                        }
                    }
                }
            }
            if (doneUP)
            {
                finishedColors++;
            }
            if (allTouched && xSize == map.Count && ySIze == map[0].Count)
            {
                var done = true;
                for (int q = 0; q < xSize; q++)
                {
                    for (int e = 6; e < ySIze; e++)
                    {
                        if (!map[q][e].IsFull)
                        {
                            if (map[q][e].Image.Color != Color.White)
                            {
                                done = false;
                                break;
                            }
                            else
                            {
                                ;
                            }
                        }
                    }
                }
                if (done)
                {
                    changeDone = -1;
                    finishedColors = -1;
                    return true;
                }
            }
            return false;
        }
        public void generate(int num)
        {
            if (num >= 4)
            {
                if (isClassic)
                {
                    pooey = gen();
                    pooey.Ready();
                    if (willProject)
                    {
                        shadowPooey = new RatPooeys(pooey);
                        shadowPlace();
                    }
                    nextPooey = gen();
                    nextPooey.Display(new Vector2(450, 340), 140);
                    lastPooey = gen();
                    lastPooey.Display(new Vector2(440, 540), 140);
                    savedPooey = gen();
                    savedPooey.Display(new Vector2(440, 740), 140);
                    return;
                }
                else
                {
                    num = 3;
                }
            }
            if (num >= 3)
            {
                pooey = gen();
                pooey.Ready();
                if (willProject)
                {
                    shadowPooey = new RatPooeys(pooey);
                    shadowPlace();
                }
                nextPooey = gen();
                nextPooey.Display(new Vector2(450, 340), 140);
                lastPooey = gen();
                lastPooey.Display(new Vector2(440, 540), 140);
                return;
            }
            if (num >= 2)
            {
                pooey = nextPooey;
                pooey.Ready();
                if (willProject)
                {
                    shadowPooey = new RatPooeys(pooey);
                    shadowPlace();
                }
                nextPooey = gen();
                nextPooey.Display(new Vector2(440, 340), 140);
                lastPooey = gen();
                lastPooey.Display(new Vector2(440, 540), 140);
                return;
            }
            pooey = nextPooey;
            pooey.Ready();
            if (willProject)
            {
                shadowPooey = new RatPooeys(pooey);
                shadowPlace();
            }
            nextPooey = lastPooey;
            nextPooey.Display(new Vector2(440, 340), 140);
            lastPooey = gen();
            if (isClassic)
            {
                if (savedPooey == null)
                {
                    savedPooey = gen();
                }
                lastPooey = savedPooey;
                savedPooey = gen();
                savedPooey.Display(new Vector2(440, 740), 140);
            }
            lastPooey.Display(new Vector2(440, 540), 140);
        }
        //SPAWNING
        public RatPooeys gen()
        {
            int ex = 0;
            int rep = 0;
            bool sp = false;
            int spawnIndex;
            int chonk = 0;
            hold--;
            while (true)
            {
                spawnIndex = random.Next(0, locations.Count);
                float spawnRoll = random.Next(0, 100);
                var diffTemp = progression;
                var diffAdd = badFactor * difficulty[spawnIndex] / Math.Abs(difficulty[spawnIndex]);
                if (difficulty[spawnIndex] == 0)
                {
                    diffAdd = 0;
                }
                if (badFactor < 0)
                {
                    badFactor = 0;
                }
                float diffFactor = (100 + difficulty[spawnIndex] * diffTemp) / 100;
                float helpFactor = (100 - difficulty[spawnIndex] * ((hold - 22) * (4 - progression / 25))) / 100;
                if (hold <= 15)
                {
                    helpFactor = 1;
                }

                //special pieces
                if (spawnIndex == spawnChances.Count - 1 && !isClassic)
                {
                    if (hold <= 0 && (spawnChances[spawnIndex] * diffFactor) >= spawnRoll - diffAdd)
                    {
                        hold = 35;
                        freeMoves += 10;
                        break;
                    }
                }
                else if ((spawnChances[spawnIndex] * diffFactor * helpFactor) >= spawnRoll - diffAdd)
                {
                    if (!isClassic)
                    {
                        if (spawnRoll <= (Math.Sqrt(progression) + 1) && hold <= 5)
                        {
                            chonk = 3 + (int)Math.Pow(progression, .42);
                            break;
                        }
                        spawnRoll = random.Next(1000);
                        if (spawnRoll <= 50 + progression / 5)
                        {
                            ex = (int)(spawnRoll - 1) / 17 + 1;
                            break;
                        }
                        spawnRoll = random.Next(1000);

                        if (spawnRoll <= 42 + progression / 4)
                        {
                            rep = (int)(spawnRoll - 1) / 14 + 1;
                            break;
                        }
                        spawnRoll = random.Next(1000);
                        if (spawnRoll <= 30 * (progression / 10 + 1) && hold <= 5)
                        {
                            sp = true;
                        }
                    }
                    break;
                }
            }

            //blah = 4;
            var temp = progression * 5;
            var currentSpeed = temp;
            if (speedAdd != 1)
            {
                temp += speedAdd;
            }
            if (temp > 325)
            {
                if (speedAdd != 1)
                {
                    if (temp > 400)
                    {
                        temp = 400;
                    }
                }
                else
                {
                    temp = 325;
                }
            }
            return new RatPooeys(new Sprite(image.Image, image.Location, image.Color, image.rotation, image.effect, image.Origin, (float)scale, image.Depth), locations[spawnIndex], sizes[spawnIndex],
                                            colors[spawnIndex], values[spawnIndex], (float)scale, symmetry[spawnIndex], 650 - temp, chonk, chonkImage, sp, speedImage, ex, explosiveImage, rep,
                                            ReppelentImage, RepellentSubstance);
        }
        public void Update(GameTime gameTime)
        {
            if (pooey.Explosive > 0 && willProject)
            {
                shadowPooey = new RatPooeys(pooey);
                shadowPlace();
            }
            if (SpeedTime <= 0)
            {
                SpeedTime = 0;
                speedAdd = 1;
            }
            scoreFactor = ((100 + scoreBonus + progression) * (speedAdd / 100 + 1) / 100) * rowBonus;
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].Update(gameTime);
                if (effects[i] != leaks && effects[i].FullFaded)
                {
                    effects.RemoveAt(i);
                }
            }
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    var currentCoord = map[i][j];

                    if (currentCoord.Animate())
                    {
                        AddLeakParticle(currentCoord);
                    }

                    if (currentCoord.Pusher.IsPushing)
                    {
                        Push(i, j, currentCoord);
                    }
                }
            }
            if (freeMoves <= 0)
            {
                overused = true;
            }
            else if (freeMoves <= 5)
            {
                dangerUse = true;
            }
            else
            {
                dangerUse = false;
            }
            if (pooey.boxes[0].GridSpot.Y + pooey.pieceSize.Y / 120 >= 6)
            {
                lifitime += gameTime.ElapsedGameTime;
            }
            var tempTexas = California;
            California = Keyboard.GetState();
            for (int i = 0; i < switchKeys.Count; i++)
            {
                if (California.IsKeyDown(switchKeys[i]) && !tempTexas.IsKeyDown(switchKeys[i]))
                {
                    Switch(i);
                }
            }
            if (!fullDown)
            {
                if (California.IsKeyDown(downKey) && (holdDown || !tempTexas.IsKeyDown(downKey)))
                {
                    if (NevadaCheck.Ready())
                    {
                        pooey.MoveDown();
                    }
                }
                else if (California.IsKeyDown(turnKey) && (holdTurn || !tempTexas.IsKeyDown(turnKey)))
                {
                    if (NevadaCheck.Ready())
                    {
                        if (pooey.Rotate())
                        {
                            if (willProject)
                            {
                                shadowPooey = new RatPooeys(pooey);
                                shadowPlace();
                            }
                            if (playSounds)
                            {
                                rotate.Play();
                            }
                        }
                    }
                }
                else if (California.IsKeyDown(leftKey) && (holdSide || !tempTexas.IsKeyDown(leftKey)))
                {
                    if (NevadaCheck.Ready())
                    {
                        pooey.MoveSide(-1);
                        if (willProject)
                        {
                            shadowPooey = new RatPooeys(pooey);
                            shadowPlace();
                        }
                    }
                }
                else if (California.IsKeyDown(rightKey) && (holdSide || !tempTexas.IsKeyDown(rightKey)))
                {
                    if (NevadaCheck.Ready())
                    {
                        pooey.MoveSide();
                        if (willProject)
                        {
                            shadowPooey = new RatPooeys(pooey);
                            shadowPlace();
                        }
                    }
                }
                shadowPooey.Animate();
                foreach (var shadowBox in shadowPooey.boxes)
                {
                    if (shadowBox.Reppellent == 2)
                    {
                        shadowBox.Image.rotation += MathHelper.ToDegrees(3);
                    }
                    else if (shadowBox.Reppellent == 3)
                    {
                        shadowBox.Image.Rotate(-180, .1f, false);
                    }
                }
                if (!(California.IsKeyDown(downKey) && (holdDown || !tempTexas.IsKeyDown(downKey))))
                {
                    Coordinate leakCoord = pooey.Update(gameTime);
                    if (leakCoord != null)
                    {
                        AddLeakParticle(leakCoord);
                    }
                }
                else
                {
                    Coordinate leakCoord = pooey.Animate();
                    if (leakCoord != null)
                    {
                        AddLeakParticle(leakCoord);
                    }
                }
            }
            if (California.IsKeyDown(TeleKey))
            {
                if (!downHeld)
                {
                    fullDown = true;
                    pooey.goDown = true;
                }
                downHeld = true;
            }
            else
            {
                downHeld = false;
            }
            if (fullDown)
            {
                pooey.MoveDown();
                Coordinate leakCoord = pooey.Animate();
                if (leakCoord != null)
                {
                    AddLeakParticle(leakCoord);
                }
            }

            NevadaCheck.Tick(gameTime);
            int result = istouching();
            if (result > 0)
            {
                if (result == 2)
                {
                    lose = true;
                    return;
                }
                if (playSounds)
                {
                    land.Play();
                }
                score += (int)(pooey.score * scoreFactor);
                if (score - 3500 >= progression * 1000 && progression < 100)
                {
                    progression++;
                }
                explosives = new List<List<Vector2>>();
                if (!hippityHoppityYourRowIsNowMyProperty())
                {
                    for (int i = 0; i < explosives.Count; i++)
                    {
                        for (int j = 0; j < explosives[i].Count; j++)
                        {
                            var explosiveSpot = map[(int)explosives[i][j].X][(int)explosives[i][j].Y];


                            if (explosiveSpot.Explosive > 0)
                            {
                                explosives.Add(explosiveSpot.Explode(map));
                                CreateExplosionParticles(explosiveSpot);
                            }
                            score += (int)(explosiveSpot.Score * scoreFactor);
                            if (explosiveSpot.Chonk <= 1)
                            {
                                explosiveSpot.empty(empty);
                                explosiveSpot.Image.Scale = (float)scale;
                            }
                            else
                            {
                                explosiveSpot.reduceChonk();
                            }
                            //explosiveSpot.image.Color = Color.Cyan;
                        }
                    }
                    rowBonus++;
                    if (boom != null)
                    {
                        if (playSounds)
                        {
                            boom.Play();
                        }
                    }
                    for (int i = 0; i < map.Count; i++)
                    {
                        for (int j = 0; j < map[i].Count; j++)
                        {
                            if (map[i][j].Image.Scale > scale && map[i][j].Explosive == 0)
                            {
                                map[i][j].Image.Scale = (float)scale;
                            }
                        }
                    }
                }
                else
                {
                    SpeedTime--;
                    if (rowBonus > 1)
                    {
                        rowBonus /= 2;
                    }
                }
                generate();
            }
        }
        public int istouching()
        {
            return (istouching(pooey));
        }
        public int istouching(RatPooeys piece)
        {
            for (int i = 0; i < piece.boxes.Count; i++)
            {
                if (piece.boxes[i].GridSpot.Y >= size.Y || map[(int)piece.boxes[i].GridSpot.X][(int)piece.boxes[i].GridSpot.Y].IsFull)
                {
                    bool good = true;
                    if (piece.rotated)
                    {
                        good = false;
                        piece.Rotate(-1);
                        if (willProject)
                        {
                            shadowPooey = new RatPooeys(piece);
                            shadowPlace();
                        }
                        piece.rotated = false;
                    }
                    if (piece.sideways != 0)
                    {
                        piece.ForceSide(-piece.sideways);
                        if (willProject)
                        {
                            shadowPooey = new RatPooeys(piece);
                            shadowPlace();
                        }
                        piece.sideways = 0;
                        return 0;
                    }
                    if (!good)
                    {
                        return 0;
                    }
                    if (piece.boxes[i].GridSpot.Y - 1 < 0)
                    {
                        return 2;
                    }
                    if (piece.boxes[i].GridSpot.Y >= size.Y || !fullDown && map[(int)piece.boxes[i].GridSpot.X][(int)piece.boxes[i].GridSpot.Y - 1].IsFull)
                    {
                        for (int j = 0; j < piece.boxes.Count; j++)
                        {
                            if (piece.boxes[j].GridSpot.Y - 1 < 0)
                            {
                                return 2;
                            }
                            map[(int)piece.boxes[j].GridSpot.X][(int)piece.boxes[j].GridSpot.Y - 1].fill(piece.boxes[j]);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < piece.boxes.Count; j++)
                        {
                            if (piece.boxes[j].GridSpot.Y - 1 < 0)
                            {
                                return 2;
                            }
                            map[(int)piece.boxes[j].GridSpot.X][(int)piece.boxes[j].GridSpot.Y - 1].fill(piece.boxes[j]);
                        }
                    }
                    return 1;
                }
            }
            for (int i = 0; i < piece.boxes.Count; i++)
            {
                if (piece.goDown && (piece.boxes[i].GridSpot.Y + 1 >= size.Y || !fullDown && map[(int)piece.boxes[i].GridSpot.X][(int)piece.boxes[i].GridSpot.Y + 1].IsFull))
                {
                    if (piece.sideways != 0)
                    {
                        piece.MoveSide(-piece.sideways);
                        piece.sideways = 0;
                        return 0;
                    }
                    for (int j = 0; j < piece.boxes.Count; j++)
                    {
                        if (piece.boxes[j].GridSpot.Y < 0)
                        {
                            return 2;
                        }
                        map[(int)piece.boxes[j].GridSpot.X][(int)piece.boxes[j].GridSpot.Y].fill(piece.boxes[j]);
                    }
                    return 1;
                }
            }

            piece.sideways = 0;
            piece.rotated = false;
            return 0;
        }
        public bool hippityHoppityYourRowIsNowMyProperty()
        {
            for (int i = 0; i < size.Y; i++)
            {
                bool fullChonk = true;
                bool isFull = true;
                for (int j = 0; j < size.X; j++)
                {
                    if (!map[j][i].IsFull)
                    {
                        isFull = false;
                    }
                    if (!map[j][i].Chonker())
                    {
                        fullChonk = false;
                    }
                }
                if (isFull)
                {
                    for (int j = 0; j < size.X; j++)
                    {
                        var currentSpot = map[j][i];

                        score += (int)(currentSpot.Score * scoreFactor);
                        if (currentSpot.Chonk == 0 || fullChonk)
                        {
                            if (!fullChonk)
                            {
                                //special piece conditions
                                if (currentSpot.Explosive > 0)
                                {
                                    CreateExplosionParticles(currentSpot);

                                    var deads = currentSpot.Explode(map);
                                    explosives.Add(deads);
                                }
                                else if (currentSpot.Speed)
                                {
                                    speedAdd = 200;
                                    SpeedTime = 10;
                                    if (playSounds)
                                    {
                                        zoom.Play();
                                    }
                                    effects.Add(new ParticleEffect(ParticleEffect.EffectType.Ray, speedParticle, currentSpot.Image.Location,
                                                new List<Color> { Color.White, Color.White }, 50, 5, new List<double> { 10, 10 },
                                                new List<float> { 10 / particleImage.Width, 10 / particleImage.Width }, new List<int> { 1, 1 }, null, 1, 30, 30, 1, 0));
                                }
                                else if (currentSpot.Reppellent > 0)
                                {
                                    ReleaseReppellingParticles(currentSpot);
                                    currentSpot.Repel(map);
                                }
                            }
                            currentSpot.empty(empty);
                            for (int q = i; q > 0; q--)
                            {
                                if (map[j][q - 1].IsFull)
                                {
                                    map[j][q - 1].Image.Scale = (float)scale;
                                    map[j][q - 1].Image.offset = Vector2.Zero;
                                    map[j][q].fill(map[j][q - 1]);
                                }
                                else
                                {
                                    map[j][q].empty(empty);
                                }

                            }

                            map[j][0] = new Coordinate(new Sprite(empty.Image, empty.Location, Color.White, empty.rotation, empty.effect, empty.Origin, (float)scale, empty.Depth), new Vector2(j, 0), map[j][0].Score, 0, 0, 0, false);
                            Vector2 oragami = new Vector2(map[j][0].Image.Origin.X * (float)scale, map[j][0].Image.Origin.Y * (float)scale);
                            map[j][0].Image.Location = new Vector2(j * (float)Math.Round(60 * scale), 0 * (float)Math.Round(60 * scale) - (float)Math.Round(360 * scale)) + oragami;
                        }
                        else
                        {
                            currentSpot.reduceChonk();
                        }

                    }
                    int bonusX = 2;
                    while (true)
                    {
                        if (hippityHoppityYourRowIsNowMyProperty())
                        {
                            break;
                        }
                        rowBonus += bonusX;
                        bonusX++;
                    }
                    return false;
                }
            }
            return true;
        }

        void CreateExplosionParticles(Coordinate explosiveSpot)
        {
            effects.Add(new ParticleEffect(ParticleEffect.EffectType.Explosion, particleImage, explosiveSpot.Image.Location, new List<Color> { Color.OrangeRed, Color.Crimson, Color.Black },
                               200, (int)explosiveSpot.Explosive - 2, new List<double> { 1 * explosiveSpot.Explosive, 2 * explosiveSpot.Explosive, 3 * explosiveSpot.Explosive },
                               new List<float> { 1f * explosiveSpot.Explosive / explosiveImage.Width, 2f * explosiveSpot.Explosive / particleImage.Width, 3f * explosiveSpot.Explosive / particleImage.Width }
                               , explosiveScales, null, 200, 0, 0, 1, 1, true, 3, 3 + 3 * (int)(explosiveSpot.Explosive / 2)));
        }

        void ReleaseReppellingParticles(Coordinate repellingSpot)
        {
            float maxScale = (2 * repellingSpot.Reppellent + 1) * empty.Image.Width * (float)scale / particleImage.Width;

            effects.Add(new ParticleEffect(ParticleEffect.EffectType.Explosion, particleImage, repellingSpot.Image.Location, new List<Color> { Color.Lime },
                   1, (int)repellingSpot.Reppellent * 150, new List<double> { 0 },
                   new List<float> { repellingSpot.Reppellent * repellingSpot.Reppellent * 5 / particleImage.Width }, new List<int>() { 50 }, null, 300, 0, 0, 1, 1, false, 3, 3 + 3 * (int)(4 - repellingSpot.Reppellent), maxScale));
        }

        void AddLeakParticle(Coordinate leaker)
        {
            var spawnLocation = new Vector2(random.Next((int)(leaker.Image.Location.X - leaker.Image.Origin.X / 2), (int)(leaker.Image.Location.X + leaker.Image.Origin.Y)),
                                random.Next((int)(leaker.Image.Location.Y - leaker.Image.Origin.Y / 2), (int)(leaker.Image.Location.Y + leaker.Image.Origin.Y)));
            var fallSpeed = (leaker.Reppellent - 1) * (leaker.Reppellent - 1);

            leaks.AddParticle(new Particle(particleImage, spawnLocation, Color.Lime, Color.DarkOliveGreen, 0, SpriteEffects.None, new Vector2(particleImage.Width / 2, particleImage.Height / 2), new Vector2(random.Next(-500, 500) / 1000, fallSpeed),
                                          .1f, (int)(170 * fallSpeed), new Vector2(0), .2f, 1, 0, true, 0, 3, 1));
        }

        void Push(int x, int y, Coordinate currentCoord)
        {
            if (currentCoord.Pusher.CanMove(x, y, map))
            {
                var newSpot = currentCoord.Pusher.GetNewSpot(x, y);
                var newPlace = new Vector2(newSpot.Item1, newSpot.Item2);
                var nextSpot = map[newSpot.Item1][newSpot.Item2];

                map[x][y] = Coordinate.Clone(currentCoord);
                map[x][y].empty(empty);

                if (nextSpot.IsFull)
                {
                    nextSpot.Pusher = new PushController(currentCoord.Pusher);
                    Push(newSpot.Item1, newSpot.Item2, nextSpot);
                }
                if (!currentCoord.Pusher.IsPushing)
                {
                    currentCoord.Pusher = PushController.None();
                }
                currentCoord.GridSpot = newPlace;
                currentCoord.Image.Location = ((newPlace - topOffset) * boxSize + currentCoord.Image.Origin) * (float)scale;
                map[newSpot.Item1][newSpot.Item2] = currentCoord;
            }
        }

        void shadowPlace()
        {
            for (int i = 0; i < shadowPooey.boxes.Count; i++)
            {
                var shadowBox = shadowPooey.boxes[i];

                if (shadowBox.Explosive > 0)
                {
                    shadowBox.Image.Color = Color.Red;
                }
                else if (shadowBox.Speed)
                {
                    shadowBox.Image.Color = Color.Cyan;
                }
                else if (shadowBox.Chonker())
                {
                    shadowBox.SecondaryImage = null;
                    shadowBox.Image.Color = Color.Black;
                }
                else if (shadowBox.Reppellent > 0)
                {
                    shadowBox.Image.Color = Color.LimeGreen;
                    shadowBox.SecondaryImage = null;
                }
                else
                {
                    shadowBox.Image.Color = Color.DarkSlateGray;
                }
                shadowBox.Image.Image = projectionImage;
                shadowBox.Image.Depth -= .01f;
            }
            while (true)
            {
                shadowPooey.MoveDown();
                for (int i = 0; i < shadowPooey.boxes.Count; i++)
                {
                    if ((int)shadowPooey.boxes[i].GridSpot.Y >= map[0].Count || map[(int)shadowPooey.boxes[i].GridSpot.X][(int)shadowPooey.boxes[i].GridSpot.Y].IsFull)
                    {
                        shadowPooey.ForceUp();
                        return;
                    }
                }
                for (int i = 0; i < shadowPooey.boxes.Count; i++)
                {
                    if (shadowPooey.boxes[i].GridSpot.Y >= size.Y - 1)
                    {
                        return;
                    }
                }
            }
        }
        public void Draw(SpriteBatch bunch)
        {
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 6; j < map[i].Count; j++)
                {
                    map[i][j].Draw(bunch);
                }
            }
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].Draw(bunch);
            }
            if (shadowPooey.boxes[0].GridSpot == pooey.boxes[0].GridSpot)
            {
                ;
            }
            pooey.Draw(bunch);
            nextPooey.Draw(bunch);
            lastPooey.Draw(bunch);
            if (willProject)
            {
                shadowPooey.Draw(bunch);
            }
            if (savedPooey != null)
            {
                savedPooey.Draw(bunch);
            }
        }
    }
}