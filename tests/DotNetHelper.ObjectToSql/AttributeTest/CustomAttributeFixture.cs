using DotNetHelper.ObjectToSql.Attribute;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.AttributeTest
{



    public class CustomAttributeFixture
    {

        [DBTableAttribute(TableName = "NotTestClass")]
        private class TestClass
        {
            public string A { get; set; }
        }

        [SetUp]
        public void Setup()
        {

        }
        [TearDown]
        public void Teardown()
        {

        }



    }
}
