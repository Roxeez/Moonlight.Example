using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Moonlight.Example.ViewModels;
using Moonlight.Example.Views;
using Moonlight.Game.Maps;
using Moonlight.Translation;

namespace Moonlight.Example
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddSingleton<MoonlightAPI>(new MoonlightAPI
            {
                Language = Language.FR
            });
            services.AddTransient<MainViewModel>();

            IServiceProvider provider = services.BuildServiceProvider();

            MainView mainView = new MainView
            {
                DataContext = provider.GetService<MainViewModel>()
            };
            
            mainView.Show();
        }
    }
}