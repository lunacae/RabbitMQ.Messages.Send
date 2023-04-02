using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace ResendMessages
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Showing rabbit env: \n" +
                $"Username: {EnvironmentConfig.Rabbit.UserName} \n" +
                $"Password: {EnvironmentConfig.Rabbit.Password} \n" +
                $"HostName: {EnvironmentConfig.Rabbit.Host} \n" +
                $"Port: {EnvironmentConfig.Rabbit.Port} \n" +
                $"VirtualHost: {EnvironmentConfig.Rabbit.VirtualHost}");

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
                    Console.WriteLine($"Encontradas {messages.Count} mensagens para enviar");
                    Console.WriteLine($"Enviando para o sistena {EnvironmentConfig.System}");
                    foreach (var message in messages)
                    {
                        if (channel.IsClosed)
                            throw new Exception("Canal fechado");

                        IBasicProperties properties = channel.CreateBasicProperties();
                        properties.Headers = new Dictionary<string, object>();
                        properties.Headers.Add(new KeyValuePair<string, object>("System", EnvironmentConfig.System));

                        var body = JsonConvert.SerializeObject(message);

                        channel.BasicPublish("orders.headers", "order-operation", properties, Encoding.UTF8.GetBytes(body));
                        Thread.Sleep(TimeSpan.FromSeconds(3));
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

