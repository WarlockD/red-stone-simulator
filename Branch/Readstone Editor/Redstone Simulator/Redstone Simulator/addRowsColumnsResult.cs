using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redstone_Simulator
{
    public class addRowsColumnsResult
    {
        private int topRows;
        private int bottomRows;
        private int leftColumns;
        private int rightColumns;
        private bool resultOk;

        public int TopRows
        {
            get { return topRows; }
            set { topRows = value; }
        }

        public int BottomRows
        {
            get { return bottomRows; }
            set { bottomRows = value; }
        }

        public int LeftColumns
        {
            get { return leftColumns; }
            set { leftColumns = value; }
        }

        public int RightColumns
        {
            get { return rightColumns; }
            set { rightColumns = value; }
        }

        public bool ResultOK
        {
            get { return resultOk; }
            set { resultOk = value; }
        }
        
        public addRowsColumnsResult()
        {
            this.topRows = 0;
            this.bottomRows = 0;
            this.leftColumns = 0;
            this.rightColumns = 0;
            this.resultOk = false;
        }

        public addRowsColumnsResult(int top, int bottom, int left, int right)
        {
            this.topRows = top;
            this.bottomRows = bottom;
            this.leftColumns = left;
            this.rightColumns = right;
            if (top != 0 || bottom != 0 || left != 0 || right != 0)
            {
                this.resultOk = true;
            }
            else
            {
                resultOk = false;
            }
        }

    }
}
