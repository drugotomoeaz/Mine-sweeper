using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mine_sweeper;


namespace Mine_sweeper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.ClientSize = new System.Drawing.Size(800, 570);
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            /*
            this.SetStyle(ControlStyles.SupportsTransparentBackColor |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.DoubleBuffer, true);
                          */
            InitializeComponent();
            Game = new Minesweeper(this);
            this.label1.Text = "Mines: " + Game.MineSweeperSizes[Game.Dificulty][0];
            this.Resize += ResizeButtons;
            
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void EndGame(int result)
        {
            Game.ShowMines();
            Game.timer.Stop();

            //result is 0 if the player lost the game and 2 if he/she won.
            string[] message = new string[4] { "Do you want to play again?", "Game over!",
                "Congratulations, you won!\n Would you like to play again?","Victory!" };
            //this.CancelButton = message.MessageBoxButtons.No;
            var dialogResult = MessageBox.Show(message[result], message[result+1], MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                NewGame();
            }
            else if (dialogResult == DialogResult.No || Control.ModifierKeys == Keys.Escape)
            {
                this.Close();
            }
        }

        public Minesweeper Game;


        public void NewGame()
        {
            while(this.Controls.Count > 0) this.Controls[0].Dispose();
            Refresh();
            InitializeComponent();
            Game = new Minesweeper(this);
            this.label1.Text = "Mines: " + Game.MineSweeperSizes[Game.Dificulty][0];
            
        }

        private void ResizeButtons(object sender, EventArgs e)
        {
            foreach(MineButton button in Game.ButtonList)
            {
                button.Side = this.Width / (2 + Game.MineSweeperSizes[Game.Dificulty][2]);
                button.Location = new System.Drawing.Point(button.Side + (button.Side * (button.Index % Game.MineSweeperSizes[Game.Dificulty][3])), 
                    (button.Side * 2) + (button.Side * (button.Index / Game.MineSweeperSizes[Game.Dificulty][3])));
                
                button.Size = new System.Drawing.Size(button.Side, button.Side);
            }
        }

        private void gameToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }


    }

        
}
