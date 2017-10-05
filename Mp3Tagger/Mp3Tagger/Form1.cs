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
using Mp3Tagger.Enums;
using Mp3Tagger.Features;
using Mp3Tagger.Features.Helpers;
using Mp3Tagger.Interfaces;
using Mp3Tagger.IO;
using Mp3Tagger.Models;
using Mp3Tagger.Presenters;
using Mp3Tagger.Settings;
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

            InitializeCheckedListBoxApplyTo();

            presenter.FeatureWorkStarted += OnPresenterFeatureWorkStarted;
            presenter.FeatureProgressUpdated += OnPresenterFeatureProgressUpdated;
            presenter.FeatureWorkCompleted += OnPresenterFeatureWorkCompleted;            

            compositionsDataGrid.DataSource = new List<DataGridComposition>();
            
            listBoxPatternRemoverPatterns.DataSource = new BindingSource(new BindingList<string>(presenter.PatternRemoverSettings.PatternList),null);
            listBoxPatternRemoverBrackets.DataSource = new BindingSource(new BindingList<string>(presenter.PatternRemoverSettings.BracketsList), null);

            checkBoxRemoveByBrackets.DataBindings.Add("Checked", presenter.PatternRemoverSettings,
                "RemoveByBracketsList");
            checkBoxRemoveByPatternList.DataBindings.Add("Checked", presenter.PatternRemoverSettings,
                "RemoveByPatternList");

            checkBoxNormalizerTrimming.DataBindings.Add("Checked", presenter.NormalizerSettings, "Trimming");
            checkBoxNormalizerChangeCase.DataBindings.Add("Checked", presenter.NormalizerSettings, "ChangeCase");
            checkBoxNormalizerRemoveChars.DataBindings.Add("Checked", presenter.NormalizerSettings, "RemoveChars");

            groupBoxPRemoverBrackets.DataBindings.Add("Enabled", checkBoxRemoveByBrackets, "Checked");
            groupBoxPRemoverPatterns.DataBindings.Add("Enabled", checkBoxRemoveByPatternList, "Checked");

            groupBoxNormalizerChangeCase.DataBindings.Add("Enabled", checkBoxNormalizerChangeCase, "Checked");
            textBoxNormalizerChars.DataBindings.Add("Enabled", checkBoxNormalizerRemoveChars, "Checked");
            groupBoxNormalizerChangeCaseApplyTo.DataBindings.Add("Enabled", checkBoxNormalizerChangeCase, "Checked");
            groupBoxNormalizerCharsRemoverApplyTo.DataBindings.Add("Enabled", checkBoxNormalizerRemoveChars, "Checked");
        }

        private void InitializeCheckedListBoxApplyTo()
        {
            /* **** PatternRemover **** */
            checkedListBoxApplyToPatternRemover.DataSource = presenter.PatternRemoverSettings.ApplyToSettings;
            checkedListBoxApplyToPatternRemover.DisplayMember = "FieldName";
            checkedListBoxApplyToPatternRemover.ValueMember = "IsApply";
            for (var i = 0; i < checkedListBoxApplyToPatternRemover.Items.Count; i++)
            {
                checkedListBoxApplyToPatternRemover.SetItemChecked(i,
                    presenter.PatternRemoverSettings.ApplyToSettings[i].IsApply);
            }

            /* **** CaseChange **** */
            checkedListBoxCaseChangeApplyTo.DataSource = presenter.NormalizerSettings.CaseChangeApplyTo;
            checkedListBoxCaseChangeApplyTo.DisplayMember = "FieldName";
            checkedListBoxCaseChangeApplyTo.ValueMember = "IsApply";
            for (var i = 0; i < checkedListBoxCaseChangeApplyTo.Items.Count; i++)
            {
                checkedListBoxCaseChangeApplyTo.SetItemChecked(i,
                    presenter.NormalizerSettings.CaseChangeApplyTo[i].IsApply);
            }

            /* **** CharsRemover **** */
            checkedListBoxCharsRemoverApplyTo.DataSource = presenter.NormalizerSettings.CharsToRemoveApplyTo;
            checkedListBoxCharsRemoverApplyTo.DisplayMember = "FieldName";
            checkedListBoxCharsRemoverApplyTo.ValueMember = "IsApply";
            for (var i = 0; i < checkedListBoxCharsRemoverApplyTo.Items.Count; i++)
            {
                checkedListBoxCharsRemoverApplyTo.SetItemChecked(i,
                    presenter.NormalizerSettings.CharsToRemoveApplyTo[i].IsApply);
            }
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
                    , "JoinedAlbumArtists", false, DataSourceUpdateMode.OnPropertyChanged);

                textBoxComposers.DataBindings.Clear();
                textBoxComposers.DataBindings.Add("Text", presenter.Compositions
                        .FirstOrDefault(
                            c => c.Path == (string)compositionsDataGrid["Path", compositionsDataGrid.SelectedRows[0].Index]
                                     .Value)
                    , "JoinedComposers", false, DataSourceUpdateMode.OnPropertyChanged);

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
                    , "JoinedGenres", false, DataSourceUpdateMode.OnPropertyChanged);
                
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
        private void fixEncodingAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want apply fix encoding for all compositions?", "Question", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;
            presenter.ApplyFeatureForAll(Feature.EncodingFixer);
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
            presenter.ApplyFeatureForSelected(Feature.EncodingFixer, selected);
        }
        private void buttonPatternRemoverAddPattern_Click(object sender, EventArgs e)
        {
            presenter.PatternRemoverSettings.PatternList.Add(textBoxPatternRemoverNewPattern.Text);
            listBoxPatternRemoverPatterns.DataSource = new BindingSource(new BindingList<string>(presenter.PatternRemoverSettings.PatternList), null);
        }       
        private void buttonPatternRemoverRemovePattern_Click(object sender, EventArgs e)
        {
            presenter.PatternRemoverSettings.PatternList.Remove((string)listBoxPatternRemoverPatterns.SelectedItem);
            listBoxPatternRemoverPatterns.DataSource = new BindingSource(new BindingList<string>(presenter.PatternRemoverSettings.PatternList), null);
        }
        private void buttonPatternRemoverAddBrackets_Click(object sender, EventArgs e)
        {
            presenter.PatternRemoverSettings.BracketsList.Add(textBoxPatternRemoverNewBrackets.Text);
            listBoxPatternRemoverBrackets.DataSource = new BindingSource(new BindingList<string>(presenter.PatternRemoverSettings.BracketsList), null);
        }
        private void buttonPatternRemoverRemoveBrackets_Click(object sender, EventArgs e)
        {
            presenter.PatternRemoverSettings.BracketsList.Remove((string)listBoxPatternRemoverBrackets.SelectedItem);
            listBoxPatternRemoverBrackets.DataSource = new BindingSource(new BindingList<string>(presenter.PatternRemoverSettings.BracketsList), null);
        }        
        private void buttonPatternRemoverApply_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want apply pattern remover?", "Question",
                MessageBoxButtons.YesNo);
            if(result != DialogResult.Yes)return;

            presenter.PatternRemoverSettings.InitializeApplyToByDefault();
            List<FeatureApplyToField> chkd = checkedListBoxApplyToPatternRemover.CheckedItems.Cast<FeatureApplyToField>()
                .ToList();
            chkd.ForEach(ch =>
            {
                FeatureApplyToField field =
                    presenter.PatternRemoverSettings.ApplyToSettings.FirstOrDefault(s => s.FieldName == ch.FieldName);
                if (field != null)
                    field.IsApply = true;
            });

            presenter.ApplyFeatureForAll(Feature.PatternRemover);
        }
        private void buttonNormalizerApply_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want apply normalizer?", "Question",
                MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;

            presenter.NormalizerSettings.InitializeCaseChangeApplyToByDefault();
            List<FeatureApplyToField> chkd = checkedListBoxCaseChangeApplyTo.CheckedItems.Cast<FeatureApplyToField>()
                .ToList();
            chkd.ForEach(ch =>
            {
                FeatureApplyToField field =
                    presenter.NormalizerSettings.CaseChangeApplyTo.FirstOrDefault(s => s.FieldName == ch.FieldName);
                if (field != null)
                    field.IsApply = true;
            });

            presenter.NormalizerSettings.InitializeCharsToRemoveApplyToByDeafult();
            chkd = checkedListBoxCharsRemoverApplyTo.CheckedItems.Cast<FeatureApplyToField>()
                .ToList();
            chkd.ForEach(ch =>
            {
                FeatureApplyToField field =
                    presenter.NormalizerSettings.CharsToRemoveApplyTo.FirstOrDefault(s => s.FieldName == ch.FieldName);
                if (field != null)
                    field.IsApply = true;
            });


            if (radioButtonNormalizerAllWordsUpper.Checked)
            { presenter.NormalizerSettings.CaseChangeMode = CaseChangeMode.AllWordsUpper;}
            if(radioButtonNormalizerFirstWordUpper.Checked)
            { presenter.NormalizerSettings.CaseChangeMode = CaseChangeMode.FirstWordUpper;}
            if (checkBoxNormalizerRemoveChars.Checked)
            {presenter.NormalizerSettings.CharsToRemove = textBoxNormalizerChars.Text.Split(' ').ToList();}
            presenter.ApplyFeatureForAll(Feature.Normalizer);
        }
        private void textBoxPatternRemoverNewPattern_TextChanged(object sender, EventArgs e)
        {
            buttonPatternRemoverAddPattern.Enabled = !string.IsNullOrWhiteSpace(textBoxPatternRemoverNewPattern.Text);
        }
        private void textBoxPatternRemoverNewBrackets_TextChanged(object sender, EventArgs e)
        {
            buttonPatternRemoverAddBrackets.Enabled = !string.IsNullOrWhiteSpace(textBoxPatternRemoverNewPattern.Text);
        }
    }
}
