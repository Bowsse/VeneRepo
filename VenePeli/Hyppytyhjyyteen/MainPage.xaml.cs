using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VenePeli
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Game game;

        public int motorSelection = 1; // motor 1 selected by default

        public List<int> Scores = new List<int>();

        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size(400, 700);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

        }

        
        private void btnMove_Click(object sender, RoutedEventArgs e)
        {
            game.Move(sliderSpeed.Value);

            UpdateControls();
            
            if (game.Shop)
            {
                ShopControls();
            }
            if (game.Finished)
            {
                ResetControls();

                UpdateScores();
            }

        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            game = new Game(motorSelection, 25); //new game with selected motor and 25 time left

            LakeControls(); //changes visible controls

            UpdateControls();

            UpdateScores();
         
        }
        
        private void btnMotor1_Click(object sender, RoutedEventArgs e)
        {
            motorSelection = 1;
        }

        private void btnMotor2_Click(object sender, RoutedEventArgs e)
        {
            motorSelection = 2;
        }

        private void btnBeer_Click(object sender, RoutedEventArgs e)
        {
            game.BuyBeer();
            tbMoney.Text = "Money: " + game.MoneyMeter();
            tbBeer.Text = "Beer: " + game.BeerMeter();
        }

        private void btnFuel_Click(object sender, RoutedEventArgs e)
        {
            game.BuyFuel();
            progFuel.Value = game.FuelMeter();
            tbMoney.Text = "Money: " + game.MoneyMeter();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            LakeControls();

            game.Shop = false;
        }

        private void UpdateScores()
        {
            if (game.Score > 0)
            {
                Scores.Add(game.Score);
            }
            if (Scores.Count > 0)
            {
                Scores.Sort((a, b) => -1 * a.CompareTo(b));

                tbScores.Text = "Top scores:";
                foreach (var item in Scores)
                {
                    tbScores.Text = tbScores.Text + "\n" + item.ToString();
                }

            }
        }
        private void UpdateControls()
        {
            progLake.Value = game.LakeProgress();

            progFuel.Value = game.FuelMeter();

            if (game.TimeLeft > 0)
            {//Clock control using TimeLeft
                tbTime.Text = "20:" + (60 - game.TimeLeft);
            }
            else if (game.TimeLeft <= 0 && game.TimeLeft > -10)
            {
                tbTime.Text = "21:0" + (game.TimeLeft * (-1));
            }
            else
            {
                tbTime.Text = "21:" + (game.TimeLeft * (-1));
            }
            tbBonus.Text = "TimeBonus: " + game.TimeBonus;
        }
        private void ResetControls()
        {
            btnStart.Visibility = Visibility.Visible;
            btnMotor1.Visibility = Visibility.Visible;
            btnMotor2.Visibility = Visibility.Visible;
            gamebg.Visibility = Visibility.Visible;
            btnMove.Visibility = Visibility.Collapsed;
            sliderSpeed.Visibility = Visibility.Collapsed;
            btnBeer.Visibility = Visibility.Collapsed;
            btnFuel.Visibility = Visibility.Collapsed;
            tbMoney.Visibility = Visibility.Collapsed;
            btnReturn.Visibility = Visibility.Collapsed;
            tbBeer.Visibility = Visibility.Collapsed;
            progLake.Visibility = Visibility.Collapsed;
            progFuel.Visibility = Visibility.Collapsed;
            tbTime.Visibility = Visibility.Collapsed;
            tbBonus.Visibility = Visibility.Collapsed;
            tbSpeedlbl.Visibility = Visibility.Collapsed;
            tbLakelbl.Visibility = Visibility.Collapsed;
            tbFuellbl.Visibility = Visibility.Collapsed;

        }
        private void LakeControls()
        {
            tbSpeedlbl.Visibility = Visibility.Visible;
            tbLakelbl.Visibility = Visibility.Visible;
            tbFuellbl.Visibility = Visibility.Visible;
            btnMove.Visibility = Visibility.Visible;
            sliderSpeed.Visibility = Visibility.Visible;
            progFuel.Visibility = Visibility.Visible;
            progLake.Visibility = Visibility.Visible;
            tbTime.Visibility = Visibility.Visible;

            btnBeer.Visibility = Visibility.Collapsed;
            btnFuel.Visibility = Visibility.Collapsed;
            tbMoney.Visibility = Visibility.Collapsed;
            btnReturn.Visibility = Visibility.Collapsed;
            tbBeer.Visibility = Visibility.Collapsed;
            shopbg.Visibility = Visibility.Collapsed;
            tbBonus.Visibility = Visibility.Visible;

            btnStart.Visibility = Visibility.Collapsed;
            btnMotor1.Visibility = Visibility.Collapsed;
            btnMotor2.Visibility = Visibility.Collapsed;

            gamebg.Visibility = Visibility.Collapsed;
        }
        private void ShopControls()
        {
            tbSpeedlbl.Visibility = Visibility.Collapsed;
            tbLakelbl.Visibility = Visibility.Collapsed;
            tbFuellbl.Visibility = Visibility.Collapsed;
            tbBonus.Visibility = Visibility.Collapsed;
            btnMove.Visibility = Visibility.Collapsed;
            sliderSpeed.Visibility = Visibility.Collapsed;
            btnBeer.Visibility = Visibility.Visible;
            btnFuel.Visibility = Visibility.Visible;
            tbMoney.Visibility = Visibility.Visible;
            tbMoney.Text = "Money: " + game.MoneyMeter();
            tbBeer.Text = "Beer: " + game.BeerMeter();
            btnReturn.Visibility = Visibility.Visible;
            tbBeer.Visibility = Visibility.Visible;
            shopbg.Visibility = Visibility.Visible;
            progLake.Visibility = Visibility.Collapsed;
            tbTime.Visibility = Visibility.Collapsed;

        }

        /*public async void WriteScores()
{
   try
   {

       StorageFolder sfolder = ApplicationData.Current.LocalFolder;

       StorageFile sfile = await sfolder.CreateFileAsync("scores.txt",CreationCollisionOption.OpenIfExists);

       var stream = await sfile.OpenAsync(FileAccessMode.ReadWrite);

       using (var output = stream.GetOutputStreamAt(0))
       {
           using (var dataWr = new Windows.Storage.Streams.DataWriter(output))
           {

               foreach(var item in Scores)
               {
                   string s = item + "\n";
                   dataWr.WriteString(s);
               }

               await dataWr.StoreAsync();
               await output.FlushAsync();
           }
       }
       stream.Dispose();
   }
   catch
   {
       System.Diagnostics.Debug.WriteLine("Can't write to file.");
   }
}
public async void ReadScores()
{
   try
   {
       StorageFolder sfolder = ApplicationData.Current.LocalFolder;

       StorageFile sfile = await sfolder.GetFileAsync("scores.txt");

       var read = await sfile.OpenAsync(FileAccessMode.ReadWrite);

       ulong size = read.Size;

       using (var input = read.GetInputStreamAt(0))
       {
           using (var dataReader = new Windows.Storage.Streams.DataReader(input))
           {
               uint bytes = await dataReader.LoadAsync((uint)size);
               string text = dataReader.ReadString(bytes);
               tbScores.Text = text;
           }
       }

   }
   catch
   {
       System.Diagnostics.Debug.WriteLine("Can't read file.");
   }
}
*/

    }
}
