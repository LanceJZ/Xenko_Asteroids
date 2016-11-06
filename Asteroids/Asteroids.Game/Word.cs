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
    struct WordData
    {
        public bool[] Lines;
    };

    public class Word : Actor
    {
        WordData[] Letters = new WordData[27];
        Vector3[] m_LetterLineStart = new Vector3[16];
        Vector3[] m_LetterLineEnd = new Vector3[16];
        public List<Entity> m_Words;
        public List<ModelComponent> m_WordMeshs;

        public override void Start()
        {
            for (int i = 0; i < 27; i++)
            {
                Letters[i].Lines = new bool[16];
            }

            InitializeWordLines();
            m_Words = new List<Entity>();
            m_WordMeshs = new List<ModelComponent>();
        }

        public override void Update()
        {
            // Do stuff every new frame
        }

        public void ProcessWords(string words, Vector3 locationStart, float size)
        {
            if (m_Words != null)
            {
                DeleteWords();
                int textSize = words.Length;
                float charsize = 3.15f;
                float space = ((-size * charsize) * (textSize - 1)) / 2;

                foreach (char letter in words)
                {
                    if ((int)letter > 64 && (int)letter < 91 || (int)letter == 95)
                    {
                        int letval = (int)letter - 65;

                        if ((int)letter == 95)
                            letval = 26;

                        if (letval > -1 && letval < 27)
                        {
                            MakeLetterMesh(space, letval, size);
                        }
                    }

                    space -= size * charsize;
                }

                this.Entity.Transform.Position = locationStart;
            }
        }

        void MakeLetterMesh(float location, int letter, float size)
        {
            for (int line = 0; line < 16; line++)
            {
                if (Letters[letter].Lines[line])
                {
                    float Xstart = m_LetterLineStart[line].X * size + location;
                    float Ystart = m_LetterLineStart[line].Y * size;

                    float Xend = m_LetterLineEnd[line].X * size + location;
                    float Yend = m_LetterLineEnd[line].Y * size;

                    Vector3 start = new Vector3(Xstart, Ystart, 0);//numbers->getMesh()->addVertex(Xstart, Ystart, 0);
                    Vector3 end = new Vector3(Xend, Yend, 0);//numbers->getMesh()->addVertex(Xend, Yend, 0);


                    // VertexPositionNormalTexture is the layout that the engine uses in the shaders
                    var vBuffer = SiliconStudio.Xenko.Graphics.Buffer.Vertex.New(GraphicsDevice, new VertexPositionNormalTexture[]
                    {
                             new VertexPositionNormalTexture(start, new Vector3(0, 1, 1), new Vector2(0, 0)), //Top.
                             new VertexPositionNormalTexture(end, new Vector3(0, 1, 1), new Vector2(0, 0)), //Bottom right.
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
                    m_WordMeshs.Add(new ModelComponent(model));
                    m_Words.Add(new Entity());
                    m_Words[m_Words.Count - 1].Add(m_WordMeshs[m_WordMeshs.Count - 1]);
                    this.Entity.AddChild(m_Words[m_Words.Count - 1]);
                }
            }
        }

        public void DeleteWords()
        {
            foreach (Entity word in m_Words)
            {
                this.Entity.RemoveChild(word);                
            }

            m_Words.Clear();
            m_WordMeshs.Clear();
        }

        public void HideWords()
        {
            if (m_WordMeshs != null)
            {
                foreach (ModelComponent word in m_WordMeshs)
                {
                    word.Enabled = false;
                }
            }
        }

        public void ShowWords()
        {
            if (m_WordMeshs != null)
            {
                foreach (ModelComponent word in m_WordMeshs)
                {
                    word.Enabled = true;
                }
            }
        }

        void InitializeWordLines()
        {
            m_LetterLineStart[0] = new Vector3(0, 0, 0); //1
            m_LetterLineStart[1] = new Vector3(-1, 0, 0); //2
            m_LetterLineStart[2] = new Vector3(-2, 0, 0); //3
            m_LetterLineStart[3] = new Vector3(-2, -2, 0); //4
            m_LetterLineStart[4] = new Vector3(-1, -4, 0); //5
            m_LetterLineStart[5] = new Vector3(0, -4, 0); //6
            m_LetterLineStart[6] = new Vector3(0, -2, 0); //7
            m_LetterLineStart[7] = new Vector3(0, 0, 0); //8
            m_LetterLineStart[8] = new Vector3(0, 0, 0); //9
            m_LetterLineStart[9] = new Vector3(-2, 0, 0); //10
            m_LetterLineStart[10] = new Vector3(-1, -2, 0); //11
            m_LetterLineStart[11] = new Vector3(-1, -2, 0); //12
            m_LetterLineStart[12] = new Vector3(-1, -2, 0); //13
            m_LetterLineStart[13] = new Vector3(0, -2, 0); //14
            m_LetterLineStart[14] = new Vector3(-1, 0, 0); //15
            m_LetterLineStart[15] = new Vector3(-1, -2, 0); //16

            m_LetterLineEnd[0] = new Vector3(-1, 0, 0); //1
            m_LetterLineEnd[1] = new Vector3(-2, 0, 0); //2
            m_LetterLineEnd[2] = new Vector3(-2, -2, 0); //3
            m_LetterLineEnd[3] = new Vector3(-2, -4, 0); //4
            m_LetterLineEnd[4] = new Vector3(-2, -4, 0); //5
            m_LetterLineEnd[5] = new Vector3(-1, -4, 0); //6
            m_LetterLineEnd[6] = new Vector3(0, -4, 0); //7
            m_LetterLineEnd[7] = new Vector3(0, -2, 0); //8
            m_LetterLineEnd[8] = new Vector3(-1, -2, 0); //9
            m_LetterLineEnd[9] = new Vector3(-1, -2, 0); //10
            m_LetterLineEnd[10] = new Vector3(-2, -2, 0); //11
            m_LetterLineEnd[11] = new Vector3(-2, -4, 0); //12
            m_LetterLineEnd[12] = new Vector3(0, -4, 0); //13
            m_LetterLineEnd[13] = new Vector3(-1, -2, 0); //14
            m_LetterLineEnd[14] = new Vector3(-1, -2, 0); //15
            m_LetterLineEnd[15] = new Vector3(-1, -4, 0); //16

            // A
            Letters[0].Lines[0] = true;
            Letters[0].Lines[1] = true;
            Letters[0].Lines[2] = true;
            Letters[0].Lines[3] = true;
            Letters[0].Lines[4] = false;
            Letters[0].Lines[5] = false;
            Letters[0].Lines[6] = true;
            Letters[0].Lines[7] = true;
            Letters[0].Lines[8] = false;
            Letters[0].Lines[9] = false;
            Letters[0].Lines[10] = true;
            Letters[0].Lines[11] = false;
            Letters[0].Lines[12] = false;
            Letters[0].Lines[13] = true;
            Letters[0].Lines[14] = false;
            Letters[0].Lines[15] = false;
            // B
            Letters[1].Lines[0] = true;
            Letters[1].Lines[1] = true;
            Letters[1].Lines[2] = true;
            Letters[1].Lines[3] = true;
            Letters[1].Lines[4] = true;
            Letters[1].Lines[5] = true;
            Letters[1].Lines[6] = false;
            Letters[1].Lines[7] = false;
            Letters[1].Lines[8] = false;
            Letters[1].Lines[9] = false;
            Letters[1].Lines[10] = true;
            Letters[1].Lines[11] = false;
            Letters[1].Lines[12] = false;
            Letters[1].Lines[13] = false;
            Letters[1].Lines[14] = true;
            Letters[1].Lines[15] = true;
            // C
            Letters[2].Lines[0] = true;
            Letters[2].Lines[1] = true;
            Letters[2].Lines[2] = false;
            Letters[2].Lines[3] = false;
            Letters[2].Lines[4] = true;
            Letters[2].Lines[5] = true;
            Letters[2].Lines[6] = true;
            Letters[2].Lines[7] = true;
            Letters[2].Lines[8] = false;
            Letters[2].Lines[9] = false;
            Letters[2].Lines[10] = false;
            Letters[2].Lines[11] = false;
            Letters[2].Lines[12] = false;
            Letters[2].Lines[13] = false;
            Letters[2].Lines[14] = false;
            Letters[2].Lines[15] = false;
            // D
            Letters[3].Lines[0] = true;
            Letters[3].Lines[1] = true;
            Letters[3].Lines[2] = true;
            Letters[3].Lines[3] = true;
            Letters[3].Lines[4] = true;
            Letters[3].Lines[5] = true;
            Letters[3].Lines[6] = false;
            Letters[3].Lines[7] = false;
            Letters[3].Lines[8] = false;
            Letters[3].Lines[9] = false;
            Letters[3].Lines[10] = false;
            Letters[3].Lines[11] = false;
            Letters[3].Lines[12] = false;
            Letters[3].Lines[13] = false;
            Letters[3].Lines[14] = true;
            Letters[3].Lines[15] = true;
            // E
            Letters[4].Lines[0] = true;
            Letters[4].Lines[1] = true;
            Letters[4].Lines[2] = false;
            Letters[4].Lines[3] = false;
            Letters[4].Lines[4] = true;
            Letters[4].Lines[5] = true;
            Letters[4].Lines[6] = true;
            Letters[4].Lines[7] = true;
            Letters[4].Lines[8] = false;
            Letters[4].Lines[9] = false;
            Letters[4].Lines[10] = false;
            Letters[4].Lines[11] = false;
            Letters[4].Lines[12] = false;
            Letters[4].Lines[13] = true;
            Letters[4].Lines[14] = false;
            Letters[4].Lines[15] = false;
            // F
            Letters[5].Lines[0] = true;
            Letters[5].Lines[1] = true;
            Letters[5].Lines[2] = false;
            Letters[5].Lines[3] = false;
            Letters[5].Lines[4] = false;
            Letters[5].Lines[5] = false;
            Letters[5].Lines[6] = true;
            Letters[5].Lines[7] = true;
            Letters[5].Lines[8] = false;
            Letters[5].Lines[9] = false;
            Letters[5].Lines[10] = false;
            Letters[5].Lines[11] = false;
            Letters[5].Lines[12] = false;
            Letters[5].Lines[13] = true;
            Letters[5].Lines[14] = false;
            Letters[5].Lines[15] = false;
            // G
            Letters[6].Lines[0] = true;
            Letters[6].Lines[1] = true;
            Letters[6].Lines[2] = false;
            Letters[6].Lines[3] = true;
            Letters[6].Lines[4] = true;
            Letters[6].Lines[5] = true;
            Letters[6].Lines[6] = true;
            Letters[6].Lines[7] = true;
            Letters[6].Lines[8] = false;
            Letters[6].Lines[9] = false;
            Letters[6].Lines[10] = true;
            Letters[6].Lines[11] = false;
            Letters[6].Lines[12] = false;
            Letters[6].Lines[13] = false;
            Letters[6].Lines[14] = false;
            Letters[6].Lines[15] = false;
            // H
            Letters[7].Lines[0] = false;
            Letters[7].Lines[1] = false;
            Letters[7].Lines[2] = true;
            Letters[7].Lines[3] = true;
            Letters[7].Lines[4] = false;
            Letters[7].Lines[5] = false;
            Letters[7].Lines[6] = true;
            Letters[7].Lines[7] = true;
            Letters[7].Lines[8] = false;
            Letters[7].Lines[9] = false;
            Letters[7].Lines[10] = true;
            Letters[7].Lines[11] = false;
            Letters[7].Lines[12] = false;
            Letters[7].Lines[13] = true;
            Letters[7].Lines[14] = false;
            Letters[7].Lines[15] = false;
            // I
            Letters[8].Lines[0] = true;
            Letters[8].Lines[1] = true;
            Letters[8].Lines[2] = false;
            Letters[8].Lines[3] = false;
            Letters[8].Lines[4] = true;
            Letters[8].Lines[5] = true;
            Letters[8].Lines[6] = false;
            Letters[8].Lines[7] = false;
            Letters[8].Lines[8] = false;
            Letters[8].Lines[9] = false;
            Letters[8].Lines[10] = false;
            Letters[8].Lines[11] = false;
            Letters[8].Lines[12] = false;
            Letters[8].Lines[13] = false;
            Letters[8].Lines[14] = true;
            Letters[8].Lines[15] = true;
            // J
            Letters[9].Lines[0] = false;
            Letters[9].Lines[1] = true;
            Letters[9].Lines[2] = true;
            Letters[9].Lines[3] = true;
            Letters[9].Lines[4] = true;
            Letters[9].Lines[5] = true;
            Letters[9].Lines[6] = true;
            Letters[9].Lines[7] = false;
            Letters[9].Lines[8] = false;
            Letters[9].Lines[9] = false;
            Letters[9].Lines[10] = false;
            Letters[9].Lines[11] = false;
            Letters[9].Lines[12] = false;
            Letters[9].Lines[13] = false;
            Letters[9].Lines[14] = false;
            Letters[9].Lines[15] = false;
            // K
            Letters[10].Lines[0] = false;
            Letters[10].Lines[1] = false;
            Letters[10].Lines[2] = false;
            Letters[10].Lines[3] = false;
            Letters[10].Lines[4] = false;
            Letters[10].Lines[5] = false;
            Letters[10].Lines[6] = true;
            Letters[10].Lines[7] = true;
            Letters[10].Lines[8] = false;
            Letters[10].Lines[9] = true;
            Letters[10].Lines[10] = false;
            Letters[10].Lines[11] = true;
            Letters[10].Lines[12] = false;
            Letters[10].Lines[13] = true;
            Letters[10].Lines[14] = false;
            Letters[10].Lines[15] = false;
            // L
            Letters[11].Lines[0] = false;
            Letters[11].Lines[1] = false;
            Letters[11].Lines[2] = false;
            Letters[11].Lines[3] = false;
            Letters[11].Lines[4] = true;
            Letters[11].Lines[5] = true;
            Letters[11].Lines[6] = true;
            Letters[11].Lines[7] = true;
            Letters[11].Lines[8] = false;
            Letters[11].Lines[9] = false;
            Letters[11].Lines[10] = false;
            Letters[11].Lines[11] = false;
            Letters[11].Lines[12] = false;
            Letters[11].Lines[13] = false;
            Letters[11].Lines[14] = false;
            Letters[11].Lines[15] = false;
            // M
            Letters[12].Lines[0] = false;
            Letters[12].Lines[1] = false;
            Letters[12].Lines[2] = true;
            Letters[12].Lines[3] = true;
            Letters[12].Lines[4] = false;
            Letters[12].Lines[5] = false;
            Letters[12].Lines[6] = true;
            Letters[12].Lines[7] = true;
            Letters[12].Lines[8] = true;
            Letters[12].Lines[9] = true;
            Letters[12].Lines[10] = false;
            Letters[12].Lines[11] = false;
            Letters[12].Lines[12] = false;
            Letters[12].Lines[13] = false;
            Letters[12].Lines[14] = false;
            Letters[12].Lines[15] = false;
            // N
            Letters[13].Lines[0] = false;
            Letters[13].Lines[1] = false;
            Letters[13].Lines[2] = true;
            Letters[13].Lines[3] = true;
            Letters[13].Lines[4] = false;
            Letters[13].Lines[5] = false;
            Letters[13].Lines[6] = true;
            Letters[13].Lines[7] = true;
            Letters[13].Lines[8] = true;
            Letters[13].Lines[9] = false;
            Letters[13].Lines[10] = false;
            Letters[13].Lines[11] = true;
            Letters[13].Lines[12] = false;
            Letters[13].Lines[13] = false;
            Letters[13].Lines[14] = false;
            Letters[13].Lines[15] = false;
            // O
            Letters[14].Lines[0] = true;
            Letters[14].Lines[1] = true;
            Letters[14].Lines[2] = true;
            Letters[14].Lines[3] = true;
            Letters[14].Lines[4] = true;
            Letters[14].Lines[5] = true;
            Letters[14].Lines[6] = true;
            Letters[14].Lines[7] = true;
            Letters[14].Lines[8] = false;
            Letters[14].Lines[9] = false;
            Letters[14].Lines[10] = false;
            Letters[14].Lines[11] = false;
            Letters[14].Lines[12] = false;
            Letters[14].Lines[13] = false;
            Letters[14].Lines[14] = false;
            Letters[14].Lines[15] = false;
            // P
            Letters[15].Lines[0] = true;
            Letters[15].Lines[1] = true;
            Letters[15].Lines[2] = true;
            Letters[15].Lines[3] = false;
            Letters[15].Lines[4] = false;
            Letters[15].Lines[5] = false;
            Letters[15].Lines[6] = true;
            Letters[15].Lines[7] = true;
            Letters[15].Lines[8] = false;
            Letters[15].Lines[9] = false;
            Letters[15].Lines[10] = true;
            Letters[15].Lines[11] = false;
            Letters[15].Lines[12] = false;
            Letters[15].Lines[13] = true;
            Letters[15].Lines[14] = false;
            Letters[15].Lines[15] = false;
            // Q
            Letters[16].Lines[0] = true;
            Letters[16].Lines[1] = true;
            Letters[16].Lines[2] = true;
            Letters[16].Lines[3] = true;
            Letters[16].Lines[4] = true;
            Letters[16].Lines[5] = true;
            Letters[16].Lines[6] = true;
            Letters[16].Lines[7] = true;
            Letters[16].Lines[8] = false;
            Letters[16].Lines[9] = false;
            Letters[16].Lines[10] = false;
            Letters[16].Lines[11] = true;
            Letters[16].Lines[12] = false;
            Letters[16].Lines[13] = false;
            Letters[16].Lines[14] = false;
            Letters[16].Lines[15] = false;
            // R
            Letters[17].Lines[0] = true;
            Letters[17].Lines[1] = true;
            Letters[17].Lines[2] = true;
            Letters[17].Lines[3] = false;
            Letters[17].Lines[4] = false;
            Letters[17].Lines[5] = false;
            Letters[17].Lines[6] = true;
            Letters[17].Lines[7] = true;
            Letters[17].Lines[8] = false;
            Letters[17].Lines[9] = false;
            Letters[17].Lines[10] = true;
            Letters[17].Lines[11] = true;
            Letters[17].Lines[12] = false;
            Letters[17].Lines[13] = true;
            Letters[17].Lines[14] = false;
            Letters[17].Lines[15] = false;
            // S
            Letters[18].Lines[0] = true;
            Letters[18].Lines[1] = true;
            Letters[18].Lines[2] = false;
            Letters[18].Lines[3] = true;
            Letters[18].Lines[4] = true;
            Letters[18].Lines[5] = true;
            Letters[18].Lines[6] = false;
            Letters[18].Lines[7] = true;
            Letters[18].Lines[8] = false;
            Letters[18].Lines[9] = false;
            Letters[18].Lines[10] = true;
            Letters[18].Lines[11] = false;
            Letters[18].Lines[12] = false;
            Letters[18].Lines[13] = true;
            Letters[18].Lines[14] = false;
            Letters[18].Lines[15] = false;
            // T
            Letters[19].Lines[0] = true;
            Letters[19].Lines[1] = true;
            Letters[19].Lines[2] = false;
            Letters[19].Lines[3] = false;
            Letters[19].Lines[4] = false;
            Letters[19].Lines[5] = false;
            Letters[19].Lines[6] = false;
            Letters[19].Lines[7] = false;
            Letters[19].Lines[8] = false;
            Letters[19].Lines[9] = false;
            Letters[19].Lines[10] = false;
            Letters[19].Lines[11] = false;
            Letters[19].Lines[12] = false;
            Letters[19].Lines[13] = false;
            Letters[19].Lines[14] = true;
            Letters[19].Lines[15] = true;
            // U
            Letters[20].Lines[0] = false;
            Letters[20].Lines[1] = false;
            Letters[20].Lines[2] = true;
            Letters[20].Lines[3] = true;
            Letters[20].Lines[4] = true;
            Letters[20].Lines[5] = true;
            Letters[20].Lines[6] = true;
            Letters[20].Lines[7] = true;
            Letters[20].Lines[8] = false;
            Letters[20].Lines[9] = false;
            Letters[20].Lines[10] = false;
            Letters[20].Lines[11] = false;
            Letters[20].Lines[12] = false;
            Letters[20].Lines[13] = false;
            Letters[20].Lines[14] = false;
            Letters[20].Lines[15] = false;
            // V
            Letters[21].Lines[0] = false;
            Letters[21].Lines[1] = false;
            Letters[21].Lines[2] = false;
            Letters[21].Lines[3] = false;
            Letters[21].Lines[4] = false;
            Letters[21].Lines[5] = false;
            Letters[21].Lines[6] = true;
            Letters[21].Lines[7] = true;
            Letters[21].Lines[8] = false;
            Letters[21].Lines[9] = true;
            Letters[21].Lines[10] = false;
            Letters[21].Lines[11] = false;
            Letters[21].Lines[12] = true;
            Letters[21].Lines[13] = false;
            Letters[21].Lines[14] = false;
            Letters[21].Lines[15] = false;
            // W
            Letters[22].Lines[0] = false;
            Letters[22].Lines[1] = false;
            Letters[22].Lines[2] = true;
            Letters[22].Lines[3] = true;
            Letters[22].Lines[4] = false;
            Letters[22].Lines[5] = false;
            Letters[22].Lines[6] = true;
            Letters[22].Lines[7] = true;
            Letters[22].Lines[8] = false;
            Letters[22].Lines[9] = false;
            Letters[22].Lines[10] = false;
            Letters[22].Lines[11] = true;
            Letters[22].Lines[12] = true;
            Letters[22].Lines[13] = false;
            Letters[22].Lines[14] = false;
            Letters[22].Lines[15] = false;
            // X
            Letters[23].Lines[0] = false;
            Letters[23].Lines[1] = false;
            Letters[23].Lines[2] = false;
            Letters[23].Lines[3] = false;
            Letters[23].Lines[4] = false;
            Letters[23].Lines[5] = false;
            Letters[23].Lines[6] = false;
            Letters[23].Lines[7] = false;
            Letters[23].Lines[8] = true;
            Letters[23].Lines[9] = true;
            Letters[23].Lines[10] = false;
            Letters[23].Lines[11] = true;
            Letters[23].Lines[12] = true;
            Letters[23].Lines[13] = false;
            Letters[23].Lines[14] = false;
            Letters[23].Lines[15] = false;
            // Y
            Letters[24].Lines[0] = false;
            Letters[24].Lines[1] = false;
            Letters[24].Lines[2] = false;
            Letters[24].Lines[3] = false;
            Letters[24].Lines[4] = false;
            Letters[24].Lines[5] = false;
            Letters[24].Lines[6] = false;
            Letters[24].Lines[7] = false;
            Letters[24].Lines[8] = true;
            Letters[24].Lines[9] = true;
            Letters[24].Lines[10] = false;
            Letters[24].Lines[11] = false;
            Letters[24].Lines[12] = false;
            Letters[24].Lines[13] = false;
            Letters[24].Lines[14] = false;
            Letters[24].Lines[15] = true;
            // Z
            Letters[25].Lines[0] = true;
            Letters[25].Lines[1] = true;
            Letters[25].Lines[2] = false;
            Letters[25].Lines[3] = false;
            Letters[25].Lines[4] = true;
            Letters[25].Lines[5] = true;
            Letters[25].Lines[6] = false;
            Letters[25].Lines[7] = false;
            Letters[25].Lines[8] = false;
            Letters[25].Lines[9] = true;
            Letters[25].Lines[10] = false;
            Letters[25].Lines[11] = false;
            Letters[25].Lines[12] = true;
            Letters[25].Lines[13] = false;
            Letters[25].Lines[14] = false;
            Letters[25].Lines[15] = false;
            // _
            Letters[26].Lines[0] = false;
            Letters[26].Lines[1] = false;
            Letters[26].Lines[2] = false;
            Letters[26].Lines[3] = false;
            Letters[26].Lines[4] = true;
            Letters[26].Lines[5] = true;
            Letters[26].Lines[6] = false;
            Letters[26].Lines[7] = false;
            Letters[26].Lines[8] = false;
            Letters[26].Lines[9] = false;
            Letters[26].Lines[10] = false;
            Letters[26].Lines[11] = false;
            Letters[26].Lines[12] = false;
            Letters[26].Lines[13] = false;
            Letters[26].Lines[14] = false;
            Letters[26].Lines[15] = false;
        }
    }
}
