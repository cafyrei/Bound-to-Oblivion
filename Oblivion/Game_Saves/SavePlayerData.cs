using System;
using System.IO;
using System.Xml.Serialization;

namespace Oblivion
{
    public static class SaveSystem
    {
        private static string SavePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "savefile.xml");

        public static void SavePlayerData(PlayerData data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
            using (FileStream stream = new FileStream(SavePath, FileMode.Create))
            {
                serializer.Serialize(stream, data);
                Console.WriteLine("Save file location: " + SavePath);
            }
        }

        public static PlayerData LoadPlayerData()
        {
            if (!File.Exists(SavePath))
                return null;

            XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
            using (FileStream stream = new FileStream(SavePath, FileMode.Open))
            {
                return serializer.Deserialize(stream) as PlayerData;
            }
        }

        public static void DeleteSave()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);
        }
    }
}
