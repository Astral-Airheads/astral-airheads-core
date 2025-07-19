using System;

namespace Matt.AddIn;


[Serializable]
public class AddInException : Exception
{
	public AddInException() { }
	public AddInException(string message) : base(message) { }
	public AddInException(string message, Exception inner) : base(message, inner) { }

	[Obsolete]
	protected AddInException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
