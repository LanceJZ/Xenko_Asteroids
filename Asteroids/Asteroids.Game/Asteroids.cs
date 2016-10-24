using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Games.Time;

namespace Asteroids
{
    public class Asteroids : SyncScript
    {
        // Declared public member fields and properties will show in the game studio
        private readonly Random m_Random = new Random(DateTime.Now.Millisecond);
        Prefab m_PlayerPrefab;
        Prefab m_RockPrefab;
        Prefab m_UFOPrefab;
        List<Entity> m_LargeRocks;
        List<Entity> m_MedRocks;
        List<Entity> m_SmallRocks;
        Entity m_Player;
        Entity m_UFO;
        TimerTick m_UFOTimer = new TimerTick();
        int m_LargeRockCount = 4;
        int m_Wave = 0;
        int m_UFOCount;
        float m_UFOTimerAmount = 10.15f;

        public override void Start()
        {
            m_LargeRocks = new List<Entity>();
            m_MedRocks = new List<Entity>();
            m_SmallRocks = new List<Entity>();

            // Initialization of the script.
            m_RockPrefab = Content.Load<Prefab>("Asteroid");
            m_PlayerPrefab = Content.Load<Prefab>("Player");
            m_UFOPrefab = Content.Load<Prefab>("UFO");

            m_Player = m_PlayerPrefab.Instantiate().First();
            m_Player.Components.Get<Player>().m_Random = m_Random;
            SceneSystem.SceneInstance.Scene.Entities.Add(m_Player);
            m_UFO = m_UFOPrefab.Instantiate().First();
            m_UFO.Components.Get<UFO>().m_Random = m_Random;
            SceneSystem.SceneInstance.Scene.Entities.Add(m_UFO);
            SpawnLargeRocks(m_LargeRockCount);
        }

        public override void Update()
        {
            int rockCount = 0;
            // Do stuff every new frame
            foreach (Entity rock in m_LargeRocks)
            {
                if (rock.Components.Get<Rock>().m_Hit)
                {
                    rock.Components.Get<Rock>().Destroy();
                    SpawnMedRocks(rock.Components.Get<Rock>().m_Position);
                }

                if (rock.Components.Get<Rock>().Active())
                    rockCount++;
            }

            foreach (Entity rock in m_MedRocks)
            {
                if (rock.Components.Get<Rock>().m_Hit)
                {
                    rock.Components.Get<Rock>().Destroy();
                    SpawnSmallRocks(rock.Components.Get<Rock>().m_Position);
                }

                if (rock.Components.Get<Rock>().Active())
                    rockCount++;
            }

            foreach (Entity rock in m_SmallRocks)
            {
                if (rock.Components.Get<Rock>().m_Hit)
                {
                    rock.Components.Get<Rock>().Destroy();
                }

                if (rock.Components.Get<Rock>().Active())
                    rockCount++;
            }

            if (rockCount == 0)
            {
                m_LargeRockCount++;
                SpawnLargeRocks(m_LargeRockCount);
            }

            if (m_UFOTimer.TotalTime.Seconds > m_UFOTimerAmount && !m_UFO.Components.Get<UFO>().Active())
            {
                m_UFOTimer.Reset();
                m_UFO.Components.Get<UFO>().Spawn(m_UFOCount, m_Wave);
                m_UFOCount++;
            }

            if (m_UFO.Components.Get<UFO>().m_Done || m_UFO.Components.Get<UFO>().m_Hit)
            {
                m_UFOTimer.Reset();
                m_UFO.Components.Get<UFO>().Destroy();
            }

            m_UFOTimer.Tick();
        }

        void SpawnLargeRocks(int count)
        {
            m_Wave++;

            for (int i = 0; i < count; i++)
            {
                bool spawnNewRock = true;

                foreach (Entity rock in m_LargeRocks)
                {
                    if (!rock.Components.Get<Rock>().Active())
                    {
                        spawnNewRock = false;
                        rock.Components.Get<Rock>().Large();
                        break;
                    }
                }

                if (spawnNewRock)
                {
                    int rock = m_LargeRocks.Count;
                    m_LargeRocks.Add(m_RockPrefab.Instantiate().First());
                    SceneSystem.SceneInstance.Scene.Entities.Add(m_LargeRocks[rock]);
                    m_LargeRocks[rock].Components.Get<Rock>().Initialize(m_Random, m_Player, m_UFO);
                    m_LargeRocks[rock].Components.Get<Rock>().Large();
                }
            }
        }

        void SpawnMedRocks(Vector3 position)
        {
            for (int i = 0; i < 2; i++)
            {
                bool spawnNewRock = true;

                foreach (Entity rock in m_MedRocks)
                {
                    if (!rock.Components.Get<Rock>().Active())
                    {
                        spawnNewRock = false;
                        rock.Components.Get<Rock>().Spawn(position);
                        break;
                    }
                }

                if (spawnNewRock)
                {
                    int rock = m_MedRocks.Count;
                    m_MedRocks.Add(m_RockPrefab.Instantiate().First());
                    SceneSystem.SceneInstance.Scene.Entities.Add(m_MedRocks[rock]);
                    m_MedRocks[rock].Components.Get<Rock>().Spawn(position, 0.5f, 10, m_Random, m_Player, m_UFO);
                }
            }
        }


        void SpawnSmallRocks(Vector3 position)
        {
            for (int i = 0; i < 2; i++)
            {
                bool spawnNewRock = true;

                foreach (Entity rock in m_SmallRocks)
                {
                    if (!rock.Components.Get<Rock>().Active())
                    {
                        spawnNewRock = false;
                        rock.Components.Get<Rock>().Spawn(position);
                        break;
                    }
                }

                if (spawnNewRock)
                {
                    int rock = m_SmallRocks.Count;
                    m_SmallRocks.Add(m_RockPrefab.Instantiate().First());
                    SceneSystem.SceneInstance.Scene.Entities.Add(m_SmallRocks[rock]);
                    m_SmallRocks[rock].Components.Get<Rock>().Spawn(position, 0.25f, 20, m_Random, m_Player, m_UFO);
                }
            }
        }
    }
}
