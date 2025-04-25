namespace PKiK.Server.Shared;

public class EnvironmentalSettings {
    private static EnvironmentalSettings? _instance;
    private EnvironmentalSettings() {
        string dataDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
        if (dataDirectoryPath.Contains(".build")) {
            RootPath = dataDirectoryPath.Split(".build").First(); //TODO: Check different dir names
        } else {
            RootPath = dataDirectoryPath.Split("Publish").First();
        }
        DBPath = RootPath + "Server//DB//DB.mdf";
        AppSettingsPath = RootPath + "//Server//.config//";
    }
    public string RootPath { get; set; }
    public string DBPath { get; set; }
    public string AppSettingsPath { get; set; }

    public static EnvironmentalSettings Get() {
        if (_instance is null) {
            _instance = new EnvironmentalSettings();
        }
        return _instance;
    }
}
