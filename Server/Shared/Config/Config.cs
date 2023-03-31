using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace PKiK.Server.Shared {
    public class Config {
        [JsonPropertyName("IsDevelopmentEnvironment")]
        public bool IsDevelopmentEnvironment { get; set; }
        [JsonPropertyName("JWT")]
        public JWTConfig JWT { get; set; }
        [JsonIgnore]
        public string? ConnectionString { get; set; }
        [JsonIgnore]
        private static Config? instance;

        public Config() { 
            JWT = new JWTConfig();
        }

        public static Config Get() {
            if(instance == null) {
                instance = new Config();
            }
            return instance;
        }

        public static void Set(Config config) {
            instance = config;
        }

        public bool IsValid {
            get {
                return JWT.IsValid;
            }
        }

        public static Config ReadConfig() {
            var env = EnvironmentalSettings.Get();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(env.AppSettingsPath)
                .AddJsonFile("appsettings.json").Build();
            var config = configuration.GetSection("Config").Get<Config>();
            if (config != null) {
                config.ConnectionString = configuration.GetConnectionString("DefaultConnection");
                if (config.ConnectionString == null) { throw new Exception("Cannot obtain connection string!"); }
                Set(config);
                return config;
            } else {
                throw new Exception("Cannot read config!");
            }
        }
    }
}
