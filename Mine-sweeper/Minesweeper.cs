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
        //Constructor
        public Minesweeper(Form1 form)
        {
            
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            MineForm = form;
            label1 = form.label1;
            label2 = form.label2;

            indicesRange_ = new List<int>(Enumerable.Range(0, mineSweeperSizes_[dificulty_][1]));
            notMineIndices_ = GenNotMineIndices(indicesRange_);
            GenButtons();
            AddButtons();
            UpdateNotMineButtons();
            GenNumbers3();
            EventClick();

        }

       

        //Properties related to the Form1 class
        private Form1 mineForm_;
        public Form1 MineForm { set {mineForm_ = value; } get {return mineForm_; } }
        public Label label1 = new Label();
        public Label label2 = new Label();
        


        public Timer timer = new Timer();
        int timePassed = 0;
        private void Timer_Tick(object sender, EventArgs e)
        {
            timePassed++;
            label2.Text = "Time in seconds: " + timePassed;
        }


        private static Random rnd = new Random();

        //Three dificulties; First digit shows the number of mines, second one - number of squares/buttons, third - columns, fourth - rows
        private static int[][] mineSweeperSizes_ = new int[3][] { new int [] { 10, 81, 9, 9 }, new int [] { 40, 256, 16, 16 }, new int[] { 99, 480, 16, 30 } };
        public int[][] MineSweeperSizes{ get { return mineSweeperSizes_; } set { mineSweeperSizes_ = value; }}

        //Can be changed from GameStripMenu. The number indicates the index of MineSweeperSizes that will be used in the button's generation.
        private static int dificulty_ = 1;
        public int Dificulty { set { dificulty_ = value; } get { return dificulty_; } }

        //Stores data for the buttons that will be initialized
        private MineButton[] buttonList_ = new MineButton[mineSweeperSizes_[dificulty_][1]];
        public MineButton[] ButtonList { set { buttonList_ = value; } get { return buttonList_; } }

        //Stores how many times the left mouse button is clicked for every Minebutton
        int[] timesClicked = Enumerable.Repeat<int>(0, mineSweeperSizes_[dificulty_][1]).ToArray();
        int FirstClick = 0;
        


        private  List<int> indicesRange_ = new List<int>(Enumerable.Range(0, mineSweeperSizes_[dificulty_][1]));

        //Stores indices of buttons that don't have mine
        private List<int> notMineIndices_ = null;
        

        Image bombImage = new Bitmap(@"bomb.png");
        Image flagImage = new Bitmap(@"flag.jpg");
        Image markImage = new Bitmap(@"question-mark.jpg");



        //Generates mine's indices
        private  List<int> GenNotMineIndices(List<int> array)
        {
            array.RemoveAt(rnd.Next(array.Count));
            if (array.Count > mineSweeperSizes_[dificulty_][1] - mineSweeperSizes_[dificulty_][0]) return GenNotMineIndices(array);
            else return array;
        }
        

        //Update the buttons with Background minePicture
        private void UpdateNotMineButtons()
        {
            foreach(int mineIndex in notMineIndices_) ButtonList[mineIndex].HasMine = false;
        }

        //Generates buttons
        public void GenButtons()
        {
            for(int i =0; i < buttonList_.Length; i++)
            {
                this.ButtonList[i] = new MineButton();

                ButtonList[i].Side = MineForm.Width/(2 + mineSweeperSizes_[dificulty_][2]);
                ButtonList[i].Index = i;
                ButtonList[i].Name = i.ToString();
                ButtonList[i].Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                ButtonList[i].ForeColor = System.Drawing.Color.FromArgb(100, 33, 56, 206);
                ButtonList[i].BackColor = System.Drawing.Color.FromArgb(100, 248, 170, 237);
                //ButtonList[i].BackColor = Color.FromArgb(100, 248, 23, 109);
                ButtonList[i].Location = new System.Drawing.Point(ButtonList[i].Side + (ButtonList[i].Side * (i % mineSweeperSizes_[dificulty_][3])), (ButtonList[i].Side*2) + (ButtonList[i].Side * (i / mineSweeperSizes_[dificulty_][3])));
                ButtonList[i].Size = new System.Drawing.Size(ButtonList[i].Side, ButtonList[i].Side);
            }
        }

        private void EventClick()
        {
            foreach(Button button in buttonList_)
            {
                
                button.MouseDown += Button_MouseClick;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            MineButton button = sender as MineButton;
            if (NumberList[button.Index] == 0)
            {
                OpenEmptyButtons(button);
                button.Text = null;
            }
            else button.Text = NumberList[button.Index].ToString();
        }

        private void Button_MouseClick(object sender, MouseEventArgs e)
        {
            MineButton button = sender as MineButton;
            if (FirstClick == 0) timer.Start();
            FirstClick++;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    
                    if (button.HasMine == false)
                    {
                        if (button.HasFlag) this.Counter--;
                        label1.Text = "Mines: " + (mineSweeperSizes_[dificulty_][0] - Counter);
                        label1.Update();
                        button.BackgroundImage = null;
                        if (NumberList[button.Index] == 0)
                        {
                            OpenEmptyButtons(button);
                            button.Text = null;
                        }
                        else button.Text = NumberList[button.Index].ToString();
                    }
                    else
                    {
                        button.SetImage2(bombImage);
                        MineForm.EndGame(0);
                    }
                    button.Enabled = false;
                    button.BackColor = Color.FromArgb(100, 248, 23, 109);
                    break;

                case MouseButtons.Right:
                    //Set an image as background - 0 for flagImage and 1 for questionmarkImage
                    if (timesClicked[button.Index] % 3 == 0)
                    {
                        button.HasFlag = true;
                        button.SetImage2(flagImage);
                        Counter++;
                    }
                    else if (timesClicked[button.Index] % 3 == 1)
                    {
                        button.HasFlag = false;
                        button.HasMark = true;
                        button.SetImage2(markImage);
                        Counter--;
                    }
                    else if (timesClicked[button.Index] % 3 == 2)
                    {
                        button.HasMark = false;
                        button.SetImage2(null);
                    }
                    
                    label1.Text = "Mines: " + (mineSweeperSizes_[dificulty_][0] - Counter);
                    label1.Update();
                    timesClicked[button.Index]++;

                    if (mineSweeperSizes_[dificulty_][0] == Counter)
                    {
                        if(CheckFlags()) MineForm.EndGame(2);
                        else MineForm.EndGame(0);
                    }
                    break;
            }
        }

        private void AddButtons()
        {
            MineForm.Controls.AddRange(buttonList_);
        }

        private void HideButtons()
        {
            MineForm.Controls.Clear();

        }

        
         

    }
}
