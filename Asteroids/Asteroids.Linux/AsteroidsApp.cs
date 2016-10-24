using SiliconStudio.Xenko.Engine;

namespace Asteroids
{
    class AsteroidsApp
    {
        static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
