using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalaha {
    class Analyse {
        private Gameboard game;
        private BoardState root;
        private int moveCount;
        private BoardState[] moves;
        private int startDepth;


        public Analyse(Gameboard gb) {
            game = gb;
            root = new BoardState(game.Board, game.CurrentPlayer);
            moveCount = 0;
            moves = new BoardState[6];
        }

        //calculates all possibilitys till a given depth
        public void BuildTree(int depth) {
            startDepth = depth;
            root = RecursivTreeBuild(root, depth);
            //PrintTree(root,0);
            Console.WriteLine("Baum wurde erstellt mit " + moveCount + " unterschiedlichen Spielverläufen in den nächsten " + depth + " Zügen");           
        }

        private BoardState RecursivTreeBuild(BoardState startState, int depth) {
            if (depth > 0) {                
                switch (startState.Player) {
                    case 'A':
                        startState.AddChildren(CreateChilds(0, 6, startState, depth));
                        return startState;
                    case 'B':
                        startState.AddChildren(CreateChilds(7, 13, startState, depth));
                        return startState;
                }                              
            }
            return startState;
        }

        private List<BoardState> CreateChilds(int start, int end, BoardState state, int depth) {
            List<BoardState> children = new List<BoardState>();
            if (!game.IsGameFinished()) {
                int[] parentArray = CreateBoardCopy(state.Board);
                char parentPlayer = state.Player;

                for (int i = start; i < end; i++) {
                    game.Board = CreateBoardCopy(parentArray);
                    game.CurrentPlayer = parentPlayer;

                    if (game.Board[i] > 0) { // field is not empty, so player can pick field
                        game.DropStones(i, false);
                        int[] resultBoard = CreateBoardCopy(game.Board);
                        char nextPlayer = game.CurrentPlayer;

                        BoardState newChild = RecursivTreeBuild(new BoardState(resultBoard, nextPlayer), depth - 1);
                        children.Add(newChild);
                        if (depth == startDepth) {
                            moves[i] = newChild;
                        }
                        moveCount++;
                    }
                }
                game.Board = CreateBoardCopy(parentArray);
                game.CurrentPlayer = parentPlayer;
            }           
            return children;
        }

        private int[] CreateBoardCopy(int[] source) {
            int[] copy = new int[14];
            Array.Copy(source, copy, 14);
            return copy;
        }

        private void PrintTree(BoardState parent, int depth) {
            String lines = "-----------------------------" + DepthIllustrationString(depth);
            Console.WriteLine(lines);

            PrintBoard(parent.Board);
            if (parent.Childs.Count > 0) {
                Console.WriteLine("\nPossibilitys (Depth: "+ depth +"):\n");
                foreach (BoardState child in parent.Childs) {
                    //PrintBoard(child.Board);
                    PrintTree(child, depth+1);
                    Console.WriteLine("\n");
                }
            }            
            
            Console.WriteLine(lines);
        }

        private String DepthIllustrationString(int depth) {
            String output = "";
            for (int i= 0; i<depth; i++) {
                output += "-----";
            }
            return output;
        }

        private void PrintBoard(int[] board) {
            String str = "  | ";
            for (int i = 5; i > -1; i--) {
                str += board[i] + " | ";
            }
            Console.WriteLine(str);

            Console.WriteLine(board[6] + " |                       | " + board[13]);

            str = "  | ";
            for (int i = 7; i < 13; i++) {
                str += board[i] + " | ";
            }
            Console.WriteLine(str);
        }


        public int IdealMove() {
            double maxDiff = int.MinValue;
            int index = int.MinValue;

            for (int i = 0; i < 6; i++) {
                if (moves[i] != null) {
                    double diff = moves[i].AvgTreeDiff;
                    Console.WriteLine("Feld " + i + ": " + diff);
                    if (diff > maxDiff) {                        
                        maxDiff = moves[i].AvgTreeDiff;
                        index = i;
                    }                                                     
                } else {
                    Console.WriteLine("Feld " + i + ": Keine Kinder");
                }               
            }
            if (index == int.MinValue) {
                Console.WriteLine("stop");               
            }

            Console.WriteLine("Die Analyse ergab das Feld: " + index);
            return index;
        }

        /*
        public double RecursiveTreeSearch(BoardState parent) {
            double diff = parent.Board[6] - parent.Board[13];
            if (parent.Childs.Count > 0) {
                double diffSum = 0;
                int parentChildSum = parent.Childs.Count;
                foreach (BoardState child in parent.Childs) {
                    diffSum += RecursiveTreeSearch(child);
                }
                diff = diffSum / parentChildSum;
            }
            return diff;
        }*/

    }
}
