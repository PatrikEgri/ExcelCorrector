using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Reflection;
using ExcelCorrector.Models;

namespace ExcelCorrector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SetDataDirectory();
            //var a = Serializer.Load("test.xml");
            //Serializer.Save("test.xml", new List<Key>
            //{
            //    new Key("XFD1")
            //    {
            //        Conditions = new List<Models.Condition>
            //        {
            //            new Models.Condition
            //            {
            //                Type = ConditionTypes.Equivalence,
            //                Expression = "=",
            //                Points = 7.5F
            //            }
            //        }
            //    }
            //});
        }

        void SetDataDirectory()
        {
            AppDomain.CurrentDomain.SetData(
                "DataDirectory", 
                Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly()
                    .Location
                    .Replace(@"bin\Debug\netcoreapp3.1", "Source")));
        }

    }
}
