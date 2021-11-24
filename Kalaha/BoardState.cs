using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalaha {
    class BoardState {
        public int[] Board { get; }
        public char Player { get; }
        public List<BoardState> Childs { get; }
        public double AvgTreeDiff { get; set; }

        public BoardState(int[] board, char player) {
            this.Board = board;
            this.Player = player;
            Childs = new List<BoardState>();
            AvgTreeDiff = GetDiff();
        }

        public void AddChildren(List<BoardState> children) {            
            foreach (BoardState bs in children) {
                Childs.Add(bs);
            }
            double childDiffSum = 0;
            foreach (BoardState bs in Childs) {
                childDiffSum += bs.AvgTreeDiff;
            }
            if (Childs.Count > 0) {
                AvgTreeDiff = childDiffSum / Childs.Count;
            }            
        }

        public int GetDiff() {
            return Board[6] - Board[13];
        }

    }
}
