using DefaultWPFLibrary.Core;
using DefaultWPFLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DefaultWPFLibrary.Navigations;

public class DefaultNavigationSystem : ObservableObject, INavigationSystem
{
    private ViewModel _currentViewModel;
    private readonly Func<Type, ViewModel> _viewModelFactory;

    public ViewModel CurrentViewModel
    {
        get { return _currentViewModel; }
        set
        {
            _currentViewModel = value;
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }

    public DefaultNavigationSystem(Func<Type, ViewModel> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }
    /// <summary>
    /// Navigates to a new ViewModel.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the ViewModel to navigate to</typeparam>
    public void NavigateTo<TViewModel>() where TViewModel : ViewModel
    {
        ViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
        CurrentViewModel = viewModel;

    }
    /// <summary>
    /// Navigates to a new ViewModel.
    /// </summary>
    /// <param name="viewModel">The ViewModel to navigate to.</param>
    /// <remarks>
    /// Method for navigating to a ViewModel with custom Dependencies and parameters
    /// </remarks>
    public void Navigate(ViewModel viewModel)
    {
        CurrentViewModel = viewModel;

    }

    public ICommand NavigateCommand(ViewModel viewModel)
    {
        var navigateCommand = new RelayCommand(o =>
        {
            Navigate(viewModel);
        }, o => true);

        return navigateCommand;
    }
    public ICommand NavigateToCommand<TViewModel>() where TViewModel : ViewModel
    {
        var navigateCommand = new RelayCommand(o =>
        {
            NavigateTo<TViewModel>();
        }, o => true);

        return navigateCommand;
    }

}
