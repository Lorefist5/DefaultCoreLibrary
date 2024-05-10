using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DefaultWPFLibrary.Core;

public class RelayCommand : ICommand
{
    private readonly Predicate<object> _canExecute;
    private readonly Action<object> _execute;
    private readonly INotifyPropertyChanged _propertyChangedSource;

    public RelayCommand(Action<object> execute, Predicate<object> canExecute, INotifyPropertyChanged propertyChangedSource = null)
    {
        _canExecute = canExecute;
        _execute = execute;
        _propertyChangedSource = propertyChangedSource;

        if (_propertyChangedSource != null)
        {
            _propertyChangedSource.PropertyChanged += PropertyChangedSource_PropertyChanged;
        }
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }

    public void RaiseCanExecuteChanged()
    {
        OnCanExecuteChanged();
    }

    protected virtual void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    private void PropertyChangedSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        OnCanExecuteChanged();
    }
}
