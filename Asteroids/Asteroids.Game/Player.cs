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
        // Declared public member fields and properties will show in the game studio        
        public List<Entity> m_Shots;

        Entity ship;
        Entity shipFlame;
        ModelComponent flameMesh;
        ModelComponent shipMesh;

        public override void Start()
        {
            // Initialization of the script.
            m_Radius = 1.15f;
            m_Shots = new List<Entity>();
            Prefab myShotPrefab = Content.Load<Prefab>("Shot");

            for (int i = 0; i < 4; i++)
            {
                m_Shots.Add(myShotPrefab.Instantiate().First());
                SceneSystem.SceneInstance.Scene.Entities.Add(m_Shots[i]);
            }

            m_Rotation = m_Random.Next(0, 7);

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

            Model model = new Model();
            model.Add(mesh);
            shipMesh = new ModelComponent(model);

            ship = new Entity();
            ship.Add(shipMesh);
            this.Entity.AddChild(ship);

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

            model = new Model();
            model.Add(mesh);
            flameMesh = new ModelComponent(model);
            

            shipFlame = new Entity();
            shipFlame.Add(flameMesh);
            this.Entity.AddChild(shipFlame);
        }

        public override void Update()
        {
            base.Update();
            // Do stuff every new frame

            if (Input.IsKeyDown(Keys.Left))
            {
                m_RotationAmount = -3;
            }
            else if (Input.IsKeyDown(Keys.Right))
            {
                m_RotationAmount = 3;
            }
            else
                m_RotationAmount = 0;

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
                    flameMesh.Enabled = true;
                }
                else
                {
                    m_Acceleration.X = -m_Velocity.X * 0.001f;
                    m_Acceleration.Y = -m_Velocity.Y * 0.001f;
                }
            }
            else
            {
                flameMesh.Enabled = false;
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

            CheckForEdge();

            if (Input.IsKeyPressed(Keys.LeftCtrl))
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
        }
    }
}
