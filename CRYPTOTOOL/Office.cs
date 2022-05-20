using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRYPTOTOOL
{
    public partial class Office : Form
    {
        public string domainePath = "U:\\PrivacyTool-LAG\\CRYPTOTOOL\\";
        //public string domainePath = "U:\\" + Environment.UserName + "\\Mes documents\\GNUpg\\";
        public Office()
        {
            if (System.IO.Directory.Exists(domainePath) == false)
            {
                if (System.IO.Directory.Exists("U:\\"))
                {
                    System.IO.Directory.CreateDirectory(domainePath);
                } else
                {
                    MessageBox.Show("Cette application a été créée pour être utilisé dans l'établissement scolaire.", "DATA PRIVACY ARGOUGES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            InitializeComponent();
            this.Height = 155;
            if (System.IO.File.Exists("C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe"))
            {
                Check_Identity();
            } else
            {
                MessageBox.Show("Installez le logiciel GNUpg avant de continuer.", "DATA PRIVACY ARGOUGES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                Application.Exit();
            }
            Program.loginCert = "";
            this.SMI_Create.MouseEnter += SMI_Create_MouseEnter;
            this.SMI_Import.MouseEnter += SMI_Import_MouseEnter;
            this.SMI_Select.MouseEnter += SMI_Select_MouseEnter;
            this.SMI_Create.MouseLeave += SMI_Create_MouseLeave;
            this.SMI_Import.MouseLeave += SMI_Import_MouseLeave;
            this.SMI_Select.MouseLeave += SMI_Select_MouseLeave;
        }

        private void Check_Identity()
        {
            /* Liste des identités disponibles sur le poste */
            ProcessStartInfo pInfo = new ProcessStartInfo();
            pInfo.FileName = "C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe";
            pInfo.Arguments = "-k";
            pInfo.UseShellExecute = false;
            pInfo.RedirectStandardOutput = true;
            pInfo.CreateNoWindow = true;
            pInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(pInfo);
            string output = p.StandardOutput.ReadToEnd();
            TB_Informations.Text = output;
            //pBis.WaitForInputIdle();
            p.WaitForExit();
            /* Liste les identités disponibles sur le réseau */
            CB_Select.Items.Clear();
            var filesCert = System.IO.Directory.EnumerateFiles(domainePath, "*.pem");
            foreach (string files in filesCert)
            {
                string[] subs = files.Split('\\');
                string filePath = subs[subs.Length - 1];
                string identity = filePath.Substring(0, filePath.Length - 4);
                if (output.IndexOf(identity) >= 0)
                {
                    CB_Select.Items.Add(identity);
                }
            }
            
        }

        private void SMI_Create_MouseEnter(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            SMI_Create.Image = CRYPTOTOOL.Properties.Resources.Bt_Creer_TF_flou;
        }
        private void SMI_Create_MouseLeave(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            SMI_Create.Image = CRYPTOTOOL.Properties.Resources.Bt_Creer_130px;
        }

        private void SMI_Select_MouseEnter(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            SMI_Select.Image = CRYPTOTOOL.Properties.Resources.Bt_Selectionner_TF_flou;
        }
        private void SMI_Select_MouseLeave(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            SMI_Select.Image = CRYPTOTOOL.Properties.Resources.Bt_Selectionner_130px;
        }

        private void SMI_Import_MouseEnter(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            SMI_Import.Image = CRYPTOTOOL.Properties.Resources.Bt_Importer_TF_flou;
        }
        private void SMI_Import_MouseLeave(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            SMI_Import.Image = CRYPTOTOOL.Properties.Resources.Bt_Importer_130px;
        }


        private void CB_Select_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* Liste des identités disponibles */
            ProcessStartInfo pInfo = new ProcessStartInfo();
            pInfo.FileName = "C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe";
            pInfo.Arguments = "--fingerprint " + CB_Select.SelectedItem.ToString();
            pInfo.UseShellExecute = false;
            pInfo.RedirectStandardOutput = true;
            pInfo.CreateNoWindow = true;
            pInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(pInfo);
            string output = p.StandardOutput.ReadToEnd();
            TB_Informations.Text = output;
            //pBis.WaitForInputIdle();
            p.WaitForExit();
        }

        private void SMI_Select_Click(object sender, EventArgs e)
        {
            if(this.Height == 155)
            {
                this.Height = 395;
            }
            else if (this.Height == 395)
            {
                this.Height = 155;
            }
        }


        private void SMI_Import_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = domainePath;
                openFileDialog.Filter = "pem files (*.pem)|*.pem|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    string extension = filePath.Substring(0, filePath.Length - 3);
                    string fileTrust = extension + "txt";
                    /* Import de la clée */
                    ProcessStartInfo pInfo = new ProcessStartInfo();
                    pInfo.FileName = "C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe";
                    pInfo.Arguments = "--import \"" + filePath + "\"";
                    pInfo.CreateNoWindow = true;
                    pInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    Process p = Process.Start(pInfo);
                    //p.WaitForInputIdle();
                    p.WaitForExit();

                    /* Signature de la clée */
                    ProcessStartInfo pInfoTer = new ProcessStartInfo();
                    pInfoTer.FileName = "C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe";
                    pInfoTer.Arguments = "--import-ownertrust \"" + fileTrust + "\"";
                    pInfoTer.CreateNoWindow = true;
                    pInfoTer.WindowStyle = ProcessWindowStyle.Hidden;
                    Process pTer = Process.Start(pInfoTer);
                    //pBis.WaitForInputIdle();
                    pTer.WaitForExit();

                    /* Certification de la clée */
                    ProcessStartInfo pInfoBis = new ProcessStartInfo();
                    pInfoBis.FileName = "C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe";
                    pInfoBis.Arguments = "--update-trustdb";
                    pInfoBis.CreateNoWindow = true;
                    pInfoBis.WindowStyle = ProcessWindowStyle.Hidden;
                    Process pBis = Process.Start(pInfoBis);
                    //pBis.WaitForInputIdle();
                    pBis.WaitForExit();

                    //Notification à l'utilisateur
                    MessageBox.Show("La clée a été correctement importée.", "DATA PRIVACY ARGOUGES", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string[] subs = filePath.Split('\\');
                    string login = subs[subs.Length - 1].Remove(subs[subs.Length - 1].Length - 4);
                    Check_Identity();
                    Launch_AppBox(login);
                }
            }
        }

        

        private void Launch_AppBox(string login)
        {
            Program.loginCert = login;
            DragDrop form = new DragDrop();
            this.Height = 155;
            this.Hide();
            form.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - form.Width, Screen.PrimaryScreen.WorkingArea.Height - form.Height);
            form.ShowDialog();
            this.Show();
            Program.loginCert = "";
        }

        private void SMI_Create_Click(object sender, EventArgs e)
        {
            if (CB_Select.Items.Count > 0 && CB_Select.SelectedIndex > -1)
            {
                Program.loginCert = CB_Select.SelectedItem.ToString();
            }
            Config form = new Config();
            this.Hide();
            form.ShowDialog();
            this.Show();
            Check_Identity();
            if(Program.loginCert != "")
            {
                Launch_AppBox(Program.loginCert);
            }
        }

        private void Bt_Continuer_Click(object sender, EventArgs e)
        {
            if (CB_Select.Items.Count > 0)
            {
                if (CB_Select.SelectedIndex > -1)
                {
                    Launch_AppBox(CB_Select.SelectedItem.ToString());
                }
                else
                {
                    MessageBox.Show("Choisissez une identité avant de continuer.", "DATA PRIVACY ARGOUGES", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Créez une identité avant de continuer.", "DATA PRIVACY ARGOUGES", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.freepik.com/vectors/software-icon");
        }

        private void Bt_help_Click(object sender, EventArgs e)
        {
            Help form = new Help();
            form.ShowDialog();
        }
    }

}
