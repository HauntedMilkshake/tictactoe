using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicTacToe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private async void StartButton_Clicked(object sender, EventArgs e)
        {
            var player1 = new Player { Name = Player1Entry.Text, Symbol = "X" };
            var player2 = new Player { Name = Player2Entry.Text, Symbol = "O" };

            await Navigation.PushAsync(new MainPage(player1, player2));
        }
    }

}