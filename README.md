#### 使用说明 ####

 挂载文件
 builder.UseNpgsql(connectionString, p => p.UseJSON());

 实体特性
 [Column(name: "attr", TypeName = "jsonb")]
 public virtual JObject AttrObject { get; set; }

 使用时
 var data = query.Where(p=> (string)m.AttrObject["LowerFreq"]  == "Test").ToList();

 生成的SQL
 SELECT p.attr as 'Attr' Where p.attr->'LowerFreq' = 'Test'

 #### 环境需要 ####
 Npgsql.EntityFrameworkCore.PostgreSQL >= 2.2.4