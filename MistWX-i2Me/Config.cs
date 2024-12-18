using System.Xml.Serialization;

namespace MistWX_i2Me;

/// <summary>
/// Class for building the config.xml file used to configure the software
/// </summary>
[XmlRoot("Config")]
public class Config
{
    // Config Elements \\
    
    [XmlElement] public string TwcApiKey { get; set; } = "REPLACE_ME";
    [XmlElement] public string LogLevel { get; set; } = "info";

    // Used to process what locations to generate
    [XmlElement]
    public string MachineProductConfig { get; set; } =
        "C:\\Program Files (x86)\\TWC\\i2\\managed\\MachineProductConfig.xml";

    [XmlElement] public bool UseNationalLocations { get; set; } = false;
    [XmlElement] public int RecordGenTimeSeconds { get; set; } = 3600;      // Defaults to 1 hour
    [XmlElement] public int CheckAlertTimeSeconds { get; set; } = 600;      // Defaults to 10 minutes

    [XmlElement] public NetworkConfig UnitConfig { get; set; } = new NetworkConfig();
    [XmlElement("RadarConfig")] public RadarConfig RadarConfiguration { get; set; } = new RadarConfig();

    // Actual configuration setup \\
    
    public static Config config = new Config();

    
    /// <summary>
    /// Loads values from the configuration file into the software
    /// </summary>
    /// <returns>Config object</returns>
    public static Config Load()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "config.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(Config));

        // Create a base config if none exists
        if (!File.Exists(path))
        {
            config = new Config();
            serializer.Serialize(File.Create(path), config);

            return config;
        }

        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            var deserializedConfig = serializer.Deserialize(stream);

            if (deserializedConfig != null && deserializedConfig is Config cfg)
            {
                config = cfg;
                return config;
            }

            return new Config();
        }
        
    }

    [XmlRoot("UnitConfig")]
    public class NetworkConfig
    {
        [XmlElement] public int RoutineMsgPort { get; set; } = 7787;
        [XmlElement] public int PriorityMsgPort { get; set; } = 7788;
        [XmlElement] public string I2MsgAddress { get; set; } = "224.1.1.77";
        [XmlElement] public string InterfaceAddress { get; set; } = "127.0.0.1";
    }

    [XmlRoot("RadarConfig")]
    public class RadarConfig
    {
        [XmlElement] public bool UseRadarServer { get; set; } = false;
        [XmlElement] public string RadarServerUrl { get; set; } = "REPLACE_ME";
    }
}