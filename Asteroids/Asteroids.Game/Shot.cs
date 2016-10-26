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
            m_Radius = 0.1f;
            // VertexPositionNormalTexture is the layout that the engine uses in the shaders
            var vBuffer = SiliconStudio.Xenko.Graphics.Buffer.Vertex.New(GraphicsDevice, new VertexPositionNormalTexture[]
            {
                 new VertexPositionNormalTexture(new Vector3(0.0f, 0.05f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Top.
                 new VertexPositionNormalTexture(new Vector3(-0.05f, -0.05f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Bottom right.
                 new VertexPositionNormalTexture(new Vector3(0.05f, -0.05f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)), //Bottom left.
                 new VertexPositionNormalTexture(new Vector3(0.0f, 0.05f, 0), new Vector3(0, 1, 1), new Vector2(0, 0)) //Top.
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
            // Do stuff every new frame
            if (m_ShotMesh.Enabled)
            {
                base.Update();
                CheckForEdge();

                if (m_Timer.TotalTime.Seconds > m_TimerAmount)
                {
                    Destroy();
                }

                m_Timer.Tick();
            }
        }

        public bool CheckPlayerClear()
        {
            if (CirclesIntersect(Vector3.Zero, 20))
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
