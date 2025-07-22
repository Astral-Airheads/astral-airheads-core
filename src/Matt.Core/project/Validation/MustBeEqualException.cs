using System;

namespace Matt.Validation;

// i must add an documentation otherwise <GenerateDocumentationFile> will
// shove a rabbit up in my ass.

/// <summary>
/// Represents an error if something is not equal to eachother.
/// </summary>
[Serializable]
public class MustBeEqualException : Exception
{
	/// <summary>
	/// Throws a new <seealso cref="MustBeEqualException"/>.
	/// </summary>
	public MustBeEqualException() { }

    /// <summary>
    /// Throws a new <seealso cref="MustBeEqualException"/>.
    /// </summary>
    public MustBeEqualException(string message) : base(message) { }

    /// <summary>
    /// Throws a new <seealso cref="MustBeEqualException"/>.
    /// </summary>
    public MustBeEqualException(string message, Exception inner) : base(message, inner) { }


    /// <summary>
    /// Throws a new <seealso cref="MustBeEqualException"/>.
    /// </summary>
    [Obsolete]
	protected MustBeEqualException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
