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
using thread = System.Threading;
using System.Threading.Tasks;

namespace GraineDeChourbe
{
    public partial class Form1 : Form
    {

        private delegate void SafeCallDelegate();
        public delegate void MyEventHandler();

        //
        public event MyEventHandler MyEvent;    

        // Thread pour le refresh
        thread.Thread threadRefresh;



        //Pigeon environment = new Pigeon();
        Environment environment = new Environment();

        bool refreshDisplay = false;

        // Liste des pigeons associés à une image
        List<Tuple<int, PictureBox>> pigeonsAndImg = new List<Tuple<int, PictureBox>>();

        // Liste des graines associés à une image
        //List<Tuple<Tuple<int,int>, PictureBox>> seedsAndImg = new List<Tuple<Tuple<int,int>, PictureBox>>();
        List<PictureBox> seedsImg = new List<PictureBox>();

        // Graph
        Graphics graph;

        public Form1()
        {
            InitializeComponent();

            environment.udpateSeeds += deleteSeedImg;


        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void launch_Click(object sender, EventArgs e)
        {


            graph = this.CreateGraphics();
            Pen BlackPen = new Pen(Color.Black, 3);
            graph.DrawRectangle(BlackPen, 1, 1, 700, 400);

            pictureBox1.SendToBack();//put it behinds


            // Création de la grille visuelle


            environment.initialise();
            List<Pigeon> pigeons = environment.pigeons;
            Debug.WriteLine("Nombre de pigeons : " + pigeons.Count);

            for (int i = 0; i < pigeons.Count; i++)
            {
                PictureBox a_pigeon = new PictureBox();
                a_pigeon.Image = (Image)GraineDeChourbe.Properties.Resources.pigeon_1;
                int x = pigeons[i].xpos;
                int y = pigeons[i].ypos;
                a_pigeon.Location = new Point(x, y);

                a_pigeon.SizeMode = PictureBoxSizeMode.Zoom;
                pigeonsAndImg.Add(Tuple.Create(i, a_pigeon));
                this.Controls.Add(a_pigeon);
            }
            // Lancement d'un thread pour un pigeon
            threadRefresh = new thread.Thread(new thread.ThreadStart(refresh));
        }

        private void start_Click(object sender, EventArgs e)
        {

            if (!threadRefresh.IsAlive)
            {
                refreshDisplay = true;
                threadRefresh.Start();
            }
        }

        private void refresh()
        {
            while (refreshDisplay) {
                move_item();
                thread.Thread.Sleep(50);
            }
            // Suppresion des graines manger
        }

        public void remove_Seed()
        {
            MessageBox.Show("REMOVED SEED");
        }

        private void move_item()
        {
            List<Pigeon> pigeons = environment.pigeons;
            
            // Debug.WriteLine("Nombre de pigeons : " + pigeons.Count);

            for (int i = 0; i < pigeons.Count; i++)
            {
                if (pigeonsAndImg[i].Item2.InvokeRequired)
                {
                    var d = new SafeCallDelegate(move_item);
                    pigeonsAndImg[i].Item2.Invoke(d, new object[] { });
                }
                else
                {
                    pigeonsAndImg[i].Item2.Location = new Point(pigeons[i].xpos, pigeons[i].ypos);
                    pigeonsAndImg[i].Item2.BringToFront();
                    pigeonsAndImg[i].Item2.BackColor = Color.Transparent;
                }
            }
        }

        private void pause_Click(object sender, EventArgs e)
        {
            refreshDisplay = false;
        }

        // Création d'une graine lors de l'appui 
        private void pictureBox1_Click(object sender, EventArgs e)
        {

            MouseEventArgs e1 = (MouseEventArgs)e;

            int seedX = e1.Location.X;
            int seedY = e1.Location.Y;

            // Mise à jour du visuel
            PictureBox a_seed = new PictureBox();
            a_seed.Image = (Image)GraineDeChourbe.Properties.Resources.graines;
            a_seed.SizeMode = PictureBoxSizeMode.Zoom;
            a_seed.Location = new Point(seedX - a_seed.Size.Width/2, seedY - a_seed.Size.Height / 2);
            this.Controls.Add(a_seed);
            a_seed.BringToFront();

            // seedsImg.Add()
            this.environment.addSeed(seedX, seedY);
            seedsImg.Add(a_seed);
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            environment.run();
            // refresh();
        }

        private void deleteSeedImg(object sender, EventArgs e)
        {
            MessageBox.Show("SEED DELETED");
        }

        public void testdeleteSeed()
        {
            environment.deleteSeed(0,0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            testdeleteSeed();
        }
    }
}
