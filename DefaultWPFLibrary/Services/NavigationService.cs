using DefaultWPFLibrary.Core;
using DefaultWPFLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultWPFLibrary.Services;

public class NavigationService : INavigationService
{
    private INavigationSystem _navigationSystem;
    public NavigationService(INavigationSystem navigationSystem)
    {
        _navigationSystem = navigationSystem;
    }
    public RelayCommand NavigateTo<TViewModel>() where TViewModel : ViewModel
    {
        return new RelayCommand(o =>
        {
            _navigationSystem.NavigateTo<TViewModel>();
        }, o => true);
    }
    public RelayCommand Navigate(ViewModel viewModel)
    {
        return new RelayCommand(o =>
        {
            _navigationSystem.Navigate(viewModel);
        }, o => true);
    }
    public ViewModel GetCurrentViewModel()
    {
        return _navigationSystem.CurrentViewModel;
    }
}
