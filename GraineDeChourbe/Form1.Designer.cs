
namespace GraineDeChourbe
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.launch = new System.Windows.Forms.Button();
            this.start = new System.Windows.Forms.Button();
            this.pause = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // launch
            // 
            this.launch.Location = new System.Drawing.Point(664, 47);
            this.launch.Name = "launch";
            this.launch.Size = new System.Drawing.Size(75, 23);
            this.launch.TabIndex = 0;
            this.launch.Text = "launch";
            this.launch.UseVisualStyleBackColor = true;
            this.launch.Click += new System.EventHandler(this.launch_Click);
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(664, 76);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(75, 23);
            this.start.TabIndex = 1;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // pause
            // 
            this.pause.Location = new System.Drawing.Point(664, 105);
            this.pause.Name = "pause";
            this.pause.Size = new System.Drawing.Size(75, 23);
            this.pause.TabIndex = 2;
            this.pause.Text = "Pause";
            this.pause.UseVisualStyleBackColor = true;
            this.pause.Click += new System.EventHandler(this.pause_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pause);
            this.Controls.Add(this.start);
            this.Controls.Add(this.launch);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button launch;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button pause;
    }
}

