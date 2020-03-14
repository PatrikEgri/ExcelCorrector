using ExcelCorrector.Controls;
using ExcelCorrector.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ExcelCorrector.Pages
{
    /// <summary>
    /// Interaction logic for CorrectionKeysPage.xaml
    /// </summary>
    public partial class CorrectionKeyPage : Page
    {
        /// <summary>
        /// The MainWindow that contains this page.
        /// </summary>
        MainWindow _owner;

        /// <summary>
        /// The main object to be edit.
        /// </summary>
        public List<Models.Key> Keys { get; set; }

        /// <summary>
        /// The name of keys (main object).
        /// </summary>
        public string CorrectionKeyName { get; set; }

        /// <summary>
        /// Sets the main properties and fills the ListView.
        /// </summary>
        /// <param name="keys">The CorrectionKey to be edit</param>
        /// <param name="correctionKeyName">The name of CorrectionKey</param>
        public CorrectionKeyPage(List<Models.Key> keys, string correctionKeyName, MainWindow owner)
        {
            _owner = owner;
            Keys = keys;
            CorrectionKeyName = correctionKeyName;
            InitializeComponent();
            DataContext = this;
            FillListView();
        }

        /// <summary>
        /// Fills the ListView with the items of Keys.
        /// </summary>
        void FillListView()
        {
            lsvKeys.Items.Clear();

            foreach (var item in Keys)
                lsvKeys.Items.Add(item);   
        }

        /// <summary>
        /// Removes the specified item from the keys.
        /// </summary>
        /// <param name="key">The specific item to be removed</param>
        public void RemoveKey(Models.Key key)
        {
            grdKeys.Children.Remove(grdKeys.Children[1]);           
            Keys.Remove(key);           
            FillListView();
        }

        /// <summary>
        /// Initializes a KeyEditor and puts it into the second column of grid.
        /// </summary>
        /// <param name="sender">The ListView that triggered this event</param>
        private void lsvKeys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvKeys.SelectedItem != null) 
            {
                if (grdKeys.Children.Count == 2)
                    grdKeys.Children.Remove(grdKeys.Children[1]);

                var a = new KeyEditor(lsvKeys.SelectedItem as Models.Key, this);
                grdKeys.Children.Add(a);
                Grid.SetColumn(a, 1);
            }
        }

        /// <summary>
        /// Adds a new item to the keys and refills the ListView.
        /// </summary>
        /// <param name="sender">The Button that triggered this event</param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Keys.Add(new Models.Key() { Name = "Default", Conditions = new List<Models.Condition>() });
            FillListView();
        }

        /// <summary>
        /// Requests the serialization.
        /// </summary>
        /// <param name="sender">The Button that triggered this event</param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Serializer.Save(CorrectionKeyName, Keys);
                MessageBox.Show("Sikeres mentés!");

                _owner.frmMain.Content = new CorrectionKeysListPage(_owner);
            }
            catch(Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
    }
}
