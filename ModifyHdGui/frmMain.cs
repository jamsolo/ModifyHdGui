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
using System.IO;

namespace ModifyHdGui
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if(openVHDFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = openVHDFileDialog.FileName;
            }

            txtOutput.Clear();

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "C:\\Program Files\\Oracle\\VirtualBox\\VBoxManage.exe",
                Arguments = string.Format("showmediuminfo disk \"{0}\"", txtPath.Text),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            try
            {
                using (Process cmd = Process.Start(startInfo))
                {
                    txtOutput.Text += "Disk Info:\r\n";

                    using (StreamReader errorReader = cmd.StandardError)
                    {
                        string error = errorReader.ReadToEnd();
                        txtOutput.Text += error;
                    }

                    using (StreamReader outputReader = cmd.StandardOutput)
                    {
                        string output = outputReader.ReadToEnd();
                        txtOutput.Text += output;
                    }
                }

                foreach (string line in txtOutput.Lines)
                {
                    if (line.StartsWith("Capacity"))
                    {
                        this.udSize.Value = Int32.Parse(line.Split(' ')[7]);
                    }
                }
            }
            catch (Exception ex)
            {
                txtOutput.Text += "Invalid file!\r\n";
            }
            
        }

        private void btnResize_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "C:\\Program Files\\Oracle\\VirtualBox\\VBoxManage.exe",
                Arguments = string.Format("modifyhd \"{0}\" --resize {1}", txtPath.Text, udSize.Value.ToString()),
                RedirectStandardOutput = true,
                RedirectStandardError  = true,
                UseShellExecute        = false
            }; 

            try
            {
                using (Process cmd = Process.Start(startInfo))
                {
                    txtOutput.Text += "Starting with resizing...\r\n";

                    using (StreamReader errorReader = cmd.StandardError)
                    {
                        string error = errorReader.ReadToEnd();
                        txtOutput.Text += error;
                    }

                    using (StreamReader outputReader = cmd.StandardOutput)
                    {
                        string output = outputReader.ReadToEnd();
                        txtOutput.Text += output;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);

            }

        }

    }
}
