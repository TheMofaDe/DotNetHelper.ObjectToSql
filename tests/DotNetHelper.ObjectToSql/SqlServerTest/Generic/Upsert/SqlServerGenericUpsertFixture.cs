using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Upsert
{
    public class SqlServerGenericUpsertFixture
    {

        public StringBuilder StringBuilder { get; set; }

        [SetUp]
        public void Setup()
        {
            StringBuilder = new StringBuilder();
        }

        [TearDown]
        public void Teardown()
        {
            StringBuilder.Clear();
        }

        [Test]
        public void Test_Generic_BuildUpsertQuery_Ensure_MissingKeyException_Is_Thrown_()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            Assert.That(() => sqlServerObjectToSql.BuildUpsertQuery<Employee>(StringBuilder, nameof(Employee)),
                Throws.Exception
                    .TypeOf<MissingKeyAttributeException>());
        }



    }
}