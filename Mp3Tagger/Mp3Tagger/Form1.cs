using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mp3Tagger.IO;
using Mp3Tagger.Models;
using TagLib.Mpeg;

namespace Mp3Tagger
{
    public partial class Form1 : Form
    {
        private List<Composition> compositions;

        public Form1()
        {
            InitializeComponent();                       
        }

        private async void InitializeCompositions(string path)
        {
            compositions = new List<Composition>();

            statusBarProgressLabel.Text = "Getting files..";

            List<string> files =  await FilesProvider.GetFiles(path, true);

            statusBarProgressBar.Minimum = 0;
            statusBarProgressBar.Maximum = files.Count;            
            await Task.Run(() =>
            {
                for (var i = 0; i < files.Count; i++)
                {
                    statusBarProgressLabel.Text = $@"Loaded: {i+1}/{files.Count}";
                    statusBarProgressBar.GetCurrentParent().Invoke(new MethodInvoker(delegate{ statusBarProgressBar.Value = i; }));

                    try
                    {
                        compositions.Add(new Composition(new AudioFile(files[i])));
                    }
                    catch (Exception e)
                    {
                        
                    }
                }
            });
            InitializeDataGrid();
        }

        private async void InitializeDataGrid()
        {
            compositionsDataGrid.DataSource = compositions.Select(c=>new DataGridComposition(c)).ToList();
            compositionsDataGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            DataGridViewColumn column = compositionsDataGrid.Columns["Cover"];
            if (column != null)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            compositionsDataGrid.Columns["Bitrate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            for (int i = 0; i < compositionsDataGrid.Rows.Count; i++)
            {               
                if ((int)compositionsDataGrid["Bitrate", i].Value < 320 && (int)compositionsDataGrid["Bitrate", i].Value >= 256)
                {
                    compositionsDataGrid["Bitrate", i].Style.BackColor = Color.FromArgb(255, 167, 38);
                }
                else
                if ((int)compositionsDataGrid["Bitrate", i].Value < 256 && (int)compositionsDataGrid["Bitrate", i].Value >= 192)
                {
                    compositionsDataGrid["Bitrate", i].Style.BackColor = Color.FromArgb(251, 140, 0);
                }
                else
                if ((int)compositionsDataGrid["Bitrate", i].Value < 192 && ((int)compositionsDataGrid["Bitrate", i].Value >= 128 || ((int)compositionsDataGrid["Bitrate", i].Value >= 160)))
                {
                    compositionsDataGrid["Bitrate", i].Style.BackColor = Color.FromArgb(230, 81, 0);
                }
                else if ((int)compositionsDataGrid["Bitrate", i].Value < 128)
                {
                    compositionsDataGrid["Bitrate", i].Style.BackColor = Color.FromArgb(213, 0, 0);
                }
                else
                {
                    compositionsDataGrid["Bitrate", i].Style.BackColor = Color.FromArgb(33, 150, 243);
                }
            }            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;                
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    InitializeCompositions(fbd.SelectedPath);
                }
            }            
        }
    }
}
