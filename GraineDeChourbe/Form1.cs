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

        // Liste des pigeons associés à une image
        List<Tuple<int, PictureBox>> pigeonsAndImg = new List<Tuple<int, PictureBox>>();

        Graphics graph;

        // Creation d'un pictureBox qui va permettre d'afficher nos objets
        //private PictureBox pigeon_1;
        //    PictureBox pb1 = new PictureBox();

        public Form1()
        {
            InitializeComponent();
            //pigeon_1 = new PictureBox();
            //pigeon_1.Image = (Image)GraineDeChourbe.Properties.Resources.pigeon_1;
            //pigeon_1.Location = new Point(50, 50);
            //pb1.ClientSize = new Size(20, 20);
            //pigeon_1.SizeMode = PictureBoxSizeMode.Zoom;
            //pb1.SizeMode = PictureBoxSizeMode.AutoSize;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //pigeon_1 = new PictureBox();
            //pigeon_1.Image = (Image)GraineDeChourbe.Properties.Resources.pigeon_1;
            //pigeon_1.Location = new Point(50, 50);
            ////pb1.ClientSize = new Size(20, 20);
            //pigeon_1.SizeMode = PictureBoxSizeMode.Zoom;
            ////pb1.SizeMode = PictureBoxSizeMode.AutoSize;
            //this.Controls.Add(pigeon_1);
        }

        private void launch_Click(object sender, EventArgs e)
        {
            graph = this.CreateGraphics();
            Pen BlackPen = new Pen(Color.Black, 3);
            graph.DrawRectangle(BlackPen, 1, 1, 700, 400);


            // Création de la grille visuelle


            environment.initialise();
            List<Pigeon> pigeons = environment.pigeons;
            //List<Tuple<Pigeon, PictureBox>> pigeonsAndImg = new List<Tuple<Pigeon, PictureBox>>();
            Debug.WriteLine("Nombre de pigeons : " + pigeons.Count);

            for (int i = 0; i < pigeons.Count; i++)
            {
                PictureBox a_pigeon = new PictureBox();
                a_pigeon.Image = (Image)GraineDeChourbe.Properties.Resources.pigeon_1;
                int x = pigeons[i].xpos;
                int y = pigeons[i].ypos;
                a_pigeon.Location = new Point(x, y);
                a_pigeon.SizeMode = PictureBoxSizeMode.Zoom;
                pigeonsAndImg.Add(Tuple.Create(i,a_pigeon));
                this.Controls.Add(a_pigeon);
            }
        }

        private void start_Click(object sender, EventArgs e)
        {
            environment.run();
            
            refresh();

        }

        private void refresh()
        {
            List<Pigeon> pigeons = environment.pigeons;
            //List<Tuple<Pigeon, PictureBox>> pigeonsAndImg = new List<Tuple<Pigeon, PictureBox>>();
            Debug.WriteLine("Nombre de pigeons : " + pigeons.Count);

            for (int i = 0; i < pigeons.Count; i++)
            {
                pigeonsAndImg[i].Item2.Location = new Point(pigeons[i].xpos, pigeons[i].ypos);
            }
        }

        private void pause_Click(object sender, EventArgs e)
        {

        }
    }
}
