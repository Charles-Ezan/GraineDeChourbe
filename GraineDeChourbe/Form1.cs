using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
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
        }

        private void launch_Click(object sender, EventArgs e)
        {
            Graphics graph = this.CreateGraphics();
            Pen BlackPen = new Pen(Color.Black, 3);
            graph.DrawRectangle(BlackPen, 1, 1, 700, 400);


            // Création de la grille visuelle


            environment.initialise();
            List<Pigeon> pigeons = environment.pigeons;
            List<Tuple<Pigeon, PictureBox>> pigeonsAndImg = new List<Tuple<Pigeon, PictureBox>>();
            Debug.WriteLine("Nombre de pigeons : " + pigeons.Count);

            for (int i = 0; i < pigeons.Count; i++)
            {
                Point a_position = new Point(pigeons[i].get_xpos(), pigeons[i].get_ypos());
                graph.DrawImage(pigeons[i].img, a_position);
            }
        }


        private void start_Click(object sender, EventArgs e)
        {
            environment.run();
        }
    }
}
