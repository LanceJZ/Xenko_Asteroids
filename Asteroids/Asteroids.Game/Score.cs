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
    struct NumberData
    {
        public bool[] Lines;
    };

    public class Score : Actor
    {
        NumberData[] Numbers = new NumberData[10];
        Vector3[] m_NumberLineStart = new Vector3[7];
        Vector3[] m_NumberLineEnd = new Vector3[7];
        public int m_TotalScore = 0;
        int m_PointsToNextFreeLife = 0;
        int m_PointsForFreeLife = 5000;
        List<Entity> m_Numbers;
        public Entity m_Player;

        public override void Start()
        {
            m_PointsToNextFreeLife = m_PointsForFreeLife;

            for (int i = 0; i < 10; i++)
            {
                Numbers[i].Lines = new bool[7];
            }

            InitializeNumberLines();
            m_Numbers = new List<Entity>();
            NewGame();
        }

        public override void Update()
        {
            // Do stuff every new frame
        }

        public void NewGame()
        {
            m_TotalScore = 0;
            m_PointsToNextFreeLife = m_PointsForFreeLife;
            PlayerScore(0);
        }

        public void PlayerScore(int points)
        {
            m_TotalScore += points;

            if (m_TotalScore > m_PointsToNextFreeLife)
            {
                m_Player.Components.Get<Player>().BunusLife();
                m_PointsToNextFreeLife += m_PointsForFreeLife;
            }

            ProcessNumber(m_TotalScore, new Vector3(m_Edge.X * 0.5f, m_Edge.Y - 1, 0), 1);
        }

        public void ProcessNumber(int number, Vector3 locationStart, float size)
        {
            foreach (Entity num in m_Numbers)
            {
                this.Entity.RemoveChild(num);
            }

            int amountOfDigits = 0;
            int numberIn = number;
            float space = 0;

            do
            {
                //Make digit the modulus of 10 from number.
                int digit = numberIn % 10;
                //This sends a digit to the draw function with the location and size.
                MakeNumberMesh(space, digit, size, amountOfDigits);
                amountOfDigits++;
                // Dividing the int by 10, we discard the digit that was derived from the modulus operation.
                numberIn /= 10;
                // Move the location for the next digit location to the left. We start on the right hand side with the lowest digit.
                space += size * 2;
            } while (numberIn > 0);
                                    
            this.Entity.Transform.Position = locationStart;
        }

        void MakeNumberMesh(float location, int number, float size, int digit)
        {
            if (number > -1 && number < 10)
            {
                for (int line = 0; line < 7; line++)
                {
                    if (Numbers[number].Lines[line])
                    {
                        float Xstart = m_NumberLineStart[line].X * size + location;
                        float Ystart = m_NumberLineStart[line].Y * size;

                        float Xend = m_NumberLineEnd[line].X * size + location;
                        float Yend = m_NumberLineEnd[line].Y * size;

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
                        ModelComponent m_NumberMesh = new ModelComponent(model);
                        m_Numbers.Add(new Entity());
                        m_Numbers[m_Numbers.Count - 1].Add(m_NumberMesh);
                        this.Entity.AddChild(m_Numbers[m_Numbers.Count - 1]);
                    }
                }
            }
        }

        void InitializeNumberLines()
        {
            // LED Grid location of line start and end points. 0, 0 is the top left corner.
            //The left of screen, positive X is the direction for rotation zero.
            //The top of the screen, positive Y is the direction for rotation zero.
            m_NumberLineStart[0] = new Vector3(0, 0, 0);
            m_NumberLineStart[1] = new Vector3(-1, 0, 0);
            m_NumberLineStart[2] = new Vector3(-1, -1, 0);
            m_NumberLineStart[3] = new Vector3(0, -2, 0);
            m_NumberLineStart[4] = new Vector3(0, -1, 0);
            m_NumberLineStart[5] = new Vector3(0, 0, 0);
            m_NumberLineStart[6] = new Vector3(0, -1, 0);

            m_NumberLineEnd[0] = new Vector3(-1, 0, 0);
            m_NumberLineEnd[1] = new Vector3(-1, -1, 0);
            m_NumberLineEnd[2] = new Vector3(-1, -2, 0);
            m_NumberLineEnd[3] = new Vector3(-1, -2, 0);
            m_NumberLineEnd[4] = new Vector3(0, -2, 0);
            m_NumberLineEnd[5] = new Vector3(0, -1, 0);
            m_NumberLineEnd[6] = new Vector3(-1, -1, 0);

            // LED Grid, what lines are on for each number.
            // Line 0 is the top line.
            // Line 1 is upper right side line.
            // Line 2 is lower right side line.
            // Line 3 is bottom line.
            // Line 4 is lower left side line.
            // Line 5 is upper left side line.
            // Line 6 is the middle line.

            Numbers[0].Lines[0] = true;
            Numbers[0].Lines[1] = true;
            Numbers[0].Lines[2] = true;
            Numbers[0].Lines[3] = true;
            Numbers[0].Lines[4] = true;
            Numbers[0].Lines[5] = true;
            Numbers[0].Lines[6] = false;

            Numbers[1].Lines[0] = false;
            Numbers[1].Lines[1] = true;
            Numbers[1].Lines[2] = true;
            Numbers[1].Lines[3] = false;
            Numbers[1].Lines[4] = false;
            Numbers[1].Lines[5] = false;
            Numbers[1].Lines[6] = false;

            Numbers[2].Lines[0] = true;
            Numbers[2].Lines[1] = true;
            Numbers[2].Lines[2] = false;
            Numbers[2].Lines[3] = true;
            Numbers[2].Lines[4] = true;
            Numbers[2].Lines[5] = false;
            Numbers[2].Lines[6] = true;

            Numbers[3].Lines[0] = true;
            Numbers[3].Lines[1] = true;
            Numbers[3].Lines[2] = true;
            Numbers[3].Lines[3] = true;
            Numbers[3].Lines[4] = false;
            Numbers[3].Lines[5] = false;
            Numbers[3].Lines[6] = true;

            Numbers[4].Lines[0] = false;
            Numbers[4].Lines[1] = true;
            Numbers[4].Lines[2] = true;
            Numbers[4].Lines[3] = false;
            Numbers[4].Lines[4] = false;
            Numbers[4].Lines[5] = true;
            Numbers[4].Lines[6] = true;

            Numbers[5].Lines[0] = true;
            Numbers[5].Lines[1] = false;
            Numbers[5].Lines[2] = true;
            Numbers[5].Lines[3] = true;
            Numbers[5].Lines[4] = false;
            Numbers[5].Lines[5] = true;
            Numbers[5].Lines[6] = true;

            Numbers[6].Lines[0] = true;
            Numbers[6].Lines[1] = false;
            Numbers[6].Lines[2] = true;
            Numbers[6].Lines[3] = true;
            Numbers[6].Lines[4] = true;
            Numbers[6].Lines[5] = true;
            Numbers[6].Lines[6] = true;

            Numbers[7].Lines[0] = true;
            Numbers[7].Lines[1] = true;
            Numbers[7].Lines[2] = true;
            Numbers[7].Lines[3] = false;
            Numbers[7].Lines[4] = false;
            Numbers[7].Lines[5] = false;
            Numbers[7].Lines[6] = false;

            Numbers[8].Lines[0] = true;
            Numbers[8].Lines[1] = true;
            Numbers[8].Lines[2] = true;
            Numbers[8].Lines[3] = true;
            Numbers[8].Lines[4] = true;
            Numbers[8].Lines[5] = true;
            Numbers[8].Lines[6] = true;

            Numbers[9].Lines[0] = true;
            Numbers[9].Lines[1] = true;
            Numbers[9].Lines[2] = true;
            Numbers[9].Lines[3] = false;
            Numbers[9].Lines[4] = false;
            Numbers[9].Lines[5] = true;
            Numbers[9].Lines[6] = true;
        }
    }
}
