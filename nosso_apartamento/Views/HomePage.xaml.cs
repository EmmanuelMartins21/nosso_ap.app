using System.Windows.Input;

namespace nosso_apartamento.Views;

public partial class HomePage : ContentPage
{
	public ICommand GoToListaComprasCommand { get; }

    public HomePage()
	{
		InitializeComponent();
		GoToListaComprasCommand = new Command(async () => await IrParaListaCompras());
		BindingContext = this;
	}
	private async Task IrParaListaCompras() => await Shell.Current.GoToAsync("//listaCompras");
}

   