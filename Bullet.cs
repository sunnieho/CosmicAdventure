using System;
using SplashKitSDK;

namespace PlayerClass
{
    public class Bullet
    {
        private Bitmap _bulletBitmap;
        private double _x, _y;
        private double _speed;
        private double _angle;

        public bool IsOffscreen(Window window)
        {
            return _x < 0 || _x > window.Width || _y < 0 || _y > window.Height;
        }

        public bool CollidedWith(Alien alien)
        {
            Circle bulletCircle = SplashKit.CircleAt(_x + 5, _y + 5, 5); // Adjusted for bullet size
            return SplashKit.CirclesIntersect(bulletCircle, alien.CollisionCircle);
        }

        public Bullet(double x, double y, double speed, double angle)
        {
            _bulletBitmap = SplashKit.LoadBitmap("Bullet", "Bullet.png");
            _x = x;
            _y = y;
            _speed = speed;
            _angle = angle;
        }

        public void Move()
        {
            _x += Math.Cos(_angle) * _speed;
            _y += Math.Sin(_angle) * _speed;
        }

        public void Draw()
        {
            SplashKit.DrawBitmap(_bulletBitmap, _x, _y);
        }
    }
}