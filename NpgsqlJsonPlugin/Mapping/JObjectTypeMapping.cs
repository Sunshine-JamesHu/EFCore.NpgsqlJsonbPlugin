using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ronds.Epm.EntityFrameworkCore.NpgsqlJsonPlugin.Mapping
{
    internal class JObjectTypeMapping : NpgsqlTypeMapping
    {
        public JObjectTypeMapping() : base("jsonb", typeof(JObject), NpgsqlTypes.NpgsqlDbType.Jsonb)
        {
        }

        protected JObjectTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters, NpgsqlTypes.NpgsqlDbType.Jsonb)
        {
        }

        public override RelationalTypeMapping Clone(string storeType, int? size)
            => new JObjectTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size));

        public override CoreTypeMapping Clone(ValueConverter converter)
            => new JObjectTypeMapping(Parameters.WithComposedConverter(Converter.ComposeWith(converter)));

        public override ValueConverter Converter => new JObjectConverter();

        private class JObjectConverter : ValueConverter<JObject, string>
        {
            public JObjectConverter() : base(model => model.ToString(Formatting.None), json => JObject.Parse(json)) //JObject.Parse(json))
            {
            }

            //public JObjectConverter() : base(model => model != null ? JsonConvert.SerializeObject(model) : null, json => JObject.Parse(json)) //JObject.Parse(json))
            //{
            //}
        }
    }
}
