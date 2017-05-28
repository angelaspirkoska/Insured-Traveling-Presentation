using InsuredTraveling.DI;
using Ninject;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Schedulers
{
    public class NinjectJobFactory : SimpleJobFactory
    {
        readonly IKernel _kernel;

        public NinjectJobFactory(IKernel kernel)
        {
            this._kernel = kernel;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                // this will inject dependencies that the job requires
                return (IJob)this._kernel.Get(bundle.JobDetail.JobType);
            }
            catch (Exception e)
            {
                throw new SchedulerException(string.Format("Problem while instantiating job '{0}' from the NinjectJobFactory.", bundle.JobDetail.Key), e);
            }
        }

    }
}