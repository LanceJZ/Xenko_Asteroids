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
        List<Rock> m_LargeRocks;
        List<Rock> m_MedRocks;
        List<Rock> m_SmallRocks;
        Player m_Player;
        UFO m_UFO;
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
            m_LargeRocks = new List<Rock>();
            m_MedRocks = new List<Rock>();
            m_SmallRocks = new List<Rock>();

            m_RockPrefab = Content.Load<Prefab>("Asteroid");
            m_PlayerPrefab = Content.Load<Prefab>("Player");
            m_UFOPrefab = Content.Load<Prefab>("UFO");

            Entity player;
            player = m_PlayerPrefab.Instantiate().First();
            SceneSystem.SceneInstance.Scene.Entities.Add(player);
            m_Player = player.Components.Get<Player>();
            m_Player.Initilize(m_Random);
            Entity ufo = m_UFOPrefab.Instantiate().First(); ;
            SceneSystem.SceneInstance.Scene.Entities.Add(ufo);
            m_UFO = ufo.Components.Get<UFO>();
            m_UFO.Initialize(m_Player, m_Random);
            SpawnLargeRocks(m_LargeRockCount);

            Sound background = Content.Load<Sound>("Background");
            m_Background = background.CreateInstance();
            m_Background.Volume = 0.50f;
        }

        public override void Update()
        {
            m_NumberOfRocksLastFrame = m_NumberOfRocksThisFrame;
            int rockCount = 0;
            int smrockCount = 0;
            int mdrockCount = 0;
            int lgrockCount = 0;
            bool playerClear = true;

            foreach (Rock rock in m_LargeRocks)
            {
                if (rock.m_Hit)
                {
                    rock.Destroy();
                    SpawnMedRocks(rock.m_Position);
                }

                if (rock.Active())
                {
                    rockCount++;
                    lgrockCount++;
                }
            }

            foreach (Rock rock in m_MedRocks)
            {
                if (rock.m_Hit)
                {
                    rock.Destroy();
                    SpawnSmallRocks(rock.m_Position);
                }

                if (rock.Active())
                {
                    rockCount++;
                    mdrockCount++;
                }
            }

            foreach (Rock rock in m_SmallRocks)
            {
                if (rock.m_Hit)
                {
                    rock.Destroy();
                }

                if (rock.Active())
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

            if (m_UFOTimer.TotalTime.TotalSeconds > m_UFOTimerSet && !m_UFO.Active())
            {
                m_UFOTimerSet = (float)m_Random.NextDouble() * m_UFOTimerAmount + ((m_UFOTimerAmount - m_Wave) * 0.5f);
                m_UFOTimer.Reset();
                m_UFO.Spawn(m_UFOCount, m_Wave);
                m_UFOCount++;
            }

            if (m_UFO.m_Done || m_UFO.m_Hit)
            {
                m_UFOTimer.Reset();
                m_UFO.Destroy();
            }

            if (m_Player.m_Hit)
            {
                foreach (Rock rock in m_LargeRocks)
                {
                    if (rock.Active())
                    {
                        if (m_Player.m_Hit)
                        {
                            if (!rock.CheckPlayerCLear())
                            {
                                playerClear = false;
                                break;
                            }
                        }
                    }
                }

                foreach (Rock rock in m_MedRocks)
                {
                    if (rock.Active())
                    {
                        if (m_Player.m_Hit)
                        {
                            if (!playerClear)
                                break;

                            if (!rock.CheckPlayerCLear())
                            {
                                playerClear = false;
                                break;
                            }
                        }
                    }
                }

                foreach (Rock rock in m_SmallRocks)
                {
                    if (rock.Active())
                    {
                        if (m_Player.m_Hit)
                        {
                            if (!playerClear)
                                break;

                            if (!rock.CheckPlayerCLear())
                            {
                                playerClear = false;
                                break;
                            }
                        }
                    }
                }

                if (playerClear)
                    m_Player.m_Spawn = true;

                if (m_UFO.Active())
                {
                    if (!m_UFO.CheckPlayerClear())
                        m_Player.m_Spawn = false;
                }

                if (m_UFO.m_Shot.Active())
                {
                    if (!m_UFO.m_Shot.CheckPlayerClear())
                        m_Player.m_Spawn = false;
                }
            }

            m_UFOTimer.Tick();

            if (m_Player.m_GameOver)
            {
                if (Input.IsKeyPressed(Keys.N) || Input.IsKeyPressed(Keys.S) || Input.IsKeyPressed(Keys.Return))
                {
                    NewGame();
                }

                if (!m_UFO.m_GameOver)
                {
                    m_Background.Stop();

                    foreach (Rock rock in m_LargeRocks)
                    {
                        rock.m_GameOver = true;
                    }

                    foreach (Rock rock in m_MedRocks)
                    {
                        rock.m_GameOver = true;
                    }

                    foreach (Rock rock in m_SmallRocks)
                    {
                        rock.m_GameOver = true;
                    }

                    m_UFO.m_GameOver = true;
                }
            }
            else
            {
                m_UFO.m_GameOver = false;

                if (m_Player.m_Pause)
                {
                    if (Input.IsKeyPressed(Keys.P))
                    {
                        m_Background.Play();

                        foreach (Rock rock in m_LargeRocks)
                        {
                            rock.Pause(false);
                        }

                        foreach (Rock rock in m_MedRocks)
                        {
                            rock.Pause(false);
                        }

                        foreach (Rock rock in m_SmallRocks)
                        {
                            rock.Pause(false);
                        }

                        m_UFO.Pause(false);
                        m_Player.Pause(false);

                        foreach (Shot shot in m_Player.m_Shots)
                        {
                            shot.Pause(false);
                        }
                    }
                }
                else
                {
                    m_Background.Play();

                    if (Input.IsKeyPressed(Keys.P))
                    {
                        m_Background.Stop();

                        foreach (Rock rock in m_LargeRocks)
                        {
                            rock.Pause(true);
                        }

                        foreach (Rock rock in m_MedRocks)
                        {
                            rock.Pause(true);
                        }

                        foreach (Rock rock in m_SmallRocks)
                        {
                            rock.Pause(true);
                        }

                        m_UFO.Pause(true);
                        m_Player.Pause(true);

                        foreach (Shot shot in m_Player.m_Shots)
                        {
                            shot.Pause(true);
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

            foreach (Rock rock in m_LargeRocks)
            {
                if (rock.Active())
                {
                    rock.Destroy();
                }
            }

            foreach (Rock rock in m_MedRocks)
            {
                if (rock.Active())
                {
                    rock.Destroy();
                }
            }

            foreach (Rock rock in m_SmallRocks)
            {
                if (rock.Active())
                {
                    rock.Destroy();
                }
            }

            if (m_UFO.Active())
                m_UFO.Destroy();

            if (m_UFO.m_Shot != null)
            {
                if (m_UFO.m_Shot.Active())
                    m_UFO.m_Shot.Destroy();
            }

            m_UFOTimer.Reset();
            m_Player.NewGame();

            SpawnLargeRocks(m_LargeRockCount);
        }

        void SpawnLargeRocks(int count)
        {
            m_Wave++;

            for (int i = 0; i < count; i++)
            {
                bool spawnNewRock = true;

                foreach (Rock rock in m_LargeRocks)
                {
                    if (!rock.Active() && !rock.ExplosionActive())
                    {
                        spawnNewRock = false;
                        rock.Large();
                        rock.m_GameOver = m_Player.m_GameOver;
                        break;
                    }
                }

                if (spawnNewRock)
                {
                    int rock = m_LargeRocks.Count;
                    Entity rockE = m_RockPrefab.Instantiate().First();
                    SceneSystem.SceneInstance.Scene.Entities.Add(rockE);
                    m_LargeRocks.Add(rockE.Components.Get<Rock>());
                    m_LargeRocks[rock].Initialize(m_Random, m_Player, m_UFO);
                    m_LargeRocks[rock].Large();
                    m_LargeRocks[rock].m_GameOver = m_Player.m_GameOver;
                }
            }
        }

        void SpawnMedRocks(Vector3 position)
        {
            for (int i = 0; i < 2; i++)
            {
                bool spawnNewRock = true;

                foreach (Rock rock in m_MedRocks)
                {
                    if (!rock.Active() && !rock.ExplosionActive())
                    {
                        spawnNewRock = false;
                        rock.Spawn(position);
                        rock.m_GameOver = m_Player.m_GameOver;
                        break;
                    }
                }

                if (spawnNewRock)
                {
                    int rock = m_MedRocks.Count;
                    Entity rockE = m_RockPrefab.Instantiate().First();
                    SceneSystem.SceneInstance.Scene.Entities.Add(rockE);
                    m_MedRocks.Add(rockE.Components.Get<Rock>());
                    m_MedRocks[rock].Spawn(position, 0.5f, 10, 50, m_Random, m_Player, m_UFO);
                    m_MedRocks[rock].m_GameOver = m_Player.m_GameOver;
                }
            }
        }


        void SpawnSmallRocks(Vector3 position)
        {
            for (int i = 0; i < 2; i++)
            {
                bool spawnNewRock = true;

                foreach (Rock rock in m_SmallRocks)
                {
                    if (!rock.Active() && !rock.ExplosionActive())
                    {
                        spawnNewRock = false;
                        rock.Spawn(position);
                        rock.m_GameOver = m_Player.m_GameOver;
                        break;
                    }
                }

                if (spawnNewRock)
                {
                    int rock = m_SmallRocks.Count;
                    Entity rockE = m_RockPrefab.Instantiate().First();
                    SceneSystem.SceneInstance.Scene.Entities.Add(rockE);
                    m_SmallRocks.Add(rockE.Components.Get<Rock>());
                    m_SmallRocks[rock].Spawn(position, 0.25f, 20, 100, m_Random, m_Player, m_UFO);
                    m_SmallRocks[rock].m_GameOver = m_Player.m_GameOver;
                }
            }
        }
    }
}
