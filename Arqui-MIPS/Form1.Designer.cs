namespace Arqui_MIPS
{
    partial class FrmPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtQuantum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbOpenedFile = new System.Windows.Forms.RichTextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.rdbRapida = new System.Windows.Forms.RadioButton();
            this.rdbLenta = new System.Windows.Forms.RadioButton();
            this.lvSelectedFiles = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtQuantum
            // 
            resources.ApplyResources(this.txtQuantum, "txtQuantum");
            this.txtQuantum.Name = "txtQuantum";
            this.txtQuantum.TextChanged += new System.EventHandler(this.txtQuantum_TextChanged);
            this.txtQuantum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQuantum_KeyPress);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // rtbOpenedFile
            // 
            resources.ApplyResources(this.rtbOpenedFile, "rtbOpenedFile");
            this.rtbOpenedFile.Name = "rtbOpenedFile";
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // rdbRapida
            // 
            resources.ApplyResources(this.rdbRapida, "rdbRapida");
            this.rdbRapida.Checked = true;
            this.rdbRapida.Name = "rdbRapida";
            this.rdbRapida.TabStop = true;
            this.rdbRapida.UseVisualStyleBackColor = true;
            // 
            // rdbLenta
            // 
            resources.ApplyResources(this.rdbLenta, "rdbLenta");
            this.rdbLenta.Name = "rdbLenta";
            this.rdbLenta.UseVisualStyleBackColor = true;
            // 
            // lvSelectedFiles
            // 
            resources.ApplyResources(this.lvSelectedFiles, "lvSelectedFiles");
            this.lvSelectedFiles.Name = "lvSelectedFiles";
            this.lvSelectedFiles.UseCompatibleStateImageBehavior = false;
            this.lvSelectedFiles.View = System.Windows.Forms.View.List;
            // 
            // FrmPrincipal
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvSelectedFiles);
            this.Controls.Add(this.rdbLenta);
            this.Controls.Add(this.rdbRapida);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.rtbOpenedFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtQuantum);
            this.Controls.Add(this.btnBrowse);
            this.Name = "FrmPrincipal";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtQuantum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtbOpenedFile;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdbRapida;
        private System.Windows.Forms.RadioButton rdbLenta;
        private System.Windows.Forms.ListView lvSelectedFiles;
    }
}

