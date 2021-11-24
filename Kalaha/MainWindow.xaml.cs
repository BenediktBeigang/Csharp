using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kalaha{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Button[] buttons = new Button[12];
        private Gameboard game;
        private int depth;

        public MainWindow(){
            InitializeComponent();
            buttons = new Button[12];
            depth = 6;
            InitializeButtons();
        }

        public void InitializeButtons() {
            buttons[0] = A1;
            buttons[1] = A2;
            buttons[2] = A3;
            buttons[3] = A4;
            buttons[4] = A5;
            buttons[5] = A6;
            buttons[6] = B1;
            buttons[7] = B2;
            buttons[8] = B3;
            buttons[9] = B4;
            buttons[10] = B5;
            buttons[11] = B6;
            DisableAllButtons();
        }

        private void EnableAllButtons() {
            foreach (Button b in buttons) {
                b.IsEnabled = true;
            }
        }
        private void DisableAllButtons() {
            foreach (Button b in buttons) {
                b.IsEnabled = false;
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e){
            game = new Gameboard(int.Parse(stoneCount.Text));
            Playerdisplay.Text = "Erster Spieler darf Feld wählen";
            startButton.Content = "Playing";
            WinnerDisplay.Text = "";
            moveDisplay.Text = "";
            startButton.IsEnabled = false;
            aiStartButton.IsEnabled = false;
            EnableAllButtons();
            UpdateBoardUI();
            showMovesButton.IsEnabled = true;
        }

        private void AiStartButton_Click(object sender, RoutedEventArgs e) {
            game = new Gameboard(int.Parse(stoneCount.Text));
            aiStartButton.Content = "Playing";
            moveDisplay.Text = "";
            startButton.IsEnabled = false;
            aiStartButton.IsEnabled = false;
            showMovesButton.IsEnabled = true;
            WinnerDisplay.Text = "";
            game.CurrentPlayer = 'B';
            SwitchButtonActivation();
            UpdateBoardUI();
        }

        private void RunAiRound(int field) {
            if (field >= 7 && field <= 12) {
                DisableAllButtons();
                game.DropStones(field, true);
                game.AiMove();
                SwitchButtonActivation();
                AddMoveToDisplay();
                if (game.IsGameFinished()) {
                    ResetGame();
                }
                ActivateButtonsOf('B');
            }                
        }

        private void RunRound(int field) {          
            game.DropStones(field, true);
            SwitchButtonActivation();         
            if (game.IsGameFinished()) {
                ResetGame();
            }
        }

        private void SaveMoves() {
            ArrayList goodMoves = game.Moves;
            //format and create string
            String output="";
            //int count = goodMoves.Count;
            foreach (String[] state in goodMoves) {
                for (int i = 0; i<15; i++) {
                    output += state[i] + ';';
                }
                output += '\n';
            }

            //save in textfile
            File.WriteAllText(fileRoot.Text + @"\game.csv", output);
        }

        private void ResetGame() {
            game.CollectRemainingStones();
            String winner = game.WhoHasWon().ToString();
            if (aiStartButton.Content.ToString() == "Playing") {
                if (winner == "A") {
                    winner = "AI";
                } else if (winner == "B") {
                    winner = "Spieler";
                }
            } else {
                winner = "Spieler " + winner;
            }
                      
            //SaveMoves();

            DisableAllButtons();
            UpdateBoardUI();

            String output;
            if (winner != "/") {
                output = winner + " hat gewonnen mit " + game.GetScore() + " !";
            } else {
                output = "Unentschieden";
            }
            WinnerDisplay.Text = output;            

            DeactivateBoard();
        }

        private void UpdateBoardUI() {
            for (int i = 0; i < 6; i++) {
                Dispatcher.Invoke(new Action(() => buttons[i].Content = game.Board[i].ToString()));
            }
            for (int i = 7; i < 13; i++) {
                Dispatcher.Invoke(new Action(() => buttons[i - 1].Content = game.Board[i].ToString()));
            }
            Dispatcher.Invoke(new Action(() => fieldA.Text = game.Board[6].ToString()));
            Dispatcher.Invoke(new Action(() => fieldB.Text = game.Board[13].ToString()));
        }

        private void SwitchButtonActivation() {
            DisableAllButtons();
            if (game.CurrentPlayer == 'A') {
                Playerdisplay.Text = "Aktueller Spieler: A";
                ActivateButtonsOf('A');
            } else if (game.CurrentPlayer == 'B') {
                Playerdisplay.Text = "Aktueller Spieler: B";
                ActivateButtonsOf('B');
            }
        }

        public void ActivateButtonsOf(char player) {
            switch (player) {
                case 'A':
                    for (int i = 0; i < 6; i++) {
                        if (game.Board[i] > 0) {
                            buttons[i].IsEnabled = true;
                        }
                    }
                    break;
                case 'B':
                    for (int i = 7; i < 13; i++) {
                        if (game.Board[i] > 0) {
                            buttons[i - 1].IsEnabled = true;
                        }
                    }
                    break;
            }
        }

        private void DeactivateBoard() {
            String name;
            for (int i = 0; i<12; i++) {
                if (i<6) {
                    name = "A" + (i + 1);
                } else {
                    name = "B" + (i - 5);
                }
                buttons[i].Content = name;
                buttons[i].IsEnabled = false;
            }
            fieldA.Text = "A";
            fieldB.Text = "B";
            Playerdisplay.Text = "";

            startButton.IsEnabled = true;
            startButton.Content = "Start";
            aiStartButton.IsEnabled = true;
            aiStartButton.Content = "Play Against Ai";
            showMovesButton.IsEnabled = false;
        }

        private void AddMoveToDisplay() {
            moveDisplay.Text = "";
            //String[] data in game.Moves
            for (int i = 0; i<game.Moves.Count; i++) {
                String[] data = (String[])game.Moves[i];
                String move = data[14];
                char player = (data[15])[0];
                if (player == 'A') {
                    Run run = new Run("AI---" + move + "\n") {
                        Foreground = Brushes.Orange
                    };
                    moveDisplay.Inlines.Add(run);
                } else if (player == 'B'){
                    String extra = "";
                    if (i > 0 && ((String[])game.Moves[i - 1])[15] == "A") {
                        extra = "\n";
                    }
                    Run run = new Run(extra + "Spieler---" + move + "\n\n") {
                        Foreground = Brushes.Blue
                    };
                    moveDisplay.Inlines.Add(run);
                }
            }
            for (int i = 0; i<1000; i++) {
                scroller.LineDown();
            }            
        }
        
        private void Field_Click(object sender, RoutedEventArgs e) {
            if (startButton.Content.ToString() == "Playing") {
                String buttonName = ((Button)sender).Name;
                    switch (buttonName) {
                    //Spieler A
                    case "A1":
                        RunRound(0);
                        break;
                    case "A2":
                        RunRound(1);
                        break;
                    case "A3":
                        RunRound(2);
                        break;
                    case "A4":
                        RunRound(3);
                        break;
                    case "A5":
                        RunRound(4);
                        break;
                    case "A6":
                        RunRound(5);
                        break;
                    // Spieler B
                    case "B1":
                        RunRound(7);
                        break;
                    case "B2":
                        RunRound(8);
                        break;
                    case "B3":
                        RunRound(9);
                        break;
                    case "B4":
                        RunRound(10);
                        break;
                    case "B5":
                        RunRound(11);
                        break;
                    case "B6":
                        RunRound(12);
                        break;
                }                
            } else if (aiStartButton.Content.ToString() == "Playing") {
                switch (((Button)sender).Name) {
                    // Spieler
                    case "B1":
                        RunAiRound(7);
                        break;
                    case "B2":
                        RunAiRound(8);
                        break;
                    case "B3":
                        RunAiRound(9);
                        break;
                    case "B4":
                        RunAiRound(10);
                        break;
                    case "B5":
                        RunAiRound(11);
                        break;
                    case "B6":
                        RunAiRound(12);
                        break;
                }
            }
            UpdateBoardUI();
        }

        private void TrainButton_Click(object sender, RoutedEventArgs e) {
            new Training(int.Parse(iterInputBox.Text));
        }

        private void ShowMovesButton_Click(object sender, RoutedEventArgs e) {
            Analyse analyse = new Analyse(game);
            analyse.BuildTree(depth);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e) {
            DisableAllButtons();
            UpdateBoardUI();
            DeactivateBoard();
            moveDisplay.Text = "";
        }
    }  
    

}
