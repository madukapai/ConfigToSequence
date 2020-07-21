using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigToSequence
{
    class Program
    {
       static DbEntities db = new DbEntities();

        static void Main(string[] args)
        {
            // 找出Config的Id
            string strSql = "Select * From Config Where ConfigName Like '%Id'";
            List<Config> config = db.Config.SqlQuery(strSql, new object[] { }).ToList();

            for (int i = 0; i < config.Count; i++)
            {
                var seq = db.Database.SqlQuery<object>($"SELECT * FROM sys.sequences WHERE name = '{config[i].ConfigName}'").FirstOrDefault();

                if (seq == null)
                {
                    string strCreateSeq = $"CREATE SEQUENCE {config[i].ConfigName} AS INT START WITH {(int.Parse(config[i].ConfigValue) + 1).ToString()} INCREMENT BY 1";
                    db.Database.ExecuteSqlCommand(strCreateSeq);

                    Console.WriteLine($"{config[i].ConfigName} 已建立");
                }
                else
                {
                    //string strDropSeq = $"DROP SEQUENCE {config[i].ConfigName}";
                    //db.Database.ExecuteSqlCommand(strDropSeq);

                    Console.WriteLine($"{config[i].ConfigName} 已存在");
                }
            }

            Console.WriteLine($"完成");
            Console.ReadKey();

            // 移除Config裡的Id
            // db.Config.RemoveRange(config);
        }
    }
}
