using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Find_A_Pair
{
    public partial class Login : Form
    {
        string nick;
        public int mode;
        public Login()
        {
            InitializeComponent();
            radioButton1.Checked = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                mode = 1;
            }
            else
            {
                mode = 2;
            }
            if (textBox1.Text.Length == 0)
                MessageBox.Show("Пустая строка");
            else
            {
                nick = textBox1.Text;
                this.Close();
            }
        }
        public string GetNick { get { return nick; } set { nick = value; } }
    }
}
