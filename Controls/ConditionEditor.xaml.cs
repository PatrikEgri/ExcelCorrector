using ExcelCorrector.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ExcelCorrector.Controls
{
    /// <summary>
    /// Interaction logic for ConditionEditor.xaml
    /// </summary>
    public partial class ConditionEditor : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Parent object.
        /// </summary>
        KeyEditor _owner;

        /// <summary>
        /// The event that will be invoked when a property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The main object to be edit.
        /// </summary>
        public Models.Condition Condition { get; set; }

        /// <summary>
        /// Property to handle easily the type of condition.
        /// </summary>
        public ConditionTypes Type
        {
            get => Condition.Type;
            set
            {
                if (value != Condition.Type)
                {
                    Condition.Type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        /// <summary>
        /// Property to handle easily the expression of condition.
        /// </summary>
        public string Expression
        {
            get => Condition.Expression;
            set
            {
                if (value != Condition.Expression)
                {
                    Condition.Expression = value;
                    OnPropertyChanged("Expression");
                }
            }
        }

        /// <summary>
        /// Property to handle easily the points of condition.
        /// </summary>
        public string Points
        {
            get => Condition.Points.ToString();
            set
            {
                try
                {
                    if (float.Parse(value) != Condition.Points)
                    {
                        Condition.Points = float.Parse(value);
                        OnPropertyChanged("Points");
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
            }
        }

        /// <summary>
        /// Constructor to open editor for an existing object.
        /// </summary>
        /// <param name="condition"></param>
        public ConditionEditor(Models.Condition condition, KeyEditor owner)
        {
            Condition = condition;
            _owner = owner;
            
            InitializeComponent();
            this.DataContext = this;
            cbType.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
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
        /// Removes this condition from the conditions of key.
        /// </summary>
        /// <param name="sender">The button that triggered this event.</param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            _owner.RemoveCondition(Condition);
        }
    }
}
