using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRYPTOTOOL
{
    public partial class DragDrop : Form
    {
        public DragDrop()
        {
            InitializeComponent();
            this.LB_DD.DragDrop += new
                System.Windows.Forms.DragEventHandler(this.LB_DD_DragDrop);
            this.LB_DD.DragEnter += new
                 System.Windows.Forms.DragEventHandler(this.LB_DD_DragEnter);
        }

        private void LB_DD_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void LB_DD_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int i;
            //string pathFile = "";
            for (i = 0; i < s.Length; i++)
                LB_DD.Items.Add(s[i]);
            //pathFile += s[i];
            LaunchEncryption();
        }

        private void LaunchEncryption()
        {
            foreach (string item in LB_DD.Items)
            {
                if (System.IO.File.Exists(item))
                    Encryption(item);
                else if (System.IO.Directory.Exists(item))
                {
                    ZipFile.CreateFromDirectory(item, item + ".zip");
                    Encryption(item + ".zip");
                    System.IO.Directory.Delete(item,true);
                }
            }
            LB_DD.Items.Clear();
        }

        private void Encryption(string pathFile)
        {
            //On récupère l'extention du fichier
            string extension = pathFile.Substring(pathFile.Length - 3);
            if (extension == "gpg")
            {
                string[] subs = pathFile.Split('\\');
                int j;
                string pathDest = "";
                for (j = 0; j < subs.Length - 1; j++)
                    pathDest += subs[j] + "\\";
                string fileDest = subs[subs.Length - 1].Remove(subs[subs.Length - 1].Length - 4);
                //On décrypt le fichier
                ProcessStartInfo pInfo = new ProcessStartInfo();
                pInfo.FileName = "C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe";
                pInfo.Arguments = "-q -o \"" + pathDest + fileDest + "\" --decrypt \"" + pathFile + "\"";
                pInfo.CreateNoWindow = true;
                pInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Process p = Process.Start(pInfo);
                //p.WaitForInputIdle();
                p.WaitForExit();
                if (System.IO.File.Exists(pathDest + fileDest))
                {
                    System.IO.File.Delete(pathFile);
                }
                if (fileDest.Substring(fileDest.Length - 4) == ".zip")
                {
                    ZipFile.ExtractToDirectory(pathDest + fileDest, pathDest + fileDest.Substring(0,fileDest.Length - 4));
                    System.IO.File.Delete(pathDest + fileDest);
                }
            }
            else
            {
                //On encrypt le fichier
                ProcessStartInfo pInfo = new ProcessStartInfo();
                pInfo.FileName = "C:\\Program Files (x86)\\GnuPG\\bin\\gpg.exe";
                pInfo.Arguments = "-q --encrypt -r \"" + Program.loginCert + "\" \"" + pathFile + "\"";
                pInfo.CreateNoWindow = true;
                pInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Process p = Process.Start(pInfo);
                //p.WaitForInputIdle();
                p.WaitForExit();
                if (System.IO.File.Exists(pathFile + ".gpg"))
                {
                    System.IO.File.Delete(pathFile);
                }
            }
        }

    }
}
