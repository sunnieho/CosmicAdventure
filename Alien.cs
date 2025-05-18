using System;
using System.Security.Cryptography.X509Certificates;
using SplashKitSDK;

namespace PlayerClass
{
    public class Alien
    {
        protected int _health;
        //base attributes and methods
        protected Bitmap _alienBitmap;
        public double X {get; protected set;}
        public double Y {get; protected set;}
        public Vector2D Velocity {get; protected set;}
        public Color MainColor{get; protected set;}

        // methods
        public int Width =>64;
        public int Height =>64;
        public Bitmap Bitmap => _alienBitmap;
        public Alien(Window gameWindow, Player player, double elapsedSeconds)
        {
            _health = 1;
            _alienBitmap = SplashKit.LoadBitmap("Default", "Alien.png");
            MainColor = Color.White;
        }
        public void TakeDamage()
        {
            _health--;
        }
        public bool IsDead()
        {
            return _health <= 0;
        }
        public Circle CollisionCircle
        {
            get
            {
                double centerX = X + Width / 2;
                double centerY = Y + Height / 2;
                return SplashKit.CircleAt(centerX, centerY, 20);
            }
        }
        public virtual void Update()
        {
            X += Velocity.X;
            Y += Velocity.Y;
        }
        public bool IsOffscreen(Window screen)
        {
            return X < -Width || X > screen.Width || Y < -Height || Y > screen.Height;
        }
        public virtual void Draw()
        {
            _alienBitmap.Draw(X, Y);
        }
        public virtual void SetSpeed(double speed){}   
    }
}