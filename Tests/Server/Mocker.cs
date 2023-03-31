namespace PKiK.Tests.Server {
    internal static class Mocker {
        internal static Config MockConfig() {
            var config = Config.Get();
            if(config.IsValid) {
                return config;
            }
            config = Config.ReadConfig();
            if (config == null) {
                throw new Exception("Cannot read config!");
            }
            Config.Set(config);
            return config;
        }
    }
}
