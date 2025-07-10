using System.IO;
using System.Xml.Serialization;

public static class SaveSystem
{
    public static void SavePlayerData(PlayerData data, string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));

        using (StreamWriter writer = new StreamWriter(path))
        {
            serializer.Serialize(writer, data);
        }
    }
}


