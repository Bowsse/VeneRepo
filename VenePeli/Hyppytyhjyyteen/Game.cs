using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.Storage;

namespace VenePeli
{
    class Game
    {
        private Boat boat;

        private Lake lake;

        private Player player;

        public int Score;

        public int MaxSpeed;
        public int Consumption;
        public double StartingMoney;
        public int TimeBonus = 0;
        public bool ReturnTrip = false;
        public bool Shop = false;
        public bool Finished = false;

        public int TimeLeft { get; set; }

        public Game(int motor , int timeLeft)
        {
            if( motor == 1)
            {//small motor
                MaxSpeed = 2;
                Consumption = 20;
                StartingMoney = 30;
            }
            else if (motor == 2)
            {//big motor
                MaxSpeed = 4;
                Consumption = 48;
                StartingMoney = 20;
            }

            TimeLeft = timeLeft;
            CreateBoat();
            CreateLake();
            CreatePlayer();
        }
        private void CreateBoat()
        {
            boat = new Boat(MaxSpeed, Consumption, 0, 20, 0);
        }
        private void CreateLake()
        {
            lake = new Lake(15); 
        }
        private void CreatePlayer()
        {
            player = new Player();
            player.Beer = 0;
            player.Money = StartingMoney;
        }
        private void AtDestination()
        {
            if (boat.Position >= lake.Length)
            {
                if (ReturnTrip)
                {
                    Finish();
                    Finished = true;
                }
                else
                {
                    Shop = true;

                    ReturnTrip = true;
                    boat.Position = 0;
                }
            }
        }
       
 
        public async void Finish() // victory dialog
        {
            Score = TimeBonus + player.Beer * 100;

            string victory = "Beer: " + player.Beer + "\nTime bonus: " + TimeBonus + "\nTotal score: " + Score;
  
            var finishDialog = new MessageDialog(victory);

            finishDialog.Commands.Add(new UICommand("Close"));

            await finishDialog.ShowAsync();
        }
        public async void Gg(string s) // game over dialog
        {
            var ggDialog = new MessageDialog(s + " ran out.");

            await ggDialog.ShowAsync();
        }
        private void GameOver() //check if game is over
        {
            if (boat.Position < lake.Length)
            {
                if (ReturnTrip == false)
                {
                    if (TimeLeft <= 0)
                    {
                        Gg("Time");
                        Finished = true;
                    }
                }
                if (boat.Fuel <= 0)
                {
                    Gg("Fuel");
                    Finished = true;
                }
            }
        }
        public void Move(double speed)
        {
            boat.Speed = speed; 
            boat.Position = boat.Position + (((boat.Speed + 2) / 7) * boat.MaxSpeed) / 2; 
            boat.Fuel = boat.Fuel - Math.Pow((boat.Consumption / 5), (boat.Speed / 4)) / 4;

            TimeLeft = TimeLeft - 1;

            AtDestination();
            GameOver();
            TimeBonus = (TimeLeft + 60) * 20;
        }
        public double LakeProgress()
        {
            double prog = boat.Position;

            return prog;
        }
        public double FuelMeter()
        {
            double fuel = boat.Fuel;

            return fuel;
        }
        public double MoneyMeter()
        {
            double money = Math.Round(player.Money, 2);

            return money;
        }
        public double BeerMeter()
        {
            int beer = player.Beer;

            return beer;
        }
        public void BuyBeer()
        {
            if(player.Money >= 0.80)
            {
                player.Money = player.Money - 0.80;
                player.Beer = player.Beer + 1;
            }
        }
        public void BuyFuel()
        {
            if (player.Money >= 0.75)
            {
                player.Money = player.Money - 0.75;
                boat.Fuel = boat.Fuel + 0.5;
            }
        }
    }
}
