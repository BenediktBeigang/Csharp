using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Kalaha {
    class Gameboard {
        public int[] Board { get; set; }
        public char CurrentPlayer { get; set; }
        private Boolean FirstRound { get; set; }
        public ArrayList Moves { get; set; }

        public Gameboard(int startstones) {
            int s = startstones;
            Moves = new ArrayList();
            Board = new int[] { s,s,s,s,s,s,0,s,s,s,s,s,s,0 };
            CurrentPlayer = 'A';
            FirstRound = true;            
        }

        // represents one move in the game of a player
        public void DropStones(int f, Boolean realInput) {
            if (realInput) {
                String[] tempArray = GetBoardAsString();
                tempArray[14] = f.ToString(); //gamemove
                tempArray[15] = CurrentPlayer.ToString(); //player            
                Moves.Add(tempArray);
            }            

            Boolean roundEnd = false;
            int stones = 0;
            stones = Board[f];
                      
            int currentField = f;
            Board[f] = 0;

            if (FirstRound) {
                if (GetSiteOfField(f) == 'A') {
                    CurrentPlayer = 'A';
                } else {
                    CurrentPlayer = 'B';
                }
                FirstRound = false;
            }

            while (!roundEnd) {
                ConsoleOut();
                // Go around and put stones in fields, until no stone ist left (stones == 0)
                for (int i = stones; i > 0; i--) {
                    currentField = NextField(currentField);
                    // Put stone in field
                    if (currentField == 6) { // A
                        if (CurrentPlayer == 'A') {
                            IncrementField(currentField);
                        } else {
                            i++;
                        }
                    } else if (currentField == 13) {
                        if (CurrentPlayer == 'B') {
                            IncrementField(currentField);
                        } else {
                            i++;
                        }
                    } else if (GetSiteOfField(currentField) == 'A') { // A1-6
                        IncrementField(currentField);
                    } else if (GetSiteOfField(currentField) == 'B') { // B1-6
                        IncrementField(currentField);
                    }
                }
                ConsoleOut();

                // check if player can steal stones (last field empty, own field, rival has stones)
                if (!IsFieldAorB(currentField) //not in storageField
                    && Board[currentField] == 1 //lastField empty
                    && Board[GetOppositeField(currentField)] > 0 //opposite field has stones to steal
                    && IsPlayerOnOwnSite(currentField)) { //on own site
                    stones = TakeField(GetOppositeField(currentField)) + TakeField(currentField);
                } else {
                    roundEnd = true;
                }
            }
            SwitchPlayer(currentField);
        }

        public void AiMove() {
            while (CurrentPlayer == 'A' && !IsGameFinished()) {
                Analyse analyse = new Analyse(this);
                analyse.BuildTree(6);
                int aimove = analyse.IdealMove();               
                DropStones(aimove, true);
            }
        }

        public Boolean IsPlayerOnOwnSite(int field) {
            if ((CurrentPlayer == 'A' && GetSiteOfField(field) == 'A')
                        || (CurrentPlayer == 'B' && GetSiteOfField(field) == 'B')) {
                return true;
            }
            return false;
        }

        public int NextField(int f) {
            if (f == 13) {
                return 0;
            } else {
                return f+1;
            }
        }

        // switches the currentPlayer value
        public void SwitchPlayer(int lastField) {
            if (CurrentPlayer == 'A' && lastField != 6) {
                CurrentPlayer = 'B';
            } else if (CurrentPlayer == 'B' && lastField != 13) {
                CurrentPlayer = 'A';
            }
        }

        public void CollectRemainingStones() {
            Board[6] += CountStonesOnSite('A');
            Board[13] += CountStonesOnSite('B');
            for (int i = 0; i<13; i++) {
                if (i != 6) {
                    Board[i] = 0;
                }
            }
        }

        public int CountStonesOnSite(char player) {
            int sum = 0;
            switch (player) {
                case 'A':
                    for (int i = 0; i < 6; i++) {
                        sum += Board[i];
                    }
                    break;
                case 'B':
                    for (int i = 7; i < 13; i++) {
                        sum += Board[i];
                    }
                    break;
            }
            return sum;
        }

        public Boolean IsGameFinished() {
            if (CountStonesOnSite('A') == 0 || CountStonesOnSite('B') == 0) {
                return true;
            }
            return false;
        }
        
        // increments specific field
        public void IncrementField(int f) {
            Board[f] = Board[f] + 1;
        }

        // returns value of field and zeros field
        public int TakeField(int f) {
            if (f >= 0 && f <= 12 && f != 6) {
                int value = Board[f];
                Board[f] = 0;
                return value;
            }
            return -1;
        }

        // returns value of field
        public int PeekField(int f) {
            if (f >= 0 && f <= 13) {
                return Board[f];
            }
            return -1;
        }

        // returns index of opposite field
        private int GetOppositeField(int f) {
            switch (f) {
                case 0:
                    return 12;
                case 1:
                    return 11;
                case 2:
                    return 10;
                case 3:
                    return 9;
                case 4:
                    return 8;
                case 5:
                    return 7;
                case 7:
                    return 5;
                case 8:
                    return 4;
                case 9:
                    return 3;
                case 10:
                    return 2;
                case 11:
                    return 1;
                case 12:
                    return 0;
                default:
                    return -1;
            }
        }

        // returns site as char (A or B) of input field
        private char GetSiteOfField(int f) {
            if (f >= 0 && f <= 5) {
                return 'A';
            } else if (f >= 7 && f <= 12) {
                return 'B';
            }
            return '!';
        }

        // return if field is one of the two big fields A or B
        private Boolean IsFieldAorB(int f) {
            if (f == 6 || f == 13) {
                return true;
            }
            return false;
        }

        // returns value of main field of A
        public int GetFieldA() {
            return Board[6];
        }
        //returns value of main field of B
        public int GetFieldB() {
            return Board[13];
        }

        // Retruns Board as StringArray
        public String[] GetBoardAsString() {
            String[] output = new String[16];
            for (int i = 0; i < 14; i++) {
                output[i] = Board[i].ToString();
            }
            return output;
        }

        // returns all fieldIndexes of currentPlayer that are not zero as ArrayList
        public ArrayList GetFieldOptions() {
            ArrayList options = new ArrayList();
            switch (CurrentPlayer) {
                case 'A':
                    for (int i=0; i<6; i++) {
                        if (Board[i]>0) {
                            options.Add(i);
                        }                      
                    }
                    break;
                case 'B':
                    for (int i = 7; i < 13; i++) {
                        if (Board[i] > 0) {
                            options.Add(i);
                        }                        
                    }
                    break;
            }
            return options;
        }

        public char OppositePlayer(char player) {
            switch (player) {
                case 'A': return 'B';
                case 'B': return 'A';
                default: return '/';
            }
        }

        public char WhoHasWon() {
            char winner = '/';
            int a = GetFieldA();
            int b = GetFieldB();
            if (a < b) {
                winner = 'B';
            } else if (a > b) {
                winner = 'A';
            }
            return winner;
        }

        public String GetScore() {
            return Board[6].ToString() + " : " + Board[13].ToString();
        }

        // prints board in console
        private void ConsoleOut() {
            /*
            String str = "  | ";
            for (int i = 5; i > -1; i--) {
                str += board[i] + " | ";
            }
            Console.WriteLine(str);

            Console.WriteLine(GetFieldA() + " |                       | " + GetFieldB());

            str = "  | ";
            for (int i = 7; i < 13; i++) {
                str += board[i] + " | ";
            }
            Console.WriteLine(str);*/
        }
    }
}
