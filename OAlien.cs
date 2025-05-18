using System;
using System.Security.Cryptography.X509Certificates;
using SplashKitSDK;

namespace PlayerClass
{
  public class OAlien : Alien
{
    public OAlien(Window gameWindow, Player player, double elapsedSeconds) : base(gameWindow, player, elapsedSeconds)
    {
        _health = 2;
        _alienBitmap = SplashKit.LoadBitmap("OAlien", "OAlien.png");
        X = SplashKit.Rnd(gameWindow.Width - Width);
        Y = -Height;

        // Level 3 Alien
        Point2D fromPT = new Point2D() { X = X, Y = Y };
        Point2D toPT = new Point2D() { X = player.X, Y = player.Y };
        Vector2D dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPT, toPT));
        
        // different speed
        double speed = 2 + (elapsedSeconds * 1);
        if (speed > 15) speed = 15;
        Velocity = SplashKit.VectorMultiply(dir, speed);
    }
}
}