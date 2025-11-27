using Microsoft.Extensions.DependencyInjection;

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
            await Shell.Current.GoToAsync("//login");
        }
    }
}