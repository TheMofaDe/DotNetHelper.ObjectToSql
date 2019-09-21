using System;
using DotNetHelper.ObjectToSql.Enum;

#pragma warning disable IDE1006 // Naming Styless
// ReSharper disable InconsistentNaming

namespace DotNetHelper.ObjectToSql.Attribute
{




    /// <inheritdoc />
    /// <summary>
    /// Class SqlColumnAttribute.
    /// </summary>
    /// <seealso cref="T:System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class SqlColumnAttribute : System.Attribute
    {
        /// <summary>
        /// Defaults To MAX used for creating table
        /// </summary>
        /// <value>The maximum size of the column.</value>
        public int? MaxColumnSize { get; set; }

        /// <summary>
        /// Gets or sets the size of the set maximum column.
        /// </summary>
        /// <value>The size of the set maximum column.</value>
        /// <exception cref="Exception">Nooo...  Your Using SqlColumnAttribute wrong do not try to get from the Set Property use the orignial</exception>
        public int SetMaxColumnSize
        {
            get => throw new Exception("Nooo...  Your Using SqlColumnAttribute wrong do not try to get from the Set Property use the orignial ");
            set => MaxColumnSize = value;
        }

        /// <summary>
        /// Defaults To MAX used for creating table
        /// </summary>
        /// <value>The maximum size of the column.</value>
        public SerializableType SerializableType { get; set; } = SerializableType.None;

        /// <summary>
        /// Gets or sets the automatic increment by. If this value is set then the property will be treated as an IDENTITY column
        /// </summary>
        /// <value>The automatic increment by.</value>
        public bool? IsIdentityKey { get; set; } = null;
        /// <summary>
        /// Gets or sets the set automatic increment by.
        /// </summary>
        /// <value>The set automatic increment by.</value>
        /// <exception cref="Exception">Nooo...  Your Using SqlColumnAttribute wrong do not try to get from the Set Property use the orignial</exception>
        public bool SetIsIdentityKey
        {
            get => throw new Exception("Nooo...  Your Using SqlColumnAttribute wrong do not try to get from the Set Property use the orignial ");
            set => IsIdentityKey = value;
        }


        /// <summary>
        /// If true this property will never be included when creating insert sql. This is meant for senarios where you want to use the database default value
        /// </summary>
        /// <value>The automatic increment by.</value>
        public bool? IsReadOnly { get; set; } = null;
        /// <summary>
        /// If true this property will never be included when creating insert sql.This is meant for senarios where you want to use the database default value
        /// </summary>
        /// <value>The set automatic increment by.</value>
        /// <exception cref="Exception">Nooo...  Your Using SqlColumnAttribute wrong do not try to get from the Set Property use the orignial</exception>
        public bool SetIsReadOnly
        {
            get => throw new Exception("Nooo...  Your Using SqlColumnAttribute wrong do not try to get from the Set Property use the orignial ");
            set => IsIdentityKey = value;
        }


      
        /// <summary>
        /// Gets or sets a value indicating whether [primary key].
        /// </summary>
        /// <value><c>null</c> if [primary key] contains no value, <c>true</c> if [primary key]; otherwise, <c>false</c>.</value>
        public bool? PrimaryKey { get; set; } = null;
        /// <summary>
        /// Gets or sets a value indicating whether [set primary key].
        /// </summary>
        /// <value><c>true</c> if [set primary key]; otherwise, <c>false</c>.</value>
        /// <exception cref="Exception">Nooo...  Your Using SqlColumnAttribute wrong do not try to get from the Set Property use the orignial</exception>
        public bool SetPrimaryKey
        {
            get => throw new Exception("Nooo...  Your Using SqlColumnAttribute wrong do not try to get from the Set Property use the orignial ");
            set => PrimaryKey = value;
        }


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SqlColumnAttribute"/> is nullable.
        /// </summary>
        /// <value><c>null</c> if [nullable] contains no value, <c>true</c> if [nullable]; otherwise, <c>false</c>.</value>
        public bool? Nullable { get; set; } = null;
        /// <summary>
        /// Gets or sets a value indicating whether [set nullable].
        /// </summary>
        /// <value><c>true</c> if [set nullable]; otherwise, <c>false</c>.</value>
        /// <exception cref="Exception">Nooo...  Your Using SqlColumnAttribute wrong do not try to get from the Set Property use the orignial</exception>
        public bool SetNullable
        {
            get => throw new Exception("Nooo...  Your Using SqlColumnAttribute wrong do not try to get from the Set Property use the orignial ");
            set => Nullable = value;
        }
        

        /// <summary>
        /// If true property will be use when the class is being used by a DATASOURCE Object
        /// </summary>
        /// <value><c>null</c> if [ignore] contains no value, <c>true</c> if [ignore]; otherwise, <c>false</c>.</value>
        public bool? Ignore { get; set; } = null;
        /// <summary>
        /// Gets or sets a value indicating whether [set ignore].
        /// </summary>
        /// <value><c>true</c> if [set ignore]; otherwise, <c>false</c>.</value>
        /// <exception cref="Exception">Nooo...  Your Using SqlColumnAttribute wrong do not try to get from the Set Property use the orignial</exception>
        public bool SetIgnore
        {
            get => throw new Exception("Nooo...  Your Using SqlColumnAttribute wrong do not try to get from the Set Property use the orignial ");
            set => Ignore = value;
        }

        ///// <summary>
        ///// Gets or sets the default value.
        ///// </summary>
        ///// <value>The default value.</value>
        //public object DefaultValue { get; set; }


        /// <summary>
        /// If true property will be use when the class is being used by a DATASOURCE Object
        /// </summary>
        /// <value>The map to.</value>
        public string MapTo { get; set; } = null;



        /// <summary>
        /// Gets or sets the mappings for keys to join with.
        /// </summary>
        /// <value>an array of ids, that will join a column to another table.</value>
        public string[] MappingIds = null;

    }




}
#pragma warning restore IDE1006 // Naming Styles