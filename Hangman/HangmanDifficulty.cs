using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace devian.gr.Hangman
{
    public class HangmanDifficulty
    {
        public static List<HangmanDifficulty> List = new List<HangmanDifficulty>();
        public String Name { get; private set; }

        public int ToleretableErrors { get; private set; }

        public int MinimumLetters { get; private set; }

        public bool IsTimeLimited { get; private set; }

        public double TimeLimit { get; private set; }

        static HangmanDifficulty()
        {
            var difficultyDefaults = typeof(HangmanDifficulty).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var property in difficultyDefaults)
            {
                var obj = (HangmanDifficulty)property.GetValue(null);
                List.Add(obj);
            }
        }

        public HangmanDifficulty(String name, int toleretableErrors, int minimumLetters, bool timeLimit = false, int timeLimitSec = 0)
        {
            Name = name;
            ToleretableErrors = toleretableErrors.LimitToRange(0, 6);
            MinimumLetters = minimumLetters.LimitToRange(4, 20);
            IsTimeLimited = timeLimit;
            TimeLimit = timeLimitSec.LimitToRange(0, 600);
        }

        public static HangmanDifficulty Easy => new HangmanDifficulty("Easy", 6, 4);
        public static HangmanDifficulty Medium => new HangmanDifficulty("Medium", 5, 5);
        public static HangmanDifficulty Hard => new HangmanDifficulty("Hard", 3, 6, true, 300);
        public static HangmanDifficulty Extreme => new HangmanDifficulty("Extreme", 2, 8, true, 120);

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name: " + Name);
            sb.Append("\nToleretable Errors: " + ToleretableErrors);
            sb.Append("\nMinimum Letters: " + MinimumLetters);
            sb.Append("\nIs Time Limited: " + IsTimeLimited);
            sb.Append("\nTime Limit: " + TimeLimit + "secs");
            return sb.ToString();
        }

    }
}
