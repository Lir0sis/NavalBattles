using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavalBattles
{
    class User
    {
        public bool IsFirst = false;
        public Mesh mesh;

        public User(bool IsFirst)
        {
            this.IsFirst = IsFirst;
            mesh = new Mesh();
        }
    }
}