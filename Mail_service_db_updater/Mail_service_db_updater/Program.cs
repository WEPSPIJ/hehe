using Mail_service_db_updater;
using System;
using Dapper;
using System.Data.SqlClient;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<ImailProcessor, mailProcessor>();



var host = builder.Build();

     

host.Run();

