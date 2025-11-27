using nosso_apartamento.Utils;
using System.Windows.Input;

namespace nosso_apartamento.Views;

public partial class LoginPage : ContentPage
{
    public ICommand LoginCommand { get; }

    private string SenhaPadrao = string.Empty; // Buscar de forma segura em um ambiente real

    public LoginPage()
	{
		InitializeComponent();
        CarregarDados();
        LoginCommand = new Command(async () => await EntrarAsync());
        BindingContext = this;
    }

    private async Task EntrarAsync()
    {
        if (SenhaEntry.Text == SenhaPadrao && !SenhaPadrao.Equals(string.Empty))
        {
            await Shell.Current.GoToAsync("//home");
        }
        else
        {
            await DisplayAlertAsync("Erro", "Senha incorreta.", "OK");
        }
    }

    private async Task CarregarDados()
    {
        if(SenhaPadrao.Equals(string.Empty))
        {
            await ConfiguracaoApp.CarregarConfiguracaoAsync();

            SenhaPadrao = ConfiguracaoApp.SenhaAdmin;
        }
    }
}