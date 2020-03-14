using ExcelCorrector.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ExcelCorrector.Pages
{
    /// <summary>
    /// Interaction logic for CorrectionKeysListPage.xaml
    /// </summary>
    public partial class CorrectionKeysListPage : Page
    {
        /// <summary>
        /// An event that will be invoked if the CorrectionKey has been changed.
        /// </summary>
        event EventHandler CorrectionKeyChanged;

        /// <summary>
        /// The MainWindow that contains this page.
        /// </summary>
        MainWindow _owner;

        /// <summary>
        /// A readonly path that points to the source directory.
        /// </summary>
        readonly string _dir = $@"{ AppDomain.CurrentDomain.GetData("DataDirectory").ToString() }\";

        /// <summary>
        /// The main object to be edited or to be removed.
        /// </summary>
        List<Models.Key> _correctionKey;

        /// <summary>
        /// Contains the names of the read files.
        /// </summary>
        public List<string> Files { get; set; }

        /// <summary>
        /// The name of currently selected file.
        /// </summary>
        public string SelectedFile { get; set; }

        /// <summary>
        /// Property to handle easily the _correctionKey.
        /// </summary>
        public List<Models.Key> CorrectionKey 
        { 
            get => _correctionKey;
            set
            {
                _correctionKey = value;
                CorrectionKeyChanged?.Invoke(this, null);
            } 
        }

        /// <summary>
        /// Initializes the properties and lists the files.
        /// </summary>
        /// <param name="owner"></param>
        public CorrectionKeysListPage(MainWindow owner)
        {
            _owner = owner;
            DataContext = this;
            Files = new List<string>();
            ListFiles();            
            CorrectionKeyChanged += SetData;
            InitializeComponent();
        }

        /// <summary>
        /// Lists the files from source directory.
        /// </summary>
        void ListFiles()
        {
            Files.Clear();
            string[] files = Directory.GetFiles(_dir);

            foreach (string x in files)
                Files.Add(x.Replace(_dir, "").Replace(".xml", ""));
        }

        /// <summary>
        /// Deserializes the selected file and assigns the CorrectionKey.
        /// </summary>
        /// <param name="file"></param>
        void ReadFile(string file)
        {
            CorrectionKey = Serializer.Load(file);
        }

        /// <summary>
        /// Reads the selected file and enables the UIElements.
        /// </summary>
        /// <param name="sender">The ListView that triggered this event</param>
        void lsvCorrectionKeys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvCorrectionKeys.SelectedItem != null && lsvCorrectionKeys.SelectedItem as string != "")
            {
                SelectedFile = lsvCorrectionKeys.SelectedItem as string;
                btnEdit.IsEnabled = true;
                btnRemove.IsEnabled = true;
                lsvData.IsEnabled = false;

                ReadFile(SelectedFile);
            }
            else
            {
                SelectedFile = null;
                btnEdit.IsEnabled = false;
                btnRemove.IsEnabled = false;
                lsvData.IsEnabled = false;
            }
        }

        /// <summary>
        /// Fills the lsvData ListView with some information about the selected CorrectionKey.
        /// </summary>
        void SetData(object sender, EventArgs e)
        {
            int countOfConditions = 0;
            float sumOfPoints = 0;

            foreach (Models.Key key in CorrectionKey)
            {
                countOfConditions += key.Conditions.Count;
                sumOfPoints += key.Conditions.Sum(x => x.Points);
            }

            lsvData.Items.Clear();
            lsvData.Items.Add($"Count of keys in file: { CorrectionKey.Count }");
            lsvData.Items.Add($"Count of conditions in file: { countOfConditions }");
            lsvData.Items.Add($"Sum of points: { sumOfPoints }");
        }

        /// <summary>
        /// Creates a new CorrectionKey and opens editor for it.
        /// </summary>
        /// <param name="sender"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            _owner.frmMain.Content = new CorrectionKeyPage(new List<Models.Key>(), "Default", _owner);
        }

        /// <summary>
        /// Opens editor for the selected CorrectionKey.
        /// </summary>
        /// <param name="sender">The Button that triggered this event</param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            _owner.frmMain.Content = new CorrectionKeyPage(CorrectionKey, SelectedFile, _owner);
        }

        /// <summary>
        /// Removes the selected file.
        /// </summary>
        /// <param name="sender">The Button that triggered this event</param>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to remove this file?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result.ToString().Equals("Yes"))
                File.Delete($@"{ _dir }{ SelectedFile }.xml");

            ListFiles();
        }
    }
}
