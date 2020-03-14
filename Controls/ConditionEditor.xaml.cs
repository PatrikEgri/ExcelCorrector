using ExcelCorrector.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ExcelCorrector.Controls
{
    /// <summary>
    /// Interaction logic for ConditionEditor.xaml
    /// </summary>
    public partial class ConditionEditor : UserControl
    {
        /// <summary>
        /// Parent object.
        /// </summary>
        KeyEditor _owner;

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
            set => Condition.Type = value;            
        }

        /// <summary>
        /// Property to handle easily the expression of condition.
        /// </summary>
        public string Expression
        {
            get => Condition.Expression; 
            set=> Condition.Expression = value;
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
                    Condition.Points = float.Parse(value);
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
        /// Removes this condition from the conditions of key.
        /// </summary>
        /// <param name="sender">The button that triggered this event.</param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            _owner.RemoveCondition(Condition);
        }
    }
}
