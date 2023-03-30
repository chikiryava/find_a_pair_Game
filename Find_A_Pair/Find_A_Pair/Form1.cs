using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Find_A_Pair
{
    public partial class Form1 : Form
    {
        // Use this Random object to choose random icons for the squares
        Random random = new Random();
        Login login = new Login();

        // Each of these letters is an interesting icon
        // in the Webdings font,
        // and each icon appears twice in this list
        List<string> icons = new List<string>()
    {
        "!", "!", "N", "N", ",", ",", "k", "k",
        "b", "b", "v", "v", "w", "w", "z", "z"
    };
        List<string> icons1 = new List<string>()
        {
            "h", "h", "j", "j", "k", "k", "l", "l",
            "q", "q", "w", "w", "e", "e", "r", "r",
            "a", "a", "s", "s", "d", "d", "f", "f",
            "z",  "z", "x", "x", "c", "c", "m","m",
            "g","g","v","v"
        };
        // firstClicked points to the first Label control 
        // that the player clicks, but it will be null 
        // if the player hasn't clicked a label yet
        Label firstClicked = null;

        // secondClicked points to the second Label control 
        // that the player clicks
        Label secondClicked = null;
        int i = 0;
        
        public Form1()
        {
            login.ShowDialog();            
            InitializeComponent();
            if(login.mode==2)
                CREATE_TABLE(6, 6,icons1);
            else
                CREATE_TABLE(4, 4,icons);
        }
     

        private void label1_Click(object sender, EventArgs e)
        {
            // The timer is only on after two non-matching 
            // icons have been shown to the player, 
            // so ignore any clicks if the timer is running
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // If the clicked label is black, the player clicked
                // an icon that's already been revealed --
                // ignore the click
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                // If firstClicked is null, this is the first icon
                // in the pair that the player clicked, 
                // so set firstClicked to the label that the player 
                // clicked, change its color to black, and return
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                // If the player gets this far, the timer isn't
                // running and firstClicked isn't null,
                // so this must be the second icon the player clicked
                // Set its color to black
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;
                i++;
                CheckForWinner();
                
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }


                // If the player gets this far, the player 
                // clicked two different icons, so start the 
                // timer (which will wait three quarters of 
                // a second, and then hide the icons)
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            timer1.Stop();

            // Hide both icons
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // Reset firstClicked and secondClicked 
            // so the next time a label is
            // clicked, the program knows it's the first click
            firstClicked = null;
            secondClicked = null;
        }
        private void CheckForWinner()
        {
            // Go through all of the labels in the TableLayoutPanel, 
            // checking each one to see if its icon is matched
            foreach (Control control in table.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            // If the loop didn’t return, it didn't find
            // any unmatched icons
            // That means the user won. Show a message and close the form
            string lvl;
            if (login.mode == 1)
                lvl = "Легкого";
            else
                lvl = "Сложного";

            MessageBox.Show($"Игроку {login.GetNick} понадобилось {i} шагов для прохождения {lvl} уровня. Поздравляем!");
            StreamWriter sw = File.AppendText("logs.txt");
            sw.WriteLine($"Игрок {login.GetNick} справился за {i} шагов");
            sw.Close();
            
        }
        Panel panel = new Panel();
        public TableLayoutPanel table = new TableLayoutPanel(); //важно:
        public void CREATE_TABLE(int Columns, int Rows,List<string> icons1)
        {
            panel.Controls.Clear();
            table.Dock = DockStyle.Fill;
            table.BackColor = Color.CornflowerBlue;


            panel.Width = Convert.ToInt32(this.Width * 0.75f);
            panel.Height = Convert.ToInt32(this.Height * 0.9f);
            //границы
            table.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            table.Location = new System.Drawing.Point(0, 0);
            table.Visible = true;
            table.ColumnCount = Convert.ToInt32(Columns);
            table.RowCount = Convert.ToInt32(Rows);

            int width = 100 / table.ColumnCount;
            int height = 100 / table.RowCount;

            table.Font = new Font("Webdings", 48);

            // добавляем колонки и строки
            for (int col = 0; col < table.ColumnCount; col++)
            {
                // добавляем колонку
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, width));

                for (int row = 0; row < table.RowCount; row++)
                {
                    // добавляем строку
                    if (col == 0)
                    {
                        table.RowStyles.Add(new RowStyle(SizeType.Percent, height));
                    }


                    var label = new Label();
                    label.Click += label1_Click;
                    label.AutoSize = false;
                    label.Font = new Font("Webdings", 48);
                    label.Name = ("label" + row + col).ToString();
                    label.Text = 'с'.ToString();
                    label.Dock = DockStyle.Fill;
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    table.Controls.Add(label, col, row);

                }

            }
            Controls.Add(panel);
            panel.Controls.Add(table);
            foreach (Control control in table.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons1.Count);
                    iconLabel.Text = icons1[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons1.RemoveAt(randomNumber);
                }
            }
            table.AutoSize = true;
            table.Dock = DockStyle.Fill;
        }


            private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
            {

            }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            timer1.Stop();
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // Reset firstClicked and secondClicked 
            // so the next time a label is
            // clicked, the program knows it's the first click
            firstClicked = null;
            secondClicked = null;
        }
    }
}
