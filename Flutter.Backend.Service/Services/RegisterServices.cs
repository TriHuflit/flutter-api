using Flutter.Backend.Service.IServices;
using Flutter.Backend.DAL.Contracts;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RegisterServices
    {
        public static void AddInterfaceServices(this IServiceCollection @this)
        {
            // Register Interface Service
            var interfaceType = typeof(IProductServices);
            var assembly = interfaceType.Assembly;
            var definedTypes = assembly.DefinedTypes;

            var interfaceServices = definedTypes
                .Where(t => t.GetTypeInfo().IsInterface)
                .Where(t => t.GetTypeInfo().Name != "ICloundinarySetting");

            foreach (var interfaceService in interfaceServices)
            {
                var service = definedTypes
                 .Where(t => !t.GetTypeInfo().IsInterface)
                 .Where(t => t.GetTypeInfo().ImplementedInterfaces.Count() > 0)
                 .Where(t => t.GetTypeInfo().ImplementedInterfaces.FirstOrDefault().GetTypeInfo() == interfaceService).FirstOrDefault();

                @this.AddTransient(interfaceService,service);
            }

        }
    }
}
