using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Diagnostics;

namespace GraineDeChourbe
{
    class Environment
    {
        public List<Pigeon> pigeons = new List<Pigeon>();
        public List<Graine> graines = new List<Graine>();

        int widthEnv = 10;
        int heightEnv = 10;

        // Créer les pigeons
        public void initialise()
        {
            Debug.WriteLine("INITIALISATION ! ");
            for(int i=1; i<2 ; i++)
            {
                addPigeon(i * 1, i * 1, i, GraineDeChourbe.Properties.Resources.pigeon_1);
            }
        }

        // Dessiner l'environnement initiale

        // Ajout de graines dans l'environnement
        public void addSeed(int newX, int newY, bool isRotten)
        {
            //Graine newSeed = new Graine(newX, newY, isRotten);
            //graines.Add(newSeed);
        }

        // Ajout de graines dans l'environnement
        public void addPigeon(int newX, int newY, int newIndex, Image newImg)
        {

            Pigeon newPigeon = new Pigeon(newX, newY, newIndex, newImg);
            pigeons.Add(newPigeon);
        }

        // Mise à jour de l'affichage
        public void refresh()
        {

        }

        // Run environment
        // fais vivre les pigeons
        // Fais apparaitre des graines
        public void run()
        {
            Debug.WriteLine("RUN !");
            // Faire bouger les pigeons
            foreach (var pigeon in pigeons)
            {
                pigeon.run("food", 3);
            }
        }
    }
}
