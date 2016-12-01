using System;

namespace devian.gr.Hangman
{
    public class HangmanGameReport
    {
        public HangmanResult Result { get; internal set; }

        public String Word { get; internal set; }

        public HangmanGameState State { get; internal set; }
    }
}
