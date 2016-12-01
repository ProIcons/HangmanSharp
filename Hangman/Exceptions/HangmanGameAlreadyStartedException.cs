using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devian.gr.Hangman.Exceptions
{
    [Serializable]
    public class HangmanGameAlreadyStartedException : Exception
    {
        public HangmanGameAlreadyStartedException() : base("There is a game in progress.")
        {
        }
    }
}
