using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Games.Time;
using SiliconStudio.Xenko.Audio;
using System.Diagnostics;

namespace Asteroids
{
    public class Asteroids : SyncScript
    {
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
        int m_UFOCount = 0;
        int m_NumberOfRocksThisFrame;
        int m_NumberOfRocksLastFrame;
        readonly float m_UFOTimerAmount = 10.15f;
        float m_UFOTimerSet = 10.15f;
        SoundInstance m_Background;
        GameData m_Data = new GameData();
        
        public override void Start()
        {
            m_LargeRocks = new List<Entity>();
            m_MedRocks = new List<Entity>();
            m_SmallRocks = new List<Entity>();

            m_RockPrefab = Content.Load<Prefab>("Asteroid");
            m_PlayerPrefab = Content.Load<Prefab>("Player");
            m_UFOPrefab = Content.Load<Prefab>("UFO");

            m_Player = m_PlayerPrefab.Instantiate().First();
            m_Player.Components.Get<Player>().m_Random = m_Random;
            SceneSystem.SceneInstance.Scene.Entities.Add(m_Player);
            m_UFO = m_UFOPrefab.Instantiate().First();
            m_UFO.Components.Get<UFO>().m_Random = m_Random;
            m_UFO.Components.Get<UFO>().m_Player = m_Player;
            SceneSystem.SceneInstance.Scene.Entities.Add(m_UFO);
            SpawnLargeRocks(m_LargeRockCount);

            Sound background = Content.Load<Sound>("Background");
            m_Background = background.CreateInstance();
            m_Background.Volume = 0.50f;

            //m_Data.OpenForWrite();
            //HishScoreData score = new HishScoreData();
            //score.Name = "ZIM";
            //score.Score = 666;
            //m_Data.Write(score);

            //score.Name = "LJZ";
            //score.Score = 6666;
            //m_Data.Write(score);

            //m_Data.Close();

            //m_Data.OpenForRead();
            //string scoreData = m_Data.Read();
            

            //Debug.WriteLine(scoreData);
            
        }

        public override void Update()
        {
            m_NumberOfRocksLastFrame = m_NumberOfRocksThisFrame;
            int rockCount = 0;
            int smrockCount = 0;
            int mdrockCount = 0;
            int lgrockCount = 0;
            bool playerClear = true;

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
                    lgrockCount++;
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
                    mdrockCount++;
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
                    smrockCount++;
                }
            }

            m_NumberOfRocksThisFrame = rockCount;

            if (m_NumberOfRocksLastFrame != m_NumberOfRocksThisFrame) //If([size]rockCount  < 4) {pitch = 1.2f + (0.05f × ( 4 - mdrockCount))}
            {
                float pitch = 1;

                if (lgrockCount == 0)
                {
                    if (mdrockCount > 3)
                        pitch = 1.2f;
                    else if (mdrockCount == 3)
                        pitch = 1.25f;
                    else if (mdrockCount == 2)
                        pitch = 1.3f;
                    else if (mdrockCount == 1)
                        pitch = 1.35f;

                    if (mdrockCount == 0)
                    {
                        if (smrockCount > 3)
                            pitch = 1.4f;
                        else if (smrockCount == 3)
                            pitch = 1.45f;
                        else if (smrockCount == 2)
                            pitch = 1.5f;
                        else if (smrockCount == 1)
                            pitch = 1.55f;
                    }
                }
                else if (lgrockCount > 3)
                    pitch = 1;
                else if (lgrockCount == 3)
                    pitch = 1.05f;
                else if (lgrockCount == 2)
                    pitch = 1.1f;
                else if (lgrockCount == 1)
                    pitch = 1.15f;

                m_Background.Pitch = pitch;
            }

            if (rockCount == 0)
            {
                m_LargeRockCount += 2;
                SpawnLargeRocks(m_LargeRockCount);
            }

            if (m_UFOTimer.TotalTime.TotalSeconds > m_UFOTimerSet && !m_UFO.Components.Get<UFO>().Active())
            {
                m_UFOTimerSet = (float)m_Random.NextDouble() * m_UFOTimerAmount + ((m_UFOTimerAmount - m_Wave) * 0.5f);
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
                if (Input.IsKeyPressed(Keys.N) || Input.IsKeyPressed(Keys.S) || Input.IsKeyPressed(Keys.Return))
                {
                    NewGame();
                }

                if (!m_UFO.Components.Get<UFO>().m_GameOver)
                {
                    m_Background.Stop();

                    foreach (Entity rock in m_LargeRocks)
                    {
                        rock.Components.Get<Rock>().m_GameOver = true;
                    }

                    foreach (Entity rock in m_MedRocks)
                    {
                        rock.Components.Get<Rock>().m_GameOver = true;
                    }

                    foreach (Entity rock in m_SmallRocks)
                    {
                        rock.Components.Get<Rock>().m_GameOver = true;
                    }

                    m_UFO.Components.Get<UFO>().m_GameOver = true;
                }
            }
            else
            {
                if (m_UFO.Components.Get<UFO>().m_GameOver)
                {                 
                    foreach (Entity rock in m_LargeRocks)
                    {
                        rock.Components.Get<Rock>().m_GameOver = false;
                    }

                    foreach (Entity rock in m_MedRocks)
                    {
                        rock.Components.Get<Rock>().m_GameOver = false;
                    }

                    foreach (Entity rock in m_SmallRocks)
                    {
                        rock.Components.Get<Rock>().m_GameOver = false;
                    }

                    m_UFO.Components.Get<UFO>().m_GameOver = false;
                }

                if (m_Player.Components.Get<Player>().m_Pause)
                {
                    if (Input.IsKeyPressed(Keys.P))
                    {
                        m_Background.Play();

                        foreach (Entity rock in m_LargeRocks)
                        {
                            rock.Components.Get<Rock>().Pause(false);
                        }

                        foreach (Entity rock in m_MedRocks)
                        {
                            rock.Components.Get<Rock>().Pause(false);
                        }

                        foreach (Entity rock in m_SmallRocks)
                        {
                            rock.Components.Get<Rock>().Pause(false);
                        }

                        m_UFO.Components.Get<UFO>().Pause(false);
                        m_Player.Components.Get<Player>().Pause(false);

                        foreach (Entity shot in m_Player.Components.Get<Player>().m_Shots)
                        {
                            shot.Components.Get<Shot>().Pause(false);
                        }
                    }
                }
                else
                {
                    m_Background.Play();

                    if (Input.IsKeyPressed(Keys.P))
                    {
                        m_Background.Stop();

                        foreach (Entity rock in m_LargeRocks)
                        {
                            rock.Components.Get<Rock>().Pause(true);
                        }

                        foreach (Entity rock in m_MedRocks)
                        {
                            rock.Components.Get<Rock>().Pause(true);
                        }

                        foreach (Entity rock in m_SmallRocks)
                        {
                            rock.Components.Get<Rock>().Pause(true);
                        }

                        m_UFO.Components.Get<UFO>().Pause(true);
                        m_Player.Components.Get<Player>().Pause(true);

                        foreach (Entity shot in m_Player.Components.Get<Player>().m_Shots)
                        {
                            shot.Components.Get<Shot>().Pause(true);
                        }
                    }
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
                    m_MedRocks[rock].Components.Get<Rock>().Spawn(position, 0.5f, 10, 50, m_Random, m_Player, m_UFO);
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
                    m_SmallRocks[rock].Components.Get<Rock>().Spawn(position, 0.25f, 20, 100, m_Random, m_Player, m_UFO);
                }
            }
        }
    }
}
