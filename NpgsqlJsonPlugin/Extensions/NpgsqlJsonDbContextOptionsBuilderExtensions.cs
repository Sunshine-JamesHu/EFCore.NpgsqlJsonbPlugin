using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ronds.Epm.EntityFrameworkCore.NpgsqlJsonPlugin.Extensions
{
    public static class NpgsqlJsonDbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Use NetTopologySuite to access SQL Server spatial data.
        /// </summary>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static NpgsqlDbContextOptionsBuilder UseJSON(this NpgsqlDbContextOptionsBuilder optionsBuilder)
        {
            // TODO: Global-only setup at the ADO.NET level for now, optionally allow per-connection?
            //     NpgsqlConnection.GlobalTypeMapper.AddMapping();//.UseNodaTime();

            var coreOptionsBuilder = ((IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder).OptionsBuilder;

            var extension = coreOptionsBuilder.Options.FindExtension<NpgsqlJsonOptionsExtension>()
                            ?? new NpgsqlJsonOptionsExtension();

            ((IDbContextOptionsBuilderInfrastructure)coreOptionsBuilder).AddOrUpdateExtension(extension);

            return optionsBuilder;
        }
    }
}
