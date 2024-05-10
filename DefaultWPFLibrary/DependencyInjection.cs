
using DefaultWPFLibrary.Core;
using DefaultWPFLibrary.Interfaces;
using DefaultWPFLibrary.Navigations;
using DefaultWPFLibrary.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using System.Windows;
using System.Reflection;


namespace DefaultWPFLibrary;

public static class DependencyInjection
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<INavigationService, NavigationService>();
    }
    public static void AddDefaultNavigationSystem(this IServiceCollection services)
    {
        services.AddSingleton<INavigationSystem, DefaultNavigationSystem>();
    }
    public static void RegisterPages(this IServiceCollection services, string pagesFolder, string viewModelsFolder)
    {
        var pages = Assembly.GetEntryAssembly()?.GetTypes()
            .Where(t => t.Namespace == pagesFolder && t.IsSubclassOf(typeof(Page)))
            .ToList();
        var viewModels = Assembly.GetEntryAssembly()?.GetTypes()
            .Where(t => t.Namespace == viewModelsFolder && t.IsSubclassOf(typeof(ViewModel)))
            .ToList();
        if (pages == null) return;
        if(viewModels == null) return;
        foreach (var page in pages)
        {
            if(page == null || viewModels == null)
            {
                continue;
            }
            var viewModel = viewModels.FirstOrDefault(vm => vm.Name == page.Name + "ViewModel");
            if (viewModel != null)
            {
                services.AddScoped(viewModel);
                AddPage(services, page, viewModel);
            }
        }
    }
    
    public static void AddScopedPage<TView, TViewModel>(this IServiceCollection services)
        where TView : Control
        where TViewModel : ViewModel
    {
        services.AddScoped<TViewModel>();
        AddPage<TView, TViewModel>(services);
    }
    public static void AddSingletonPage<TView, TViewModel>(this IServiceCollection services)
        where TView : Control
        where TViewModel : ViewModel
    {
        services.AddSingleton<TViewModel>();
        AddPage<TView, TViewModel>(services);
    }
    public static void AddTransientPage<TView, TViewModel>(this IServiceCollection services)
        where TView : Control
        where TViewModel : ViewModel
    {
        services.AddTransient<TViewModel>();
        AddPage<TView, TViewModel>(services);
    }
    private static void AddPage<TView, TViewModel>(this IServiceCollection services)
        where TView : Control
        where TViewModel : ViewModel
    {
        
        DataTemplate dataTemplate = new DataTemplate();
        FrameworkElementFactory elementFactory = new FrameworkElementFactory(typeof(TView));
        elementFactory.SetBinding(FrameworkElement.DataContextProperty, new System.Windows.Data.Binding());
        dataTemplate.VisualTree = elementFactory;

        // Register the DataTemplate for the specified view model type
        Application.Current.Resources.Add(new DataTemplateKey(typeof(TViewModel)), dataTemplate);
    }
    private static void AddPage(IServiceCollection services, Type page, Type viewModel)
    {
        DataTemplate dataTemplate = new DataTemplate();
        FrameworkElementFactory elementFactory = new FrameworkElementFactory(page);
        elementFactory.SetBinding(FrameworkElement.DataContextProperty, new System.Windows.Data.Binding());
        dataTemplate.VisualTree = elementFactory;

        // Register the DataTemplate for the specified view model type
        Application.Current.Resources.Add(new DataTemplateKey(viewModel), dataTemplate);
    }
}
