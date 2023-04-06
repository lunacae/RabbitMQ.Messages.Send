using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace ResendMessages
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                UserName = EnvironmentConfig.Rabbit.UserName,
                Password = EnvironmentConfig.Rabbit.Password,
                HostName = EnvironmentConfig.Rabbit.Host,
                Port = EnvironmentConfig.Rabbit.Port,
                VirtualHost = EnvironmentConfig.Rabbit.VirtualHost
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var messages = ReadMessages();
                    foreach (var message in messages)
                    {
                        if (channel.IsClosed)
                            throw new Exception("Canal fechado");

                        IBasicProperties properties = channel.CreateBasicProperties();
                        properties.Headers = new Dictionary<string, object>();
                        properties.Headers.Add(new KeyValuePair<string, object>("System", EnvironmentConfig.DemoArg));

                        var body = JsonConvert.SerializeObject(message);

                        channel.BasicPublish("demoExchange", "directexchange_key", properties, Encoding.UTF8.GetBytes(body));
                        await Task.Delay(TimeSpan.FromSeconds(3));
                    }
                }
            }
            Console.WriteLine("Mensagens enviadas");
        }

        public static List<Object> ReadMessages()
        {
            string currentPath = Directory.GetCurrentDirectory();
            currentPath = currentPath.Replace("\\bin\\Debug\\net6.0", "");
            string path = currentPath + Path.DirectorySeparatorChar + @"Messages.json";

            string text = System.IO.File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Object>>(text);

        }
    }
}
