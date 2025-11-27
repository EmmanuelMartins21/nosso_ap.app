using nosso_apartamento.Models;
using nosso_apartamento.Services;
using System.Collections.ObjectModel;

namespace nosso_apartamento.Views;

public partial class ListaComprasPage : ContentPage
{
	public ObservableCollection<Compra> Compras { get; set; } = new ObservableCollection<Compra>();
    private DbService _dbService;

    public ListaComprasPage(DbService dbService)
	{
		InitializeComponent();
        _dbService = dbService;
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CarregarDados();
    }

    private async Task CarregarDados()
    {
        // vai buscar os dados futuramente do supabase
        //var item1 = new CompraItem
        //{
        //    Nome = "Arroz",
        //    Quantidade = 2,
        //};
        //Compras.Add(new Compra {Titulo= "Compra Novembro", DataCriacao = DateTime.UtcNow, Itens = new List<CompraItem>() { item1} });

        var client = await _dbService.GetClientAsync();

        client.Postgrest.Table<Compra>().Get().ContinueWith(t =>
        {
            if (t.Exception == null)
            {
                var comprasDoBanco = t.Result.Models;
                foreach (var compra in comprasDoBanco)
                {
                    // Adiciona na coleção de Compras na thread principal
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Compras.Add(compra);
                    });
                }
            }
            else
            {
                // Tratar erro
            }
        });

        Compras.DistinctBy(x => x.PrimaryKey);
    }

    private async void Editar_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Compra compra)
        {
            var novaCompraPage = new AdicionarCompraPage(
                onCompraSalva: (compraAtualizada) =>
                {
                    // A compra é atualizada diretamente, pois é a mesma referência
                },
                compraParaEditar: compra
            );

            await Navigation.PushModalAsync(novaCompraPage);
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