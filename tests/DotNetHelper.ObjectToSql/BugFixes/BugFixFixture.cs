using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DotNetHelper.FastMember.Extension;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Extension;
using DotNetHelper.ObjectToSql.Tests.Models;
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

    public  class BugFixFixture : BaseTest
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



    }
}
