using Microsoft.Extensions.Hosting;
using NLog;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mailer_Scheduler.Services
{
    public class Mailer : IJob
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        public Task Execute(IJobExecutionContext context)
        {
            log.Info("===== TESTING MY 5 Secs QUARTZ SCHEDULER. LOGGED AT: " + DateTime.Now + " =====");
            return Task.FromResult(0);
        }
    }

    public class Mailer_2 : IJob
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        public Task Execute(IJobExecutionContext context)
        {
            log.Info("==================== TESTING MY 20 Secs QUARTZ SCHEDULER. LOGGED AT: " + DateTime.Now + " ====================");
            return Task.FromResult(0);
        }
    }

    public class CustomQuartzJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public CustomQuartzJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IJob NewJob(TriggerFiredBundle triggerFiredBundle, IScheduler scheduler)
        {
            var jobDetail = triggerFiredBundle.JobDetail;
            return (IJob)_serviceProvider.GetService(jobDetail.JobType);
        }
        public void ReturnJob(IJob job) 
        { 
        }
    }

    //public class CustomQuartzHostedService : IHostedService
    //{
    //    private readonly ISchedulerFactory _schedulerFactory;
    //    private readonly IJobFactory _jobFactory;
    //    private readonly JobMetadata _jobMetadata;
    //    public CustomQuartzHostedService(ISchedulerFactory schedulerFactory, JobMetadata jobMetadata, IJobFactory jobFactory)
    //    {
    //        _schedulerFactory = schedulerFactory;
    //        _jobMetadata = jobMetadata;
    //        _jobFactory = jobFactory;
    //    }
    //    public IScheduler Scheduler { get; set; }
    //    //public async Task StartAsync(CancellationToken cancellationToken)
    //    //{
    //    //    Scheduler = await _schedulerFactory.GetScheduler();
    //    //    Scheduler.JobFactory = _jobFactory;
    //    //    var job = CreateJob(_jobMetadata);
    //    //    var trigger = CreateTrigger(_jobMetadata);
    //    //    await Scheduler.ScheduleJob(job, trigger, cancellationToken);
    //    //    await Scheduler.Start(cancellationToken);
    //    //}
    //    //public async Task StopAsync(CancellationToken cancellationToken)
    //    //{
    //    //    await Scheduler?.Shutdown(cancellationToken);
    //    //}
    //    private ITrigger CreateTrigger(JobMetadata jobMetadata)
    //    {
    //        return TriggerBuilder.Create()
    //        .WithIdentity(jobMetadata.JobId.ToString())
    //        .WithCronSchedule(jobMetadata.CronExpression)
    //        .WithDescription($"{jobMetadata.JobName}")
    //        .Build();
    //    }

    //    ITrigger everydayTrigger = TriggerBuilder.Create()
    //        .WithIdentity("everydayTrigger")
    //        // fires 
    //        .WithCronSchedule("0 0 12 1/1 * ?")
    //        // start immediately
    //        .StartAt(DateBuilder.DateOf(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year))
    //        .Build();
    //    private IJobDetail CreateJob(JobMetadata jobMetadata)
    //    {
    //        return JobBuilder
    //        .Create(jobMetadata.JobType)
    //        .WithIdentity(jobMetadata.JobId.ToString())
    //        .WithDescription($"{jobMetadata.JobName}")
    //        .Build();
    //    }
    //}

    public class JobMetadata
    {
        public Guid JobId { get; set; }
        public Type JobType { get; }
        public string JobName { get; }
        public string CronExpression { get; }
        public JobMetadata(Guid Id, Type jobType, string jobName,
        string cronExpression)
        {
            JobId = Id;
            JobType = jobType;
            JobName = jobName;
            CronExpression = cronExpression;
        }
    }
}
