using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TicTacToe
{
    public partial class MainPage : ContentPage
    {
        private const int BoardSize = 5;
        private Button[,] _buttons;
        private Player _player1;
        private Player _player2;
        private Player _currentPlayer;
        private int _moveCount;

        public MainPage(Player player1, Player player2)
        {
            InitializeComponent();
            _player1 = player1;
            _player2 = player2;
            _currentPlayer = _player1;
            _moveCount = 0;

            System.Diagnostics.Debug.WriteLine($"Player 1: Name = {_player1.Name}, Symbol = {_player1.Symbol}");
            System.Diagnostics.Debug.WriteLine($"Player 2: Name = {_player2.Name}, Symbol = {_player2.Symbol}");
            InitializeBoard();
            UpdateStatusMessage();
        }

        private void InitializeBoard()
        {
            _buttons = new Button[BoardSize, BoardSize];

            for (int i = 0; i < BoardSize; i++)
            {
                BoardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                for (int j = 0; j < BoardSize; j++)
                {
                    var button = new Button
                    {
                        BackgroundColor = Color.LightGray,
                        TextColor = Color.Black,
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
                    };

                    BoardGrid.Children.Add(button, j, i);
                    int currentRow = i;
                    int currentCol = j;
                    button.Clicked += (s, e) => ButtonClicked(currentCol, currentRow);

                    _buttons[i, j] = button;
                }
            }
        }




        private void UpdateStatusMessage()
        {
            StatusMessage.Text = $"Player {_currentPlayer.Name}'s turn ({_currentPlayer.Symbol})";
        }

        private void ButtonClicked(int col, int row)
        {
            System.Diagnostics.Debug.WriteLine($"Button clicked at row {row}, col {col}");
            if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
            {
                System.Diagnostics.Debug.WriteLine($"Invalid button click: row {row}, col {col}");
                return;
            }

            if (!string.IsNullOrEmpty(_buttons[row, col].Text))
            {
                System.Diagnostics.Debug.WriteLine($"Button already clicked: row {row}, col {col}");
                return;
            }

            _buttons[row, col].Text = _currentPlayer.Symbol;
            _moveCount++;

            if (CheckWinner(row, col))
            {
                System.Diagnostics.Debug.WriteLine($"Player {_currentPlayer.Name} wins!");
                DisplayAlert("Game Over", $"Player {_currentPlayer.Name} wins!", "OK");
                Navigation.PopAsync();
                return;
            }

            if (_moveCount == BoardSize * BoardSize)
            {
                System.Diagnostics.Debug.WriteLine("It's a draw!");
                DisplayAlert("Game Over", "It's a draw!", "OK");
                Navigation.PopAsync();
                return;
            }

            _currentPlayer = _currentPlayer == _player1 ? _player2 : _player1;
            UpdateStatusMessage();
        }



        private bool CheckWinner(int row, int col)
        {
            // Check the row
            for (int i = 0; i <= BoardSize - 3; i++)
            {
                if (IsLineComplete(row, i, 0, 1))
                    return true;
            }

            // Check the column
            for (int i = 0; i <= BoardSize - 3; i++)
            {
                if (IsLineComplete(i, col, 1, 0))
                    return true;
            }

            // Check diagonal from top-left to bottom-right
            for (int i = 0; i <= BoardSize - 3; i++)
            {
                for (int j = 0; j <= BoardSize - 3; j++)
                {
                    if (IsLineComplete(i, j, 1, 1))
                        return true;
                }
            }

            // Check diagonal from bottom-left to top-right
            for (int i = 0; i <= BoardSize - 3; i++)
            {
                for (int j = 0; j <= BoardSize - 3; j++)
                {
                    if (IsLineComplete(BoardSize - 1 - i, j, -1, 1))
                        return true;
                }
            }

            return false;
        }


        private bool IsLineComplete(int startRow, int startCol, int rowStep, int colStep)
        {
            int count = 0;
            for (int i = 0; i < 3; i++)
            {
                if (startRow < BoardSize && startCol < BoardSize &&
                    _buttons[startRow, startCol].Text == _currentPlayer.Symbol)
                {
                    count++;
                }
                else
                {
                    count = 0;
                    break;
                }

                startRow += rowStep;
                startCol += colStep;
            }

            return count == 3;
        }

    }
}