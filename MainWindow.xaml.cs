using ExcelCorrector.Pages;
using ExcelCorrector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ExcelCorrector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            //frmMain.Content = new CorrectionKeyPage(new List<Models.Key>
            //    {
            //        new Models.Key("A1") { Name = "ASD1", Conditions = new List<Models.Condition> { new Models.Condition(), new Models.Condition(), new Models.Condition(), new Models.Condition(), new Models.Condition(), new Models.Condition(), new Models.Condition(), new Models.Condition(), new Models.Condition(), new Models.Condition() } },
            //        new Models.Key("A2") { Name = "ASD2", Conditions = new List<Models.Condition>()}
            //    },
            //    "Test"
            //);
        }

        private void OpenCorrectionKeys(object sender, RoutedEventArgs e)
        {
            frmMain.Content = new CorrectionKeysListPage(this);
        }
    }
}
