using System;

namespace Matt.AddIn;


/// <summary>
/// Represents an error inside of the add-in.
/// </summary>
[Serializable]
public class AddInException : Exception
{
	/// <summary>
	/// Throws a new <seealso cref="AddInException"/>.
	/// </summary>
	public AddInException() { }

    /// <summary>
    /// Throws a new <seealso cref="AddInException"/>.
    /// </summary>
	/// <param name="message">The value of the error message.</param>
    public AddInException(string message) : base(message) { }

    /// <summary>
    /// Throws a new <seealso cref="AddInException"/>.
    /// </summary>
    /// <param name="message">The value of the error message.</param>
    /// <param name="inner">The value of the existing error that may be related to this one.</param>
    public AddInException(string message, Exception inner) : base(message, inner) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
	[Obsolete]
	protected AddInException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
