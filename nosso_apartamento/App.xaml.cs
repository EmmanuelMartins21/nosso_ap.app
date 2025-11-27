using Microsoft.Extensions.DependencyInjection;
using nosso_apartamento.Services;
using Supabase;
using Supabase.Functions;
using System.Diagnostics;

namespace nosso_apartamento
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {
            try
            {
                var dbService = IPlatformApplication.Current?.Services.GetService<DbService>();
                if (dbService != null)
                {
                    await dbService.InitializeAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro na inicialização: {ex}");
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await MainPage!.DisplayAlert("Erro", $"Falha ao inicializar: {ex.Message}", "OK");
                });
            }


            await Shell.Current.GoToAsync("//login");
        }
    }
}