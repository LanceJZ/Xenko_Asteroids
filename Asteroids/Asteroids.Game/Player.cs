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

namespace Asteroids
{
    public class Player : Actor
    {
        public List<Entity> m_Shots;
        public bool m_Spawn = false;
        public bool m_GameOver = true;
        public bool m_Exploding = false;
        Entity m_Score;
        Entity m_HighScore;
        Entity m_AtariDateEntity;
        Entity m_AtariEntity;
        Entity m_GameOverEntity;
        Entity m_PushStartEntity;
        Entity m_CoinPlayEntity;
        Entity[] m_CoinPlayNumbersEntity = new Entity[2];
        Entity m_Ship;
        Entity m_ShipFlame;
        Model m_ShipModel;
        List<Entity> m_DisplayShips;
        List<Entity> m_Explosion;
        ModelComponent m_FlameMesh;
        ModelComponent m_ShipMesh;
        bool m_FirstRun = true;

        int m_Lives = 0;
        int m_TotalScore = 0;
        int m_TotalHighScore = 0;
        int m_PointsToNextFreeLife = 0;
        int m_PointsForFreeLife = 5000;

        string m_GameOverText = "GAME OVER";
        string m_Ataritext = "ATARI INC";
        int m_AtariDate = 1979;
        string m_PushStartText = "PUSH START";
        string m_CoinPlayText = "COIN   PLAY";

        public override void Start()
        {
            m_PointsToNextFreeLife = m_PointsForFreeLife;
            m_Radius = 1.15f;
            m_Shots = new List<Entity>();
            m_DisplayShips = new List<Entity>();
            m_Explosion = new List<Entity>();

            Prefab myShotPrefab = Content.Load<Prefab>("Shot");
            Prefab myLinePrefab = Content.Load<Prefab>("Line");
            Prefab myNumberPrefab = Content.Load<Prefab>("Number");
            Prefab myWordPrefab = Content.Load<Prefab>("Word");

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

            for (int i = 0; i < 2; i++)
            {
                m_CoinPlayNumbersEntity[i] = myNumberPrefab.Instantiate().First();
                SceneSystem.SceneInstance.Scene.Entities.Add(m_CoinPlayNumbersEntity[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                m_Shots.Add(myShotPrefab.Instantiate().First());
                SceneSystem.SceneInstance.Scene.Entities.Add(m_Shots[i]);
            }

            for (int i = 0; i < 6; i++)
            {
                m_Explosion.Add(myLinePrefab.Instantiate().First());
                SceneSystem.SceneInstance.Scene.Entities.Add(m_Explosion[i]);
                m_Explosion[i].Components.Get<Line>().m_Random = m_Random;
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

            ShipLives();

            this.Entity.AddChild(m_Ship);

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

        public override void Update()
        {
            if (m_FirstRun)
            {
                FirstTime();
            }

            if (!m_Hit && !m_GameOver)
            {
                base.Update();
                GetInput();
                CheckForEdge();
            }
            else if (m_Hit && !m_GameOver)
            {
                if (m_Spawn && !m_Exploding)
                {
                    Reset();
                }

                if (m_Exploding)
                {
                    bool active = false;

                    foreach (Entity line in m_Explosion)
                    {
                        if (line.Components.Get<Line>().Active())
                        {
                            active = true;
                            break;
                        }
                    }

                    if (!active)
                        m_Exploding = false;
                }
            }
            else if (m_GameOver)
            {

            }
        }

        public void SetScore(int points)
        {
            m_TotalScore += points;

            if (m_TotalScore > m_PointsToNextFreeLife)
            {
                BunusLife();
                m_PointsToNextFreeLife += m_PointsForFreeLife;
            }

            if (m_TotalScore > m_TotalHighScore || m_TotalHighScore == 0)
            {
                m_TotalHighScore = m_TotalScore;
                m_HighScore.Components.Get<Number>().ProcessNumber(m_TotalHighScore, new Vector3(0.5f, m_Edge.Y - 0.5f, 0), 0.666f);
            }

            m_Score.Components.Get<Number>().ProcessNumber(m_TotalScore, new Vector3(m_Edge.X * 0.5f, m_Edge.Y + 0.15f, 0), 1);
        }

        public void FirstTime()
        {
            if (m_Score.Components.Get<Number>().m_Numbers != null)
            {
                m_AtariDateEntity.Components.Get<Number>().ProcessNumber(m_AtariDate, new Vector3(3.5f, -m_Edge.Y + 1, 0), 0.5f);
                m_AtariEntity.Components.Get<Word>().ProcessWords(m_Ataritext, new Vector3(4.5f, -m_Edge.Y + 1, 0), 0.25f);
                m_PushStartEntity.Components.Get<Word>().ProcessWords(m_PushStartText, new Vector3(30, m_Edge.Y - 10, 0), 1);
                m_CoinPlayEntity.Components.Get<Word>().ProcessWords(m_CoinPlayText, new Vector3(33, -m_Edge.Y + 10, 0), 1);
                m_CoinPlayNumbersEntity[0].Get<Number>().ProcessNumber(1, new Vector3(25, -m_Edge.Y + 10, 0), 2);
                m_CoinPlayNumbersEntity[1].Get<Number>().ProcessNumber(1, new Vector3(3, -m_Edge.Y + 10, 0), 2);
                SetScore(0);
                m_FirstRun = false;
            }
        }

        public bool Active()
        {
            if (m_ShipMesh == null)
                return false;

            return m_ShipMesh.Enabled;
        }

        public void BunusLife()
        {
            m_Lives++;
            ShipLives();
        }

        public void NewGame()
        {
            Reset();
            m_Lives = 4;
            ShipLives();
            m_TotalScore = 0;
            m_PointsToNextFreeLife = m_PointsForFreeLife;
            SetScore(0);
            m_GameOver = false;
            m_GameOverEntity.Components.Get<Word>().DeleteWords();
            m_PushStartEntity.Components.Get<Word>().DeleteWords();
            m_CoinPlayEntity.Components.Get<Word>().DeleteWords();
            m_CoinPlayNumbersEntity[0].Get<Number>().DeleteNumbers();
            m_CoinPlayNumbersEntity[1].Get<Number>().DeleteNumbers();
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
                    m_GameOverEntity.Components.Get<Word>().ProcessWords(m_GameOverText, new Vector3(25, 10, 0), 1);
                    m_Hit = false;
                }
            }
        }

        void GetInput()
        {
            if (Input.IsKeyDown(Keys.Left))
            {
                m_RotationVelocity = -3;
            }
            else if (Input.IsKeyDown(Keys.Right))
            {
                m_RotationVelocity = 3;
            }
            else
                m_RotationVelocity = 0;

            if (Input.IsKeyDown(Keys.Up))
            {
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
                    m_FlameMesh.Enabled = true;
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
                    m_Acceleration.X = -m_Velocity.X * 0.002f;
                }

                if (m_Velocity.Y > 0 || m_Velocity.Y < 0)
                {
                    m_Acceleration.Y = -m_Velocity.Y * 0.002f;
                }
            }

            if (Input.IsKeyPressed(Keys.LeftCtrl) || Input.IsKeyPressed(Keys.Space))
            {
                for (int shot = 0; shot < 4; shot++)
                {
                    if (!m_Shots[shot].Components.Get<Shot>().Active())
                    {
                        float speed = 35;
                        Vector3 dir = new Vector3((float)Math.Cos(m_Rotation) * speed, (float)Math.Sin(m_Rotation) * speed, 0);
                        Vector3 offset = new Vector3((float)Math.Cos(m_Rotation) * m_Radius, (float)Math.Sin(m_Rotation) * m_Radius, 0);
                        m_Shots[shot].Components.Get<Shot>().Spawn(m_Position + offset, dir + m_Velocity * 0.5f, 1.5f);
                        break;
                    }
                }
            }

            if (Input.IsKeyPressed(Keys.Down))
            {
                m_Velocity = Vector3.Zero;
                m_Acceleration = Vector3.Zero;
                m_Position = new Vector3(m_Random.Next((int)-m_Edge.X, (int)m_Edge.X), m_Random.Next((int)-m_Edge.Y, (int)m_Edge.Y), 0);
            }
        }

        void Explode()
        {
            m_Exploding = true;

            foreach (Entity line in m_Explosion)
            {
                Vector3 linePos = m_Position;
                linePos.X += (float)m_Random.NextDouble() * 2 - 1;
                linePos.Y += (float)m_Random.NextDouble() * 2 - 1;
                float timer = (float)m_Random.NextDouble() * 3 + 0.1f;
                float speed = (float)m_Random.NextDouble() * 4 + 1;
                float rotspeed = (float)m_Random.NextDouble() * 2 + 0.25f;

                line.Components.Get<Line>().Spawn(linePos, RandomRadian(), timer, speed, rotspeed);
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
    }
}
