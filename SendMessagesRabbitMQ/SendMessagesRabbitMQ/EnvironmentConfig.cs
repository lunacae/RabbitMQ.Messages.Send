namespace ResendMessages
{
    public static class EnvironmentConfig
    {
        public static class Rabbit
        {
            public static readonly string Host = GetVariableValueOrDefault("Rabbit.Host", "localhost");

            public static readonly int Port = int.Parse(GetVariableValueOrDefault("Rabbit.Port", "5672"));

            public static readonly string VirtualHost = GetVariableValueOrDefault("Rabbit.VirtualHost", "VirtualHost");

            public static readonly string UserName = GetVariableValueOrDefault("Rabbit.UserName", "rabbitUser");

            public static readonly string Password = GetVariableValueOrDefault("Rabbit.Password", "rabbitPassword");
        }

        public static readonly string DemoArg = GetVariableValueOrDefault("Argument", "ArgName");
        private static string GetVariableValueOrDefault(string variableName, string defaultValue = "")
        {
            string value = Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.Process);

            return value ?? defaultValue;
        }

    }
}
