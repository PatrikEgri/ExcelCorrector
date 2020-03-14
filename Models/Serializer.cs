using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ExcelCorrector.Models
{
    /// <summary>
    /// A static object that has a save and a load method for xml serialization.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// This method serializes the data to an xml file.
        /// </summary>
        /// <param name="filename">The name of file to be created within the data directory.</param>
        /// <param name="keys">The keys (data) to be serialized into the file to be created.</param>
        public static void Save(string filename, List<Key> keys)
        {
            using (FileStream stream = new FileStream($@"{ AppDomain.CurrentDomain.GetData("DataDirectory").ToString() }\{ filename }.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Key>), new XmlRootAttribute("Keys"));
                serializer.Serialize(stream, keys);
            }
        }

        /// <summary>
        /// This method deserializes the data from an xml file.
        /// </summary>
        /// <param name="filename">The name of file to be deserialized within the data directory.</param>
        /// <returns>The keys (data), which have been deserialized.</returns>
        public static List<Key> Load(string filename)
        {
            List<Key> list;

            using (FileStream stream = new FileStream($@"{ AppDomain.CurrentDomain.GetData("DataDirectory").ToString() }\{ filename }.xml", FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Key>), new XmlRootAttribute("Keys"));
                list = serializer.Deserialize(stream) as List<Key>;
            }

            return list;
        }
    }
}
