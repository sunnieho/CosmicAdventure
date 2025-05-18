using System;
using SplashKitSDK;

namespace PlayerClass
{
    public class Program
    {
        public static void Main()
        {
            Window gameWindow = new Window("Player", 800, 600);
            Player player = new Player(gameWindow);
            AlienDodge game = new AlienDodge(gameWindow);

            while (!gameWindow.CloseRequested && !player.Quit)
            {
                SplashKit.ProcessEvents();
                //player.HandleInput();
                player.StayOnWindow();
                //player.Draw();
                game.HandleInput();
                game.Update();
                game.Draw();
                gameWindow.Refresh(60);
            }
        }
    }
}