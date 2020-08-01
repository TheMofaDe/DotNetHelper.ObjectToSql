using System;
using System.Runtime.Serialization;

namespace DotNetHelper.ObjectToSql.Exceptions
{
	/// <inheritdoc />
	/// <summary>
	/// Custom Exception that is thrown when attempted to access properties of object that isn't decorated with either [Key] attribute , or [SqlColumn(SetPrimaryKey=true)] attribute 
	/// </summary>
	[Serializable()]
	public class MissingKeyAttributeException : System.Exception
	{


		/// <inheritdoc />
		/// <summary>
		/// Create the exception with description
		/// </summary>
		/// <param name="message">Exception description</param>
		internal MissingKeyAttributeException(string message) : base(message)
		{

		}



		/// <inheritdoc />
		/// <summary>
		/// Create the exception from serialized data.
		/// Usual scenario is when exception is occured somewhere on the remote workstation
		/// and we have to re-create/re-throw the exception on the local machine
		/// </summary>
		/// <param name="info">Serialization info</param>
		/// <param name="context">Serialization context</param>
		protected MissingKeyAttributeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}
	}
}