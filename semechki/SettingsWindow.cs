using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace semechki
{
    public partial class SettingWindow : Form
    {
        public SettingWindow()
        {
            InitializeComponent();
        }

        int GM;

        public void SetGameMode(int Gamemode)
        {
            GM = Gamemode;
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(Application.StartupPath + "/skins/");


            int n = 0;
            foreach (var item in dir.GetDirectories())
            {
                comboBox1.Items.Add(item.Name);
                if (item.Name.ToString() == this.Tag.ToString())
                {
                    n = comboBox1.Items.Count - 1;
                }
            }

            comboBox1.SelectedIndex = n;

            if (GM == 0)
            {
                radioButton1.Checked = true;

            }
            else
            {
                radioButton2.Checked = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        public string GetSkinName()
        {
            return comboBox1.SelectedItem.ToString();
        }

        public int GetGameMode()
        {
            if (radioButton1.Checked)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }


    }
}
