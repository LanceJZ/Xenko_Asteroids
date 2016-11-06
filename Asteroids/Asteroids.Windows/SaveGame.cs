using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Asteroids
{
    public struct HishScoreData
    {
        string Name;
        int Score;
    }

    public class SaveGame
    {
        string m_FileName;

        public void Write(HishScoreData scoreData)
        {
            // create the file
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(m_FileName, FileMode.Create)))
                {
                    writer.Write(1.250F);
                    writer.Write(@"c:\Temp");
                    writer.Write(10);
                    writer.Write(true);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot create file.");
                return;
            }
        }

        public HishScoreData Read()
        {
            HishScoreData readFile = new HishScoreData();

            float aspectRatio;
            string tempDirectory;
            int autoSaveTime;
            bool showStatusBar;

            if (File.Exists(m_FileName))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(m_FileName, FileMode.Open)))
                {
                    aspectRatio = reader.ReadSingle();
                    tempDirectory = reader.ReadString();
                    autoSaveTime = reader.ReadInt32();
                    showStatusBar = reader.ReadBoolean();
                }
            }

            return readFile;
        }
    }
}
