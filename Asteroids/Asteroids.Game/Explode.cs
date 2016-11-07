using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;

namespace Asteroids
{
    public class Explode : Actor
    {
        protected List<Dot> m_Explosion;

        public override void Start()
        {
            base.Start();

            m_Explosion = new List<Dot>();
            Prefab dotPrefab;
            dotPrefab = Content.Load<Prefab>("Dot");

            for (int dot = 0; dot < 12; dot++)
            {
                Entity dotE = dotPrefab.Instantiate().First();
                SceneSystem.SceneInstance.Scene.Entities.Add(dotE);
                m_Explosion.Add(dotE.Components.Get<Dot>());
                m_Explosion[dot].Initialize(m_Random);
            }
        }

        public override void Update()
        {
            base.Update(); //Needed to update rock and UFO positions.
        }

        public void SpawnExplosion()
        {
            foreach (Dot dot in m_Explosion)
            {
                Vector3 dotPos = m_Position;
                dotPos.X += (float)m_Random.NextDouble() * 2 - 1;
                dotPos.Y += (float)m_Random.NextDouble() * 2 - 1;
                float timer = (float)m_Random.NextDouble() * 2 + 0.1f;
                float speed = (float)m_Random.NextDouble() * 5 + 3;

                dot.Spawn(dotPos, timer, speed);
            }
        }

        public virtual void Pause(bool pause)
        {
            m_Pause = pause;

            if (m_Pause)
            {
                if (m_Explosion != null)
                {
                    foreach (Dot dot in m_Explosion)
                    {
                        dot.Pause(true);
                    }
                }
            }
            else
            {
                if (m_Explosion != null)
                {
                    foreach (Dot dot in m_Explosion)
                    {
                        dot.Pause(false);
                    }
                }
            }
        }
    }
}
