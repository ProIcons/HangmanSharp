using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devian.gr.Hangman.Exceptions
{
    [Serializable]
    public class HangmanException : Exception
    {
        public HangmanException(String message) : base(message)
        {
            
        }
    }
}
