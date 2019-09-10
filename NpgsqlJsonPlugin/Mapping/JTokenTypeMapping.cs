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
    internal class JTokenTypeMapping : NpgsqlTypeMapping
    {
        public JTokenTypeMapping() : base("jsonb", typeof(JToken), NpgsqlTypes.NpgsqlDbType.Jsonb)
        {
        }

        protected JTokenTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters, NpgsqlTypes.NpgsqlDbType.Jsonb)
        {
        }

        public override RelationalTypeMapping Clone(string storeType, int? size)
            => new JTokenTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size));

        public override CoreTypeMapping Clone(ValueConverter converter)
            => new JTokenTypeMapping(Parameters.WithComposedConverter(Converter.ComposeWith(converter)));

        public override ValueConverter Converter => new JTokenConverter();

        private class JTokenConverter : ValueConverter<JToken, string>
        {
            public JTokenConverter() : base(model => model.ToString(Formatting.None), json => JToken.Parse(json))
            {
            }
        }
    }
}
