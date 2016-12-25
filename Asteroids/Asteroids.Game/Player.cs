using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Rendering;
using SiliconStudio.Xenko.Audio;
using SiliconStudio.Xenko.Games.Time;
using System.Diagnostics;

namespace Asteroids
{
    public struct HighScoreListMesh
    {
        Number nRank;
        Number nScore;
        Word wName;

        public Number Rank
        {
            get
            {
                return nRank;
            }

            set
            {
                nRank = value;
            }
        }

        public Number Score
        {
            get
            {
                return nScore;
            }

            set
            {
                nScore = value;
            }
        }

        public Word Name
        {
            get
            {
                return wName;
            }

            set
            {
                wName = value;
            }
        }
    }

    public class Player : Actor
    {
        List<Shot> l_Shots;

        public List<Shot> m_Shots
        {
            get
            {
                return l_Shots;
            }

            set
            {
                l_Shots = value;
            }
        }

        bool b_Spawn = false;

        public bool m_Spawn
        {
            get
            {
                return b_Spawn;
            }

            set
            {
                b_Spawn = value;
            }
        }

        bool b_Exploding = false;

        public bool m_Exploding
        {
            get
            {
                return b_Exploding;
            }
        }

        Entity m_Score;
        Entity m_HighScore;
        Entity m_AtariDateEntity;
        Entity m_AtariEntity;
        Entity m_GameOverEntity;
        Entity m_PushStartEntity;
        Entity m_CoinPlayEntity;
        Entity m_HighScoreEntity;
        Entity m_NewHighScoreLettersEntity;
        Entity[] m_CoinPlayNumbersEntity = new Entity[2];
        Entity[] m_EnterYourInitialsEntitys = new Entity[4];
        Entity m_Ship;
        Entity m_ShipFlame;
        Model m_ShipModel;
        List<Entity> m_DisplayShips;
        List<Line> m_Explosion;
        ModelComponent m_FlameMesh;
        ModelComponent m_ShipMesh;
        bool m_FirstRun = true;
        public bool m_NewHighScore = false;
        bool m_NoHighScore = true;

        int m_Lives = 0;
        int m_TotalScore = 0;
        int m_TotalHighScore = 0;
        int m_PointsToNextExtraShip = 0;
        int m_PointsForExtraShip = 5000;
        int m_HighScoreSelector = 0;
        int m_NewHighScoreRank = 0;

        string m_GameOverText = "GAME OVER";
        string m_Ataritext = "ATARI INC";
        int m_AtariDate = 1979;
        string m_PushStartText = "PUSH START";
        string m_CoinPlayText = "COIN   PLAY";
        string m_HighScoresText = "HIGH SCORES";
        string[] m_EnterYourInitials = new string[4];
        char[] m_HighScoreSelectedLetters = new char[3];

        SoundInstance m_FireSoundInstance;
        SoundInstance m_ThurstSoundInstance;
        SoundInstance m_ExplodeSoundInstance;
        SoundInstance m_BonusSoundInstance;

        HighScoreListMesh[] m_HighScoreListMesh = new HighScoreListMesh[10];
        HighScoreData[] m_HighScoreList = new HighScoreData[10];
        GameData m_Data = new GameData();

        TimerTick m_FlameTimer;

        public override void Start()
        {
            m_PointsToNextExtraShip = m_PointsForExtraShip;
            m_Radius = 1.15f;
            m_FlameTimer = new TimerTick();
            m_Shots = new List<Shot>();
            m_DisplayShips = new List<Entity>();
            m_Explosion = new List<Line>();

            Initilize();
            InitializeAudio();
            ShipLives();

            this.Entity.AddChild(m_Ship);
        }

        public override void Update()
        {
            if (m_FirstRun)
            {
                FirstTime();
            }

            if (!m_Pause)
            {
                m_FlameTimer.Tick();

                if (!m_Hit && !m_GameOver)
                {
                    base.Update();
                    GetInput();
                    CheckForEdge();
                }

                if (m_Hit)
                {
                    if (m_Spawn && !b_Exploding)
                    {
                        Reset();
                    }

                    if (b_Exploding)
                    {
                        bool active = false;

                        foreach (Line line in m_Explosion)
                        {
                            if (line.Active())
                            {
                                active = true;
                                break;
                            }
                        }

                        if (!active)
                            b_Exploding = false;
                    }
                }
            }

            if (m_GameOver && !b_Exploding)
            {
                m_Hit = false;
                GameOver();

                if (m_NewHighScore)
                {
                    HideHighScoreList();                    
                    NewHighScore();
                }
            }
        }

        public void SetScore(int points)
        {
            m_TotalScore += points;

            if (m_TotalScore > m_PointsToNextExtraShip)
            {
                BunusLife();
                m_PointsToNextExtraShip += m_PointsForExtraShip;
            }

            if (m_TotalScore > m_TotalHighScore)
            {
                m_TotalHighScore = m_TotalScore;
            }

            m_Score.Components.Get<Number>().ProcessNumber(m_TotalScore, new Vector3(m_Edge.X * 0.5f, m_Edge.Y + 0.15f, 0), 1);
            m_HighScore.Components.Get<Number>().ProcessNumber(m_TotalHighScore, new Vector3(0.5f, m_Edge.Y - 0.5f, 0), 0.666f);
        }

        public void FirstTime()
        {
            if (m_Score.Components.Get<Number>().m_Numbers != null)
            {
                m_AtariDateEntity.Components.Get<Number>().ProcessNumber(m_AtariDate, new Vector3(3.5f, -m_Edge.Y + 1, 0), 0.5f);
                m_AtariEntity.Components.Get<Word>().ProcessWords(m_Ataritext, new Vector3(4.5f, -m_Edge.Y + 1, 0), 0.25f);
                m_PushStartEntity.Components.Get<Word>().ProcessWords(m_PushStartText, new Vector3(30, m_Edge.Y - 9, 0), 1);
                m_CoinPlayEntity.Components.Get<Word>().ProcessWords(m_CoinPlayText, new Vector3(33, -m_Edge.Y + 10, 0), 1);
                m_CoinPlayNumbersEntity[0].Get<Number>().ProcessNumber(1, new Vector3(25, -m_Edge.Y + 10, 0), 2);
                m_CoinPlayNumbersEntity[1].Get<Number>().ProcessNumber(1, new Vector3(3, -m_Edge.Y + 10, 0), 2);
                m_GameOverEntity.Components.Get<Word>().ProcessWords(m_GameOverText, new Vector3(27, m_Edge.Y - 4, 0), 1);
                m_GameOverEntity.Components.Get<Word>().HideWords();
                m_HighScoreEntity.Components.Get<Word>().ProcessWords(m_HighScoresText, new Vector3(18, m_Edge.Y - 14, 0), 0.5f);
                m_HighScoreEntity.Components.Get<Word>().HideWords();
                SetupHighScoreList();
                SetScore(0);
                m_FirstRun = false;

                for (int line = 0; line < 4; line++)
                {
                    m_EnterYourInitialsEntitys[line].Components.Get<Word>().ProcessWords(m_EnterYourInitials[line], 
                        new Vector3(m_EnterYourInitials[line].Length * 0.80f + 30, m_Edge.Y - 20 - line * 3, 0), 0.5f);

                    m_EnterYourInitialsEntitys[line].Components.Get<Word>().HideWords();
                }
            }
        }

        public void Initilize(Random random)
        {
            m_Random = random;
        }

        public bool Active()
        {
            if (m_ShipMesh == null)
                return false;

            return m_ShipMesh.Enabled;
        }

        public void Pause(bool pause)
        {
            m_Pause = pause;

            if (m_Pause)
            {
                foreach (Line line in m_Explosion)
                {
                    line.Pause(true);
                }
            }
            else
            {
                foreach (Line line in m_Explosion)
                {
                    line.Pause(false);
                }
            }
        }

        public void BunusLife()
        {
            m_BonusSoundInstance.Play();
            m_Lives++;
            ShipLives();
        }

        public void NewGame()
        {
            Reset();
            m_Lives = 4;
            ShipLives();
            m_TotalScore = 0;
            m_PointsToNextExtraShip = m_PointsForExtraShip;
            SetScore(0);
            m_GameOver = false;
            HideHighScoreList();
            m_HighScoreSelectedLetters = "___".ToCharArray();            
            m_GameOverEntity.Components.Get<Word>().HideWords();
            m_HighScoreEntity.Components.Get<Word>().HideWords();
            m_PushStartEntity.Components.Get<Word>().HideWords();
            m_CoinPlayEntity.Components.Get<Word>().HideWords();
            m_CoinPlayNumbersEntity[0].Get<Number>().HideNumbers();
            m_CoinPlayNumbersEntity[1].Get<Number>().HideNumbers();
        }

        public void Hit()
        {
            if (!m_Hit)
            {
                if (m_Lives > 0)
                    m_Lives--;

                m_Hit = true;
                m_Spawn = false;
                m_ShipMesh.Enabled = false;
                m_FlameMesh.Enabled = false;
                ShipLives();
                Explode();

                if (m_Lives < 1)
                {
                    m_GameOver = true;
                }
            }
        }

        void GameOver()
        {
            for (int rank = 0; rank < 10; rank++)
            {
                if (m_TotalScore > m_HighScoreList[rank].Score)
                {
                    if (rank < 9)
                    {
                        // Move High Score at rank list to make room for new High Score.
                        HighScoreData[] oldScores = new HighScoreData[10];

                        for (int oldranks = rank; oldranks < 10; oldranks++)
                        {
                            oldScores[oldranks].Score = m_HighScoreList[oldranks].Score;
                            oldScores[oldranks].Name = m_HighScoreList[oldranks].Name;
                        }

                        for (int oldranks = rank; oldranks < 9; oldranks++)
                        {
                            m_HighScoreList[oldranks + 1].Score = oldScores[oldranks].Score;
                            m_HighScoreList[oldranks + 1].Name = oldScores[oldranks].Name;
                        }
                    }

                    m_HighScoreList[rank].Score = m_TotalScore;
                    SaveNewHighScoreList();
                    m_NewHighScoreRank = rank;
                    m_NewHighScore = true;
                    m_NewHighScoreLettersEntity.Components.Get<Word>().ShowWords();

                    for (int line = 0; line < 4; line++)
                    {
                        m_EnterYourInitialsEntitys[line].Components.Get<Word>().ShowWords();
                    }

                    break;
                }
            }

            if (!m_NewHighScore)
            {
                DisplayHighScoreList();
                m_GameOverEntity.Components.Get<Word>().ShowWords();
            }
        }

        void NewHighScore()
        {
            m_NewHighScore = true;
            string name = "";

            for (int i = 0; i < 3; i++)
            {
                name += m_HighScoreSelectedLetters[i];
            }

            m_NewHighScoreLettersEntity.Components.Get<Word>().ProcessWords(name, new Vector3(8, -m_Edge.Y + 8, 0), 1);

            if (Input.IsKeyPressed(Keys.Right))
            {
                m_HighScoreSelectedLetters[m_HighScoreSelector]++;

                if (m_HighScoreSelectedLetters[m_HighScoreSelector] > 90)
                    m_HighScoreSelectedLetters[m_HighScoreSelector] = (char)65;
            }

            if (Input.IsKeyPressed(Keys.Left))
            {
                m_HighScoreSelectedLetters[m_HighScoreSelector]--;

                if (m_HighScoreSelectedLetters[m_HighScoreSelector] < 65)
                    m_HighScoreSelectedLetters[m_HighScoreSelector] = (char)90;
            }

            if (Input.IsKeyPressed(Keys.Down))
            {
                m_HighScoreSelector++;

                if (m_HighScoreSelector > 2)
                {
                    m_HighScoreSelector = 0;
                    m_NewHighScore = false;
                    m_HighScoreList[m_NewHighScoreRank].Name = name;
                    SaveNewHighScoreList();
                    UpdateHighScoreList();
                    m_NewHighScoreLettersEntity.Components.Get<Word>().HideWords();

                    for (int line = 0; line < 4; line++)
                    {
                        m_EnterYourInitialsEntitys[line].Components.Get<Word>().HideWords();
                    }
                }
            }
        }

        void SetupHighScoreList()
        {
            if (m_Data.DoesFileExist())
            {
                m_Data.OpenForRead();
                string scoreData = m_Data.Read();

                int score = 0;
                int letter = 0;
                bool isLetter = true;
                string fromNumber = "";

                foreach (char ch in scoreData)
                {
                    if (ch.ToString() == "*")
                        break;

                    if (isLetter)
                    {
                        letter++;
                        m_HighScoreList[score].Name += ch;

                        if (letter == 3)
                            isLetter = false;
                    }
                    else
                    {
                        if (ch.ToString() == ":")
                        {
                            m_HighScoreList[score].Score = int.Parse(fromNumber);

                            score++;
                            letter = 0;
                            fromNumber = "";
                            isLetter = true;                            
                        }
                        else
                        {
                            fromNumber += ch.ToString();
                        }
                    }
                }

                UpdateHighScoreList();
                m_NoHighScore = false;
            }
        }

        void SaveNewHighScoreList()
        {
            m_Data.OpenForWrite();

            for (int i = 0; i < 10; i++)
            {
                if (m_HighScoreList[i].Score > 0 && m_HighScoreList[i].Name != "")
                    m_Data.Write(m_HighScoreList[i]);
            }

            m_Data.Close();
        }

        void UpdateHighScoreList()
        {
            Vector3 loc = new Vector3(2, m_Edge.Y - 18, 0);

            for (int i = 0; i < 10; i++)
            {
                if (m_HighScoreList[i].Score > 0)
                {
                    //Debug.WriteLine(m_HighScoreList[i].Name + " " + m_HighScoreList[i].Score);

                    m_HighScoreListMesh[i].Rank.ProcessNumber(i + 1, new Vector3(loc.X + 12, loc.Y - i * 2.5f, 0), 1);
                    m_HighScoreListMesh[i].Score.ProcessNumber(m_HighScoreList[i].Score, new Vector3(loc.X - 4, loc.Y - i * 2.5f, 0), 1);
                    m_HighScoreListMesh[i].Name.ProcessWords(m_HighScoreList[i].Name, new Vector3(loc.X - 6, loc.Y - i * 2.5f, 0), 0.5f);

                    if (m_HighScoreList[i].Score > m_TotalHighScore)
                        m_TotalHighScore = m_HighScoreList[i].Score;

                }
            }

            
        }

        void DisplayHighScoreList()
        {
            for (int i = 0; i < 10; i++)
            {
                if (m_HighScoreList[i].Score > 0)
                {
                    m_HighScoreListMesh[i].Rank.ShowNumbers();
                    m_HighScoreListMesh[i].Score.ShowNumbers();
                    m_HighScoreListMesh[i].Name.ShowWords();
                    m_CoinPlayEntity.Components.Get<Word>().ShowWords();
                    m_CoinPlayNumbersEntity[0].Get<Number>().ShowNumbers();
                    m_CoinPlayNumbersEntity[1].Get<Number>().ShowNumbers();

                    if (!m_NoHighScore)
                        m_HighScoreEntity.Components.Get<Word>().ShowWords();
                }
            }
        }

        void HideHighScoreList()
        {
            for (int i = 0; i < 10; i++)
            {
                if (m_HighScoreList[i].Score > 0)
                {
                    m_HighScoreListMesh[i].Rank.HideNumbers();
                    m_HighScoreListMesh[i].Score.HideNumbers();
                    m_HighScoreListMesh[i].Name.HideWords();
                }
            }
        }

        void GetInput()
        {
            if (Input.IsKeyDown(Keys.Left))
            {
                m_RotationVelocity = -3.5f;
            }
            else if (Input.IsKeyDown(Keys.Right))
            {
                m_RotationVelocity = 3.5f;
            }
            else
                m_RotationVelocity = 0;

            Thrust();

            FireShot();

            if (Input.IsKeyPressed(Keys.Down))
            {
                m_Velocity = Vector3.Zero;
                m_Acceleration = Vector3.Zero;
                m_Position = new Vector3(m_Random.Next((int)-m_Edge.X, (int)m_Edge.X), m_Random.Next((int)-m_Edge.Y, (int)m_Edge.Y), 0);
            }
        }

        void FireShot()
        {
            if (Input.IsKeyPressed(Keys.LeftCtrl) || Input.IsKeyPressed(Keys.Space))
            {
                for (int shot = 0; shot < 4; shot++)
                {
                    if (!m_Shots[shot].Active())
                    {
                        m_FireSoundInstance.Stop();
                        m_FireSoundInstance.Play();
                        float speed = 35;
                        Vector3 dir = new Vector3((float)Math.Cos(m_Rotation) * speed, (float)Math.Sin(m_Rotation) * speed, 0);
                        Vector3 offset = new Vector3((float)Math.Cos(m_Rotation) * m_Radius, (float)Math.Sin(m_Rotation) * m_Radius, 0);
                        m_Shots[shot].Spawn(m_Position + offset, dir + m_Velocity * 0.75f, 1.55f);
                        break;
                    }
                }
            }
        }

        void Thrust()
        {
            if (Input.IsKeyDown(Keys.Up))
            {
                m_ThurstSoundInstance.Play();

                float max = 50;
                float thrustAmount = 0.4f;
                float testX;
                float testY;

                if (m_Velocity.Y < 0)
                    testY = -m_Velocity.Y;
                else
                    testY = m_Velocity.Y;

                if (m_Velocity.X < 0)
                    testX = -m_Velocity.X;
                else
                    testX = m_Velocity.X;

                if (testX + testY < max)
                {
                    m_Acceleration = new Vector3((float)Math.Cos(m_Rotation) * thrustAmount, (float)Math.Sin(m_Rotation) * thrustAmount, 0);

                    if (m_FlameTimer.TotalTime.Milliseconds > 18)
                    {
                        m_FlameTimer.Reset();

                        if (m_FlameMesh.Enabled)
                            m_FlameMesh.Enabled = false;
                        else
                            m_FlameMesh.Enabled = true;
                    }
                }
                else
                {
                    m_Acceleration.X = -m_Velocity.X * 0.001f;
                    m_Acceleration.Y = -m_Velocity.Y * 0.001f;
                }
            }
            else
            {
                m_FlameMesh.Enabled = false;
                m_Acceleration = Vector3.Zero;

                if (m_Velocity.X > 0 || m_Velocity.X < 0)
                {
                    m_Acceleration.X = -m_Velocity.X * 0.001f;
                }

                if (m_Velocity.Y > 0 || m_Velocity.Y < 0)
                {
                    m_Acceleration.Y = -m_Velocity.Y * 0.001f;
                }
            }
        }

        void Explode()
        {
            m_ExplodeSoundInstance.Stop();
            m_ExplodeSoundInstance.Play();
            b_Exploding = true;

            foreach (Line line in m_Explosion)
            {
                Vector3 linePos = m_Position;
                linePos.X += (float)m_Random.NextDouble() * 2 - 1;
                linePos.Y += (float)m_Random.NextDouble() * 2 - 1;
                float timer = (float)m_Random.NextDouble() * 3 + 0.1f;
                float speed = (float)m_Random.NextDouble() * 4 + 1;
                float rotspeed = (float)m_Random.NextDouble() * 2 + 0.25f;

                line.Spawn(linePos, RandomRadian(), timer, speed, rotspeed);
            }
        }

        void ShipLives()
        {
            foreach (Entity ship in m_DisplayShips)
            {
                SceneSystem.SceneInstance.Scene.Entities.Remove(ship);
            }

            m_DisplayShips.Clear();

            for (int ship = 0; ship < m_Lives; ship++)
            {
                m_DisplayShips.Add(new Entity());
                ModelComponent shipmesh = new ModelComponent(m_ShipModel);
                m_DisplayShips[ship].Add(shipmesh);
                m_DisplayShips[ship].Transform.Position = new Vector3((m_Edge.X * 0.5f) + (ship * 2), m_Edge.Y - 5, 0);
                m_DisplayShips[ship].Transform.Scale = new Vector3(0.75f);
                m_DisplayShips[ship].Transform.RotationEulerXYZ = new Vector3(0, 0, MathUtil.PiOverTwo);
                SceneSystem.SceneInstance.Scene.Entities.Add(m_DisplayShips[ship]);
            }
        }

        void Reset()
        {
            m_Hit = false;
            m_ShipMesh.Enabled = true;
            m_FlameMesh.Enabled = false;
            m_Position = Vector3.Zero;
            m_Velocity = Vector3.Zero;
            m_Acceleration = Vector3.Zero;
            m_Rotation = m_Random.Next(0, 7);
            UpdatePR();
        }

        void Initilize() //Initialize class
        {
            m_EnterYourInitials[0] = "YOUR SCORE IS ONE OF THE TEN BEST";
            m_EnterYourInitials[1] = "PLEASE ENTER YOUR INITIALS";
            m_EnterYourInitials[2] = "PUSH ROTATE TO SELECT LETTER";
            m_EnterYourInitials[3] = "PUSH HYPERSPECE WHEN LETTER IS CURRECT";

            Prefab myShotPrefab = Content.Load<Prefab>("Shot");
            Prefab myLinePrefab = Content.Load<Prefab>("Line");
            Prefab myNumberPrefab = Content.Load<Prefab>("Number");
            Prefab myWordPrefab = Content.Load<Prefab>("Word");

            m_HighScoreEntity = myWordPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_HighScoreEntity);
            m_Score = myNumberPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_Score);
            m_HighScore = myNumberPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_HighScore);
            m_AtariEntity = myWordPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_AtariEntity);
            m_AtariDateEntity = myNumberPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_AtariDateEntity);
            m_GameOverEntity = myWordPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_GameOverEntity);
            m_PushStartEntity = myWordPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_PushStartEntity);
            m_CoinPlayEntity = myWordPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_CoinPlayEntity);
            m_NewHighScoreLettersEntity = myWordPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_NewHighScoreLettersEntity);

            for (int line = 0; line < 4; line++)
            {
                m_EnterYourInitialsEntitys[line] = myWordPrefab.Instantiate().First();
                SceneSystem.SceneInstance.Scene.Entities.Add(m_EnterYourInitialsEntitys[line]);
            }

            for (int i = 0; i < 10; i++)
            {
                Entity rank = myNumberPrefab.Instantiate().First();
                SceneSystem.SceneInstance.Scene.Entities.Add(rank);
                m_HighScoreListMesh[i].Rank = rank.Components.Get<Number>();
                Entity score = myNumberPrefab.Instantiate().First();
                SceneSystem.SceneInstance.Scene.Entities.Add(score);
                m_HighScoreListMesh[i].Score = score.Components.Get<Number>();
                Entity name = myWordPrefab.Instantiate().First();
                SceneSystem.SceneInstance.Scene.Entities.Add(name);
                m_HighScoreListMesh[i].Name = name.Components.Get<Word>();
                m_HighScoreList[i].Name = "";
            }

            for (int i = 0; i < 2; i++)
            {
                m_CoinPlayNumbersEntity[i] = myNumberPrefab.Instantiate().First();
                SceneSystem.SceneInstance.Scene.Entities.Add(m_CoinPlayNumbersEntity[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                Entity shot;
                shot = (myShotPrefab.Instantiate().First());
                SceneSystem.SceneInstance.Scene.Entities.Add(shot);
                m_Shots.Add(shot.Components.Get<Shot>());
            }

            for (int i = 0; i < 6; i++)
            {
                Entity line = myLinePrefab.Instantiate().First();
                SceneSystem.SceneInstance.Scene.Entities.Add(line);
                m_Explosion.Add(line.Components.Get<Line>());
                m_Explosion[i].Initialize(m_Random);
            }

            // VertexPositionNormalTexture is the layout that the engine uses in the shaders
            var vBuffer = SiliconStudio.Xenko.Graphics.Buffer.Vertex.New(GraphicsDevice, new VertexPositionNormalTexture[]
            {
                 new VertexPositionNormalTexture(new Vector3(-1.15f, 0.8f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Top back tip.
                 new VertexPositionNormalTexture(new Vector3(1.15f, 0, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Nose pointing to the left of screen.
                 new VertexPositionNormalTexture(new Vector3(-1.15f, -0.8f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Bottom back tip.
                 new VertexPositionNormalTexture(new Vector3(-0.95f, -0.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Bottom inside back.
                 new VertexPositionNormalTexture(new Vector3(-0.95f, 0.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Top inside back.
                 new VertexPositionNormalTexture(new Vector3(-1.15f, 0.8f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)) //Top Back Tip.
            });

            MeshDraw meshDraw = new MeshDraw
            {
                PrimitiveType = PrimitiveType.LineStrip, // Tell the GPU that this is a line.
                VertexBuffers = new[] { new VertexBufferBinding(vBuffer, VertexPositionNormalTexture.Layout, vBuffer.ElementCount) },
                DrawCount = vBuffer.ElementCount
            };

            Mesh mesh = new Mesh();
            mesh.Draw = meshDraw;

            m_ShipModel = new Model();
            m_ShipModel.Add(mesh);
            m_ShipMesh = new ModelComponent(m_ShipModel);
            m_Ship = new Entity();
            m_Ship.Add(m_ShipMesh);
            // VertexPositionNormalTexture is the layout that the engine uses in the shaders
            vBuffer = SiliconStudio.Xenko.Graphics.Buffer.Vertex.New(GraphicsDevice, new VertexPositionNormalTexture[]
            {
                 new VertexPositionNormalTexture(new Vector3(-0.95f, -0.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Bottom inside back.
                 new VertexPositionNormalTexture(new Vector3(-1.75f, 0, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Tip of flame.
                 new VertexPositionNormalTexture(new Vector3(-0.95f, 0.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Top inside back.
                 new VertexPositionNormalTexture(new Vector3(-1.75f, 0, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Tip of flame.
            });

            meshDraw = new MeshDraw
            {
                PrimitiveType = PrimitiveType.LineStrip, // Tell the GPU that this is a line.
                VertexBuffers = new[] { new VertexBufferBinding(vBuffer, VertexPositionNormalTexture.Layout, vBuffer.ElementCount) },
                DrawCount = vBuffer.ElementCount
            };

            mesh = new Mesh();
            mesh.Draw = meshDraw;
            Model model = new Model();
            model.Add(mesh);
            m_FlameMesh = new ModelComponent(model);
            m_ShipFlame = new Entity();
            m_ShipFlame.Add(m_FlameMesh);
            this.Entity.AddChild(m_ShipFlame);

            m_ShipMesh.Enabled = false;
            m_FlameMesh.Enabled = false;
        }

        void InitializeAudio()
        {
            Sound firesound = Content.Load<Sound>("PlayerFire");
            Sound thrustsound = Content.Load<Sound>("Thrust");
            Sound explodesound = Content.Load<Sound>("PlayerExplosion");
            Sound bonussound = Content.Load<Sound>("BonusShip");

            m_FireSoundInstance = firesound.CreateInstance();
            m_FireSoundInstance.Volume = 0.33f;
            m_FireSoundInstance.Pitch = 1.75f;

            m_ThurstSoundInstance = thrustsound.CreateInstance();
            m_ThurstSoundInstance.Volume = 1.75f;
            m_ThurstSoundInstance.Pitch = 0.75f;

            m_ExplodeSoundInstance = explodesound.CreateInstance();
            m_ExplodeSoundInstance.Volume = 0.5f;
            m_ExplodeSoundInstance.Pitch = 0.5f;

            m_BonusSoundInstance = bonussound.CreateInstance();
            m_BonusSoundInstance.Volume = 0.5f;
            m_BonusSoundInstance.Pitch = 3;
        }
    }
}

