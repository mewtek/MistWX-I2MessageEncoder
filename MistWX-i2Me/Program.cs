﻿using System.Xml.Serialization;
using MistWX_i2Me;
using MistWX_i2Me.Schema.System;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("d8b  .d8888b.  888b     d888 8888888888 \nY8P d88P  Y88b 8888b   d8888 888        \n           " +
                          "888 88888b.d88888 888        \n888      .d88P 888Y88888P888 8888888    \n888  .od888P" +
                          "\"  888 Y888P 888 888        " +
                          "\n888 d88P\"      888  Y8P  888 888        \n888 888\"       888   \"   888 888        " +
                          "\n888 888888888  888       " +
                          "888 8888888888 ");
        
        Console.WriteLine("(C) Mist Weather Media");
        Console.WriteLine("This project is licensed under the AGPL v3.0 license.");
        Console.WriteLine("Weather information collected from The National Weather Service & The Weather Company");
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Log.Info("Starting i2ME...");

        Config config = Config.Load();

        List<string> locations = await GetMachineLocations(config);
        
        
    }

    /// <summary>
    /// Runs through the pre-existing MachineProductConfig.xml file to scrape what locations that need
    /// weather information collected.
    /// </summary>
    public static async Task<List<string>> GetMachineLocations(Config config)
    {
        List<string> locations = new List<string>();

        Log.Info("Getting locations for this unit..");

        string copyPath = Path.Combine(AppContext.BaseDirectory, "MachineProductConfig.xml");

        if (File.Exists(copyPath))
        {
            File.Delete(copyPath);
        }

        if (!File.Exists(config.MachineProductConfig))
        {
            Log.Error("Unable to locate MachineProductConfig.xml");
            return locations;
        }
        
        File.Copy(config.MachineProductConfig, copyPath);

        MachineProductConfig mpc;
        
        using (var reader = new StreamReader(copyPath))
        {
            mpc = (MachineProductConfig) new XmlSerializer(typeof(MachineProductConfig)).Deserialize(reader);
        }

        foreach (ConfigItem i in mpc.ConfigDef.ConfigItems.ConfigItem)
        {
            if (i.Key == "PrimaryLocation" || i.Key == "NearbyLocation1" || i.Key == "NearbyLocation2" ||
                i.Key == "NearbyLocation3" || i.Key == "NearbyLocation4" || i.Key == "NearbyLocation5" ||
                i.Key == "NearbyLocation6" || i.Key == "NearbyLocation7" || i.Key == "NearbyLocation8")
            {
                if (string.IsNullOrEmpty(i.Value.ToString()))
                {
                    continue;
                }
                
                string choppedValue = i.Value.ToString().Split("1_US_")[1];
                locations.Add(choppedValue);
            }
        }
        
        return locations;
    }
}