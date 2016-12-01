using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devian.gr.Hangman.Exceptions
{
    [Serializable]
    public class HangmanGameUnableToStartException : Exception
    {

        public HangmanGameUnableToStartException(string message) : base(message)
        {
        }

    }
}
