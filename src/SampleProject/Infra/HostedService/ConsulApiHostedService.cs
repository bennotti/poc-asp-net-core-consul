using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SampleProject.Core.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleProject.Infra.HostedService
{
    public class ConsulApiHostedService : IHostedService
    {
        private CancellationTokenSource _cts;
        private readonly IConsulClient _consulClient;
        private readonly IOptions<ConsulConfig> _consulConfig;
        private readonly ILogger<ConsulApiHostedService> _logger;
        private readonly IServer _server;
        private string _registrationID;

        public ConsulApiHostedService(IConsulClient consulClient, IOptions<ConsulConfig> consulConfig, ILogger<ConsulApiHostedService> logger, IServer server)
        {
            _server = server;
            _logger = logger;
            _consulConfig = consulConfig;
            _consulClient = consulClient;

        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a linked token so we can trigger cancellation outside of this token's cancellation
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _registrationID = $"{_consulConfig.Value.ServiceID}-5000";
            var registration = new AgentServiceRegistration()
            {
                ID = _registrationID,
                Name = _consulConfig.Value.ServiceName,
                Address = $"http://host.docker.internal",
                Port = 5000,
                Tags = new[] { "api-service" },
                Check = new AgentServiceCheck()
                {
                    HTTP = $"http://host.docker.internal:5000/health/status",
                    Timeout = TimeSpan.FromSeconds(3),
                    Interval = TimeSpan.FromSeconds(10),
                    TLSSkipVerify = true,
                }
            };

            _logger.LogInformation("Registering in Consul");
            await _consulClient.Agent.ServiceDeregister(registration.ID, _cts.Token);
            await _consulClient.Agent.ServiceRegister(registration, _cts.Token);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            _logger.LogInformation("Deregistering from Consul");
            try
            {
                await _consulClient.Agent.ServiceDeregister(_registrationID, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Deregisteration failed");
            }
        }
    }
}
