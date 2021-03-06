﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Mp3Tagger.Annotations;
using Mp3Tagger.Converters;
using Mp3Tagger.Helpers;
using Mp3Tagger.Kernel;
using Mp3Tagger.Kernel.Base.Attributes;
using Mp3Tagger.Kernel.Base.Extensions;
using Mp3Tagger.Kernel.Enums;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Kernel.Presenters;
using Mp3Tagger.Models;
using Binding = System.Windows.Data.Binding;
using Cursors = System.Windows.Input.Cursors;
using MenuItem = System.Windows.Controls.MenuItem;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using TextBox = System.Windows.Controls.TextBox;

namespace Mp3Tagger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView, INotifyPropertyChanged
    {
        private List<TextBox> editTabBoxes { get; set; }
        
        public ProcessingStateViewModel StateModel { get; set; }
        public DataGridModel DataGridModel { get; set; }

        public MainPresenter Presenter { get; set; }

        public bool IsBitrateMarking { get; set; }
        public bool IsEmptyMarking { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            editTabBoxes = new List<TextBox>();            
            Presenter = new MainPresenter(this);
            DataGridModel = new DataGridModel();            

            StateModel = new ProcessingStateViewModel();            
            //GenerateEditTabPage();            
            Presenter.Kernel.ProcessingStateChanged += SetState;
            Presenter.FeatureCompleted += state => DataGridModel.Compositions = Presenter.CurrentCompositions;
            Loaded += new RoutedEventHandler(MainWindow_Loaded);
            
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ((MenuItem) this.Template.FindName("menuOpen", this)).Click += this.open_Click;
        }

        private void SetState(ProcessingState state)
        {
            StateModel.IsBusy = state.IsBusy;
            StateModel.CurrentFeature = state.CurrentFeature;
            StateModel.Elapsed = state.Elapsed;
            StateModel.OperationsCount = state.OperationsCount;
            StateModel.OperationsPerformed = state.PerformedOperations;
        }

        private void GenerateEditTabPage()
        {
            StackPanel panel = EditTabPanel;

            foreach (PropertyInfo property in typeof(Composition).GetProperties())
            {
                if (property.PropertyType != typeof(string) && property.PropertyType != typeof(int)) continue;

                TextBlock block = new TextBlock
                {
                    Text = property.Name
                               .Trim()
                               .Replace("Joined", "")
                               .AddSpaceBetwenUpperChars()
                           + ":"
                };
                TextBox box = new TextBox
                {
                    Name = "editTabTextBox" + property.Name.Trim(),
                    Height = 20,
                    Margin = new Thickness(0, 2, 0, 5),
                    TextWrapping = TextWrapping.Wrap
                };

                Binding binding = new Binding
                {
                    Source = CompositionsDataGrid,
                    Path = new PropertyPath("SelectedItem."+property.Name),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                BindingOperations.SetBinding(box, TextBox.TextProperty, binding);

                CustomFieldHeightRequired customHeight =
                    (CustomFieldHeightRequired)property.GetCustomAttribute(typeof(CustomFieldHeightRequired),
                        true);
                if (customHeight != null)
                {
                    box.Height = customHeight.Height;
                    box.Padding = Padding = new Thickness(5);
                }
                panel.Children.Add(block);
                panel.Children.Add(box);
                editTabBoxes.Add(box);
            }
        }


        private void open_Click(object sender, RoutedEventArgs e)
        {            
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Choose a scan folder";
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();                               
                Presenter.OpenCompositions(dialog.SelectedPath);                
            }            
        }

        private void buttonFixEncodingAll_Click(object sender, RoutedEventArgs e)
        {
            Presenter.ApplyFeatureForAll(FeatureName.EncodingFixer);
        }

        private void buttonFixEncodingSelected_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CompositionsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ToggleBitrateMarkingButton_Click(object sender, RoutedEventArgs e)
        {
            IsBitrateMarking = !IsBitrateMarking;      

            OnPropertyChanged(nameof(IsBitrateMarking));
        }
        private void ToggleEmptyMarkingButton_Click(object sender, RoutedEventArgs e)
        {
            IsEmptyMarking = !IsEmptyMarking;
            OnPropertyChanged(nameof(IsEmptyMarking));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ToggleBitrateMarkingButton_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}


/*
 
     DisplayFieldsTogether firstTogether =
                        (DisplayFieldsTogether) property.GetCustomAttribute(typeof(DisplayFieldsTogether), true);
                    if (firstTogether == null)
                    {                        
                        
                    }
                    else
                    {
                        PropertyInfo secondTogether = typeof(Composition).GetProperties()
                            .Where(p => p.GetCustomAttribute(typeof(DisplayFieldsTogether)) != null)
                            .Where(pp => !panel.Children.)
                            .FirstOrDefault(
                                prop => prop.Name != property.Name);

                        if(secondTogether == null)continue;                        

                        StackPanel togetherPanelForBlocks = new StackPanel()
                        {
                            Orientation = Orientation.Horizontal
                        };
                        StackPanel togetherPanelForBoxes = new StackPanel()
                        {
                            Orientation = Orientation.Horizontal
                        };
                                   
                        togetherPanelForBlocks.Children.Add(block);
                        togetherPanelForBlocks.Children.Add(new TextBlock()
                        {
                            Text = secondTogether.Name
                                       .Trim()
                                       .Replace("Joined", "")
                                       .AddSpaceBetwenUpperChars()
                                   + ":"
                        });

                        togetherPanelForBoxes.Children.Add(box);
                        togetherPanelForBoxes.Children.Add(new TextBox()
                        {
                            Height = 20,
                            Name = secondTogether.Name.Trim(),
                            Margin = new Thickness(0, 2, 0, 5)
                        });
                        
                        panel.Children.Add(togetherPanelForBlocks);
                        panel.Children.Add(togetherPanelForBoxes);
                    }
     */
