using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Dispatcher;

namespace InsuredTraveling
{
    public class CustomAssemblyResolver : IAssembliesResolver
    {
        public ICollection<Assembly> GetAssemblies()
        {
            var halkbankPaymentPluginPath = System.Web.Hosting.HostingEnvironment.MapPath("~/HalkbankPaymentPlugin"); List<Assembly> baseAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList(); string path = halkbankPaymentPluginPath + "\\HalkbankPayment.HalkbankSmartPayment.dll";

            var controllersAssembly = Assembly.LoadFrom(path); baseAssemblies.Add(controllersAssembly);

            return baseAssemblies;
        }


    }


}