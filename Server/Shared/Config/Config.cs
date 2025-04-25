using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace PKiK.Server.Shared;

public class Config {
    [JsonPropertyName("IsDevelopmentEnvironment")]
    public bool IsDevelopmentEnvironment { get; set; }
    [JsonPropertyName("JWT")]
    public JWTConfig JWT { get; set; }
    [JsonPropertyName("Hashing")]
    public HashingConfig Hashing { get; set; }
    [JsonIgnore]
    public string? ConnectionString { get; set; }
    [JsonIgnore]
    private static Config? _instance;

    public Config() {
        JWT = new JWTConfig();
        Hashing = new HashingConfig();
    }

    public static Config Get() {
        if (_instance is null) {
            _instance = new Config();
        }
        return _instance;
    }

    public static void Set(Config config) {
        _instance = config;
    }

    public bool IsValid => JWT.IsValid;

    public static Config ReadConfig() {
        var env = EnvironmentalSettings.Get();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(env.AppSettingsPath)
            .AddJsonFile("appsettings.json").Build();
        var config = configuration.GetSection("Config").Get<Config>();
        if (config is not null) {
            config.ConnectionString = configuration.GetConnectionString("DefaultConnection");
            if (config.ConnectionString == null) { throw new Exception("Cannot obtain connection string!"); }
            Set(config);
            return config;
        } else {
            throw new Exception("Cannot read config!");
        }
    }
}
