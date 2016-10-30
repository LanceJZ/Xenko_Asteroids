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
        protected List<Entity> m_Explosion;

        public override void Start()
        {
            base.Start();

            m_Explosion = new List<Entity>();
            Prefab dotPrefab;
            dotPrefab = Content.Load<Prefab>("Dot");

            for (int dot = 0; dot < 12; dot++)
            {
                m_Explosion.Add(dotPrefab.Instantiate().First());
                m_Explosion[dot].Components.Get<Dot>().m_Random = m_Random;
                SceneSystem.SceneInstance.Scene.Entities.Add(m_Explosion[dot]);
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public void SpawnExplosion()
        {
            foreach (Entity dot in m_Explosion)
            {
                Vector3 dotPos = m_Position;
                dotPos.X += (float)m_Random.NextDouble() * 2 - 1;
                dotPos.Y += (float)m_Random.NextDouble() * 2 - 1;
                float timer = (float)m_Random.NextDouble() * 2 + 0.1f;
                float speed = (float)m_Random.NextDouble() * 5 + 3;

                dot.Components.Get<Dot>().Spawn(dotPos, timer, speed);
            }
        }
    }
}
