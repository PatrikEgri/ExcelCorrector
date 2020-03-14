using System;
using System.Xml.Serialization;

namespace ExcelCorrector.Models
{
    /// <summary>
    /// Represents a condition that will be checked on a specified cell.
    /// </summary>
    [Serializable]
    [XmlRoot]
    public class Condition
    {
        /// <summary>
        /// Specifies the type of the condition.
        /// </summary>
        [XmlAttribute]
        public ConditionTypes Type { get; set; }

        /// <summary>
        /// Specifies the expression of the condition that will be checked depending on type of the condition.
        /// </summary>
        [XmlAttribute]
        public string Expression { get; set; }

        /// <summary>
        /// Specifies that how many points it worths, if the condition is met. 
        /// </summary>
        [XmlAttribute]
        public float Points { get; set; }

    }
}
