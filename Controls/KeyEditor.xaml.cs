using ExcelCorrector.Pages;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExcelCorrector.Controls
{
    /// <summary>
    /// Interaction logic for KeyEditor.xaml
    /// </summary>
    public partial class KeyEditor : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Parent object.
        /// </summary>
        CorrectionKeyPage _owner;

        /// <summary>
        /// The event that will be invoked when a property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The main object to be edit.
        /// </summary>
        public Models.Key Key { get; set; }

        /// <summary>
        /// Property to handle easily the Key's column- and rowindex.
        /// </summary>
        public string Coordinate 
        {
            get 
            { 
                return Key.CalculateCoordinateFromIndexes(); 
            } 
            set
            {
                if (value != Key.CalculateCoordinateFromIndexes())
                {
                    Key.CalculateIndexesFromCoordinate(value);
                    OnPropertyChanged("Coordinate");
                }
            }
        }

        /// <summary>
        /// Property to handle easily the Key's name.
        /// </summary>
        public string KeyName
        {
            get => Key.Name;
            set
            {
                if (value != Key.Name)
                {
                    Key.Name = value;
                    OnPropertyChanged("KeyName");
                }
            }
        }

        /// <summary>
        /// Constructor to open editor for an existing object.
        /// </summary>
        /// <param name="key"></param>
        public KeyEditor(Models.Key key, CorrectionKeyPage owner)
        {
            _owner = owner;
            Key = key;
            InitializeComponent();
            this.DataContext = this;           
            FillGrid();               
        }

        /// <summary>
        /// Refills the grid of conditions with RowDefinitions and UIElements.
        /// </summary>
        void FillGrid()
        {
            ClearGrid();
            AddRowDefinitions();

            int rowIndex = 0;
            int columnIndex = 0;
            foreach (Models.Condition x in Key.Conditions)
            {
                var editor = new ConditionEditor(x, this);
                grdConditions.Children.Add(editor);

                Grid.SetColumn(editor, columnIndex);
                Grid.SetRow(editor, rowIndex);

                columnIndex++;

                if (columnIndex == 3)
                {
                    columnIndex = 0;
                    rowIndex++;
                }
            }
        }

        /// <summary>
        /// Clears the children and RowDefinitions of grid.
        /// </summary>
        void ClearGrid()
        {
            grdConditions.Children.Clear();
            grdConditions.RowDefinitions.Clear();
        }

        /// <summary>
        /// Adds RowDefinitions to the grid.
        /// </summary>
        void AddRowDefinitions()
        {
            float ratio = (float)Key.Conditions.Count / 3;
            int rows = ratio % 1 == 0 ? (int)ratio : (int)ratio + 1;

            for (int i = 0; i < rows; i++)
                grdConditions.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
        }

        /// <summary>
        /// Removes the specified element from conditions of key.
        /// </summary>
        /// <param name="condition">The condition to be removed</param>
        public void RemoveCondition(Models.Condition condition)
        {
            Key.Conditions.Remove(condition);
            FillGrid();
        }

        /// <summary>
        /// Invokes the PropertyChanged event with the specified property.
        /// </summary>
        /// <param name="property">The property that has been changed</param>
        void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Adds a new element to the conditions of key.
        /// </summary>
        /// <param name="sender">The Button that triggered this event</param>
        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Key.Conditions.Add(new Models.Condition());
            FillGrid();
        }

        /// <summary>
        /// Removes this key.
        /// </summary>
        /// <param name="sender">The Button that triggered this event</param>
        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            _owner.RemoveKey(Key);
        }
    }
}
