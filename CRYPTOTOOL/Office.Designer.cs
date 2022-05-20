
namespace CRYPTOTOOL
{
    partial class Office
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Office));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.SMI_Create = new System.Windows.Forms.ToolStripMenuItem();
            this.SMI_Import = new System.Windows.Forms.ToolStripMenuItem();
            this.SMI_Select = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.CB_Select = new System.Windows.Forms.ComboBox();
            this.TB_Informations = new System.Windows.Forms.TextBox();
            this.Bt_Continuer = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.Bt_help = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SMI_Create,
            this.SMI_Import,
            this.SMI_Select});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(401, 116);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // SMI_Create
            // 
            this.SMI_Create.AutoSize = false;
            this.SMI_Create.BackColor = System.Drawing.Color.Transparent;
            this.SMI_Create.BackgroundImage = global::CRYPTOTOOL.Properties.Resources.Bt_Creer_130px;
            this.SMI_Create.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.SMI_Create.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.SMI_Create.Name = "SMI_Create";
            this.SMI_Create.Padding = new System.Windows.Forms.Padding(3);
            this.SMI_Create.Size = new System.Drawing.Size(130, 110);
            this.SMI_Create.Click += new System.EventHandler(this.SMI_Create_Click);
            // 
            // SMI_Import
            // 
            this.SMI_Import.AutoSize = false;
            this.SMI_Import.BackColor = System.Drawing.Color.Transparent;
            this.SMI_Import.BackgroundImage = global::CRYPTOTOOL.Properties.Resources.Bt_Importer_130px;
            this.SMI_Import.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.SMI_Import.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.SMI_Import.Name = "SMI_Import";
            this.SMI_Import.Padding = new System.Windows.Forms.Padding(3);
            this.SMI_Import.Size = new System.Drawing.Size(130, 110);
            this.SMI_Import.Click += new System.EventHandler(this.SMI_Import_Click);
            // 
            // SMI_Select
            // 
            this.SMI_Select.AutoSize = false;
            this.SMI_Select.BackColor = System.Drawing.Color.Transparent;
            this.SMI_Select.BackgroundImage = global::CRYPTOTOOL.Properties.Resources.Bt_Selectionner_130px;
            this.SMI_Select.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.SMI_Select.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.SMI_Select.Name = "SMI_Select";
            this.SMI_Select.Padding = new System.Windows.Forms.Padding(3);
            this.SMI_Select.Size = new System.Drawing.Size(130, 110);
            this.SMI_Select.Click += new System.EventHandler(this.SMI_Select_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Selectionner";
            // 
            // CB_Select
            // 
            this.CB_Select.FormattingEnabled = true;
            this.CB_Select.Location = new System.Drawing.Point(146, 130);
            this.CB_Select.Name = "CB_Select";
            this.CB_Select.Size = new System.Drawing.Size(223, 21);
            this.CB_Select.TabIndex = 2;
            this.CB_Select.SelectedIndexChanged += new System.EventHandler(this.CB_Select_SelectedIndexChanged);
            // 
            // TB_Informations
            // 
            this.TB_Informations.Location = new System.Drawing.Point(32, 166);
            this.TB_Informations.Multiline = true;
            this.TB_Informations.Name = "TB_Informations";
            this.TB_Informations.ReadOnly = true;
            this.TB_Informations.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TB_Informations.Size = new System.Drawing.Size(337, 130);
            this.TB_Informations.TabIndex = 3;
            // 
            // Bt_Continuer
            // 
            this.Bt_Continuer.Location = new System.Drawing.Point(294, 305);
            this.Bt_Continuer.Name = "Bt_Continuer";
            this.Bt_Continuer.Size = new System.Drawing.Size(75, 23);
            this.Bt_Continuer.TabIndex = 4;
            this.Bt_Continuer.Text = "Continuer";
            this.Bt_Continuer.UseVisualStyleBackColor = true;
            this.Bt_Continuer.Click += new System.EventHandler(this.Bt_Continuer_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(-3, 343);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(311, 13);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Software icon vector created by macrovector - www.freepik.com";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // Bt_help
            // 
            this.Bt_help.BackColor = System.Drawing.SystemColors.Control;
            this.Bt_help.ForeColor = System.Drawing.Color.Red;
            this.Bt_help.Location = new System.Drawing.Point(32, 304);
            this.Bt_help.Name = "Bt_help";
            this.Bt_help.Size = new System.Drawing.Size(75, 23);
            this.Bt_help.TabIndex = 6;
            this.Bt_help.Text = "Aide";
            this.Bt_help.UseVisualStyleBackColor = false;
            this.Bt_help.Click += new System.EventHandler(this.Bt_help_Click);
            // 
            // Office
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(401, 356);
            this.Controls.Add(this.Bt_help);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.Bt_Continuer);
            this.Controls.Add(this.TB_Informations);
            this.Controls.Add(this.CB_Select);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Office";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DATA PRIVACY ARGOUGES";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem SMI_Create;
        private System.Windows.Forms.ToolStripMenuItem SMI_Import;
        private System.Windows.Forms.ToolStripMenuItem SMI_Select;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CB_Select;
        private System.Windows.Forms.TextBox TB_Informations;
        private System.Windows.Forms.Button Bt_Continuer;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button Bt_help;
    }
}