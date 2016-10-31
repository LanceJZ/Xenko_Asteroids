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
using SiliconStudio.Xenko.Games.Time;

namespace Asteroids
{
    public class UFO : Explode
    {
        // Declared public member fields and properties will show in the game studio
        public float m_ShotTimerAmount = 2.75f;
        public float m_VectorTimerAmount = 3;
        public float m_Speed = 5;
        public bool m_Done = false;
        public bool m_Large = true;

        Entity m_UFO;
        ModelComponent m_UFOMesh;
        ModelComponent m_UFOTIMesh;
        ModelComponent m_UFOBIMesh;
        TimerTick m_ShotTimer = new TimerTick();
        TimerTick m_VectorTimer = new TimerTick();
        public Entity m_Shot;
        public Entity m_Player;
        int m_Points;

        public override void Start()
        {
            base.Start();
            // Initialization of the script.
            m_Radius = 1.9f;
            // VertexPositionNormalTexture is the layout that the engine uses in the shaders
            var vBuffer = SiliconStudio.Xenko.Graphics.Buffer.Vertex.New(GraphicsDevice, new VertexPositionNormalTexture[]
            {
                 new VertexPositionNormalTexture(new Vector3(1.9f, -0.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), // Far left edge Bottom line
                 new VertexPositionNormalTexture(new Vector3(0.7f, 0.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), // Top line left edge
                 new VertexPositionNormalTexture(new Vector3(0.3f, 1.1f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //
                 new VertexPositionNormalTexture(new Vector3(-0.3f, 1.1f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //
                 new VertexPositionNormalTexture(new Vector3(-0.7f, 0.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), // Top line right edge
                 new VertexPositionNormalTexture(new Vector3(-1.9f, -0.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), // Right edge bottom line
                 new VertexPositionNormalTexture(new Vector3(-0.8f, -1.1f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //
                 new VertexPositionNormalTexture(new Vector3(0.8f, -1.1f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //
                 new VertexPositionNormalTexture(new Vector3(1.9f, -0.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), // Far left edge Bottom line
            });

            MeshDraw meshDraw = new MeshDraw
            {
                PrimitiveType = PrimitiveType.LineStrip, // Tell the GPU that this is a line.
                VertexBuffers = new[] { new VertexBufferBinding(vBuffer, VertexPositionNormalTexture.Layout, vBuffer.ElementCount) },
                DrawCount = vBuffer.ElementCount
            };

            Mesh mesh = new Mesh();
            mesh.Draw = meshDraw;

            Model model = new Model();
            model.Add(mesh);
            m_UFOMesh = new ModelComponent(model);

            m_UFO = new Entity();
            m_UFO.Add(m_UFOMesh);
            this.Entity.AddChild(m_UFO);
            // Top inside lines for UFO
            var vBufferti = SiliconStudio.Xenko.Graphics.Buffer.Vertex.New(GraphicsDevice, new VertexPositionNormalTexture[]
            {
                 new VertexPositionNormalTexture(new Vector3(0.7f, 0.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), // Top inside line left
                 new VertexPositionNormalTexture(new Vector3(-0.7f, 0.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), // Top inside line right
            });

            MeshDraw meshDrawti = new MeshDraw
            {
                PrimitiveType = PrimitiveType.LineStrip, // Tell the GPU that this is a line.
                VertexBuffers = new[] { new VertexBufferBinding(vBufferti, VertexPositionNormalTexture.Layout, vBufferti.ElementCount) },
                DrawCount = vBufferti.ElementCount
            };

            Mesh meshti = new Mesh();
            meshti.Draw = meshDrawti;

            Model modelti = new Model();
            modelti.Add(meshti);
            m_UFOTIMesh = new ModelComponent(modelti);
            Entity UFOti = new Entity();
            UFOti.Add(m_UFOTIMesh);
            m_UFO.AddChild(UFOti);

            // Bottom inside lines for UFO
            var vBufferbi = SiliconStudio.Xenko.Graphics.Buffer.Vertex.New(GraphicsDevice, new VertexPositionNormalTexture[]
            {
                 new VertexPositionNormalTexture(new Vector3(1.9f, -0.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), // Bottom inside line left
                 new VertexPositionNormalTexture(new Vector3(-1.9f, -0.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)) // Bottom inside line right
            });

            MeshDraw meshDrawbi = new MeshDraw
            {
                PrimitiveType = PrimitiveType.LineStrip, // Tell the GPU that this is a line.
                VertexBuffers = new[] { new VertexBufferBinding(vBufferbi, VertexPositionNormalTexture.Layout, vBufferbi.ElementCount) },
                DrawCount = vBufferbi.ElementCount
            };

            Mesh meshbi = new Mesh();
            meshbi.Draw = meshDrawbi;

            Model modelbi = new Model();
            modelbi.Add(meshbi);
            m_UFOBIMesh = new ModelComponent(modelbi);
            Entity UFObi = new Entity();
            UFObi.Add(m_UFOBIMesh);
            m_UFO.AddChild(UFObi);

            Destroy();

            Prefab myShotPrefab = Content.Load<Prefab>("Shot");
            m_Shot = myShotPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_Shot);
        }

        public override void Update()
        {
            if (m_Hit = CheckCollisions())
            {
                SpawnExplosion();
            }

            if (Active() && !m_Hit && !m_Done)
            {
                base.Update();

                if (m_Position.X > m_Edge.X || m_Position.X < -m_Edge.X)
                    m_Done = true;

                CheckForEdge();

                if (m_ShotTimer.TotalTime.Seconds > m_ShotTimerAmount)
                {
                    m_ShotTimer.Reset();
                    float speed = 30;
                    float rad = 0;

                    if (m_Large)
                        rad = RandomRadian();
                    else
                    {
                        rad = (float)Math.Atan2(m_Player.Components.Get<Player>().m_Position.Y - m_Position.Y,
                            m_Player.Components.Get<Player>().m_Position.X - m_Position.X);
                    }
                    
                    Vector3 dir = new Vector3((float)Math.Cos(rad) * speed, (float)Math.Sin(rad) * speed, 0);
                    Vector3 offset = new Vector3((float)Math.Cos(rad) * m_Radius, (float)Math.Sin(rad) * m_Radius, 0);
                    m_Shot.Components.Get<Shot>().Spawn(m_Position + offset, dir + m_Velocity * 0.25f, 0.95f);
                }

                if (m_VectorTimer.TotalTime.Seconds > m_VectorTimerAmount)
                {
                    m_VectorTimer.Reset();
                    float vChange = (float)m_Random.NextDouble() * 10;

                    if (vChange < 5)
                    {
                        if ((int)m_Velocity.Y == 0 && vChange < 2.5)
                            m_Velocity.Y = m_Speed;
                        else if ((int)m_Velocity.Y == 0)
                            m_Velocity.Y = -m_Speed;
                        else
                            m_Velocity.Y = 0;
                    }
                }

                m_ShotTimer.Tick();
                m_VectorTimer.Tick();
            }
        }

        void SetScore()
        {
            m_Player.Components.Get<Player>().SetScore(m_Points);
        }

        public bool CheckPlayerClear()
        {
            if (CirclesIntersect(Vector3.Zero, 20))
                return false;

            return true;
        }

        bool CheckCollisions()
        {
            if (Active())
            {
                for (int shot = 0; shot < 4; shot++)
                {
                    if (m_Player.Components.Get<Player>().m_Shots[shot].Components.Get<Shot>().Active())
                    {
                        if (CirclesIntersect(m_Player.Components.Get<Player>().m_Shots[shot].Components.Get<Shot>().m_Position,
                            m_Player.Components.Get<Player>().m_Shots[shot].Components.Get<Shot>().m_Radius))
                        {
                            SetScore();
                            m_Player.Components.Get<Player>().m_Shots[shot].Components.Get<Shot>().Destroy();
                            return true;
                        }
                    }
                }

                if (m_Player.Components.Get<Player>().Active())
                {
                    if (CirclesIntersect(m_Player.Components.Get<Player>().m_Position, m_Player.Components.Get<Player>().m_Radius))
                    {
                        SetScore();
                        m_Player.Components.Get<Player>().Hit();
                        return true;
                    }
                }
            }

            if (m_Shot.Components.Get<Shot>().Active())
            {
                if (m_Player.Components.Get<Player>().Active())
                {
                    if (m_Shot.Components.Get<Shot>().CirclesIntersect(m_Player.Components.Get<Player>().m_Position,
                        m_Player.Components.Get<Player>().m_Radius))
                    {
                        m_Player.Components.Get<Player>().Hit();
                        m_Shot.Components.Get<Shot>().Destroy();
                    }
                }
            }

            return false;
        }

        public void Spawn(int SpawnCount, int Wave)
        {
            float spawnPercent = (float)(Math.Pow(0.915, (SpawnCount * 2) / ((Wave * 2) + 1)));

            // Size 0 is the large one.
            if (m_Random.Next(0, 99) < spawnPercent * 100)
            {
                m_Large = true;
                m_Points = 200;
                m_UFO.Transform.Scale = new Vector3(1);
            }
            else
            {
                m_Large = false;
                m_Points = 1000;
                m_UFO.Transform.Scale = new Vector3(0.5f);
            }

            if (m_Random.Next (0, 10) > 5)
            {
                m_Position.X = -m_Edge.X;
                m_Velocity.X = m_Speed;
            }
            else
            {
                m_Position.X = m_Edge.X;
                m_Velocity.X = -m_Speed;
            }

            m_Position.Y = RandomHieght();

            UpdatePR();
            m_ShotTimer.Reset();
            m_VectorTimer.Reset();
            m_UFOMesh.Enabled = true;
            m_UFOBIMesh.Enabled = true;
            m_UFOTIMesh.Enabled = true;
            m_Done = false;
            m_Hit = false;
        }

        public void Destroy()
        {
            m_UFOMesh.Enabled = false;
            m_UFOTIMesh.Enabled = false;
            m_UFOBIMesh.Enabled = false;
            m_Done = false;
            m_Hit = false;
        }

        public bool Active()
        {
            if (m_UFOMesh != null)
                return m_UFOMesh.Enabled;
            else
                return false;
        }
    }
}
