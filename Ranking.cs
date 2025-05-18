using System;
using SplashKitSDK;

namespace PlayerClass
{
    public class ScoreRecord
    {
        public string? PlayerName { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }
        public DateTime Date { get; set; }
    }
    public class Ranking
    {
        private List<ScoreRecord> _scores;
        public Ranking()
        {
            _scores = new List<ScoreRecord>();
        }
        public void AddScore(string playerName, int score, int level)
        {
            var record = new ScoreRecord()
            {
                PlayerName = playerName,
                Score = score,
                Level = level,
                Date = DateTime.Now
            };
            _scores.Add(record);
        }
        public void Draw()
        {
            var sortedScores = _scores.OrderByDescending(score => score.Score).ToList();
            double x = 100;
            double y = 100;
            double lineHeight = 30;

            SplashKit.DrawText("Ranking", Color.White, "GameFont", 36, x, y);
            y += lineHeight *2;

            string header = string.Format("PlayerName      Score          Level            Date");
            SplashKit.DrawText(header, Color.White, "GameFont", 24, x, y);
            y += lineHeight *1.5;
            

            foreach (var score in sortedScores)
            {
                string scoreText = $"{score.PlayerName, -10} {score.Score, 15} {score.Level, 20} {score.Date.ToString("yyyy-MM-dd"),25}";
                SplashKit.DrawText(scoreText, Color.White, "GameFont", 24, x, y);
                y += lineHeight;
            }

        }
    }

}




