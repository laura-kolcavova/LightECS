using LightECS;
using Xunit.Categories;

namespace XunitTests;

[Category("unit")]
[Category("coverage")]
public sealed class ContextStateTests
{
    [Fact]
    public void Set_ShouldStoreValue()
    {
        // Arrange
        var state = new ContextState();
        var key = "myKey";
        var value = 123;

        // Act
        var result = state.Set(key, value);

        // Assert
        Assert.Equal(value, result);
        Assert.True(state.Contains(key));
    }

    [Fact]
    public void Get_ShouldReturnStoredValue()
    {
        // Arrange
        var state = new ContextState();
        var key = "myKey";
        var value = "hello";
        state.Set(key, value);

        // Act
        var result = state.Get<string>(key);

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void Get_ShouldReturnDefault_WhenKeyDoesNotExist()
    {
        // Arrange
        var state = new ContextState();
        var key = "nonexistent";

        // Act
        var result = state.Get<int>(key);

        // Assert
        Assert.Equal(default(int), result);
    }

    [Fact]
    public void Get_ShouldThrowInvalidCastException_WhenTypeMismatch()
    {
        // Arrange
        var state = new ContextState();
        var key = "myKey";
        state.Set(key, 123);

        // Act & Assert
        Assert.Throws<InvalidCastException>(() => state.Get<string>(key));
    }

    [Fact]
    public void GetRequired_ShouldReturnStoredValue()
    {
        // Arrange
        var state = new ContextState();
        var key = "myKey";
        var value = 42.5;
        state.Set(key, value);

        // Act
        var result = state.GetRequired<double>(key);

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void GetRequired_ShouldThrowKeyNotFoundException_WhenKeyMissing()
    {
        // Arrange
        var state = new ContextState();
        var key = "missing";

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => state.GetRequired<string>(key));
    }

    [Fact]
    public void GetRequired_ShouldThrowInvalidCastException_WhenTypeMismatch()
    {
        // Arrange
        var state = new ContextState();
        var key = "myKey";
        state.Set(key, true);

        // Act & Assert
        Assert.Throws<InvalidCastException>(() => state.GetRequired<int>(key));
    }

    [Fact]
    public void Unset_ShouldRemoveKey()
    {
        // Arrange
        var state = new ContextState();
        var key = "toRemove";
        state.Set(key, "value");

        // Act
        state.Unset(key);

        // Assert
        Assert.False(state.Contains(key));
    }

    [Fact]
    public void Clear_ShouldRemoveAllKeys()
    {
        // Arrange
        var state = new ContextState();
        state.Set("key1", 1);
        state.Set("key2", 2);

        // Act
        state.Clear();

        // Assert
        Assert.False(state.Contains("key1"));
        Assert.False(state.Contains("key2"));
    }

    [Fact]
    public void Contains_ShouldReturnTrue_WhenKeyExists()
    {
        // Arrange
        var state = new ContextState();
        state.Set("existing", 10);

        // Act
        var result = state.Contains("existing");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_ShouldReturnFalse_WhenKeyDoesNotExist()
    {
        // Arrange
        var state = new ContextState();

        // Act
        var result = state.Contains("missing");

        // Assert
        Assert.False(result);
    }
}
