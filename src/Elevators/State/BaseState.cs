using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Elevators.State;

public abstract class BaseState : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = default)
        => PropertyChanged?.Invoke(this, new(propertyName));
}