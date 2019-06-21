using System;
using DotNetHelper.ObjectToSql.Enum;

namespace DotNetHelper.ObjectToSql.Attribute
{
    /// <inheritdoc />
    /// <summary>
    /// This specifies that the following property is also an SQL table
    /// </summary>
    /// <seealso cref="T:System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class DBTableAttribute : System.Attribute
    {
        /// <summary>
        /// The Sql Table name that this class data belongs to.
        /// </summary>
        /// <value>The map to.</value>
        public string TableName { get; set; } = null;

        public Type XReferenceTable { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether [x reference on delete cascade].
        /// </summary>
        /// <value><c>null</c> if [x reference on delete cascade] contains no value, <c>true</c> if [x reference on delete cascade]; otherwise, <c>false</c>.</value>
        public SQLJoinType JoinType { get; set; }
    }
}
