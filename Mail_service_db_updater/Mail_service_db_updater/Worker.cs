using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace Mail_service_db_updater
{

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ImailProcessor _processor;

        public Worker(ILogger<Worker> logger, ImailProcessor processor)
        {
            _logger = logger;
            _processor = processor;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                //var mailComps = SqlRequest.findUnsent();  //main logic of code looping in the worker below
                //bool mailSucess = MailSender.SendMail(mailComps);
                //if (mailSucess == true)
                //{
                //    SqlRequest.editStatusDB(mailComps[0], "1"); //updating DB to tell it that the mail has been sent
                //    _logger.LogInformation("mail sent to:" + mailComps[1]);
                //}
                //else
                //{
                //    _logger.LogInformation("Mail failed to send. Not updating DB.");
                //}
                _processor.processMail();
                //mailProcessor.processMail(_logger);


                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
