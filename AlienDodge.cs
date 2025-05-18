using System;
using SplashKitSDK;

namespace PlayerClass
{
    public class AlienDodge
    {
        private Ranking _ranking;
        private bool _scoreRecorded = false;
        private int _score = 0;
        private uint _lastScoreTime = 0;
        private Bitmap _heart;
        private Player _Player;
        private Bitmap _background;
        private Window _gameWindow;
        private List<Alien> _Aliens = new List<Alien>();
        private List<Bullet> _Bullets = new List<Bullet>();
        // Level system
        private int _currentLevel = 1;
        private int _aliensToNextLevel; //
        private int _aliensDefeated = 0;
        private SoundEffect _menuSound;
        private SoundEffect _bulletSound;
        private SoundEffect _dodgeSound;
        private SoundEffect _getScoreSound;
        private Music _menuMusic;
        private Music _gameMusic;
        public bool Quit
        {
            get { return _Player.Quit; }
        }
        private enum GameState
        {
            Menu,
            Playing,
            GameOver,
            TheEnd,
            LevelComplete,
            Paused,
            Ranking
        }
        private GameState _currentState = GameState.Menu;
        private int _selectedOption = 0; // 0: New Game, 1: Ranking, 2: Exit

        private int GetCenteredX(string text, int fontSize)
        {
            int textWidth = SplashKit.TextWidth(text, "GameFont", fontSize);
            return (_gameWindow.Width - textWidth) / 2;
        }
        private void GameOver()
        {
            string playerName = "Player1";
            int score = _score;
            int level = _currentLevel;
            _ranking.AddScore(playerName, score, level);
            _scoreRecorded = true;
        }
        private void TheEnd()
        {
            string playerName = "Player1";
            int score = _score;
            int level = _currentLevel;
            _ranking.AddScore(playerName, score, level);
            _scoreRecorded = true;
        }

        private void DrawMenu()
        {
            _gameWindow.Clear(Color.White);
            _gameWindow.DrawBitmap(_background, 0, 0);

            // title
            string title = "Cosmic Adventure";
            SplashKit.DrawText(title, Color.White, "GameFont", 80, GetCenteredX(title, 80), 80);
            // options
            string[] options = { "New Game", "Ranking", "Exit" };
            for (int i = 0; i < options.Length; i++)
            {
                Color textColor = (i == _selectedOption) ? Color.Yellow : Color.White;
                SplashKit.DrawText(options[i], textColor, "GameFont", 60, GetCenteredX(options[i], 60), 250 + i * 80);
            }
        }

        // Constructor
        public AlienDodge(Window gameWindow)
        {
            _ranking = new Ranking();

            SplashKit.CreateTimer("ScoreTimer");
            SplashKit.StartTimer("ScoreTimer");
            SplashKit.LoadFont("GameFont", "Resources/fonts/PixelifySans-VariableFont_wght.ttf");
            _heart = SplashKit.LoadBitmap("Heart", "Heart.png");
            _background = SplashKit.LoadBitmap("Background", "background.png");
            _gameWindow = gameWindow;
            _Player = new Player(gameWindow);

            _menuMusic = SplashKit.LoadMusic("MenuMusic", "Resources/sounds/background_music.mp3");
            _menuSound = SplashKit.LoadSoundEffect("MenuSound", "Resources/sounds/button02a.mp3");
            _gameMusic = SplashKit.LoadMusic("GameMusic", "Resources/sounds/game.mp3");
            _bulletSound = SplashKit.LoadSoundEffect("BulletSound", "Resources/sounds/laser.mp3");
            _dodgeSound = SplashKit.LoadSoundEffect("DodgeSound", "Resources/sounds/dodge.mp3");
            _getScoreSound = SplashKit.LoadSoundEffect("GetScoreSound", "Resources/sounds/score.mp3");
            SplashKit.PlayMusic(_menuMusic, -1);
        }
        private void CheckLevelProgress()
        {
            int targetAliens = GetTargetAliensForLevel(_currentLevel);

            if (_aliensDefeated >= _aliensToNextLevel)
            {
                if (_currentLevel == 3)
                {
                    if (!_scoreRecorded)
                    {
                        TheEnd();
                    }
                    _currentState = GameState.TheEnd;
                }
                else
                {
                    _currentState = GameState.LevelComplete;
                    ShowLevelComplete();
                }

                if (SplashKit.KeyTyped(KeyCode.ReturnKey))
                {
                    _currentLevel++;
                    _aliensDefeated = 0;
                    _aliensToNextLevel = GetTargetAliensForLevel(_currentLevel);

                    _Aliens.Clear();
                    _Bullets.Clear();
                    // bonus points for clearing aliens
                    _score += 100 * _currentLevel;
                    _currentState = GameState.Playing;
                }
            }
        }
        private int GetTargetAliensForLevel(int level)
        {
            switch (level)
            {
                case 1:
                    return 10;
                case 2:
                    return 15;
                case 3:
                    return 20;
                default:
                    return 10 + (level * 5);
            }
        }
        private void ShowLevelComplete()
        {
            SplashKit.DrawText("Level Complete!", Color.White, "GameFont", 60, GetCenteredX("Level Complete!", 60), 250);
            SplashKit.DrawText("Press Enter to continue", Color.White, "GameFont", 20, GetCenteredX("Press Enter to continue", 20), 330);
            SplashKit.DrawText("Press [ R ] to restart", Color.White, "GameFont", 20, GetCenteredX("Press [ R ] to restart", 20), 360);
            SplashKit.DrawText("Press ESC go to MENU", Color.White, "GameFont", 20, GetCenteredX("Press ESC go to MENU", 20), 390);
        }
        public Window GameWindow // Accepts a Window and stores this in the GameWindow field
        {
            get { return _gameWindow; }
        }
        public Player Player // Creates the Player object and stores it in the Player field.
        {
            get { return _Player; }
        }
        public void HandleInput()
        {
            if (_selectedOption == 1)
            {
                if (SplashKit.KeyTyped(KeyCode.EscapeKey))
                {
                    _selectedOption = 0;
                }
            }
            if (_currentState == GameState.Menu)
            {
                // up and down key to select the option
                if (SplashKit.KeyTyped(KeyCode.UpKey))
                {
                    _selectedOption = (_selectedOption - 1 + 3) % 3;
                    _menuSound.Play();
                }
                if (SplashKit.KeyTyped(KeyCode.DownKey))
                {
                    _selectedOption = (_selectedOption + 1) % 3;
                    _menuSound.Play();
                }

                // Enter
                if (SplashKit.KeyTyped(KeyCode.ReturnKey))
                {
                    switch (_selectedOption)
                    {
                        case 0: // New Game
                            _currentState = GameState.Playing;
                            SplashKit.StopMusic();
                            SplashKit.PlayMusic(_gameMusic, -1);
                            ResetGame();
                            break;
                        case 1: // Ranking
                            _currentState = GameState.Ranking;
                            break;
                        case 2: // Exit
                            _gameWindow.Close();
                            break;
                    }
                }
            }

            else if (_currentState == GameState.Ranking)
            {
                if (SplashKit.KeyTyped(KeyCode.EscapeKey))
                {
                    _currentState = GameState.Menu;
                }
            }
            else if (_currentState == GameState.Playing)
            {
                if (SplashKit.KeyTyped(KeyCode.SpaceKey))
                {
                    // start a bullet at the player's position
                    double startX = _Player.X + _Player.Width / 2;
                    double startY = _Player.Y + _Player.Height / 2;

                    // mouse position
                    double mouseX = SplashKit.MouseX();
                    double mouseY = SplashKit.MouseY();

                    double angle = -Math.PI / 2;
                    _Bullets.Add(new Bullet(startX, startY, 10, angle));
                    _bulletSound.Play();
                }
                // Close the game window if Escape is pressed
                if (SplashKit.KeyTyped(KeyCode.EscapeKey))
                {
                    _currentState = GameState.Paused;
                    SplashKit.PauseMusic();
                    SplashKit.PauseTimer("ScoreTimer");
                }

                if (_Player.Lives <= 0)
                {
                    _currentState = GameState.GameOver;
                    return;
                }
                _Player.HandleInput();
                _Player.StayOnWindow(); // Ensure the player stays within the window bounds

                if (_aliensDefeated >= _aliensToNextLevel)
                {
                    _currentState = GameState.TheEnd;
                }
            }
            else if (_currentState == GameState.LevelComplete)
            {
                if (SplashKit.KeyTyped(KeyCode.ReturnKey))
                {
                    _currentState = GameState.Playing;
                }
                else if (SplashKit.KeyTyped(KeyCode.RKey))
                {
                    _currentState = GameState.Playing;
                    ResetGame();
                }
                else if (SplashKit.KeyTyped(KeyCode.EscapeKey))
                {
                    _currentState = GameState.Menu;
                }
            }
            else if (_currentState == GameState.TheEnd)
            {
                if (SplashKit.KeyTyped(KeyCode.EscapeKey))
                {
                    _currentState = GameState.Menu;
                }
            }
            else if (_currentState == GameState.GameOver)
            {
                if (!_scoreRecorded)
                {
                    GameOver();
                    TheEnd();
                }
                if (SplashKit.KeyTyped(KeyCode.RKey))
                {
                    _currentState = GameState.Playing;
                    ResetGame();
                }
                else if (SplashKit.KeyTyped(KeyCode.EscapeKey))
                {
                    _currentState = GameState.Menu;
                }
            }
            else if (_currentState == GameState.Paused)
            {
                if (SplashKit.KeyTyped(KeyCode.EscapeKey))
                {
                    _currentState = GameState.Playing;
                    SplashKit.ResumeMusic();
                    SplashKit.ResumeTimer("ScoreTimer");
                }
                if (SplashKit.KeyTyped(KeyCode.QKey))
                {
                    _currentState = GameState.Menu;
                    SplashKit.StopMusic();
                    SplashKit.PlayMusic(_menuMusic, -1);
                }
            }
        }
        public void ShowRanking()
        {
            if (_currentState == GameState.GameOver)
            {
                _gameWindow.Clear(Color.White);
                _gameWindow.DrawBitmap(_background, 0, 0);
            }
            else if (_currentState == GameState.TheEnd)
            {
                _gameWindow.Clear(Color.White);
                _gameWindow.DrawBitmap(_background, 0, 0);
                _ranking.Draw();
            }
        }

        public void Update()
        {
            if (_currentState != GameState.Playing)
            {
                return;
            }

            // Update Aliens every second
            if (SplashKit.TimerTicks("ScoreTimer") - _lastScoreTime >= 1000)
            {
                double elapsedSeconds = SplashKit.TimerTicks("ScoreTimer") / 1000.0;
                double spawnRate = 0.5 + elapsedSeconds * 0.01;
                if (spawnRate > 1) spawnRate = 1;

                if (SplashKit.Rnd() < spawnRate)
                    if (_currentLevel == 1)
                    {
                        _Aliens.Add(new MonsterAlien(_gameWindow, _Player, elapsedSeconds));
                    }
                    else if (_currentLevel == 2)
                    {
                        _Aliens.Add(new FreakAlien(_gameWindow, _Player, elapsedSeconds));
                    }
                    else if (_currentLevel == 3)
                    {
                        _Aliens.Add(new OAlien(_gameWindow, _Player, elapsedSeconds));
                    }
                _lastScoreTime = SplashKit.TimerTicks("ScoreTimer");
            }

            foreach (Alien alien in _Aliens)
            {
                alien.Update();
            }
            // Check for collisions and remove Aliens
            List<Alien> _aliensToRemove = new List<Alien>();

            foreach (Alien alien in _Aliens)
            {
                if (_Player.CollidedWith(alien))
                {
                    _Player.LoseLife();
                    _aliensToRemove.Add(alien);
                    if (_Player.Lives <= 0)
                    {
                        _currentState = GameState.GameOver;
                        GameOver();
                    }
                    _dodgeSound.Play();

                }
                else if (alien.IsOffscreen(_gameWindow))
                {
                    _aliensToRemove.Add(alien);
                }
            }
            // Remove the aliens that are offscreen or collided with the player
            List<Bullet> _bulletsToRemove = new List<Bullet>();

            foreach (Alien alien in _Aliens)
            {
                foreach (Bullet bullet in _Bullets)
                {
                    if (bullet.CollidedWith(alien))
                    {
                        alien.TakeDamage();
                        if (alien.IsDead())
                        {
                            _score += 10;
                            _aliensDefeated++;
                            _aliensToRemove.Add(alien);
                            _getScoreSound.Play();
                        }
                        _bulletsToRemove.Add(bullet);
                    }
                }
            }
            // Remove the bullets that collided with aliens
            foreach (Alien alien in _aliensToRemove)
            {
                _Aliens.Remove(alien);
            }

            foreach (Bullet bullet in _Bullets)
            {
                bullet.Move();
                if (bullet.IsOffscreen(_gameWindow))
                {
                    _bulletsToRemove.Add(bullet);
                }
            }
            foreach (Bullet bullet in _bulletsToRemove)
            {
                _Bullets.Remove(bullet);
            }
            CheckLevelProgress();
        }

        public void Draw()
        {
            if (_currentState == GameState.Menu)
            {
                DrawMenu();
            }
            else if (_currentState == GameState.Ranking)
            {
                _gameWindow.Clear(Color.White);
                _gameWindow.DrawBitmap(_background, 0, 0);
                _ranking.Draw();
            }
            else if (_currentState == GameState.Playing)
            {
                _gameWindow.Clear(Color.White);
                _gameWindow.DrawBitmap(_background, 0, 0);

                foreach (Alien alien in _Aliens)
                {
                    alien.Draw();
                }
                for (int i = 0; i < _Player.Lives; i++) // Draw the live hearts
                {
                    double x = 30 + i * 45; //left margin 30px, each heart 45px apart
                    double y = 20; // top margin 20px
                    SplashKit.DrawBitmap(_heart, x, y); // Draw the heart bitmap at the specified position
                }
                foreach (Bullet bullet in _Bullets)
                {
                    bullet.Draw();
                }
                SplashKit.DrawText("Score: " + _score, Color.White, "GameFont", 16, 30, 60);
                SplashKit.DrawText("Level: " + _currentLevel, Color.White, "GameFont", 16, 30, 80);
                SplashKit.DrawText("Aliens Defeated: " + _aliensDefeated + "/" + _aliensToNextLevel, Color.White, "GameFont", 16, 30, 100);

                if (_aliensDefeated >= _aliensToNextLevel)
                {
                    ShowLevelComplete();
                }
                _Player.Draw(); // Only draw player during Playing state
            }
            else if (_currentState == GameState.TheEnd)
            {
                _gameWindow.Clear(Color.White);
                _gameWindow.DrawBitmap(_background, 0, 0);

                SplashKit.DrawText("YOU WIN!", Color.Yellow, "GameFont", 80, GetCenteredX("YOU WIN!", 80), 250);
                SplashKit.DrawText("Final Score: " + _score, Color.White, "GameFont", 30, GetCenteredX("Final   Score: " + _score, 30), 370);
                SplashKit.DrawText("Level Reached: " + _currentLevel, Color.White, "GameFont", 20, GetCenteredX("Level Reached: " + _currentLevel, 20), 400);
                SplashKit.DrawText("Press ESC to return to menu", Color.White, "GameFont", 16, GetCenteredX("Press ESC to return to menu", 16), 460);
            }
            else if (_currentState == GameState.GameOver)
            {
                _gameWindow.Clear(Color.White);
                _gameWindow.DrawBitmap(_background, 0, 0);

                SplashKit.DrawText("GAME OVER", Color.Red, "GameFont", 80, GetCenteredX("GAME OVER", 80), 250);
                SplashKit.DrawText("Final Score: " + _score, Color.White, "GameFont", 30, GetCenteredX("Final   Score: " + _score, 30), 370);
                SplashKit.DrawText("Level Reached: " + _currentLevel, Color.White, "GameFont", 20, GetCenteredX("Level Reached: " + _currentLevel, 20), 400);
                SplashKit.DrawText("Press [ R ] to restart", Color.White, "GameFont", 16, GetCenteredX("Press R to restart", 16), 430);
                SplashKit.DrawText("Press ESC to return to menu", Color.White, "GameFont", 16, GetCenteredX("Press ESC to return to menu", 16), 460);
            }
            else if (_currentState == GameState.Paused)
            {
                _gameWindow.Clear(Color.White);
                _gameWindow.DrawBitmap(_background, 0, 0);
                SplashKit.DrawText("Paused. Press ESC to continue the game.", Color.White, "GameFont", 30, GetCenteredX("Paused. Press ESC to continue the game.", 30), 250);
                SplashKit.DrawText("Press Q to go back to menu.", Color.White, "GameFont", 20, GetCenteredX("Press Q to go back to menu.", 20), 300);
            }
            _gameWindow.Refresh(60);
        }
        public void ResetGame()
        {
            _score = 0;
            _currentLevel = 1;
            _aliensDefeated = 0;
            _aliensToNextLevel = GetTargetAliensForLevel(_currentLevel);
            _Player = new Player(_gameWindow);
            _Aliens.Clear();
            _Bullets.Clear();
            _scoreRecorded = false;
            SplashKit.StartTimer("ScoreTimer");
        }
    }
}