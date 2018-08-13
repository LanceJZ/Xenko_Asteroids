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
using Xenko.Core.IO;
using System.IO;

namespace Asteroids
{
    public struct HighScoreData
    {
        public string Name;
        public int Score;
    }

    public class GameData
    {
        string m_FileName = "/roaming/Score.sav";
        Stream stream;

        public bool DoesFileExist()
        {
            return VirtualFileSystem.FileExists(m_FileName);
        }

        public void OpenForWrite()
        {
            stream = VirtualFileSystem.OpenStream(m_FileName, VirtualFileMode.OpenOrCreate, VirtualFileAccess.Write);
        }

        public void OpenForRead()
        {
            stream = VirtualFileSystem.OpenStream(m_FileName, VirtualFileMode.Open, VirtualFileAccess.Read);
        }

        public void Close()
        {
            byte[] marker = new UTF8Encoding(true).GetBytes("*");
            stream.Write(marker, 0, marker.Length);

            stream.Flush();
            stream.Dispose();
        }

        public void Write(HighScoreData data)
        {
            byte[] name = new UTF8Encoding(true).GetBytes(data.Name);
            stream.Write(name, 0, name.Length);

            byte[] score = new UTF8Encoding(true).GetBytes(data.Score.ToString());
            stream.Write(score, 0, score.Length);

            byte[] marker = new UTF8Encoding(true).GetBytes(":");
            stream.Write(marker, 0, marker.Length);
        }

        public string Read()
        {
            string data = "";

            byte[] b = new byte[1024];
            UTF8Encoding buffer = new UTF8Encoding(true);

            while (stream.Read(b, 0, b.Length) > 0)
            {
                data += buffer.GetString(b, 0, b.Length);
            }

            stream.Dispose();

            return data;
        }
    }
}
