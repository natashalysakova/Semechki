using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace semechki
{
    public class Element
    {
        public Number Number { set; get; }
        public int Collumn { set; get; }
        public int Row { set; get; }
        private bool IsActive;

        public Element(Element el)
        {
            Number = el.Number;
            IsActive = el.GetActivationState();
            Row = 0;
            Collumn = 0;
        }
        public Element()
        {
            Number = null;
            IsActive = true;
            Row = 0;
            Collumn = 0;
        }
        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public bool GetActivationState()
        {
            return IsActive;
        }
        public void SetActivationState(bool value)
        {
            IsActive = value;
        }
    }

    public class Choice : IJournaled
    {

        public PictureBox FirstPB { set; get; }
        public PictureBox SecondPB { set; get; }
        public int FirstSelection { set; get; }
        public int SecondSelection { set; get; }


        public Choice()
        {
            FirstPB = null;
            SecondPB = null;
            FirstSelection = 0;
            SecondSelection = 0;
        }
        public Choice(Choice c)
        {
            FirstPB = c.FirstPB; ;
            SecondPB = c.SecondPB;
            FirstSelection = c.FirstSelection;
            SecondSelection = c.SecondSelection;
        }

        public void DisposeFirst()
        {
            FirstPB = null;
        }
        public void DisposeSecond()
        {
            SecondPB = null;
        }

        public void Dispose()
        {

            FirstPB = null;
            SecondPB = null;
        }

        public int[] Cancel(List<Element> elements)
        {
            int i = Convert.ToInt32(FirstPB.Tag);
            int j = Convert.ToInt32(SecondPB.Tag);

            elements[i].Activate();
            FirstPB.Image = elements[i].Number.Image;
            elements[j].Activate();
            SecondPB.Image = elements[j].Number.Image;

            var answer = new[] {i, j};
            return answer;
        }
    }

    public class Number
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

    interface IJournaled
    {
        int[] Cancel(List<Element> elements);
    }

    class AddedRow : IJournaled
    {
        public AddedRow(int i, int added)
        {
            StartElement = i;
            Count = added;
        }

        public int StartElement { get; set; }
        public int Count { get; set; }


        public int[] Cancel(List<Element> elements)
        {
            elements.RemoveRange(StartElement, Count);

            var answer = new[] { StartElement, Count };
            return answer;

        }
    }
}
