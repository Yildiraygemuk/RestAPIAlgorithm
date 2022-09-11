using FluentValidation;
using FluentValidation.AspNetCore;
using Quartz;
using RestAPIAlgorithm.Business.Abstract;
using RestAPIAlgorithm.Business.Concrete;
using RestAPIAlgorithm.Helper.Validation;
using RestAPIAlgorithm.Job;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation(x =>
    x.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.WebHost.UseUrls("https://localhost:5000");
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});

builder.Services.AddQuartz(q =>
{
    q.SchedulerId = "Scheduler-Core";

    q.UseMicrosoftDependencyInjectionJobFactory(options =>
    {
        options.AllowDefaultConstructor = true;
    });

    q.UseSimpleTypeLoader();
    q.UseInMemoryStore();
    q.UseDefaultThreadPool(tp => tp.MaxConcurrency = 100);

    //Job runs continuously once a minute and sends get and post requests
    #region REQUESTJOB
    var requestJob = new JobKey("RequestJob", "requestJob group");
    q.AddJob<RequestJob>(j => j
        .StoreDurably()
        .WithIdentity(requestJob)
    );

    q.AddTrigger(t => t
        .WithIdentity("RequestJobTrigger")
        .ForJob(requestJob)
        .StartNow()
        .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMinutes(1)).RepeatForever())
    );
    #endregion


    builder.Services.AddQuartzServer(options =>
    {
        options.WaitForJobsToComplete = true;
    });
});

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
    
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
