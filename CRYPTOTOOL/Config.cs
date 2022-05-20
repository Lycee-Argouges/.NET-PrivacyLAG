using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRYPTOTOOL
{
    public partial class Config : Form
    {
        public string domainePath = "U:\\PrivacyTool-LAG\\CRYPTOTOOL\\";
        //public string domainePath = "U:\\" + Environment.UserName + "\\Mes documents\\GNUpg\\";

        public Config()
        {
            InitializeComponent();
            if (Program.loginCert != "")
            {
                TB_Login.Text = Program.loginCert;
                ProcessStartInfo pInfo = new ProcessStartInfo();
                pInfo.FileName = "C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe";
                pInfo.Arguments = "-k";
                pInfo.UseShellExecute = false;
                pInfo.RedirectStandardOutput = true;
                pInfo.CreateNoWindow = true;
                pInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Process p = Process.Start(pInfo);
                string standard_output = p.StandardOutput.ReadToEnd();
                string[] subs = standard_output.Split('<');
                int i;
                for (i = 0; i < subs.Length - 1; i++)
                {
                    if (subs[i].IndexOf(Program.loginCert) >= 0)
                    {
                        string[] sube = subs[i + 1].Split('>');
                        TB_Email.Text = sube[0];
                        Program.emailCert = sube[0];
                    }
                }
                //pBis.WaitForInputIdle();
                p.WaitForExit();
            }
        }

        private void Bt_Save_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe"))
            {
                if (TB_Login.Text != "" & TB_Email.Text != "") 
                {
                    if (System.IO.File.Exists(domainePath + TB_Login.Text + ".pem") == true)
                    {
                        MessageBox.Show("Cette clée existe déjà. Veuillez entrer un autre nom de clée avant de continuer.", "DATA PRIVACY ARGOUGES", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        /* Création de la clée */
                        ProcessStartInfo pInfo = new ProcessStartInfo();
                        pInfo.FileName = "C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe";
                        pInfo.Arguments = "--quick-generate-key \"" + TB_Login.Text + " <" + TB_Email.Text + ">\" rsa4096 encr never";
                        pInfo.CreateNoWindow = true;
                        pInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        Process p = Process.Start(pInfo);
                        //p.WaitForInputIdle();
                        p.WaitForExit();

                        /* Export de la clée */
                        ProcessStartInfo pInfoBis = new ProcessStartInfo();
                        pInfoBis.FileName = "C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe";
                        pInfoBis.Arguments = "-o \"" + domainePath + TB_Login.Text + ".pem\" --export-secret-key -a \"" + TB_Login.Text + "\"";
                        pInfoBis.CreateNoWindow = true;
                        pInfoBis.WindowStyle = ProcessWindowStyle.Hidden;
                        Process pBis = Process.Start(pInfoBis);
                        //pBis.WaitForInputIdle();
                        pBis.WaitForExit();

                        /* Export de la signature de la clée */
                        ProcessStartInfo pInfoTer = new ProcessStartInfo();
                        pInfoTer.FileName = "C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe";
                        pInfoTer.Arguments = "--export-ownertrust";
                        pInfoTer.UseShellExecute = false;
                        pInfoTer.RedirectStandardOutput = true;
                        pInfoTer.CreateNoWindow = true;
                        pInfoTer.WindowStyle = ProcessWindowStyle.Hidden;
                        Process pTer = Process.Start(pInfoTer);
                        string output = pTer.StandardOutput.ReadToEnd();
                        File.WriteAllText(domainePath + TB_Login.Text + ".txt", output,Encoding.ASCII);
                        //pBis.WaitForInputIdle();
                        pTer.WaitForExit();
                        MessageBox.Show("La clée a été correctement crée. Elle est enregistrée à l'emplacement " + domainePath + TB_Login.Text, "DATA PRIVACY ARGOUGES", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Program.loginCert = TB_Login.Text;
                        this.Close();
                        //Launch_AppBox();
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez entrer un nom de clée et un email académique avant de continuer.", "DATA PRIVACY ARGOUGES", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            } 
            else
            {
                MessageBox.Show("Veuillez installer l'application GnuPG avant de continuer.", "DATA PRIVACY ARGOUGES", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Bt_Quit_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            Program.loginCert = "";
            this.Close();
        }

        private void Bt_Clear_Click_1(object sender, EventArgs e)
        {
            TB_Login.Text = "";
            TB_Email.Text = "";
        }

        private void Bt_Change_Click(object sender, EventArgs e)
        {
            if (TB_Login.Text != "")
            {
                /* Changement du password de la clée */
                ProcessStartInfo pInfo = new ProcessStartInfo();
                pInfo.FileName = "C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe";
                pInfo.Arguments = "--change-passphrase \"" + TB_Login.Text + "\"";
                pInfo.CreateNoWindow = true;
                pInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Process p = Process.Start(pInfo);
                //p.WaitForInputIdle();
                p.WaitForExit();
            } else
            {
                MessageBox.Show("Veuillez entrer une nom de clée valide avant de continuer.", "DATA PRIVACY ARGOUGES", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Bt_help_Click(object sender, EventArgs e)
        {
            Help form = new Help();
            form.ShowDialog();
        }
    }
}
