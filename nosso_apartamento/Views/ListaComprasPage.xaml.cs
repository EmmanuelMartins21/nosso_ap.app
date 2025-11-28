using nosso_apartamento.Models;
using nosso_apartamento.Services;
using nosso_apartamento.Repositories;
using System.Collections.ObjectModel;

namespace nosso_apartamento.Views;

public partial class ListaComprasPage : ContentPage
{
	public ObservableCollection<Compra> Compras { get; set; } = new ObservableCollection<Compra>();
    private DbService _dbService;
    private DbRepository _repository;

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
        // Limpar coleção anterior para evitar duplicação
        Compras.Clear();

        var client = await _dbService.GetClientAsync();
        _repository = new DbRepository(client);

        try
        {
            var comprasDoBanco = await _repository.ObterTodasAsync();
            
            foreach (var compra in comprasDoBanco)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Compras.Add(compra);
                });
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao carregar compras: {ex.Message}", "OK");
        }
    }

    private async void Editar_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Compra compra)
        {
            var novaCompraPage = new AdicionarCompraPage(
                onCompraSalva: async (compraAtualizada) =>
                {
                    await CarregarDados();
                },
                compraParaEditar: compra,
                dbService: _dbService
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
                try
                {
                    var client = await _dbService.GetClientAsync();
                    var repository = new DbRepository(client);
                    await repository.DeletarAsync(compra.Id.ToString());
                    Compras.Remove(compra);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", $"Erro ao remover: {ex.Message}", "OK");
                }
            }
        }
    }

    private async void Adicionar_Clicked(object sender, EventArgs e)
    {
        var novaCompraPage = new AdicionarCompraPage(
            onCompraSalva: async (novaCompra) =>
            {
                await CarregarDados();
            },
            dbService: _dbService
        );

        await Navigation.PushModalAsync(novaCompraPage);
    }
}