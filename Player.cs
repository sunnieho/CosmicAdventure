using System;
using SplashKitSDK;

namespace PlayerClass
{
    public class Player
    {
        private int _lives = 5;
        private Bitmap? _playerBitmap;
        public double x = 0, y = 0;
        public int width = 0, height = 0;
        public bool quit = false;

        // lives property
        public int Lives
        {
            get { return _lives; }
        }
        public void LoseLife()
        {
            _lives--;
        }

        public double X
        {
            get; private set;
        }
        public double Y
        {
            get; private set;
        }
        public bool Quit
        {
            get; private set;
        }
        public int Width
        {
            get
            {
                return _playerBitmap?.Width ?? 0;
            }
        }
        public int Height
        {
            get
            {
                return _playerBitmap?.Height ?? 0;
            }
        }
        public Player(Window gameWindow)
        {
            _playerBitmap = SplashKit.LoadBitmap("Player", "spaceship.png");
            X = (gameWindow.Width - Width) / 2;
            Y = (gameWindow.Height - Height) / 2;
            // for the player to be in the center of the window
        }

        //HandleInput method
        //3.7 Putting the user in control: video
        public void HandleInput()
        {
            int SPEED = 6; // Define the speed constant

            if (SplashKit.KeyDown(KeyCode.UpKey) || SplashKit.KeyDown(KeyCode.WKey)) Y -= SPEED;
            if (SplashKit.KeyDown(KeyCode.DownKey) || SplashKit.KeyDown(KeyCode.SKey)) Y += SPEED;
            if (SplashKit.KeyDown(KeyCode.LeftKey) || SplashKit.KeyDown(KeyCode.AKey)) X -= SPEED;
            if (SplashKit.KeyDown(KeyCode.RightKey) || SplashKit.KeyDown(KeyCode.DKey)) X += SPEED;

            if (SplashKit.KeyDown(KeyCode.EscapeKey)) quit = true;

        }
        public void StayOnWindow()
        {
            const int GAP = 10; // Define the gap constant
            if (X < GAP) X = GAP;
            if (X > SplashKit.ScreenWidth() - Width - GAP) X = SplashKit.ScreenWidth() - Width - GAP;
            if (Y < GAP) Y = GAP;
            if (Y > SplashKit.ScreenHeight() - Height - GAP) Y = SplashKit.ScreenHeight() - Height - GAP;
        }
        public bool CollidedWith(Alien alien)
        {
            if (_playerBitmap == null) return false; // No collision if the bitmap is null
            return _playerBitmap.CircleCollision(X, Y, alien.CollisionCircle);
        }
        public void Draw()
        {
            if (_playerBitmap != null)
            {
                _playerBitmap.Draw(X, Y);
            }
        }
    }
}