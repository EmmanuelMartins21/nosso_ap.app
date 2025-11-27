using nosso_apartamento.Models;
using System.Collections.ObjectModel;

namespace nosso_apartamento.Views;

public partial class ListaComprasPage : ContentPage
{
	public ObservableCollection<Compra> Compras { get; set; } = new ObservableCollection<Compra>();
    public ListaComprasPage()
	{
		InitializeComponent();
        CarregarDados();
        BindingContext = this;

    }
    private void CarregarDados()
    {
        // vai buscar os dados futuramente do supabase
        var item1 = new CompraItem
        {
            Nome = "Arroz",
            Quantidade = 2,
        };
        Compras.Add(new Compra {Titulo= "Compra Novembro", DataCriacao = DateTime.UtcNow, Itens = new List<CompraItem>() { item1} });
    }

    private async void Editar_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Compra compra)
        {
            // TODO: Implementar edição de compra
            await DisplayAlert("Info", "Edição em desenvolvimento", "OK");
        }
    }

    private async void Remover_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Compra compra)
        {
            var confirmacao = await DisplayAlert("Confirmar", $"Deseja remover '{compra.Titulo}'?", "Sim", "Não");
            
            if (confirmacao)
            {
                Compras.Remove(compra);
            }
        }
    }

    private async void Adicionar_Clicked(object sender, EventArgs e)
    {
        var novaCompraPage = new AdicionarCompraPage(onCompraSalva: (novaCompra) =>
        {
            Compras.Add(novaCompra);
        });

        await Navigation.PushModalAsync(novaCompraPage);
    }
}