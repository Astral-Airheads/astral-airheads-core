using System;

namespace Matt.Validation;


[Serializable]
public class MustBeEqualException : Exception
{
	public MustBeEqualException() { }
	public MustBeEqualException(string message) : base(message) { }
	public MustBeEqualException(string message, Exception inner) : base(message, inner) { }

	[Obsolete]
	protected MustBeEqualException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
