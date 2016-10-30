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
    public class Actor : SyncScript
    {
        public bool m_Hit = false;
        public float m_Rotation = 0;
        public float m_RotationAmount = 0;
        public float m_Radius = 0;
        public Random m_Random;
        public Vector3 m_Position = Vector3.Zero;
        public Vector3 m_Velocity = Vector3.Zero;
        public Vector3 m_Acceleration = Vector3.Zero;
        public Vector2 m_Edge = new Vector2(44, 32);

        public override void Update()
        {
            //Calculate movement this frame according to velocity and acceleration.
            float elapsed = (float)Game.UpdateTime.Elapsed.TotalSeconds;

            m_Velocity += m_Acceleration;
            m_Position += m_Velocity * elapsed;
            m_Rotation += m_RotationAmount * elapsed;

            if (m_Rotation < 0)
                m_Rotation = MathUtil.TwoPi;

            if (m_Rotation > MathUtil.TwoPi)
                m_Rotation = 0;

            UpdatePR();
        }

        public void UpdatePR()
        {
            this.Entity.Transform.Position = m_Position;
            this.Entity.Transform.RotationEulerXYZ = new Vector3(0, 0, m_Rotation);
        }

        public bool CirclesIntersect(Vector3 Target, float TargetRadius)
        {
            float dx = Target.X - m_Position.X;
            float dy = Target.Y - m_Position.Y;
            float rad = m_Radius + TargetRadius;

            if ((dx * dx) + (dy * dy) < rad * rad)
                return true;

            return false;
        }

        public void CheckForEdge()
        {
            if (m_Position.X > m_Edge.X)
            {
                m_Position.X = -m_Edge.X;
            }

            if (m_Position.X < -m_Edge.X)
            {
                m_Position.X = m_Edge.X;
            }

            if (m_Position.Y > m_Edge.Y)
            {
                m_Position.Y = -m_Edge.Y;
            }

            if (m_Position.Y < -m_Edge.Y)
            {
                m_Position.Y = m_Edge.Y;
            }
        }

        public float RandomRadian()
        {
            return (float)m_Random.NextDouble() * (float)(MathUtil.TwoPi);
        }

        public float RandomHieght()
        {
            return m_Random.Next((int)-m_Edge.Y, (int)m_Edge.Y);
        }

        public void SetVelocity(float speed)
        {
            float rad = RandomRadian();
            float amt = (float)m_Random.NextDouble() * speed + (speed * 0.15f);
            m_Velocity = new Vector3((float)Math.Cos(rad) * amt, (float)Math.Sin(rad) * amt, 0);
        }
    }
}
