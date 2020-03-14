using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExcelCorrector.Models
{
    public class Corrector
    {
        /// <summary>
        /// The file to be corrected.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// The directory where the result of correction will be findable.
        /// </summary>
        public string PathToSave { get; set; }

        /// <summary>
        /// The correction key that has the conditions to the correction.
        /// </summary>
        public List<Models.Key> CorrectionKey { get; set; }

        /// <summary>
        /// Stores the earned points per key.
        /// </summary>
        public Dictionary<string, float> EarnedPoints { get; set; }

        /// <summary>
        /// Initializes the properties.
        /// </summary>
        /// <param name="filename">File to be corrected</param>
        /// <param name="correctionKey">Correction key that has the conditions to the correction</param>
        /// <param name="pathToSave">Directory where the result of correction will be findable</param>
        public Corrector(string filename, List<Models.Key> correctionKey, string pathToSave)
        {
            Filename = filename;
            PathToSave = pathToSave;
            CorrectionKey = correctionKey;
            EarnedPoints = new Dictionary<string, float>();
        }

        /// <summary>
        /// Corrects the file to be corrected by the conditions of CorrectionKey and stores the results.
        /// </summary>
        public void CorrectSingleFile()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(Filename)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                foreach (var x in CorrectionKey)
                {
                    EarnedPoints.Add(x.Name, 0F);
                    foreach (var y in x.Conditions)
                    {
                        switch (y.Type)
                        {
                            case ConditionTypes.Containment:
                                EarnedPoints[x.Name] += worksheet.Cells[x.CalculateCoordinateFromIndexes()].Formula.Contains(y.Expression.ToUpper()) ? y.Points : 0F;
                                break;

                            case ConditionTypes.Equivalence:
                                EarnedPoints[x.Name] += worksheet.Cells[x.CalculateCoordinateFromIndexes()].Value.ToString().Equals(y.Expression) ? y.Points : 0F;
                                break;

                            default: throw new Exception();
                        }
                    }
                }
            }
            WriteSingeFileCorrectionResult();
        }

        /// <summary>
        /// Writes the results of correction into a new Excel workbook.
        /// </summary>
        void WriteSingeFileCorrectionResult()
        {
            string savepath = $@"{ PathToSave }\result.xlsx";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileStream(savepath, FileMode.Create)))
            {
                package.Workbook.Worksheets.Add("Result");
                var worksheet = package.Workbook.Worksheets["Result"];

                byte columnIndex = 65;
                foreach (var x in EarnedPoints)
                {
                    worksheet.Cells[$"{(char)columnIndex}1"].Value = x.Key;
                    worksheet.Cells[$"{(char)columnIndex}2"].Value = x.Value;
                    columnIndex++;
                }
                worksheet.Cells[$"{(char)columnIndex}2"].Value = $"{ 100 / CorrectionKey.Sum(x => x.Conditions.Sum(y => y.Points)) * EarnedPoints.Sum(x => x.Value) } %";

                package.Save();
            }
        }
    }
}
