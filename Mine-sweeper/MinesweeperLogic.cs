using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mine_sweeper
{
    public partial class Minesweeper
    {
        private int counter_ = 0;
        public int Counter { set { counter_ = value; } get { return counter_; } }

        //Shows how many mines are near that button
        private int[] numberList_ = Enumerable.Repeat(0, mineSweeperSizes_[dificulty_][1]).ToArray();
        public int[] NumberList{set{ numberList_ = value;} get { return numberList_; } }
    

        public void Refresh()
        {
            rnd = new Random();
            bombImage.Dispose();
            flagImage.Dispose();
            markImage.Dispose();
            indicesRange_ = new List<int>(Enumerable.Range(0, mineSweeperSizes_[dificulty_][1]));
            notMineIndices_ = null;
            ButtonList = null;
            ButtonList = new MineButton[mineSweeperSizes_[dificulty_][1]];
            GenButtons();
            AddButtons();
            notMineIndices_ = GenNotMineIndices(indicesRange_);
            UpdateNotMineButtons();
            GenNumbers3();
            EventClick();
        }
        
        
        //Invoked when the counter equals the mine's number. Checks whether 
        private bool CheckFlags()
        {
            foreach(MineButton button in ButtonList)
            {
                if (button.HasMine && button.HasFlag == false) return false;
            }
            return true;
        }
        
        
        
        //Shows all mines when the game ending
        public void ShowMines()
        {
            foreach(MineButton button in ButtonList)
            {
                if (button.HasMine)
                {
                    button.Enabled = false;
                    button.SetImage2(bombImage);
                }
            }
        }

        //Generates the numbers for NumberList . Uses sides
        private void GenNumbers3()
        {
            foreach (MineButton button in ButtonList)
            {
                if (button.HasMine)
                {
                    for (int i = 0; (i <= button.Index + mineSweeperSizes_[dificulty_][3] + 1) && (i < mineSweeperSizes_[dificulty_][1]); i++)
                    {
                        //left neighbour
                        if((button.Location - new Size(button.Width, 0)) == ButtonList[i].Location) NumberList[i]++;
                        //right neighbour
                        if ((button.Location + new Size(button.Width, 0)) == ButtonList[i].Location) NumberList[i]++;
                        //top neighbour
                        if ((button.Location - new Size(0, button.Height)) == ButtonList[i].Location) NumberList[i]++;
                        //bottom neighbour
                        if ((button.Location + new Size(0, button.Height)) == ButtonList[i].Location) NumberList[i]++;

                        //Diagonals
                        //////up right
                        if ((button.Location + new Size(button.Width, - button.Height)) == ButtonList[i].Location) NumberList[i]++;
                        ///// down right
                        if ((button.Location - new Size( button.Width, button.Height)) == ButtonList[i].Location) NumberList[i]++;
                        ///// up left
                        if ((button.Location + new Size(button.Width, button.Height)) == ButtonList[i].Location) NumberList[i]++;
                        ///// down left
                        if ((button.Location + new Size(- button.Width, button.Height)) == ButtonList[i].Location) NumberList[i]++;
                    }
                }
            }
        }

        void NextClick(MineButton nextButton)
        {
 
            nextButton.Enabled = false;
            nextButton.BackColor = Color.FromArgb(100, 248, 23, 109);
            if (NumberList[nextButton.Index] == 0)
            {
                nextButton.Text = null;
                OpenEmptyButtons(nextButton);
            }
            else nextButton.Text = NumberList[nextButton.Index].ToString();
            
        }


        //Opens the buttons that don't have value/0/. Called when an empty button is clicked
        private void OpenEmptyButtons(MineButton button)
        {
            if (NumberList[button.Index] == 0)
            {
                for (int i = 0; (i <= button.Index + mineSweeperSizes_[dificulty_][3] + 1) && (i < mineSweeperSizes_[dificulty_][1]); i++)
                {
                    if (!ButtonList[i].Enabled) continue;
                    //left neighbour
                    if ((button.Location - new Size(button.Width, 0)) == ButtonList[i].Location) NextClick(ButtonList[i]);
                    //right neighbour
                    if ((button.Location + new Size(button.Width, 0)) == ButtonList[i].Location) NextClick(ButtonList[i]);
                    //top neighbour
                    if ((button.Location - new Size(0, button.Height)) == ButtonList[i].Location) NextClick(ButtonList[i]);
                    //bottom neighbour
                    if ((button.Location + new Size(0, button.Height)) == ButtonList[i].Location) NextClick(ButtonList[i]);

                    //Diagonals
                    //////up right
                    if ((button.Location + new Size(button.Width, -button.Height)) == ButtonList[i].Location) NextClick(ButtonList[i]);
                    ///// down right
                    if ((button.Location - new Size(button.Width, button.Height)) == ButtonList[i].Location) NextClick(ButtonList[i]);
                    ///// up left
                    if ((button.Location + new Size(button.Width, button.Height)) == ButtonList[i].Location) NextClick(ButtonList[i]);
                    ///// down left
                    if ((button.Location + new Size(-button.Width, button.Height)) == ButtonList[i].Location) NextClick(ButtonList[i]);
                }
            }
        }

        

    }
}
