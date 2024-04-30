
using Confluent.Kafka;
using EmployeeApplication.Database;
using System.Text.Json;

namespace ConsumerAPI
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation($"About to consume data {DateTime.Now}", DateTime.Now);
            while (!stoppingToken.IsCancellationRequested)
            {
                var service = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<EmployeeDbContext>();

                var config = new ConsumerConfig
                {
                    ClientId = "myClientID",
                    BootstrapServers = "localhost:9092",
                    GroupId = "employeeGroupId"
                };

                using (var consumer = new ConsumerBuilder<string, string>(config).Build())
                {
                    consumer.Subscribe("employeeTopic");
                   var consumerData = consumer.Consume(TimeSpan.FromSeconds(4));

                    if(consumerData is not null)
                    {
                        var employee = JsonSerializer.Deserialize<Employee>(consumerData.Message.Value);

                        var employeeReport = new EmployeeReport(Guid.NewGuid(), employee!.Id, employee.Name, employee.SurName);

                       await service.Reports.AddAsync(employeeReport, stoppingToken);

                        await service.SaveChangesAsync(stoppingToken);
                    }

                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation($"Data consumption started at {DateTime.Now}", DateTime.Now);
                    }

                    await Task.Delay(1000);
                }


            }
        }
    }
}
