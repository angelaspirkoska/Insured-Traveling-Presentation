using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace InsuredTraveling.SignalR.PipelineModules
{
    public class ExceptionPipelineModule : HubPipelineModule
    {
        TraceSource trace = new TraceSource("HubExceptions");
        public ExceptionPipelineModule()
        {
            //var consoleListener = new ConsoleTraceListener();
            //trace.Listeners.Add(consoleListener);
        }
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            //1. log
            try
            {
                MethodDescriptor method = invokerContext.MethodDescriptor;
                var msg = string.Format($"Exception thrown by: {method.Hub.Name}.{method.Name}({string.Join(", ", invokerContext.Args)}):{exceptionContext.Error}");
                trace.TraceError(msg);
                Debug.WriteLine(msg);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            //2. inform client of exception
            //try
            //{
            //    invokerContext.Hub.Clients.Caller.notifyOfException();
            //}
            //finally { }

            //3. propagate error back the normal way base.
            OnIncomingError(exceptionContext, invokerContext);
        }
    }
}