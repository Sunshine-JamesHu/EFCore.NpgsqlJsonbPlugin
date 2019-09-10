using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using Ronds.Epm.EntityFrameworkCore.NpgsqlJsonPlugin.Mapping;

namespace Ronds.Epm.EntityFrameworkCore.NpgsqlJsonPlugin.Plugins
{
    public class NpgsqlJsonTypeMappingSourcePlugin : IRelationalTypeMappingSourcePlugin
    {
        private readonly JObjectTypeMapping _jObject = new JObjectTypeMapping();
        private readonly JTokenTypeMapping _jToken = new JTokenTypeMapping();
        readonly NpgsqlJsonbTypeMapping _jsonb = new NpgsqlJsonbTypeMapping();

        public ConcurrentDictionary<string, RelationalTypeMapping[]> StoreTypeMappings { get; }
        public ConcurrentDictionary<Type, RelationalTypeMapping> ClrTypeMappings { get; }


        public NpgsqlJsonTypeMappingSourcePlugin()
        {

            var storeTypeMappings = new Dictionary<string, RelationalTypeMapping[]>(StringComparer.OrdinalIgnoreCase)
            {
                { "jsonb", new RelationalTypeMapping[]{_jObject,_jToken,_jsonb} },
            };
            var clrTypeMappings = new Dictionary<Type, RelationalTypeMapping>
            {
                { typeof(JObject), _jObject },
                { typeof(JToken), _jToken },
            };
            StoreTypeMappings = new ConcurrentDictionary<string, RelationalTypeMapping[]>(storeTypeMappings, StringComparer.OrdinalIgnoreCase);
            ClrTypeMappings = new ConcurrentDictionary<Type, RelationalTypeMapping>(clrTypeMappings);

        }
        public RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo)
        {
            var clrType = mappingInfo.ClrType;
            var storeTypeName = mappingInfo.StoreTypeName;
            var storeTypeNameBase = mappingInfo.StoreTypeNameBase;
            if (storeTypeName != null)
            {
                if (StoreTypeMappings.TryGetValue(storeTypeName, out var mappings))
                {
                    if (clrType == null)
                        return mappings[0];

                    foreach (var m in mappings)
                        if (m.ClrType == clrType)
                            return m;

                    return null;
                }
                if (StoreTypeMappings.TryGetValue(storeTypeNameBase, out mappings))
                {
                    if (clrType == null)
                        return mappings[0].Clone(in mappingInfo);

                    foreach (var m in mappings)
                        if (m.ClrType == clrType)
                            return m.Clone(in mappingInfo);
                    return null;
                }
            }
            if (clrType == null || !ClrTypeMappings.TryGetValue(clrType, out var mapping))
                return null;
            // All PostgreSQL date/time types accept a precision except for date
            // TODO: Cache size/precision/scale mappings?
            //  return mappingInfo.Precision.HasValue && mapping.ClrType != typeof(LocalDate)
            //  ? mapping.Clone($"{mapping.StoreType}({mappingInfo.Precision.Value})", null)
            //  : mapping;
            return mapping;
        }
    }
}
