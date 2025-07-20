using System;
using System.IO;
using Matt.Validation;
using Xunit;

namespace Matt;

public class ValidationTests
{
    #region NotNull Tests

    [Fact]
    public void NotNull_WithValidObject_ShouldNotThrow()
    {
        // Arrange
        var obj = new object();

        // Act & Assert
        var exception = Record.Exception(() => Requires.NotNull(obj));
        Assert.Null(exception);
    }

    [Fact]
    public void NotNull_WithNullObject_ShouldThrowArgumentNullException()
    {
        // Arrange
        object? obj = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Requires.NotNull(obj));
        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void NotNull_WithNullObjectAndCustomParamName_ShouldThrowArgumentNullException()
    {
        // Arrange
        object? obj = null;
        const string paramName = "customParam";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Requires.NotNull(obj, paramName));
        Assert.Equal(paramName, exception.ParamName);
    }

    #endregion

    #region NotNull bool? Tests

    [Fact]
    public void NotNull_WithValidBool_ShouldNotThrow()
    {
        // Arrange
        bool? value = true;

        // Act & Assert
        var exception = Record.Exception(() => Requires.NotNull(value));
        Assert.Null(exception);
    }

    [Fact]
    public void NotNull_WithNullBool_ShouldThrowArgumentNullException()
    {
        // Arrange
        bool? value = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Requires.NotNull(value));
        Assert.Equal("value", exception.ParamName);
    }

    #endregion

    #region NotNull int? Tests

    [Fact]
    public void NotNull_WithValidInt_ShouldNotThrow()
    {
        // Arrange
        int? value = 42;

        // Act & Assert
        var exception = Record.Exception(() => Requires.NotNull(value));
        Assert.Null(exception);
    }

    [Fact]
    public void NotNull_WithNullInt_ShouldThrowArgumentNullException()
    {
        // Arrange
        int? value = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Requires.NotNull(value));
        Assert.Equal("value", exception.ParamName);
    }

    #endregion

    #region NotNull float? Tests

    [Fact]
    public void NotNull_WithValidFloat_ShouldNotThrow()
    {
        // Arrange
        float? value = 3.14f;

        // Act & Assert
        var exception = Record.Exception(() => Requires.NotNull(value));
        Assert.Null(exception);
    }

    [Fact]
    public void NotNull_WithNullFloat_ShouldThrowArgumentNullException()
    {
        // Arrange
        float? value = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Requires.NotNull(value));
        Assert.Equal("value", exception.ParamName);
    }

    #endregion

    #region NotNull IntPtr Tests

    [Fact]
    public void NotNull_WithValidIntPtr_ShouldNotThrow()
    {
        // Arrange
        var ptr = new IntPtr(123);

        // Act & Assert
        var exception = Record.Exception(() => Requires.NotNull(ptr));
        Assert.Null(exception);
    }

    [Fact]
    public void NotNull_WithZeroIntPtr_ShouldThrowArgumentNullException()
    {
        // Arrange
        var ptr = IntPtr.Zero;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Requires.NotNull(ptr));
        Assert.Equal("value", exception.ParamName);
    }

    #endregion

    #region NotNull string Tests

    [Fact]
    public void NotNull_WithValidString_ShouldNotThrow()
    {
        // Arrange
        string value = "test";

        // Act & Assert
        var exception = Record.Exception(() => Requires.NotNull(value));
        Assert.Null(exception);
    }

    [Fact]
    public void NotNull_WithNullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Requires.NotNull(value));
        Assert.Equal("value", exception.ParamName);
    }

    #endregion

    #region NotNull Generic Tests

    [Fact]
    public void NotNull_WithValidGenericObject_ShouldNotThrow()
    {
        // Arrange
        var obj = new TestClass();

        // Act & Assert
        var exception = Record.Exception(() => Requires.NotNull(obj));
        Assert.Null(exception);
    }

    [Fact]
    public void NotNull_WithNullGenericObject_ShouldThrowArgumentNullException()
    {
        // Arrange
        TestClass? obj = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Requires.NotNull(obj));
        Assert.Equal("value", exception.ParamName);
    }

    #endregion

    #region NotNullOrWhitespace Tests

    [Fact]
    public void NotNullOrWhitespace_WithValidString_ShouldNotThrow()
    {
        // Arrange
        string value = "test";

        // Act & Assert
        var exception = Record.Exception(() => Requires.NotNullOrWhitespace(value));
        Assert.Null(exception);
    }

    [Fact]
    public void NotNullOrWhitespace_WithNullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Requires.NotNullOrWhitespace(value));
        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void NotNullOrWhitespace_WithEmptyString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string value = "";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Requires.NotNullOrWhitespace(value));
        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void NotNullOrWhitespace_WithWhitespaceString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string value = "   ";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Requires.NotNullOrWhitespace(value));
        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void NotNullOrWhitespace_WithTabString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string value = "\t";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Requires.NotNullOrWhitespace(value));
        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void NotNullOrWhitespace_WithNewlineString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string value = "\n";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Requires.NotNullOrWhitespace(value));
        Assert.Equal("value", exception.ParamName);
    }

    #endregion

    #region FileExists Tests

    [Fact]
    public void FileExists_WithExistingFile_ShouldNotThrow()
    {
        // Arrange
        string tempFile = Path.GetTempFileName();

        try
        {
            // Act & Assert
            var exception = Record.Exception(() => Requires.FileExists(tempFile));
            Assert.Null(exception);
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void FileExists_WithNonExistingFile_ShouldThrowFileNotFoundException()
    {
        // Arrange
        string nonExistingFile = Path.Combine(Path.GetTempPath(), "NonExistingFile.txt");

        // Act & Assert
        var exception = Assert.Throws<FileNotFoundException>(() => Requires.FileExists(nonExistingFile));
        Assert.Contains("NonExistingFile.txt", exception.Message);
    }

    [Fact]
    public void FileExists_WithNonExistingFileAndCustomFileName_ShouldThrowFileNotFoundException()
    {
        // Arrange
        string nonExistingFile = Path.Combine(Path.GetTempPath(), "NonExistingFile.txt");
        const string customFileName = "CustomFileName.txt";

        // Act & Assert
        var exception = Assert.Throws<FileNotFoundException>(() => Requires.FileExists(nonExistingFile, customFileName));
        Assert.Contains(customFileName, exception.Message);
    }

    #endregion

    #region Helper Classes

    private class TestClass
    {
        public TestClass()
        {
        }
    }

    #endregion
}
