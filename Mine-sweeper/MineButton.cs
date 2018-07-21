using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mine_sweeper
{
    public class MineButton: Button
    {
        //Stores data whether there is a mine or not
        private bool hasMine_ = true;
        public bool HasMine { set { hasMine_ = value; } get { return hasMine_; } }
        //Stores data whether the button is marked with flag
        private bool hasFlag_ = false;
        public bool HasFlag { set { hasFlag_ = value; } get { return hasFlag_; } }
        //Stores data whether the button is marked with questionmark
        private bool hasMark_ = false;
        public bool HasMark { set { hasMark_ = value; } get { return hasMark_; } }

       
        private int side_;
        public int Side { set { side_ = value; } get { return side_; } }

        private int index_;
        public int Index { set { index_ = value; } get { return index_; } }

        

        //Set an image as background - 0 for flagImage and 1 for questionmarkImage
        public void SetImage2(Image picture)
        {

            BackgroundImage = picture;
            Text = null;
            BackgroundImageLayout = ImageLayout.Stretch;
        }
    }
}
