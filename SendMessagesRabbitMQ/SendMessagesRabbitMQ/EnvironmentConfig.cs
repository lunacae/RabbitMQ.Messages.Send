namespace ResendMessages
{
    public static class EnvironmentConfig
    {
        public static class Rabbit
        {
            public static readonly string Host = GetVariableValueOrDefault("Rabbit.Host", "localhost");

            public static readonly int Port = Int32.Parse(GetVariableValueOrDefault("Rabbit.Port", "5672"));

            public static readonly string VirtualHost = GetVariableValueOrDefault("Rabbit.VirtualHost", "VirtualHost");

            public static readonly string UserName = GetVariableValueOrDefault("Rabbit.UserName", "user");

            public static readonly string Password = GetVariableValueOrDefault("Rabbit.Password", "123");
        }

        public static readonly string queue = GetVariableValueOrDefault("queue", "queue");
        public static readonly string System = GetVariableValueOrDefault("System", "SystemToBind");
        private static string GetVariableValueOrDefault(string variableName, string defaultValue = "")
        {
            string value = Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.Process);

            return value ?? defaultValue;
        }

    }
}
