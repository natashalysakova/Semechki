using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;


namespace semechki
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        const int CollumnCount = 9;
        const int FirstCounOfNumbers = 27;

        List<Number> Numbers = new List<Number>();
        Choice MyChoice = new Choice();
        List<IJournaled> LastChoice = new List<IJournaled>();
        List<Element> Elements = new List<Element>();
        int ft = -1;
        int st = -1;
        bool isGameRuning = false;
        int GameMode = 0; // 0 - classic, 1 - random;
        string skinName;


        public void NewGame(object sender, EventArgs e)
        {
            Bitmap[] Images = new Bitmap[9];
            Bitmap[] SImages = new Bitmap[9];
            Bitmap[] DImages = new Bitmap[9];
            isGameRuning = true;
            Elements.Clear();
            tableLayoutPanel1.Controls.Clear();
            panel1.Refresh();
            button1.Enabled = true;

            LoadImages(Images, SImages, DImages);

            Numbers.Clear();
            for (int i = 0; i < 9; i++)
            {
                Number n = new Number(i + 1, Images[i], SImages[i], DImages[i]);
                Numbers.Add(n);

            }

            if (GameMode == 0)
            {
                GenerateLevel();
            }
            else if (GameMode == 1)
            {
                GenerateLevelRandom();
            }
            else
            {
                MessageBox.Show("Fucking Shit");
            }


        }

        public void LoadImages(Bitmap[] img, Bitmap[] simg, Bitmap[] dimg)
        {

            string[] path = Directory.GetFiles(Application.StartupPath + "/skins/" + skinName + "/", "*.png");

            int i = 0;
            int j = 0;
            int k = 0;
            int p = 0;
            foreach (string s in path)
            {
                Bitmap im = new Bitmap(s);
                if (p < 9)
                {
                    img[i] = im;
                    i++;
                }
                else if (p < 18)
                {
                    simg[j] = im;
                    j++;
                }
                else
                {
                    dimg[k] = im;
                    k++;
                }
                p++;
            }

        }

        public void LoadGame()
        {
            Bitmap[] Images = new Bitmap[9];
            Bitmap[] SImages = new Bitmap[9];
            Bitmap[] DImages = new Bitmap[9];
            isGameRuning = true;
            Elements.Clear();
            tableLayoutPanel1.Controls.Clear();
            panel1.Refresh();
            button1.Enabled = true;

            LoadImages(Images, SImages, DImages);


            for (int i = 0; i < 9; i++)
            {
                Number n = new Number(i + 1, Images[i], SImages[i], DImages[i]);
                Numbers.Add(n);

            }

            FileStream fs = new FileStream(Application.StartupPath + "/save.bin", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string[] str = new string[4];
            while (!sr.EndOfStream)
            {
                Element el = new Element();
                string s = sr.ReadLine();
                //sw.WriteLine(el.Collumn.ToString() + "|" + el.Row.ToString() + "|" + el.GetActivationState().ToString() + "|" + el.Number.Value.ToString());
                str = s.Split('|');
                el.Collumn = Convert.ToInt32(str[0]);
                el.Row = Convert.ToInt32(str[1]);
                el.SetActivationState(Convert.ToBoolean(str[2]));
                el.Number = Numbers[Convert.ToInt32(str[3]) - 1];
                Elements.Add(el);
            }

            foreach (Element el in Elements)
            {
                PictureBox pc = new PictureBox();
                pc.Click += new EventHandler(this.SelectElement);
                pc.Size = new Size(50, 50);
                pc.SizeMode = PictureBoxSizeMode.CenterImage;
                if (el.GetActivationState())
                {
                    pc.Image = el.Number.Image;
                }
                else
                {
                    pc.Image = el.Number.DeletedImage;
                }
                pc.Tag = tableLayoutPanel1.Controls.Count;
                tableLayoutPanel1.Controls.Add(pc);
            }



            fs.Dispose();
            sr.Dispose();
        }

        public void GenerateLevel()
        {
            List<Number> temp = new List<Number>();


            int k = 0;
            for (int i = 0; i < FirstCounOfNumbers; i++)
            {
                PictureBox pc = new PictureBox();
                pc.Click += new EventHandler(this.SelectElement);
                pc.Size = new Size(50, 50);
                pc.SizeMode = PictureBoxSizeMode.CenterImage;
                if (i < 9)
                {
                    pc.Image = Numbers[i].Image;
                    temp.Add(Numbers[i]);
                }
                else if (i % 2 == 1)
                {
                    pc.Image = Numbers[0].Image;
                    temp.Add(Numbers[0]);

                }
                else
                {
                    pc.Image = Numbers[k].Image;
                    temp.Add(Numbers[k]);
                    k++;
                }
                pc.Tag = i.ToString();
                tableLayoutPanel1.Controls.Add(pc);



            }
            int p = 0;
            foreach (Control pb in this.tableLayoutPanel1.Controls)
            {
                Element el = new Element();
                el.Activate();
                el.Number = temp[p];
                TableLayoutPanelCellPosition cp = tableLayoutPanel1.GetPositionFromControl(pb);
                el.Row = cp.Row;
                el.Collumn = cp.Column;
                Elements.Add(el);

                p++;
            }

        }

        public bool Check(int t, List<Number> temp)
        {
            int k = 0;
            foreach (Number n in temp)
            {
                if (n.Value == t)
                    k++;
            }

            if (k < 4)
                return false;
            else
                return true;
        }

        public void GenerateLevelRandom()
        {
            List<Number> temp = new List<Number>();
            int t;
            Random r = new Random();

            for (int i = 0; i < FirstCounOfNumbers; i++)
            {
                PictureBox pc = new PictureBox();
                pc.Click += new EventHandler(this.SelectElement);
                pc.Size = new Size(50, 50);
                pc.SizeMode = PictureBoxSizeMode.CenterImage;

                do
                {
                    t = r.Next(1, 10);
                }
                while (Check(t, temp));

                pc.Image = Numbers[t - 1].Image;
                temp.Add(Numbers[t - 1]);


                pc.Tag = i.ToString();
                tableLayoutPanel1.Controls.Add(pc);

            }

            int p = 0;
            foreach (Control pb in this.tableLayoutPanel1.Controls)
            {
                Element el = new Element();
                el.Activate();
                el.Number = temp[p];
                TableLayoutPanelCellPosition cp = tableLayoutPanel1.GetPositionFromControl(pb);
                el.Row = cp.Row;
                el.Collumn = cp.Column;
                Elements.Add(el);

                p++;
            }
        }

        private void SelectElement(object sender, EventArgs e)
        {

            if (MyChoice.FirstPB == null)
            {
                MyChoice.FirstPB = (sender) as PictureBox;
                ft = Convert.ToInt32(MyChoice.FirstPB.Tag);
                MyChoice.FirstSelection = Elements[ft].Number.Value;
                //listBox1.Items.Add("FT = " + ft.ToString());
                if (Elements[ft].GetActivationState())
                {
                    MyChoice.FirstPB.Image = Elements[ft].Number.SelectedImage;
                }
                else
                {
                    MyChoice.FirstPB.Image = Elements[ft].Number.DeletedImage;
                    MyChoice.DisposeFirst();
                }
            }
            else if (MyChoice.SecondPB == null)
            {

                MyChoice.SecondPB = (sender) as PictureBox;
                st = Convert.ToInt32(MyChoice.SecondPB.Tag);
                MyChoice.SecondSelection = Elements[st].Number.Value;
                //listBox1.Items.Add("st = " + st.ToString());
                if (Elements[st].GetActivationState())
                {
                    if (MyChoice.FirstPB == MyChoice.SecondPB)
                    {
                        MyChoice.FirstPB.Image = Elements[ft].Number.Image;
                        MyChoice.SecondPB.Image = Elements[st].Number.Image;
                        MyChoice.Dispose();
                    }
                    else
                    {
                        if (IsCorrect())
                        {
                            MyChoice.FirstPB.Image = Elements[ft].Number.DeletedImage;
                            MyChoice.SecondPB.Image = Elements[st].Number.DeletedImage;
                            Elements[ft].Deactivate();
                            Elements[st].Deactivate();
                            tableLayoutPanel1.Refresh();
                            //проверить,может нужно удалить строку)) 
                            RowIsFull(Elements[ft].Row, Elements[st].Row);
                            LastChoice.Add(new Choice(MyChoice));
                            button7.Enabled = true;
                            if (IsGameOver())
                            {
                                Elements.Clear();
                                tableLayoutPanel1.Controls.Clear();
                                button1.Enabled = false;
                                isGameRuning = false;
                                File.Delete(Application.StartupPath + "/save.bin");
                                MessageBox.Show("Вы выйграли!");
                            }

                        }
                        else
                        {
                            MyChoice.FirstPB.Image = Elements[ft].Number.Image;
                            MyChoice.SecondPB.Image = Elements[st].Number.Image;
                        }

                        MyChoice.Dispose();

                    }
                }
                else
                {
                    MyChoice.SecondPB.Image = Elements[st].Number.DeletedImage;
                    MyChoice.DisposeSecond();
                }
            }
        }

        public bool IsCorrect()
        {
            bool isCorrect = false;
            //listBox1.Items.Clear();
            //if( 
            if (MyChoice.FirstSelection + MyChoice.SecondSelection == 10 || MyChoice.FirstSelection == MyChoice.SecondSelection)
            {
                //listBox1.Items.Add(" == 10 или одинаковые");
                if (Elements[ft].Row == Elements[st].Row || Elements[ft].Collumn == Elements[st].Collumn)
                {
                    //listBox1.Items.Add("в одной строке или столбце");
                    int p = Math.Abs(Elements[ft].Collumn - Elements[st].Collumn);
                    int t = Math.Abs(Elements[ft].Row - Elements[st].Row);
                    if (p == 1 || t == 1)
                    {
                        //listBox1.Items.Add("рядышком");
                        isCorrect = true;
                    }
                    else
                    {
                        if (FindActiveInRow(ft, st) == true)
                        {
                            if (FindActiveInCol(ft, st) == true)
                            {
                                isCorrect = false;
                            }
                            else
                            {
                                isCorrect = true;
                            }

                        }
                        else
                        {
                            //listBox1.Items.Add("найдены активные");
                            isCorrect = true;
                        }
                    }
                }
                else
                {
                    //listBox1.Items.Add("В разных строках");

                    //if (FindActiveInRow(ft, st) == true)
                    //{
                    //    if (FindActiveInCol(ft, st) == true)
                    //    {
                    //        isCorrect = false;
                    //    }
                    //    else
                    //    {
                    //        isCorrect = true;
                    //    }

                    //}
                    //else
                    //{
                    //    //listBox1.Items.Add("найдены активные");
                    //    isCorrect = true;
                    //}

                    if (FindActiveInRow(ft, st) == true)
                    {
                        isCorrect = false;
                    }
                    else
                    {
                        //listBox1.Items.Add("найдены активные");
                        isCorrect = true;
                    }
                }

            }

            return isCorrect;
            //return true;
        }

        public void RowIsFull(int first, int second)
        {
            bool isFullF = true;
            bool isFullS = true;

            if (first == second)
            {
                foreach (Element el in Elements)
                {
                    if (el.Row == first && el.GetActivationState())
                    {
                        isFullF = false;
                    }
                }
            }
            else
            {
                foreach (Element el in Elements)
                {
                    if (el.Row == first && el.GetActivationState())
                    {
                        isFullF = false;
                    }
                    if (el.Row == second && el.GetActivationState())
                    {
                        isFullS = false;
                    }
                }
            }
            if (first == second)
            {
                //tableLayoutPanel1.Visible = false;
                if (isFullF)
                {
                    DeleteRow(first);
                }
                //tableLayoutPanel1.Visible = true;
            }
            else
            {
                //tableLayoutPanel1.Visible = false;
                if (isFullF)
                {
                    DeleteRow(first);
                }
                if (isFullS)
                {
                    DeleteRow(second);
                }
                //tableLayoutPanel1.Visible = true;
            }
        }

        public void DeleteRow(int numb)
        {

            foreach (PictureBox pb in tableLayoutPanel1.Controls)
            {
                if (Elements[Convert.ToInt32(pb.Tag)].Row == numb)
                {
                    pb.Visible = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool FindActiveInRow(int i, int j)
        {
            bool rowsearch = false;

            int[] val = new int[] { i, j };

            Array.Sort(val);
            int t = val[0];


            t++;
            while (t < val[1])
            {
                if (Elements[t].GetActivationState() == true)
                {
                    rowsearch = true;
                }
                t++;
            }

            return rowsearch;
        }

        public bool FindActiveInCol(int i, int j)
        {
            bool colsearch;

            int[] val = new int[] { i, j };

            Array.Sort(val);
            int t = val[0];


            t++;

            if (Math.Abs(val[1] - val[0]) < 10)
            {
                colsearch = true;
                while (t < val[1])
                {
                    if (Elements[t].GetActivationState() == false && Elements[t].Collumn == Elements[val[0]].Collumn)
                    {
                        colsearch = false;
                    }
                    t++;
                }
            }
            else
            {
                colsearch = false;
                while (t < val[1])
                {
                    if (Elements[t].GetActivationState() == true && Elements[t].Collumn == Elements[val[0]].Collumn)
                    {
                        colsearch = true;
                    }
                    t++;
                }
            }

            return colsearch;
        }

        private void AddNewElements(object sender, EventArgs e)
        {
            AddRows();
            button7.Enabled = true;
        }

        public void AddRows()
        {

            int c = Elements.Count;
            int added = 0;
            for (int i = 0; i < c; i++)
            {
                if (Elements[i].GetActivationState())
                {
                    //temp.Add(Elements[i]);
                    Element el = new Element(Elements[i]);
                    Elements.Add(el);
                    added++;
                }
                else
                {
                    //listBox1.Items.Add(i);
                }
            }

            LastChoice.Add(new AddedRow(c, added));

            //for (int i = 0; i < temp.Count; i++)
            //{
            //    Elements.Add(temp[i]);
            //}
            //temp.Clear();


            //tableLayoutPanel1.Controls.Clear();

            for (int i = c; i < Elements.Count; i++)
            {
                PictureBox pc = new PictureBox();
                pc.Click += new EventHandler(this.SelectElement);
                pc.Size = new Size(50, 50);
                pc.SizeMode = PictureBoxSizeMode.CenterImage;
                if (Elements[i].GetActivationState())
                {
                    pc.Image = Elements[i].Number.Image;
                }
                else
                {
                    pc.Image = Elements[i].Number.DeletedImage;
                }
                pc.Tag = i.ToString();
                tableLayoutPanel1.Controls.Add(pc);

            }

            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                TableLayoutPanelCellPosition cp = tableLayoutPanel1.GetPositionFromControl(tableLayoutPanel1.Controls[i]);
                Elements[i].Row = cp.Row;
                Elements[i].Collumn = cp.Column;
            }

            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                RowIsFull(i, i);
            }
        }

        public bool IsGameOver()
        {
            bool isOver = true;

            foreach (Element el in Elements)
            {
                if (el.GetActivationState())
                {
                    isOver = false;
                }
            }
            return isOver;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SettingWindow s = new SettingWindow();
            s.SetGameMode(GameMode);
            s.Tag = skinName;
            if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GameMode = s.GetGameMode();
                skinName = s.GetSkinName();

                MessageBox.Show("Для применения измененний начните новую игру");
            }
            s.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Rules r = new Rules();
            r.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.Show();
        }

        void MainWindow_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (e.Delta < 0)
                {
                    panel1.VerticalScroll.Value += 20;
                }
                else
                {
                    panel1.VerticalScroll.Value -= 20;
                }
            }
            catch (Exception) { };
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            FileStream fs2 = new FileStream(Application.StartupPath + "/config.ini", FileMode.Open, FileAccess.Read);
            StreamReader sr2 = new StreamReader(fs2);
            GameMode = Convert.ToInt32(sr2.ReadLine());
            skinName = sr2.ReadLine();
            sr2.Dispose();
            fs2.Dispose();
            if (File.Exists(Application.StartupPath + "/save.bin"))
            {
                DialogResult d = MessageBox.Show("Загрузить сохраненную игру?", "Загрузить?", MessageBoxButtons.YesNoCancel);
                if (d == System.Windows.Forms.DialogResult.Yes)
                {
                    LoadGame();
                }
                else if (d == System.Windows.Forms.DialogResult.Cancel)
                {
                    this.Close();
                }
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {


            if (isGameRuning)
            {
                DialogResult d = MessageBox.Show("Сохранить текущую игру?", "Сохранить?", MessageBoxButtons.YesNo);
                if (d == System.Windows.Forms.DialogResult.Yes)
                {
                    FileStream fs = new FileStream(Application.StartupPath + "/save.bin", FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    foreach (Element el in Elements)
                    {
                        sw.WriteLine(el.Collumn.ToString() + "|" + el.Row.ToString() + "|" + el.GetActivationState().ToString() + "|" + el.Number.Value.ToString());
                    }
                    sw.Dispose();
                    fs.Dispose();
                }
                else
                {
                    try
                    {
                        File.Delete(Application.StartupPath + "/save.bin");
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            FileStream fs2 = new FileStream(Application.StartupPath + "/config.ini", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw2 = new StreamWriter(fs2);
            sw2.WriteLine(GameMode.ToString());
            sw2.WriteLine(skinName);
            sw2.Dispose();
            fs2.Dispose();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (LastChoice.Any())
            {
                var choice = LastChoice.Last();
                var elements = choice.Cancel(Elements);
                if (choice is Choice)
                {
                    ReturnRow(Elements[elements[0]].Row);
                    ReturnRow(Elements[elements[1]].Row);
                }
                else if (choice is AddedRow)
                {
                    for (int i = tableLayoutPanel1.Controls.Count - 1; i >= elements[0]; i--)
                    {
                        tableLayoutPanel1.Controls.RemoveAt(i);
                    }
                }



                LastChoice.Remove(choice);
                if (!LastChoice.Any())
                    button7.Enabled = false;

            }
        }

        public void ReturnRow(int number)
        {
            foreach (PictureBox pb in tableLayoutPanel1.Controls)
            {
                if (Elements[Convert.ToInt32(pb.Tag)].Row == number)
                {
                    pb.Visible = true;
                }
            }
        }
    }
}
