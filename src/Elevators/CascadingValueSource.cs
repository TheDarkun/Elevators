using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Elevators;

/// <summary>
/// Static utility class for creating cascading value sources that notify on property changes.
/// </summary>
public static class CascadingValueSource
{
    /// <summary>
    /// Creates a cascading value source that listens for property changes on the provided value.
    /// </summary>
    /// <typeparam name="T">The type of the value, which must implement INotifyPropertyChanged.</typeparam>
    /// <param name="value">The value to be cascaded and monitored for property changes.</param>
    /// <param name="isFixed">Indicates whether the value is fixed. Default is false.</param>
    /// <returns>A CascadingValueSource instance that notifies about property changes.</returns>
    public static CascadingValueSource<T> CreateNotifying<T>(T value, bool isFixed = false) where T : INotifyPropertyChanged
    {
        // Create a new instance of CascadingValueSource with the specified value and isFixed flag
        var source = new CascadingValueSource<T>(value, isFixed);

        // Subscribe to the PropertyChanged event of the value
        value.PropertyChanged += (sender, args) => source.NotifyChangedAsync();

        // Return the configured source
        return source;
    }
}