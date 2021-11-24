using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalaha {
    class Training {
        private int iterations;
        private Gameboard game;
        private String data;
        private int stones;
        private int minScores;
        private Boolean specific;
        private int specificScore;

        public Training(int iter) {
            stones = 3;

            specific = false;
            specificScore = 32;
            
            minScores = 18;
            iterations = iter;
            data = "FeldA1;FeldA2;FeldA3;FeldA4;FeldA5;FeldA6;FeldWinnerA;FeldB1;FeldB2;FeldB3;FeldB4;FeldB5;FeldB6;FeldWinnerB;Move;\n";
            StartTraining();
        }

        private void StartTraining() {
            Random r = new Random();
            
            if (specific) {
                int diff = 0;
                while (diff != specificScore) {
                    game = new Gameboard(stones);
                    while (!game.IsGameFinished()) {
                        ArrayList options = game.GetFieldOptions();

                        int index = r.Next(0, options.Count);

                        game.DropStones((int)options[index],true);
                    }
                    game.CollectRemainingStones();

                    diff = game.GetFieldA() - game.GetFieldB();
                    diff = (int)Math.Sqrt(diff * diff);
                    if (diff == specificScore) {
                        SaveMovesOfWinner(game.WhoHasWon(), diff);
                    }                   
                }
            } else {
                for (int i = 0; i < iterations; i++) {
                    game = new Gameboard(stones);
                    while (!game.IsGameFinished()) {
                        ArrayList options = game.GetFieldOptions();

                        int index = r.Next(0, options.Count);

                        game.DropStones((int)options[index], true);
                    }
                    game.CollectRemainingStones();

                    int diff = game.GetFieldA() - game.GetFieldB();
                    diff = (int)Math.Sqrt(diff * diff);
                    if (diff >= minScores) {
                        SaveMovesOfWinner(game.WhoHasWon(), diff);
                    }
                    Console.WriteLine("Progress: " + (i + 1) + '/' + iterations + " Iterations");
                }
            }
           
            Console.WriteLine("Finished");
            PrintFile();
        }

        private void SaveMovesOfWinner(char winner, int diff) {
            ArrayList goodMoves = game.Moves;

            //sort out all loser moves
            SortOutLoserMoves(goodMoves, winner);
            SortOutB(goodMoves);

            String output = FormatStringAllMoves(goodMoves);

            //String output = FormatStringLastMoves(goodMoves, diff);

            data += output;
        }

        private void SortOutB(ArrayList moves) {
            for (int i = moves.Count; i>0; i--) {
                String[] move = (String[])moves[i-1];
                if(move[15] == "B"){
                    moves.RemoveAt(i-1);
                }
            }
        }

        private String FormatStringLastMoves(ArrayList moves, int diff) {
            String output = "";
            int lastIndex = moves.Count - 1;
            String[] lastRow = (String[])moves[lastIndex];

            for (int i = 0; i < 16; i++) {
                output += lastRow[i] + ';';
            }
            output += game.GetScore() + ';';
            output += diff.ToString() + ';';
            output += '\n';
            return output;
        }

        private String FormatStringAllMoves(ArrayList moves) {
            String output = "";
            foreach (String[] state in moves) {
                for (int i = 0; i < 15; i++) {
                    output += state[i] + ';';
                }
                /*
                output += game.GetScore() + ';';*/
                output += '\n';
            }
            return output;
        }

        private ArrayList SortOutLoserMoves(ArrayList moves, char winner) {
            ArrayList goodMoves = moves;
            for (int i = goodMoves.Count; i > 0; i--) {
                String[] move = (String[])goodMoves[i - 1];
                if (move[15] != winner.ToString()) {
                    goodMoves.RemoveAt(i - 1);
                }
            }
            return goodMoves;
        }

        private void PrintFile() {
            //save in textfile
            File.WriteAllText(@"C:\Users\bened\Desktop\game.csv", data);
        }
    }
}
