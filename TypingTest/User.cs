using System;
using System.Collections.Generic;
using System.Text;

namespace TypingTest
{
    internal class User
    {
        public string Name;
        public double LettersPerSecond;
        public int LettersPerMinute;
        public int Mistakes;

        public User(string name, int lettersPerMinute, double lettersPerSecond,  int mistakes)
        {
            Name = name;
            LettersPerSecond = lettersPerSecond;
            LettersPerMinute = lettersPerMinute;
            Mistakes = mistakes;
        }
    }
}
