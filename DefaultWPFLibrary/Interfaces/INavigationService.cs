using DefaultWPFLibrary.Core;

namespace DefaultWPFLibrary.Interfaces;

public interface INavigationService
{
    ViewModel GetCurrentViewModel();
    RelayCommand Navigate(ViewModel viewModel);
    RelayCommand NavigateTo<TViewModel>() where TViewModel : ViewModel;
}
