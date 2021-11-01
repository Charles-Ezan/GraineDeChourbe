using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System;

namespace GraineDeChourbe
{
    public partial class Form1 : Form
    {
        //Pigeon environment = new Pigeon();

        Environment environment = new Environment();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PictureBox pb1 = new PictureBox();
            pb1.ImageLocation = "../SamuderaJayaMotor.png";
            pb1.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        private void launch_Click(object sender, EventArgs e)
        {
            Graphics graph = this.CreateGraphics();
            Pen BlackPen = new Pen(Color.Black, 3);
            graph.DrawRectangle(BlackPen, 1, 1, 700, 400);
            launch.Enabled = false;
        }

        
        private void start_Click(object sender, EventArgs e)
        {
            List<Pigeon> pigeons = environment.pigeons;
            List<Tuple<Pigeon, PictureBox>> pigeonsAndImg = new List<Tuple<Pigeon, PictureBox>>();

            for(int i=0; i<pigeons.Count ; i++)
            {
                PictureBox pb1 = new PictureBox();
                pb1.ImageLocation = "./Ressources/pigeon_" + i.ToString() +".png";
                pb1.SizeMode = PictureBoxSizeMode.AutoSize;
                pb1.Location = new Point(pigeons[i].)
                pigeonsAndImg.Add(Tuple.Create(pigeons[i],pb1))
            }
            environment.initialise();
            environment.run();
        }
    }
}
