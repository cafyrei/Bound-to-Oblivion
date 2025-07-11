using System.IO;
using System.Xml.Serialization;

public static class LoadSystem
{
    public static PlayerData LoadPlayerData(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));

        using (StreamReader reader = new StreamReader(path))
        {
            return (PlayerData)serializer.Deserialize(reader);
        }
    }
}
