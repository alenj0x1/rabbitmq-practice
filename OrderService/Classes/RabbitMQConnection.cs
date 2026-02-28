
using RabbitMQ.Client;

namespace OrderService.Classes
{
    public class RabbitMQConnection : IHostedService
    {
        public IConnection Connection { get; private set; } = null!;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost",
                VirtualHost = "/",
                Port = 5672
            };
            Connection = await factory.CreateConnectionAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Connection.CloseAsync(cancellationToken);
        }
    }
}
