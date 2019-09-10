using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Ronds.Epm.EntityFrameworkCore.NpgsqlJsonPlugin.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ronds.Epm.EntityFrameworkCore.NpgsqlJsonPlugin.Extensions
{

    public class NpgsqlJsonOptionsExtension : IDbContextOptionsExtension
    {
        public virtual string LogFragment => "using JSONB ";

        public virtual bool ApplyServices(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsqlJson();
            return false;
        }

        public virtual long GetServiceProviderHashCode() => 0;

        public virtual void Validate(IDbContextOptions options)
        {
            var internalServiceProvider = options.FindExtension<CoreOptionsExtension>()?.InternalServiceProvider;
            if (internalServiceProvider != null)
            {
                using (var scope = internalServiceProvider.CreateScope())
                {
                    if (scope.ServiceProvider.GetService<IEnumerable<IRelationalTypeMappingSourcePlugin>>()
                            ?.Any(s => s is NpgsqlJsonTypeMappingSourcePlugin) != true)
                    {
                        throw new InvalidOperationException($"{nameof(NpgsqlJsonDbContextOptionsBuilderExtensions.UseJSON)} requires {nameof(NpgsqlJsonServiceCollectionExtensions.AddEntityFrameworkNpgsqlJson)} to be called on the internal service provider used.");
                    }
                }
            }
        }
    }
}
