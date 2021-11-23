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
        private delegate void SafeCallDelegate2();
        public delegate void MyEventHandler();

        private Tuple<int, int> seedToBeDelete;
        private Tuple<int, int> seedToBeReplace;

        public event MyEventHandler MyEvent;

        // Thread pour le refresh
        thread.Thread threadRefresh;

        //Pigeon environment = new Pigeon();
        Environment environment = new Environment();

        bool refreshDisplay = false;

        // Liste des pigeons associés à une image
        List<Tuple<int, PictureBox>> pigeonsAndImg = new List<Tuple<int, PictureBox>>();
        List<PictureBox> seedImg = new List<PictureBox>();
        // Liste des graines associés à une image
        List<PictureBox> seedsImg = new List<PictureBox>();

        // Graph
        Graphics graph;

        public Form1()
        {
            InitializeComponent();
            environment.UpdateEventHandler += deleteSeedImg;
            environment.ReplaceSeedEvent += rottenSeedImg;
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
            for (int i = 0; i < pigeons.Count; i++)
            {
                PictureBox a_pigeon = new PictureBox();
                a_pigeon.Image = (Image)GraineDeChourbe.Properties.Resources.pigeon_1;
                int x = pigeons[i].xpos;
                int y = pigeons[i].ypos;
                a_pigeon.Location = new Point(x, y);
                a_pigeon.BackColor = Color.Khaki;

                a_pigeon.SizeMode = PictureBoxSizeMode.Zoom;
                pigeonsAndImg.Add(Tuple.Create(i, a_pigeon));
                this.Controls.Add(a_pigeon);
            }
            // Lancement d'un thread pour un pigeon
            threadRefresh = new thread.Thread(new thread.ThreadStart(refresh));
            start();
        }

        private void start()
        {
            // Démarrage du thread de refresh
            if (!threadRefresh.IsAlive)
            {
                refreshDisplay = true;
                threadRefresh.Start();
            }
            // Démarrage du thread du pigeon
            if (!environment.pigeon_alive)
            {
                environment.pigeon_alive = true;
                environment.threadPigeon1.Start();
                environment.threadPigeon2.Start();
                environment.threadPigeon3.Start();
            }
        }

        private void refresh()
        {
            while (refreshDisplay)
            {
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
            Random random = new Random();
            int num = random.Next(100);

            bool fear = false;
            if (num > 75)
            {
                fear = true;
            }
            if (!fear)
            {

                MouseEventArgs e1 = (MouseEventArgs)e;

                int seedX = e1.Location.X;
                int seedY = e1.Location.Y;

                // Mise à jour du visuel
                PictureBox a_seed = new PictureBox();
                a_seed.Image = (Image)GraineDeChourbe.Properties.Resources.graines;
                a_seed.SizeMode = PictureBoxSizeMode.Zoom;
                var newSeedX = seedX - a_seed.Size.Width / 2;
                var newSeedY = seedY - a_seed.Size.Width / 2;
                a_seed.Location = new Point(newSeedX, newSeedY);
                //a_seed.Location = new Point(seedX - a_seed.Size.Width/2, seedY - a_seed.Size.Height / 2);

                this.Controls.Add(a_seed);
                a_seed.BringToFront();
                seedImg.Add(a_seed);

                // seedsImg.Add()
                this.environment.addSeed(a_seed.Location.X, a_seed.Location.Y);
                seedsImg.Add(a_seed);
                environment.fearedPigeon = false;
            }
            else
            {
                environment.fearedPigeon = true;
            }
        }

        private void deleteSeedImg(object sender, Environment.UpdateEventArgs e)
        {

            //MessageBox.Show("Message receive : " + e.y);
            seedToBeDelete = Tuple.Create(e.x, e.y);
            deleteFromIndex();
        }

        private void rottenSeedImg(object sender, Environment.UpdateEventArgs e)
        {
            seedToBeReplace = Tuple.Create(e.x, e.y);
            replaceFromIndex();
        }

        public void replaceFromIndex()
        {
            for (int i = 0; i < seedsImg.Count; i++)
            {
                if ((seedsImg[i].Location.X == seedToBeReplace.Item1) && (seedsImg[i].Location.Y == seedToBeReplace.Item2))
                    if (seedsImg[i].InvokeRequired)
                    {
                        SafeCallDelegate2 s = new SafeCallDelegate2(replaceFromIndex);
                        seedsImg[i].Invoke(s, new object[] { });
                    }
                    else
                    {
                        seedsImg[i].Image = (Image)GraineDeChourbe.Properties.Resources.graines_rotten;
                    }
            }
        }


        public void deleteFromIndex()
        {
            for (int i = 0; i < seedsImg.Count; i++)
            {
                //if (((seedsImg[j].Location.X == seeds[i].get_xpos()) && (seedsImg[j].Location.Y == seeds[i].get_ypos())))
                if ((seedsImg[i].Location.X == seedToBeDelete.Item1) && (seedsImg[i].Location.Y == seedToBeDelete.Item2))
                    if (seedsImg[i].InvokeRequired)
                    {
                        SafeCallDelegate2 s = new SafeCallDelegate2(deleteFromIndex);
                        seedsImg[i].Invoke(s, new object[] { });
                    }
                    else
                    {
                        seedsImg[i].Dispose();
                    }
            }

        }
    }
}