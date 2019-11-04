
namespace NavalBattles
{
    public class Ship
    {


        public int health;
        public int[,] Coords;

        public Ship(int ShipSize, int[,] Coords)
        {
            health = ShipSize;

            this.Coords = Coords;
        }
    }
}
