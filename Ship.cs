using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavalBattles
{
    class Ship
    {
        public int bodyX;
        public int bodyY;
        public int size;
        public int rotation;

        public Ship(int bodyX, int bodyY, int size, int rotation)
        {
            this.bodyX = bodyX;
            this.bodyY = bodyY;
            this.size = size;
            this.rotation = rotation;
        }
    }
}
