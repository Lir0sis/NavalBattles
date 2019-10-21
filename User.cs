using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavalBattles
{
    class User : Game
    {
        public bool IsFirst = false;
        public Interface Interface;
        public User(bool IsFirst)
        {
            this.IsFirst = IsFirst;
            Interface = new Interface();
        }
    }
}