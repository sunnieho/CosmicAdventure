using System;
using System.Security.Cryptography.X509Certificates;
using SplashKitSDK;

namespace PlayerClass
{
  public class FreakAlien : Alien
{
    public FreakAlien(Window gameWindow, Player player, double elapsedSeconds) : base(gameWindow, player, elapsedSeconds)
    {
        _health = 2;
        _alienBitmap = SplashKit.LoadBitmap("FreakAlien", "FreakAlien.png");
        X = SplashKit.Rnd(gameWindow.Width - Width);
        Y = -Height;

        // Level 2 Alien
        Point2D fromPT = new Point2D() { X = X, Y = Y };
        Point2D toPT = new Point2D() { X = player.X, Y = player.Y };
        Vector2D dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPT, toPT));
        
        // different speed
        double speed = 2 + (elapsedSeconds * 0.6);
        if (speed > 10) speed = 10;
        Velocity = SplashKit.VectorMultiply(dir, speed);
    }
}
}