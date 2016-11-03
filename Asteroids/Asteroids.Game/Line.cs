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
    public class Line : Actor
    {
        public Entity m_Line;
        ModelComponent m_LineMesh;
        public float m_TimerAmount = 0;
        TimerTick m_Timer = new TimerTick();

        public override void Start()
        {
            // VertexPositionNormalTexture is the layout that the engine uses in the shaders
            var vBuffer = SiliconStudio.Xenko.Graphics.Buffer.Vertex.New(GraphicsDevice, new VertexPositionNormalTexture[]
            {
                 new VertexPositionNormalTexture(new Vector3(-0.33f, 0.33f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Top Left.
                 new VertexPositionNormalTexture(new Vector3(-0.33f, -0.33f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)) //Bottom right.
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
            m_LineMesh = new ModelComponent(model);
            m_Line = new Entity();
            m_Line.Add(m_LineMesh);
            this.Entity.AddChild(m_Line);
            Destroy();
        }

        public override void Update()
        {
            if (m_LineMesh.Enabled && !m_Pause)
            {
                base.Update();

                if (m_Timer.TotalTime.TotalSeconds > m_TimerAmount)
                {
                    Destroy();
                }

                m_Timer.Tick();
            }
        }
        public void Spawn(Vector3 position, float rotation, float timer, float speed, float rotationSpeed)
        {
            m_Position = position;
            m_Rotation = rotation;
            m_RotationVelocity = rotationSpeed;
            m_Timer.Reset();
            m_TimerAmount = timer;
            m_LineMesh.Enabled = true;
            SetVelocity(speed);
            UpdatePR();
        }

        void Destroy()
        {
            m_LineMesh.Enabled = false;
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

        public bool Active()
        {
            if (m_LineMesh != null)
                return m_LineMesh.Enabled;
            else
                return false;
        }
    }
}
