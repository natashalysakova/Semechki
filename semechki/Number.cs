using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace semechki
{
    class Number
    {

        public int Value { set; get; }
        public Bitmap Image { set; get; }
        public Bitmap SelectedImage { set; get; }
        public Bitmap DeletedImage { set; get; }

        public Number(int val, Bitmap img, Bitmap simg, Bitmap dimg)
        {
            Value = val;
            Image = img;
            SelectedImage = simg;
            DeletedImage = dimg;
        }

        
    }
    class Choice
    {
        public Number FirstSelection { set; get; }
        public Number SecondSelection { set; get; }

        public Choice()
        {
        }
        ~Choice()
        { }
    }
}
