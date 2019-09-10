using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Ronds.Epm.EntityFrameworkCore.NpgsqlJsonPlugin.Plugins;

namespace Ronds.Epm.EntityFrameworkCore.NpgsqlJsonPlugin.Extensions
{
    public static class NpgsqlJsonServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkNpgsqlJson(this IServiceCollection serviceCollection)
        {
            new EntityFrameworkRelationalServicesBuilder(serviceCollection)
                .TryAddProviderSpecificServices(
                    x => x.TryAddSingletonEnumerable<IRelationalTypeMappingSourcePlugin, NpgsqlJsonTypeMappingSourcePlugin>()
                        .TryAddSingletonEnumerable<IMethodCallTranslatorPlugin, NpgsqlJsonMethodCallTranslatorPlugin>());
            //   .TryAddSingletonEnumerable<IMemberTranslatorPlugin, NpgsqlNodaTimeMemberTranslatorPlugin>()

            //var iocManager = IocManager.Instance;
            //var isRegistered = iocManager.IsRegistered<IRelationalTypeMappingSourcePlugin>();
            //var isRegistered2 = iocManager.IsRegistered<IMethodCallTranslatorPlugin>();
            //if (!isRegistered)
            //    iocManager.Register<IRelationalTypeMappingSourcePlugin, NpgsqlJsonTypeMappingSourcePlugin>(DependencyLifeStyle.Singleton);

            //if (!isRegistered2)
            //    iocManager.Register<IMethodCallTranslatorPlugin, NpgsqlJsonMethodCallTranslatorPlugin>(DependencyLifeStyle.Singleton);


            return serviceCollection;
        }
    }
}
