using InsuredTraveling.DI;
using Ninject;
using Quartz;
using Quartz.Impl;
using System;

namespace InsuredTraveling.Schedulers
{
    public class JobScheduler
    {
        public static void Start()
        {
            //IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            //scheduler.Start();
            var kernel = InitializeNinjectKernel();
            var scheduler = kernel.Get<IScheduler>();

            IJobDetail job = JobBuilder.Create<EmailJob>().Build();

            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithDailyTimeIntervalSchedule
            //      (s =>
            //         s.WithIntervalInHours(24)
            //        .OnEveryDay()
            //        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 59))
            //      )
            //    .Build();
            scheduler.ScheduleJob(
             JobBuilder.Create<EmailJob>().Build(),
             TriggerBuilder.Create().WithSimpleSchedule(s => s.WithIntervalInHours(1).RepeatForever()).Build()); // privremeno

            // start scheduler
            scheduler.Start();

            //scheduler.ScheduleJob(job, trigger);



        }
        static IKernel InitializeNinjectKernel()
        {
            var kernel = new StandardKernel();

            // setup Quartz scheduler that uses our NinjectJobFactory
            kernel.Bind<IScheduler>().ToMethod(x =>
            {
                var sched = new StdSchedulerFactory().GetScheduler();
                sched.JobFactory = new NinjectJobFactory(kernel);
                return sched;
            });

            // add our bindings as we normally would (these are the bindings that our jobs require)
            kernel.Bind<ISavaVoucherService>().To<SavaVoucherService>();
            kernel.Bind<IPointsRequestService>().To<PointsRequestService>();
            kernel.Bind<IUserService>().To<UserService>();
           
            //// etc.

            return kernel;
        }
    }
}