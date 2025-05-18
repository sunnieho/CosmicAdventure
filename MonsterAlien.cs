using System;
using System.Security.Cryptography.X509Certificates;
using SplashKitSDK;

namespace PlayerClass
{
   public class MonsterAlien : Alien
{
    public MonsterAlien(Window gameWindow, Player player, double elapsedSeconds) : base(gameWindow, player, elapsedSeconds)
    {
        _health = 2;
        _alienBitmap = SplashKit.LoadBitmap("MonsterAlien", "MonsterAlien.png");
        X = SplashKit.Rnd(gameWindow.Width - Width);
        Y = -Height;

        // level 1 alien
        Point2D fromPT = new Point2D() { X = X, Y = Y };
        Point2D toPT = new Point2D() { X = player.X, Y = player.Y };
        Vector2D dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPT, toPT));
        
        double speed = 2 + (elapsedSeconds * 0.5);
        if (speed > 5) speed = 5;
        Velocity = SplashKit.VectorMultiply(dir, speed);
    }
}
}