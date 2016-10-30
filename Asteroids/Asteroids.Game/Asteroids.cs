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
        Prefab m_NumberPrefab;
        Prefab m_WordPrefab;
        List<Entity> m_LargeRocks;
        List<Entity> m_MedRocks;
        List<Entity> m_SmallRocks;
        Entity m_Player;
        Entity m_UFO;
        Entity m_Score;
        Entity m_HighScore;
        Entity m_AtariEntity;
        Entity m_AtariDateEntity;
        Entity m_GameOver;
        TimerTick m_UFOTimer = new TimerTick();
        int m_LargeRockCount = 4;
        int m_Wave = 0;
        int m_UFOCount = 0;
        float m_UFOTimerAmount = 10.15f;

        public override void Start()
        {
            m_LargeRocks = new List<Entity>();
            m_MedRocks = new List<Entity>();
            m_SmallRocks = new List<Entity>();

            m_RockPrefab = Content.Load<Prefab>("Asteroid");
            m_PlayerPrefab = Content.Load<Prefab>("Player");
            m_UFOPrefab = Content.Load<Prefab>("UFO");
            m_NumberPrefab = Content.Load<Prefab>("Number");
            m_WordPrefab = Content.Load<Prefab>("Word");

            m_Score = m_NumberPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_Score);
            m_HighScore = m_NumberPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_HighScore);
            m_AtariEntity = m_WordPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_AtariEntity);
            m_AtariDateEntity = m_NumberPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_AtariDateEntity);
            m_GameOver = m_WordPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(m_GameOver);
            m_Player = m_PlayerPrefab.Instantiate().First();
            m_Player.Components.Get<Player>().m_Random = m_Random;
            m_Player.Components.Get<Player>().m_Score = m_Score;
            m_Player.Components.Get<Player>().m_HighScore = m_HighScore;
            m_Player.Components.Get<Player>().m_AtariEntity = m_AtariEntity;
            m_Player.Components.Get<Player>().m_AtariDateEntity = m_AtariDateEntity;
            m_Player.Components.Get<Player>().m_GameOverEntity = m_GameOver;
            SceneSystem.SceneInstance.Scene.Entities.Add(m_Player);
            m_UFO = m_UFOPrefab.Instantiate().First();
            m_UFO.Components.Get<UFO>().m_Random = m_Random;
            m_UFO.Components.Get<UFO>().m_Player = m_Player;
            SceneSystem.SceneInstance.Scene.Entities.Add(m_UFO);
            SpawnLargeRocks(m_LargeRockCount);
        }

        public override void Update()
        {
            int rockCount = 0;
            bool playerClear = true;
            // Do stuff every new frame
            foreach (Entity rock in m_LargeRocks)
            {
                if (rock.Components.Get<Rock>().m_Hit)
                {
                    rock.Components.Get<Rock>().Destroy();
                    SpawnMedRocks(rock.Components.Get<Rock>().m_Position);
                }

                if (rock.Components.Get<Rock>().Active())
                {
                    rockCount++;
                }
            }

            foreach (Entity rock in m_MedRocks)
            {
                if (rock.Components.Get<Rock>().m_Hit)
                {
                    rock.Components.Get<Rock>().Destroy();
                    SpawnSmallRocks(rock.Components.Get<Rock>().m_Position);
                }

                if (rock.Components.Get<Rock>().Active())
                {
                    rockCount++;
                }
            }

            foreach (Entity rock in m_SmallRocks)
            {
                if (rock.Components.Get<Rock>().m_Hit)
                {
                    rock.Components.Get<Rock>().Destroy();
                }

                if (rock.Components.Get<Rock>().Active())
                {
                    rockCount++;
                }
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

            if (m_Player.Components.Get<Player>().m_Hit)
            {
                foreach (Entity rock in m_LargeRocks)
                {
                    if (rock.Components.Get<Rock>().Active())
                    {
                        if (m_Player.Components.Get<Player>().m_Hit)
                        {
                            if (!rock.Components.Get<Rock>().CheckPlayerCLear())
                            {
                                playerClear = false;
                                break;
                            }
                        }
                    }
                }

                foreach (Entity rock in m_MedRocks)
                {
                    if (rock.Components.Get<Rock>().Active())
                    {
                        if (m_Player.Components.Get<Player>().m_Hit)
                        {
                            if (!playerClear)
                                break;

                            if (!rock.Components.Get<Rock>().CheckPlayerCLear())
                            {
                                playerClear = false;
                                break;
                            }
                        }
                    }
                }

                foreach (Entity rock in m_SmallRocks)
                {
                    if (rock.Components.Get<Rock>().Active())
                    {
                        if (m_Player.Components.Get<Player>().m_Hit)
                        {
                            if (!playerClear)
                                break;

                            if (!rock.Components.Get<Rock>().CheckPlayerCLear())
                            {
                                playerClear = false;
                                break;
                            }
                        }
                    }
                }

                if (playerClear)
                    m_Player.Components.Get<Player>().m_Spawn = true;

                if (m_UFO.Components.Get<UFO>().Active())
                {
                    if (!m_UFO.Components.Get<UFO>().CheckPlayerClear())
                        m_Player.Components.Get<Player>().m_Spawn = false;
                }

                if (m_UFO.Components.Get<UFO>().m_Shot.Components.Get<Shot>().Active())
                {
                    if (!m_UFO.Components.Get<UFO>().m_Shot.Components.Get<Shot>().CheckPlayerClear())
                        m_Player.Components.Get<Player>().m_Spawn = false;
                }
            }

            m_UFOTimer.Tick();

            if (m_Player.Components.Get<Player>().m_GameOver)
            {
                if (Input.IsKeyPressed(Keys.N))
                {
                    NewGame();
                }
            }
        }

        void NewGame()
        {
            m_LargeRockCount = 4;
            m_Wave = 0;
            m_UFOCount = 0;

            foreach (Entity rock in m_LargeRocks)
            {
                if (rock.Components.Get<Rock>().Active())
                {
                    rock.Components.Get<Rock>().Destroy();
                }
            }

            foreach (Entity rock in m_MedRocks)
            {
                if (rock.Components.Get<Rock>().Active())
                {
                    rock.Components.Get<Rock>().Destroy();
                }
            }

            foreach (Entity rock in m_SmallRocks)
            {
                if (rock.Components.Get<Rock>().Active())
                {
                    rock.Components.Get<Rock>().Destroy();
                }
            }

            if (m_UFO.Components.Get<UFO>().Active())
                m_UFO.Components.Get<UFO>().Destroy();

            if (m_UFO.Components.Get<UFO>().m_Shot != null)
            {
                if (m_UFO.Components.Get<UFO>().m_Shot.Components.Get<Shot>().Active())
                    m_UFO.Components.Get<UFO>().m_Shot.Components.Get<Shot>().Destroy();
            }

            m_UFOTimer.Reset();

            m_Player.Components.Get<Player>().NewGame();

            SpawnLargeRocks(m_LargeRockCount);
        }

        void SpawnLargeRocks(int count)
        {
            m_Wave++;

            for (int i = 0; i < count; i++)
            {
                bool spawnNewRock = true;

                foreach (Entity rock in m_LargeRocks)
                {
                    if (!rock.Components.Get<Rock>().Active() && !rock.Components.Get<Rock>().ExplosionActive())
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
                    m_LargeRocks[rock].Components.Get<Rock>().Initialize(m_Random, m_Player, m_UFO, m_Score);
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
                    if (!rock.Components.Get<Rock>().Active() && !rock.Components.Get<Rock>().ExplosionActive())
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
                    m_MedRocks[rock].Components.Get<Rock>().Spawn(position, 0.5f, 10, 50, m_Random, m_Player, m_UFO, m_Score);
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
                    if (!rock.Components.Get<Rock>().Active() && !rock.Components.Get<Rock>().ExplosionActive())
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
                    m_SmallRocks[rock].Components.Get<Rock>().Spawn(position, 0.25f, 20, 100, m_Random, m_Player, m_UFO, m_Score);
                }
            }
        }
    }
}
