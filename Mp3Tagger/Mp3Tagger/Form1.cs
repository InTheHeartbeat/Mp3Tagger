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
using Mp3Tagger.Features;
using Mp3Tagger.Interfaces;
using Mp3Tagger.IO;
using Mp3Tagger.Models;
using Mp3Tagger.Presenters;
using TagLib;
using TagLib.Mpeg;
using TextBox = System.Windows.Forms.TextBox;

namespace Mp3Tagger
{
    public partial class Form1 : Form, IView
    {
        private MainPresenter presenter;
        public Form1()
        {
            InitializeComponent();      
            presenter = new MainPresenter(this);
            
            presenter.FeatureWorkStarted += OnPresenterFeatureWorkStarted;
            presenter.FeatureProgressUpdated += OnPresenterFeatureProgressUpdated;
            presenter.FeatureWorkCompleted += OnPresenterFeatureWorkCompleted;

            compositionsDataGrid.DataSource = new List<DataGridComposition>();
        }

        private void OnPresenterFeatureWorkCompleted(IFeature feature)
        {
            statusBarStatusLabel.Text = "Ready";
            statusBarProgressBar.Visible = false;
            statusBarProgressLabel.Text = string.Empty;

            listBoxHistory.Items.Add(feature.Name);

            InitializeDataGrid();
        }

        private void OnPresenterFeatureWorkStarted(IFeature feature, int operationsCount)
        {
            statusBarProgressLabel.Text = $"Loaded: 0/{operationsCount}";
            statusBarProgressBar.Maximum = operationsCount;
            statusBarProgressBar.Value = 0;
            statusBarProgressBar.Visible = true;
        }

        private void OnPresenterFeatureProgressUpdated(IFeature feature, int performed, int of)
        {
            statusBarProgressLabel.GetCurrentParent().Invoke(new MethodInvoker(delegate { statusBarProgressLabel.Text = $"Loaded: {performed + 1}/{of}"; }));
            statusBarProgressBar.GetCurrentParent().Invoke(new MethodInvoker(delegate { statusBarProgressBar.Value = performed + 1; }));
        }  
        
        private void InitializeDataGrid()
        {
            compositionsDataGrid.DataSource = presenter.Compositions.Select(c => new DataGridComposition(c)).ToList();
            SetBitrateColor();
        }
        private void SetBitrateColor()
        {
            compositionsDataGrid.Columns["Bitrate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            for (int i = 0; i < compositionsDataGrid.Rows.Count; i++)
            {
                DataGridViewCell cell = compositionsDataGrid["Bitrate", i];
                int bitrate = (int)cell.Value;

                if (bitrate < 320 && bitrate >= 256)
                {
                    cell.Style.BackColor = Color.FromArgb(255, 167, 38);
                }
                else if (bitrate < 256 && bitrate >= 192)
                {
                    cell.Style.BackColor = Color.FromArgb(251, 140, 0);
                }
                else if (bitrate < 192 && (bitrate >= 128 || bitrate >= 160))
                {
                    cell.Style.BackColor = Color.FromArgb(230, 81, 0);
                }
                else if (bitrate < 128)
                {
                    cell.Style.BackColor = Color.FromArgb(213, 0, 0);
                }
                else
                {
                    cell.Style.BackColor = Color.FromArgb(33, 150, 243);
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
                    presenter.OpenCompositions(fbd.SelectedPath);
                    statusBarProgressLabel.Text = "Getting files..";
                    statusBarStatusLabel.Text = "Loading compositions..";
                }
            }            
        }

        private void compositionsDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (compositionsDataGrid.SelectedRows.Count > 0)
            {
                textBoxTitle.DataBindings.Clear();
                textBoxTitle.DataBindings.Add("Text", presenter.Compositions
                    .FirstOrDefault(
                        c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                 .Value)
                    , "Title", false,DataSourceUpdateMode.OnPropertyChanged);               
                 
                textBoxPerformer.DataBindings.Clear();
                textBoxPerformer.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "Performer", false, DataSourceUpdateMode.OnPropertyChanged);

                textBoxAlbum.DataBindings.Clear();
                textBoxAlbum.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "Album", false, DataSourceUpdateMode.OnPropertyChanged);

                textBoxAlbumArtists.DataBindings.Clear();
                textBoxAlbumArtists.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "AlbumArtists", false, DataSourceUpdateMode.OnPropertyChanged);

                textBoxComposers.DataBindings.Clear();
                textBoxComposers.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "Composers", false, DataSourceUpdateMode.OnPropertyChanged);

                textBoxConductor.DataBindings.Clear();
                textBoxConductor.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "Conductor", false, DataSourceUpdateMode.OnPropertyChanged);

                textBoxCopyright.DataBindings.Clear();
                textBoxCopyright.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "Copyright", false, DataSourceUpdateMode.OnPropertyChanged);

                textBoxDisc.DataBindings.Clear();
                textBoxDisc.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "Disc", false, DataSourceUpdateMode.OnPropertyChanged);

                textBoxDiscCount.DataBindings.Clear();
                textBoxDiscCount.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "DiscCount", false, DataSourceUpdateMode.OnPropertyChanged);

                textBoxGenres.DataBindings.Clear();
                textBoxGenres.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "Genres", false, DataSourceUpdateMode.OnPropertyChanged);

                textBoxGrouping.DataBindings.Clear();
                textBoxGrouping.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "Grouping", false, DataSourceUpdateMode.OnPropertyChanged);

                textBoxLyrics.DataBindings.Clear();
                textBoxLyrics.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "Lyrics", false, DataSourceUpdateMode.OnPropertyChanged);

                textBoxTrack.DataBindings.Clear();
                textBoxTrack.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "Track", false, DataSourceUpdateMode.OnPropertyChanged);

                textBoxTrackCount.DataBindings.Clear();
                textBoxTrackCount.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "TrackCount", false, DataSourceUpdateMode.OnPropertyChanged);

                textBoxYear.DataBindings.Clear();
                textBoxYear.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "Year", false, DataSourceUpdateMode.OnPropertyChanged);

                Composition composition = presenter.Compositions
                    .FirstOrDefault(
                        c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index].Value);
                IPicture pictureModel = composition?.Pictures?.FirstOrDefault();                               
                if (pictureModel != null && pictureModel.Data != null && pictureModel.Data.Data.Length > 0)
                    using (MemoryStream ms = new MemoryStream(pictureModel.Data.Data, 0, pictureModel.Data.Data.Length))
                    {
                        ms.Write(pictureModel.Data.Data, 0, pictureModel.Data.Data.Length);
                        pictureBox1.Image = new Bitmap(Image.FromStream(ms, true), 150, 150);
                    }
            }
        }
        private void tabPageEdit_Resize(object sender, EventArgs e)
        {           
            int minSize = tabPageEdit.ClientSize.Width - pictureBox1.Margin.Right > tabPageEdit.ClientSize.Height-pictureBox1.Margin.Right ? tabPageEdit.ClientSize.Height - pictureBox1.Margin.Right : tabPageEdit.ClientSize.Width - pictureBox1.Margin.Right;            
            pictureBox1.Height = minSize;
            pictureBox1.Width = minSize;
        }   
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            string name = "";

            if (sender is TextBox)
                name=((TextBox)sender).Name;            
            if(sender is RichTextBox)
                name = ((RichTextBox)sender).Name;

            int selectedRowIndex = compositionsDataGrid.SelectedRows[0].Index;

            if (name == "textBoxTitle")
            {
                if((string)compositionsDataGrid["Title", selectedRowIndex].Value!=textBoxTitle.Text)
                    compositionsDataGrid["Title", selectedRowIndex].Value = textBoxTitle.Text;
                return;
            }
            if (name == "textBoxPerformer")
            {
                if ((string)compositionsDataGrid["Performer", selectedRowIndex].Value != textBoxPerformer.Text)
                    compositionsDataGrid["Performer", selectedRowIndex].Value = textBoxPerformer.Text;
                return;
            }
            if (name == "textBoxAlbum")
            {
                if ((string)compositionsDataGrid["Album", selectedRowIndex].Value != textBoxAlbum.Text)
                    compositionsDataGrid["Album", selectedRowIndex].Value = textBoxAlbum.Text;
                return;
            }
            if (name == "textBoxYear")
            {
                if ((Int32)compositionsDataGrid["Year", selectedRowIndex].Value != Int32.Parse(textBoxYear.Text))
                    compositionsDataGrid["Year", selectedRowIndex].Value = Int32.Parse(textBoxYear.Text);
                return;
            }
            if (name == "textBoxAlbumArtists")
            {
                if ((string)compositionsDataGrid["AlbumArtists", selectedRowIndex].Value != textBoxAlbumArtists.Text)
                    compositionsDataGrid["AlbumArtists", selectedRowIndex].Value = textBoxAlbumArtists.Text;
                return;
            }
            if (name == "textBoxComment")
            {
                if ((string)compositionsDataGrid["Comment", selectedRowIndex].Value != textBoxComment.Text)
                    compositionsDataGrid["Comment", selectedRowIndex].Value = textBoxComment.Text;
                return;
            }
            if (name == "textBoxComposers")
            {
                if ((string)compositionsDataGrid["Composers", selectedRowIndex].Value != textBoxComposers.Text)
                    compositionsDataGrid["Composers", selectedRowIndex].Value = textBoxComposers.Text;
                return;
            }
            if (name == "textBoxConductor")
            {
                if ((string)compositionsDataGrid["Conductor", selectedRowIndex].Value != textBoxConductor.Text)
                    compositionsDataGrid["Conductor", selectedRowIndex].Value = textBoxConductor.Text;
                return;
            }
            if (name == "textBoxCopyright")
            {
                if ((string)compositionsDataGrid["Copyright", selectedRowIndex].Value != textBoxCopyright.Text)
                    compositionsDataGrid["Copyright", selectedRowIndex].Value = textBoxCopyright.Text;
                return;
            }
            if (name == "textBoxDisc")
            {
                if ((Int32)compositionsDataGrid["Disc", selectedRowIndex].Value != Int32.Parse(textBoxDisc.Text))
                    compositionsDataGrid["Disc", selectedRowIndex].Value = Int32.Parse(textBoxDisc.Text);
                return;
            }
            if (name == "textBoxDiscCount")
            {
                if ((Int32)compositionsDataGrid["DiscCount", selectedRowIndex].Value != Int32.Parse(textBoxDiscCount.Text))
                    compositionsDataGrid["DiscCount", selectedRowIndex].Value = Int32.Parse(textBoxDiscCount.Text);
                return;
            }
            if (name == "textBoxGenres")
            {
                if ((string)compositionsDataGrid["Genres", selectedRowIndex].Value != textBoxGenres.Text)
                    compositionsDataGrid["Genres", selectedRowIndex].Value = textBoxGenres.Text;
                return;
            }
            if (name == "textBoxGrouping")
            {
                if ((string)compositionsDataGrid["Grouping", selectedRowIndex].Value != textBoxGrouping.Text)
                    compositionsDataGrid["Grouping", selectedRowIndex].Value = textBoxGrouping.Text;
                return;
            }
            if (name == "textBoxTrack")
            {
                if ((Int32)compositionsDataGrid["Track", selectedRowIndex].Value != Int32.Parse(textBoxTrack.Text))
                    compositionsDataGrid["Track", selectedRowIndex].Value = Int32.Parse(textBoxTrack.Text);
                return;
            }
            if (name == "textBoxTrackCount")
            {
                if ((Int32)compositionsDataGrid["TrackCount", selectedRowIndex].Value != Int32.Parse(textBoxTrackCount.Text))
                    compositionsDataGrid["TrackCount", selectedRowIndex].Value = Int32.Parse(textBoxTrackCount.Text);
                return;
            }      
        }

        private void fixEncodingAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want apply fix encoding for all compositions?", "Question", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;
            presenter.FixAllEncoding();
        }

        private void fixEncodingSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want apply fix encoding for selected compositions?", "Question", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;
            List<Composition> selected = new List<Composition>();
            foreach (DataGridViewRow row in compositionsDataGrid.SelectedRows)
            {
                selected.Add(presenter.Compositions.FirstOrDefault(t=>t.Path == (string)compositionsDataGrid["Path",row.Index].Value));
            }
            presenter.FixSelectedEncoding(selected);
        }
    }
}
