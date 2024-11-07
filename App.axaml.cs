using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaApplication1.ViewModels;
using AvaloniaApplication1.Views;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;

namespace AvaloniaApplication1
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            // ��ʼ�� ReactiveUI
            RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
        }

        public override void OnFrameworkInitializationCompleted()
        {
            ConfigureServices();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            // ע�� MessageBus Ϊ Singleton
            services.AddSingleton(MessageBus.Current);


            // ע�� ViewModel Ϊ Transient
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<AnotherViewModel>();
            // ע�� MainWindow
            services.AddTransient<MainWindow>();

            _serviceProvider = services.BuildServiceProvider();
        }
    }
}