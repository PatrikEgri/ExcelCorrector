using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml.Serialization;

namespace ExcelCorrector.Models
{
    /// <summary>
    /// Represents a cell and its conditions to be checked.
    /// </summary>
    [Serializable]
    [XmlRoot]
    public class Key
    {
        /// <summary>
        /// The key will be identified by this property.
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Stores the index of column of the specified cell.
        /// </summary>
        [XmlAttribute]
        public int ColumnIndex { get; set; }

        /// <summary>
        /// Stores the index of row of the specified cell.
        /// </summary>
        [XmlAttribute]
        public int RowIndex { get; set; }

        /// <summary>
        /// Stores the collection of conditions to be checked.
        /// </summary>
        [XmlArray]
        public List<Condition> Conditions { get; set; }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Key() { }

        /// <summary>
        /// This constructor calculates the index of row and column from the coordinate of cell.
        /// </summary>
        /// <param name="cellCoordinate">The coordinate of cell</param>
        public Key(string cellCoordinate)
        {
            CalculateIndexesFromCoordinate(cellCoordinate);
        }

        /// <summary>
        /// This method calculates the index of row and column from the coordinate of cell.
        /// </summary>
        /// <param name="cellCoordinate">The coordinate of cell</param>
        public void CalculateIndexesFromCoordinate(string cellCoordinate)
        {
            try
            {
                char[] chars = cellCoordinate.ToUpper().ToCharArray();
                string letters = "";
                string numbers = "";

                // checks all of the characters, that are they letters, numbers or something else and orders them
                foreach (char x in chars)
                {
                    if ((byte)x >= 65 && (byte)x <= 90)
                        letters += x;
                    else if ((byte)x >= 48 && (byte)x <= 57)
                        numbers += x;
                    else throw new FormatException();
                }

                // calculates the index of column by the letters with 26 number system
                int colIndex = 0;
                double exponent = 0;
                for (int i = letters.Length - 1; i >= 0; i--)
                {
                    colIndex += (int)Math.Pow(26, exponent) * ((byte)letters[i] - 64);
                    exponent++;
                }

                // checks that index of column is valid
                if (colIndex < 1 || colIndex > 16384)
                    throw new FormatException();

                ColumnIndex = colIndex - 1;

                // checks that length of numbers is valid
                if (numbers.Length == 0)
                    throw new FormatException();

                int num = int.Parse(numbers);

                // checks that index of row is valid
                if (num <= 0 || num > 1048576)
                    throw new FormatException();
                
                RowIndex = num - 1;
            }
            catch (FormatException)
            {
                MessageBox.Show("Érvénytelen cellakoordinátát adott meg!");
            }
        }

        /// <summary>
        /// This function calulates the coordinate of cell from the index of row and column.
        /// </summary>
        /// <returns>The coordinate of cell as string</returns>
        public string CalculateCoordinateFromIndexes()
        {
            string coordinate = "";
            int column = ColumnIndex + 1;

            while (column != 0)
            {
                int wholePart = column / 26;
                int theRest = column % 26 == 0 ? 26 : column % 26;

                coordinate += (char)((byte)theRest + (byte)64);
                column = wholePart;
            }

            char[] arr = coordinate.ToCharArray();
            Array.Reverse(arr);
            coordinate = "";

            foreach (char c in arr)
                coordinate += c;

            coordinate += RowIndex + 1;

            return coordinate;
        }

    }
}
