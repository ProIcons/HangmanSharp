using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devian.gr.Hangman.Exceptions
{
    [Serializable]
    public class HangmanGameNotStartedException : Exception
    {
        public HangmanGameNotStartedException() : base("There is not an active game in progress.")
        {
        }
    }
}
