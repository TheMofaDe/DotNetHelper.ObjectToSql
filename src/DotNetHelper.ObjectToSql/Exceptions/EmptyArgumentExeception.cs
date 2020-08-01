using System;
using System.Runtime.Serialization;

namespace DotNetHelper.ObjectToSql.Exceptions
{
	/// <inheritdoc />
	/// <summary>
	/// Custom Exception that lets the user know a code change is required to fix this error
	/// </summary>
	[Serializable()]
	public class EmptyArgumentException : System.Exception
	{


		/// <inheritdoc />
		/// <summary>
		/// Create the exception with description
		/// </summary>
		/// <param name="message">Exception description</param>
		internal EmptyArgumentException(string message) : base(message)
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
		protected EmptyArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}
	}
}