﻿using ExcelCorrector.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace ExcelCorrector.Pages
{
    /// <summary>
    /// Interaction logic for SingleFileCorrector.xaml
    /// </summary>
    public partial class SingleFileCorrector : Page, INotifyPropertyChanged
    {
        /// <summary>
        /// An event that will be invoked when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The file to be corrected.
        /// </summary>
        string _filename;

        /// <summary>
        /// The directory where the result of correction will be findable.
        /// </summary>
        string _pathToSave;

        /// <summary>
        /// The file to be corrected will be corrected by this correction key.
        /// </summary>
        List<Models.Key> _correctionKey;

        /// <summary>
        /// List of correction keys.
        /// </summary>
        List<string> _files;

        /// <summary>
        /// A readonly path that points to the source directory.
        /// </summary>
        readonly string _dir = $@"{ AppDomain.CurrentDomain.GetData("DataDirectory").ToString() }\";

        /// <summary>
        /// Property to handle easily _files.
        /// </summary>
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

        /// <summary>
        /// Property to handle easily _filename.
        /// </summary>
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

        /// <summary>
        /// Property to handle easily _pathToSave.
        /// </summary>
        public string PathToSave
        {
            get => _pathToSave;
            set
            {
                if (value != _pathToSave)
                {
                    _pathToSave = value;
                    OnPropertyChanged("PathToSave");
                }
            }
        }

        /// <summary>
        /// Property to handle easily _correctionKey.
        /// </summary>
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

        /// <summary>
        /// Initializes and lists files.
        /// </summary>
        public SingleFileCorrector()
        {
            DataContext = this;
            Files = new List<string>();
            InitializeComponent();
            ListFiles();
        }

        /// <summary>
        /// Fills the ListView with name of correction keys.
        /// </summary>
        void ListFiles()
        {
            Files.Clear();
            string[] files = Directory.GetFiles(_dir);

            foreach (string x in files)
                Files.Add(x.Replace(_dir, "").Replace(".xml", ""));
        }

        /// <summary>
        /// Deserializes the specified correction key.
        /// </summary>
        /// <param name="file"></param>
        void ReadFile(string file)
        {
            CorrectionKey = Serializer.Load(file);
        }

        /// <summary>
        /// Invokes PropertyChanged event with the specified string.
        /// </summary>
        /// <param name="propertyName">The name of property that called this method</param>
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Opens FileDialog to set FileName.
        /// </summary>
        /// <param name="sender">The Button that triggered this event</param>
        void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            
            if (fileDialog.ShowDialog() == true)
                Filename = fileDialog.FileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The ListView that triggered this event</param>
        void lsvKeys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvKeys.SelectedItems != null)
            {
                ReadFile(lsvKeys.SelectedItem as string);
                btnCorrect.IsEnabled = true;
            }
            else btnCorrect.IsEnabled = true;
        }

        /// <summary>
        /// Requests the correction.
        /// </summary>
        /// <param name="sender">The Button that triggered this event</param>
        void btnCorrect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new Corrector(Filename, CorrectionKey, PathToSave).CorrectSingleFile();
                System.Windows.MessageBox.Show("Sikeres javítás!");
            }
            catch (Exception x)
            {
                System.Windows.MessageBox.Show(x.Message);
            }
        }

        /// <summary>
        /// Opens FolderBrowserDialog to set PathToSave.
        /// </summary>
        /// <param name="sender">The Button that triggered this event</param>
        void btnSetSavePath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
                PathToSave = fbd.SelectedPath;
        }

        /// <summary>
        /// Binds the path of save to the folder where the file to be corrected is.
        /// </summary>
        /// <param name="sender">The CheckBox that triggered this event</param>
        void cbSamePath_Checked(object sender, RoutedEventArgs e)
        {
            PathToSave = Filename.Remove(Filename.LastIndexOf('\\'));
            PropertyChanged += BindSavePath;
            tbPathToSave.IsEnabled = false;
            btnSetSavePath.IsEnabled = false;
        }

        /// <summary>
        /// Unbinds the path of save to the folder where the file to be corrected is.
        /// </summary>
        /// <param name="sender">The CheckBox that triggered this event</param>
        void cbSamePath_Unchecked(object sender, RoutedEventArgs e)
        {
            PropertyChanged -= BindSavePath;
            tbPathToSave.IsEnabled = true;
            btnSetSavePath.IsEnabled = true;
        }

        /// <summary>
        /// Sets the path of save to the folder where the file to be corrected is.
        /// </summary>
        void BindSavePath(object sender, PropertyChangedEventArgs e) =>
             PathToSave = Filename.Remove(Filename.LastIndexOf('\\'));
        
    }
}
