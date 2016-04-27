using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenePeli
{
    class Boat
    {
        public double MaxSpeed;
        public double Consumption;

        public double Position { get; set; }
        public double Fuel { get; set; }
        
        public double Speed { get; set; }

        public Boat(double maxSpeed, double consumption, double position, double fuel, double speed)
        {
            MaxSpeed = maxSpeed;
            Consumption = consumption;
            Position = position;
            Fuel = fuel;
            Speed = speed;
        }

    }
}
