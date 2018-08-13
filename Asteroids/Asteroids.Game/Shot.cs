using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenko.Core.Mathematics;
using Xenko.Input;
using Xenko.Engine;
using Xenko.Games.Time;
using Xenko.Graphics;
using Xenko.Rendering;
using Xenko.Audio;

namespace Asteroids
{
    public class Shot : Actor
    {
        // Declared public member fields and properties will show in the game studio
        public float m_TimerAmount = 0;

        Entity m_Shot;
        ModelComponent m_ShotMesh;
        TimerTick m_Timer = new TimerTick();

        public override void Start()
        {
            // Initialization of the script.
            m_Radius = 0.05f;
            // VertexPositionNormalTexture is the layout that the engine uses in the shaders
            var vBuffer = Xenko.Graphics.Buffer.Vertex.New(GraphicsDevice, new VertexPositionNormalTexture[]
            {
                 new VertexPositionNormalTexture(new Vector3(-0.05f, 0.05f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Top Left.
                 new VertexPositionNormalTexture(new Vector3(0.05f, -0.05f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Bottom right.
                 new VertexPositionNormalTexture(new Vector3(0.05f, 0.05f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Bottom right.
                 new VertexPositionNormalTexture(new Vector3(-0.05f, -0.05f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)) //Top Left.
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
            m_ShotMesh = new ModelComponent(model);

            m_Shot = new Entity();
            m_Shot.Add(m_ShotMesh);
            this.Entity.AddChild(m_Shot);
            Destroy();
        }

        public override void Update()
        {
            if (m_ShotMesh.Enabled && !m_Pause)
            {
                base.Update();
                CheckForEdge();

                if (m_Timer.TotalTime.TotalSeconds > m_TimerAmount)
                {
                    Destroy();
                }

                m_Timer.Tick();
            }
        }

        public bool CheckPlayerClear()
        {
            if (CirclesIntersect(Vector3.Zero, 25))
                return false;

            return true;
        }

        public void Spawn(Vector3 position, Vector3 velocity, float timer)
        {
            m_Position = position;
            m_Velocity = velocity;
            m_Timer.Reset();
            m_TimerAmount = timer;
            m_ShotMesh.Enabled = true;
            UpdatePR();
        }

        public void Pause(bool pause)
        {
            m_Pause = pause;

            if (m_Pause)
            {
                m_Timer.Pause();
            }
            else
            {
                m_Timer.Resume();
            }
        }

        public void Destroy()
        {
            m_ShotMesh.Enabled = false;
        }

        public bool Active()
        {
            if (m_ShotMesh != null)
                return m_ShotMesh.Enabled;
            else
                return false;
        }
    }
}
