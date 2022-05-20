using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;
using System.Threading;

namespace SCANROOT
{
    public partial class RunApp : Form
    {
        //variables globales pour la recherche
        private string dateFile;
        private string fileName;
        private string hashCode;
        private Boolean flagError;
        private Boolean isSilent = false;

        public RunApp(string optionalstr = "through")
        {
            if (optionalstr =="through")
            {
                InitializeComponent();
            }
            else if (optionalstr == "task")
            {
                InitializeComponent();
                isSilent = true;
                this.Hide();
                Thread t = new Thread(new ThreadStart(CheckIntegrity));
                t.IsBackground = true;
                t.Start();
            }
        }

        private void Bt_Check_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(CheckIntegrity));
            t.IsBackground = true;
            t.Start();
        }

        private void CheckIntegrity()
        {
            using (StreamWriter writetext = new StreamWriter("checksumSHA512.log", true))
            {
                writetext.WriteLine(DateTime.Now + "....: Début du traitement.");
            }
            if (System.IO.File.Exists("error.log"))
            {
                System.IO.File.Delete("error.log");
            }
            flagError = false;
            /*if(System.IO.File.Exists("CheckFsum.mdf") & System.IO.File.Exists("CheckFsum_log.ldf"))
            {
                System.IO.File.Copy("CheckFsum.mdf", "C:\\Temp\\CheckFsum.mdf",true);
                System.IO.File.Copy("CheckFsum_log.ldf", "C:\\Temp\\CheckFsum_log.ldf", true);
            }*/
            var files = GetAllAccessibleFiles(@"P:\");
            /*foreach(string getFile in System.IO.Directory.GetFiles("P:\\", "*.*", SearchOption.AllDirectories))
            {
                MessageBox.Show(getFile);
            }*/
            foreach (string filePath in files)
            {
                try
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        SearchFile(filePath);
                        if (this.hashCode == ".")
                        {
                            DateTime fileDate = System.IO.File.GetCreationTime(filePath);
                            string hashCodeCurrent = ProcFsum(filePath);
                            if (hashCodeCurrent != null) { AddFile(filePath, hashCodeCurrent, fileDate); }
                        }
                        else
                        {
                            var cultureInfo = new CultureInfo("fr-FR");
                            DateTime fileDate = System.IO.File.GetCreationTime(filePath);
                            var parsedDate = DateTime.Parse(dateFile, cultureInfo);
                            if (fileDate.ToString().Substring(0, 16) != parsedDate.ToString().Substring(0, 16))
                            {
                                string hashCodeCurrent = ProcFsum(filePath);
                                if (hashCodeCurrent != null) { EditFile(filePath, hashCodeCurrent, fileDate); }
                            }
                            else
                            {
                                string hashCodeCurrent = ProcFsum(filePath);
                                if (hashCodeCurrent != null)
                                {
                                    if (this.hashCode != hashCodeCurrent)
                                    {
                                        flagError = true;
                                        using (StreamWriter writetext = new StreamWriter("checksumSHA512.log", true))
                                        {
                                            writetext.WriteLine(DateTime.Now + "....: Erreur d'intégrité du fichier " + filePath);
                                        }
                                    }
                                    EditFile(filePath, hashCodeCurrent, fileDate);
                                }
                            }
                        }
                    } 
                    else
                    {
                        using (StreamWriter writetext = new StreamWriter("checksumSHA512.log", true))
                        {
                            writetext.WriteLine(DateTime.Now + "....: Impossible d'accéder (verrouillé par l'utilisateur) au fichier " + filePath);
                        }
                    }
                }
                catch
                {
                    using (StreamWriter writetext = new StreamWriter("checksumSHA512.log", true))
                    {
                        writetext.WriteLine(DateTime.Now + "....: Impossible d'accéder (verrouillé par l'utilisateur) au fichier " + filePath);
                    }
                }
            }
            DeleteFile();
            using (StreamWriter writetext = new StreamWriter("checksumSHA512.log", true))
            {
                writetext.WriteLine(DateTime.Now + "....: Fin du traitement.");
            }
            if (isSilent == false)
            {
                EndList();
            }
            else if (isSilent == true)
            {
                Application.Exit();
            }
        }
        
        private void SearchFile(string file)
        {
            this.hashCode = "";
            this.fileName = "";
            this.dateFile = "";
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connFsum))
            {
                // Create a SqlCommand, and identify it as a stored procedure.
                using (SqlCommand sqlCommand = new SqlCommand("isFileInList", connection))
                {
                    StringBuilder errorMessages = new StringBuilder();
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    // Add input parameter for the stored procedure and specify what to use as its value.
                    sqlCommand.Parameters.Add(new SqlParameter("@FileName", SqlDbType.VarChar, 500));
                    sqlCommand.Parameters["@FileName"].Value = file;

                    // Add the output parameter.
                    sqlCommand.Parameters.Add(new SqlParameter("@HashCode", SqlDbType.VarChar, 130));
                    sqlCommand.Parameters["@HashCode"].Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add(new SqlParameter("@DateFile", SqlDbType.VarChar, 130));
                    sqlCommand.Parameters["@DateFile"].Direction = ParameterDirection.Output;

                    try
                    {
                        connection.Open();

                        // Run the stored procedure.
                        sqlCommand.CommandTimeout = 10;
                        sqlCommand.ExecuteNonQuery();

                        // Customer ID is an IDENTITY value from the database.

                        this.hashCode = (string)sqlCommand.Parameters["@HashCode"].Value;
                        this.fileName = (string)sqlCommand.Parameters["@FileName"].Value;
                        this.dateFile = (string)sqlCommand.Parameters["@DateFile"].Value;

                    }
                    catch (SqlException ex)
                    {
                        ErrorMsg(errorMessages, ex, file);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void AddFile(string file, string hashCode, DateTime dateTime)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connFsum))
            {
                // Create a SqlCommand, and identify it as a stored procedure.
                using (SqlCommand sqlCommand = new SqlCommand("addFileInList", connection))
                {
                    StringBuilder errorMessages = new StringBuilder();
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    // Add input parameter for the stored procedure and specify what to use as its value.
                    sqlCommand.Parameters.Add(new SqlParameter("@FileName", SqlDbType.VarChar, 500));
                    sqlCommand.Parameters["@FileName"].Value = file;
                    sqlCommand.Parameters.Add(new SqlParameter("@HashCode", SqlDbType.VarChar, 130));
                    sqlCommand.Parameters["@HashCode"].Value = hashCode;
                    sqlCommand.Parameters.Add(new SqlParameter("@DateFile", SqlDbType.DateTime));
                    sqlCommand.Parameters["@DateFile"].Value = dateTime;

                    try
                    {
                        connection.Open();
                        // Run the stored procedure.
                        sqlCommand.CommandTimeout = 10;
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        ErrorMsg(errorMessages, ex, file);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void EditFile(string file, string hashCode, DateTime dateTime)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connFsum))
            {
                // Create a SqlCommand, and identify it as a stored procedure.
                using (SqlCommand sqlCommand = new SqlCommand("editFileToList", connection))
                {
                    StringBuilder errorMessages = new StringBuilder();
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    // Add input parameter for the stored procedure and specify what to use as its value.
                    sqlCommand.Parameters.Add(new SqlParameter("@FileName", SqlDbType.VarChar, 500));
                    sqlCommand.Parameters["@FileName"].Value = file;
                    sqlCommand.Parameters.Add(new SqlParameter("@HashCode", SqlDbType.VarChar, 130));
                    sqlCommand.Parameters["@HashCode"].Value = hashCode;
                    sqlCommand.Parameters.Add(new SqlParameter("@DateFile", SqlDbType.DateTime));
                    sqlCommand.Parameters["@DateFile"].Value = dateTime;

                    try
                    {
                        connection.Open();
                        // Run the stored procedure.
                        sqlCommand.CommandTimeout = 10;
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        ErrorMsg(errorMessages, ex, file);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void DeleteFile()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connFsum))
            {
                // Create a SqlCommand, and identify it as a stored procedure.
                using (SqlCommand sqlCommand = new SqlCommand("delFileInList", connection))
                {
                    StringBuilder errorMessages = new StringBuilder();
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        connection.Open();
                        // Run the stored procedure.
                        sqlCommand.CommandTimeout = 900;
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        ErrorMsg(errorMessages, ex, "-> COMMANDE DELETE ALL FILES...");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public static List<string> GetAllAccessibleFiles(string rootPath, List<string> alreadyFound = null, List<string> exclusionFolders = null)
        {
            if (alreadyFound == null)
                alreadyFound = new List<string>();
            if (exclusionFolders == null)
                exclusionFolders = new List<string>();
            DirectoryInfo di = new DirectoryInfo(rootPath);
            var dirs = di.EnumerateDirectories();
            List<string> folders = new List<string>();
            foreach (string folderShare in dirs.Select(f => f.FullName).ToArray())
            { 
                try
                {
                    if(System.IO.Directory.Exists(folderShare))
                    {
                        alreadyFound = GetAllAccessibleFiles(folderShare, alreadyFound);
                        /*foreach (DirectoryInfo dir in dirs)
                        {
                            if (!((dir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden))
                            {
                                alreadyFound = GetAllAccessibleFiles(dir.FullName, alreadyFound);
                            }
                        }*/
                    }
                    else
                    {
                        using (StreamWriter writetext = new StreamWriter("checksumSHA512.log", true))
                        {
                            writetext.WriteLine(DateTime.Now + "....: Impossible d'accéder (permission non accordée) au dossier " + folderShare);
                        }
                    }
                }
                catch
                {
                    using (StreamWriter writetext = new StreamWriter("checksumSHA512.log", true))
                    {
                        writetext.WriteLine(DateTime.Now + "....: Impossible d'accéder (permission non accordée) au dossier " + folderShare);
                    }
                }
            }
            
            try
            {
                var files = Directory.GetFiles(rootPath);
                foreach (string s in files)
                {
                    alreadyFound.Add(s);
                }
            }
            catch (UnauthorizedAccessException)
            {
                using (StreamWriter writetext = new StreamWriter("checksumSHA512.log", true))
                {
                    writetext.WriteLine(DateTime.Now + "....: Impossible d'accéder (permission non accordée) au dossier " + rootPath);
                }
            }

            return alreadyFound;
        }

        private void ErrorMsg(StringBuilder errorMessages, SqlException ex, String file)
        {
            for (int i = 0; i < ex.Errors.Count; i++)
            {
                errorMessages.Append(DateTime.Now + "... Index #" + i + "\n" +
                    "Message: " + ex.Errors[i].Message + "\n" +
                    "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                    "Source: " + ex.Errors[i].Source + "\n" +
                    "Procedure: " + ex.Errors[i].Procedure + "\n\n");
            }
            if(isSilent == false)
            {
                //MessageBox.Show("Error : " + errorMessages.ToString());
                using (StreamWriter writetext = new StreamWriter("error.log", true))
                {
                    writetext.WriteLine("Error file " + file + ": " + errorMessages.ToString());
                }
            }
        }

        private void EndList()
        {
            if (flagError == true)
            {
                MessageBox.Show("Traitement terminé. Des erreurs ont été reportées.", "PRIVACYLAG", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Traitement terminé.", "PRIVACYLAG", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string ProcFsum(string fileName)
        {
            try
            {
                using (SHA512 shaM = new SHA512Managed())
                {
                    using (FileStream fileStream = File.OpenRead(fileName))
                        return Convert.ToBase64String(shaM.ComputeHash(fileStream));
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
