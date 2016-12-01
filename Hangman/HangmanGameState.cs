using System;
using System.Collections.Generic;

namespace devian.gr.Hangman
{
    public class HangmanGameState
    {
        public HangmanDifficulty Difficulty { get; internal set; }
        public int CorrectAttempts { get; internal set; }
        public int FailedAttempts { get; internal set; }
        public List<String> CorrectLetters { get; internal set; }
        public List<String> IncorrectLetters { get; internal set; }
        public int TotalLetters { get; internal set; }
        public int FoundLetters { get; internal set; }
        public HangmanState State { get; internal set; }
        public TimeSpan TimeElapsed { get; internal set; }
        public String DisplayWord { get; internal set; }
    }
}
