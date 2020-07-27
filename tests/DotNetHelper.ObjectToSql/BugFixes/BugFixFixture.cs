
using System.Data.SqlClient;
using System.Threading;
using DotNetHelper.FastMember.Extension;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Enum;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.BugFixes
{
    public class BugReadOnlyBreakUpsertStatement
    {

        [SqlColumn(SetIsReadOnly = true)]
        public string IsReadOnly { get; set; }

        [SqlColumn(SetMaxColumnSize = 50)]
        public string MaxColumnSize { get; set; }

        [SqlColumn(SetPrimaryKey = true)]
        public string PrimaryKey { get; set; }

        [SqlColumn(SetIsIdentityKey = true)]
        public string IdentityKey { get; set; }

    }

    public class BugFixFixture : BaseTest
    {



        [SetUp]
        public void Setup()
        {

        }
        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void Test_EnsureAttributeValueAreAccurate()
        {
            var members = ExtFastMember.GetMemberWrappers<BugReadOnlyBreakUpsertStatement>(true);
            foreach (var member in members)
            {
                if (member.Name == "IsReadOnly")
                {
                    Assert.That(member.GetCustomAttribute<SqlColumnAttribute>().IsReadOnly == true);
                }
                if (member.Name == "MaxColumnSize")
                {
                    Assert.That(member.GetCustomAttribute<SqlColumnAttribute>().MaxColumnSize == 50);
                }
                if (member.Name == "PrimaryKey")
                {
                    Assert.That(member.GetCustomAttribute<SqlColumnAttribute>().PrimaryKey == true);
                }
                if (member.Name == "IdentityKey")
                {
                    Assert.That(member.GetCustomAttribute<SqlColumnAttribute>().IsIdentityKey == true);
                }
            }

        }


        [Test]
        public void Test_MultiThreadBuildQuery()
        {
            var instance = new BugReadOnlyBreakUpsertStatement();
            var obj2Sql = new Services.ObjectToSql(DataBaseType.SqlServer);

            for (var i2 = 0; i2 < 200; i2++)
            {
                Thread[] threads = new Thread[6];

                for (int i = 0; i < threads.Length; i++)
                {
                    threads[i] = new Thread(delegate (object sdo)
                    {
                        var quwery = obj2Sql.BuildQuery(ActionType.Insert, instance);
                        var parameters = obj2Sql.BuildDbParameterList(instance, (s, o) => new SqlParameter(s, o));
                    });
                }

                foreach (Thread thread in threads)
                {
                    thread.Start();
                }

                foreach (Thread thread in threads)
                {
                    thread.Join();
                }
            }
        }



    }
}
