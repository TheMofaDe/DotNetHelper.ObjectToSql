namespace DotNetHelper.ObjectToSql.Enum
{
    public enum SQLJoinType
    {
        /// <summary>
        /// SQL-style LEFT OUTER JOIN semantics: All records of the left table are returned. If the right table holds no matching records, the right 
        /// side's columns contain NULL. 
        /// </summary>
        LEFT = 1,
        /// <summary>
        /// SQL-style INNER JOIN semantics: Only records that produce a match are returned.
        /// </summary>
        INNER = 2,

    }
}
