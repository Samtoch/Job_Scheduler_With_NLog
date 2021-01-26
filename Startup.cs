using Mailer_Scheduler.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Mailer_Scheduler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Quartz services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // Job
            services.AddSingleton<Mailer>();
            services.AddSingleton<Mailer_2>();
            services.AddSingleton(new JobSchedule(jobType: typeof(Mailer), cronExpression: "0/5 * * * * ?"));    // run every 05 seconds
            services.AddSingleton(new JobSchedule(jobType: typeof(Mailer_2), cronExpression: "0/20 * * * * ?")); // run every 20 seconds
            services.AddHostedService<QuartzHostedService>();


            //services.AddSingleton(provider => GetScheduler()); // /Views/Home/Index.cshtml

            //services.AddSingleton<IJobFactory, CustomQuartzJobFactory>();
            //services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            //services.AddSingleton<Mailer>();
            //services.AddSingleton<Mailer_2>();
            //services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(Mailer), "10 SECONDS LOGGER JOB", "0/10 * * * * ?"));
            //services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(Mailer_2), "20 SECONDS LOGGER JOB", "0/20 * * * * ?"));
            //services.AddHostedService<CustomQuartzHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        //private IScheduler GetScheduler()
        //{
        //    var properties = new NameValueCollection
        //    {
        //        ["quartz.scheduler.instanceName"] = "Mailer_Scheduler",
        //        ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
        //        ["quartz.threadPool.threadCount"] = "3",
        //        ["quartz.jobStore.type"] = "Quartz.Simpl.RAMJobStore, Quartz",
        //    };
        //    var schedulerFactory = new StdSchedulerFactory();
        //    var scheduler = schedulerFactory.GetScheduler().Result;
        //    scheduler.Start();
        //    return scheduler;
        //}
    }
}
