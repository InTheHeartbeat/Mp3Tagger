using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mp3Tagger.Kernel.Base.Attributes;
using Mp3Tagger.Kernel.Base.Extensions;
using Mp3Tagger.Kernel.Interfaces;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Kernel.Presenters;
using Mp3Tagger.Models;

namespace Mp3Tagger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView
    {
        private List<TextBox> editTabBoxes { get; set; }

        public ProcessState CurrentState => presenter.CurrentState;

        private MainPresenter presenter;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            editTabBoxes = new List<TextBox>();

            presenter = new MainPresenter(this);
            presenter.FeatureWorkStarted += Presenter_FeatureWorkStarted;
            presenter.FeatureProgressUpdated += Presenter_FeatureProgressUpdated;
            presenter.FeatureWorkCompleted += Presenter_FeatureWorkCompleted;
            CompositionsDataGrid.ItemsSource = new List<DataGridComposition>();

            GenerateEditTabPage();            
        }

        private void Presenter_FeatureWorkStarted(IFeature arg1, int arg2)
        {                        
        }

        private void Presenter_FeatureProgressUpdated(IFeature arg1, int arg2, int arg3)
        {                        
        }

        private void Presenter_FeatureWorkCompleted(IFeature obj)
        {            
            CompositionsDataGrid.ItemsSource = presenter.Compositions.Select(c => new DataGridComposition(c));                        
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

        private void CompositionsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        private void open_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Choose a scane folder";
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();                               
                presenter.OpenCompositions(dialog.SelectedPath);                
            }            
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
