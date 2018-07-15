namespace Arqui_MIPS
{
    partial class Simulacion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lblReloj = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblHilillo = new System.Windows.Forms.Label();
            this.lblModoLento = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Reloj";
            // 
            // lblReloj
            // 
            this.lblReloj.Location = new System.Drawing.Point(181, 60);
            this.lblReloj.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReloj.Name = "lblReloj";
            this.lblReloj.Size = new System.Drawing.Size(312, 20);
            this.lblReloj.TabIndex = 1;
            this.lblReloj.Text = "000";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 99);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Hilillo en ejecución";
            // 
            // lblHilillo
            // 
            this.lblHilillo.Location = new System.Drawing.Point(181, 99);
            this.lblHilillo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHilillo.Name = "lblHilillo";
            this.lblHilillo.Size = new System.Drawing.Size(312, 20);
            this.lblHilillo.TabIndex = 3;
            this.lblHilillo.Text = "Hilillo 00";
            // 
            // lblModoLento
            // 
            this.lblModoLento.Location = new System.Drawing.Point(3, 144);
            this.lblModoLento.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblModoLento.Name = "lblModoLento";
            this.lblModoLento.Size = new System.Drawing.Size(503, 20);
            this.lblModoLento.TabIndex = 4;
            this.lblModoLento.Text = "Presione <Espacio> para avanzar 20 ciclos de reloj";
            this.lblModoLento.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblModoLento.Visible = false;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(503, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Simulación en ejecución";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(393, 94);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 30);
            this.button1.TabIndex = 6;
            this.button1.Text = "Comenzar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Simulacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 177);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblModoLento);
            this.Controls.Add(this.lblHilillo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblReloj);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Simulacion";
            this.Text = "Simulacion";
            this.Load += new System.EventHandler(this.Simulacion_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Simulacion_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblReloj;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblHilillo;
        private System.Windows.Forms.Label lblModoLento;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
    }
}