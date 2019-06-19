﻿using System;
using System.Runtime.Serialization;

namespace DotNetHelper.ObjectToSql.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Custom Exception that is thrown when attempted to access properties of object that isn't decorated with either [Key] attribute , or [SqlColumn(SetPrimaryKey=true)] attribute 
    /// </summary>
    [Serializable()]
    public class NoValidRuntimeAttributeException : System.Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Just create the exception
        /// </summary>
        public NoValidRuntimeAttributeException() : base()
        {

        }

        /// <inheritdoc />
        /// <summary>
        /// Create the exception with description
        /// </summary>
        /// <param name="message">Exception description</param>
        public NoValidRuntimeAttributeException(string message) : base(message)
        {

        }

        /// <inheritdoc />
        /// <summary>
        /// Create the exception with description and inner cause
        /// </summary>
        /// <param name="message">Exception description</param>
        /// <param name="innerException">Exception inner cause</param>
        public NoValidRuntimeAttributeException(string message, System.Exception innerException) : base(message, innerException)
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
        protected NoValidRuntimeAttributeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
