using DefaultWPFLibrary.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DefaultWPFLibrary.Interfaces;

public interface INavigationSystem
{
    ViewModel CurrentViewModel { get; }
    public void NavigateTo<TViewModel>() where TViewModel : ViewModel;
    public void Navigate(ViewModel viewModel);
    public ICommand NavigateToCommand<TViewModel>() where TViewModel : ViewModel;
    public ICommand NavigateCommand(ViewModel viewModel);
}
