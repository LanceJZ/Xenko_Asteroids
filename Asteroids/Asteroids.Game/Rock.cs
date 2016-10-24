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
    public class Rock : Actor
    {
        // Declared public member fields and properties will show in the game studio
        Entity m_Player;
        Entity m_UFO;
        Entity m_Rock;
        Entity m_Score;
        ModelComponent m_RockMesh;
        int m_Points;
        float m_Speed;

        public override void Start()
        {
            // Initialization of the script.

        }

        public override void Update()
        {
            if (m_RockMesh.Enabled)
            {
                base.Update();
                CheckForEdge();

                if (CheckCollision())
                {
                    m_Hit = true;
                }
            }
        }

        bool CheckCollision()
        {
            if (CirclesIntersect(m_Player.Components.Get<Player>().m_Position, m_Player.Components.Get<Player>().m_Radius))
            {

            }

            for (int shot = 0; shot < 4; shot++)
            {
                if (m_Player.Components.Get<Player>().m_Shots[shot].Components.Get<Shot>().Active())
                {
                    if (CirclesIntersect(m_Player.Components.Get<Player>().m_Shots[shot].Components.Get<Shot>().m_Position,
                        m_Player.Components.Get<Player>().m_Shots[shot].Components.Get<Shot>().m_Radius))
                    {
                        m_Player.Components.Get<Player>().m_Shots[shot].Components.Get<Shot>().Destroy();
                        m_Score.Components.Get<Score>().m_TotalScore += m_Points;
                        m_Score.Components.Get<Score>().ProcessNumber(m_Score.Components.Get<Score>().m_TotalScore,
                            new Vector3(m_Edge.X * 0.5f, m_Edge.Y - 1, 0), 1);
                        return true;
                    }
                }
            }

            if (m_UFO.Components.Get<UFO>().m_Shot.Components.Get<Shot>().Active())
            {
                if (CirclesIntersect(m_UFO.Components.Get<UFO>().m_Shot.Components.Get<Shot>().m_Position,
                    m_UFO.Components.Get<UFO>().m_Shot.Components.Get<Shot>().m_Radius))
                {
                    m_UFO.Components.Get<UFO>().m_Shot.Components.Get<Shot>().Destroy();
                    return true;
                }
            }

            if (m_UFO.Components.Get<UFO>().Active())
            {
                if (CirclesIntersect(m_UFO.Components.Get<UFO>().m_Position, m_UFO.Components.Get<UFO>().m_Radius))
                {                    
                    m_UFO.Components.Get<UFO>().m_Hit = true;
                    return true;
                }
            }

            if (CirclesIntersect(m_Player.Components.Get<Player>().m_Position, m_Player.Components.Get<Player>().m_Radius))
            {
                return true;
            }

            return false;
        }

        public void Spawn(Vector3 position, float scale, float speed, int points, Random random, Entity player, Entity UFO, Entity score)
        {
            Initialize(random, player, UFO, score);
            m_Speed = speed;
            Spawn(position);
            m_Rock.Transform.Scale = new Vector3(scale);
            m_Radius = m_Radius * scale;
            m_Points = points;
        }

        public void Spawn(Vector3 position)
        {
            this.Entity.Transform.Position = position;
            m_Position = position;
            m_RockMesh.Enabled = true;
            Setup(m_Speed);
        }

        public void Initialize(Random random, Entity player, Entity UFO, Entity score)
        {
            m_Radius = 2.9f;
            m_Random = random;
            m_Player = player;
            m_UFO = UFO;
            m_Score = score;
            m_Points = 20;

            int m_RockType = m_Random.Next(0, 4);

            if (m_RockType == 1)
                m_RockOne();
            else if (m_RockType == 2)
                m_RockTwo();
            else if (m_RockType == 3)
                m_RockThree();
            else
                m_RockFour();

            m_Rock = new Entity();
            m_Rock.Add(m_RockMesh);
            this.Entity.AddChild(m_Rock);
        }

        public void Setup(float speed)
        {
            float rad = RandomRadian();
            float amt = (float)m_Random.NextDouble() * speed + (speed * 0.15f);
            m_Velocity = new Vector3((float)Math.Cos(rad) * amt, (float)Math.Sin(rad) * amt, 0);
        }

        public void Large()
        {
            Setup(5);
            m_RockMesh.Enabled = true;

            if (m_Velocity.X < 0)
            {
                m_Position.X = m_Edge.X;
            }
            else
            {
                m_Position.X = -m_Edge.X;
            }

            m_Position.Y = RandomHieght();

            this.Entity.Transform.Position = m_Position;
        }

        public void Destroy()
        {
            m_RockMesh.Enabled = false;
            m_Hit = false;
        }

        public bool Active()
        {
            if (m_RockMesh == null)
                return false;

            return m_RockMesh.Enabled;
        }

        private void m_RockOne()
        {
            // VertexPositionNormalTexture is the layout that the engine uses in the shaders
            var vBuffer = SiliconStudio.Xenko.Graphics.Buffer.Vertex.New(GraphicsDevice, new VertexPositionNormalTexture[]
            {
                 new VertexPositionNormalTexture(new Vector3(2.9f, 1.5f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(1.5f, 3.0f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(0.0f, 2.2f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-1.5f, 3.0f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-2.9f, 1.5f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-1.5f, 0.7f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-2.9f, -0.7f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-1.5f, -3.0f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(0.7f, -2.1f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(1.5f, -3.0f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(2.9f, -1.5f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(2.1f, 0.0f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(2.9f, 1.5f, 0), new Vector3(0, 1, 1), new Vector2(0, 0))
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
            m_RockMesh = new ModelComponent(model);
        }

        private void m_RockTwo()
        {
            // VertexPositionNormalTexture is the layout that the engine uses in the shaders
            var vBuffer = SiliconStudio.Xenko.Graphics.Buffer.Vertex.New(GraphicsDevice, new VertexPositionNormalTexture[]
            {
                 new VertexPositionNormalTexture(new Vector3(2.9f, 1.5f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(1.4f, 2.9f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(0.0f, 1.5f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-1.5f, 2.9f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-2.9f, 1.5f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-2.2f, 0.0f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-2.9f, -1.5f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-0.7f, -2.9f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(1.4f, -2.9f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(2.9f, -1.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(2.9f, 1.5f, 0), new Vector3(0, 1, 1), new Vector2(0, 0))
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
            m_RockMesh = new ModelComponent(model);
        }

        private void m_RockThree()
        {
            // VertexPositionNormalTexture is the layout that the engine uses in the shaders
            var vBuffer = SiliconStudio.Xenko.Graphics.Buffer.Vertex.New(GraphicsDevice, new VertexPositionNormalTexture[]
            {
                 new VertexPositionNormalTexture(new Vector3(2.9f, 1.5f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(0.7f, 1.5f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(1.6f, 2.9f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-0.8f, 2.9f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-2.9f, 1.5f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-2.9f, 0.8f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-0.8f, 0.0f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-2.9f, -1.4f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-1.4f, -2.8f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-0.7f, -2.1f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(1.5f, -2.9f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(2.9f, -0.8f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(2.9f, 1.5f, 0), new Vector3(0, 1, 1), new Vector2(0, 0))
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
            m_RockMesh = new ModelComponent(model);
        }

        private void m_RockFour()
        {
            // VertexPositionNormalTexture is the layout that the engine uses in the shaders
            var vBuffer = SiliconStudio.Xenko.Graphics.Buffer.Vertex.New(GraphicsDevice, new VertexPositionNormalTexture[]
            {
                 new VertexPositionNormalTexture(new Vector3(2.9f, 0.8f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(0.6f, 2.9f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-1.5f, 2.9f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-3.0f, 0.7f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-3.0f, -0.7f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-1.6f, -2.9f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(-1.4f, -2.9f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(0.0f, -2.9f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(0.0f, -0.8f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(1.4f, -2.8f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(2.9f, -0.7f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(1.5f, 0.0f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)),
                 new VertexPositionNormalTexture(new Vector3(2.9f, 0.8f, 0), new Vector3(0, 1, 1), new Vector2(0, 0))
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
            m_RockMesh = new ModelComponent(model);
        }
    }
}
