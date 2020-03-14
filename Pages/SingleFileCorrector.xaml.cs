using ExcelCorrector.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ExcelCorrector.Pages
{
    /// <summary>
    /// Interaction logic for SingleFileCorrector.xaml
    /// </summary>
    public partial class SingleFileCorrector : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string _filename;
        List<Models.Key> _correctionKey;
        List<string> _files;

        /// <summary>
        /// A readonly path that points to the source directory.
        /// </summary>
        readonly string _dir = $@"{ AppDomain.CurrentDomain.GetData("DataDirectory").ToString() }\";

        public List<string> Files
        {
            get => _files;
            set
            {
                if (value != _files)
                {
                    _files = value;
                    OnPropertyChanged("Files");
                }
            }
        }
        public string Filename
        {
            get => _filename;
            set
            {
                if (value != _filename)
                {
                    _filename = value;
                    OnPropertyChanged("Filename");
                }
            }
        }
        public List<Models.Key> CorrectionKey
        {
            get => _correctionKey;
            set
            {
                if (value != _correctionKey)
                {
                    _correctionKey = value;
                    OnPropertyChanged("CorrectionKey");
                }
            }
        }

        public SingleFileCorrector()
        {
            DataContext = this;
            Files = new List<string>();
            InitializeComponent();
            ListFiles();
        }

        void ListFiles()
        {
            Files.Clear();
            string[] files = Directory.GetFiles(_dir);

            foreach (string x in files)
                Files.Add(x.Replace(_dir, "").Replace(".xml", ""));
        }

        void ReadFile(string file)
        {
            CorrectionKey = Serializer.Load(file);
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            
            if (fileDialog.ShowDialog() == true)
                Filename = fileDialog.FileName;
        }

        private void lsvKeys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvKeys.SelectedItems != null)
            {
                ReadFile(lsvKeys.SelectedItem as string);
                btnCorrect.IsEnabled = true;
            }
            else btnCorrect.IsEnabled = true;
        }
    }
}
