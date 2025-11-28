using nosso_apartamento.Models;
using nosso_apartamento.Services;
using nosso_apartamento.Repositories;
using System.Collections.ObjectModel;

namespace nosso_apartamento.Views;

public partial class AdicionarCompraPage : ContentPage
{
	public ObservableCollection<CompraItem> ItensTemporarios { get; set; } = new ObservableCollection<CompraItem>();
    private Action<Compra> _onCompraSalva;
    private Compra _compraEditando;
    private bool _isEdicao;
    private DbService _dbService;
    private DbRepository _repository;

    public AdicionarCompraPage(Action<Compra> onCompraSalva = null, Compra compraParaEditar = null, DbService dbService = null)
	{
		InitializeComponent();
        _onCompraSalva = onCompraSalva;
        _compraEditando = compraParaEditar;
        _isEdicao = compraParaEditar != null;
        _dbService = dbService;

        if (_isEdicao)
        {
            Title = "Editar Compra";
            TituloEntry.Text = _compraEditando.Titulo;
            
            if (_compraEditando.Itens != null)
            {
                foreach (var item in _compraEditando.Itens)
                {
                    ItensTemporarios.Add(item);
                }
            }
        }

        BindingContext = this;
	}

    private void AdicionarItem_Clicked(object sender, EventArgs e)
    {
        var nomeItem = NovoItemEntry.Text?.Trim();
        
        if (string.IsNullOrEmpty(nomeItem))
        {
            DisplayAlert("Aviso", "Digite o nome do item", "OK");
            return;
        }

        if (!int.TryParse(QuantidadeEntry.Text, out int quantidade) || quantidade <= 0)
        {
            DisplayAlert("Aviso", "Digite uma quantidade válida", "OK");
            return;
        }

        var novoItem = new CompraItem
        {
            Nome = nomeItem,
            Quantidade = quantidade,
            Comprado = false
        };

        ItensTemporarios.Add(novoItem);
        NovoItemEntry.Text = string.Empty;
        QuantidadeEntry.Text = "1";
        NovoItemEntry.Focus();
    }

    private void RemoverItem_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is CompraItem item)
        {
            ItensTemporarios.Remove(item);
        }
    }

    private async void Salvar_Clicked(object sender, EventArgs e)
    {
        var titulo = TituloEntry.Text?.Trim();

        if (string.IsNullOrEmpty(titulo))
        {
            await DisplayAlert("Aviso", "Digite um título para a compra", "OK");
            return;
        }

        if (ItensTemporarios.Count == 0)
        {
            await DisplayAlert("Aviso", "Adicione pelo menos um item à compra", "OK");
            return;
        }

        try
        {
            var client = await _dbService.GetClientAsync();
            _repository = new DbRepository(client);

            Compra compra;

            if (_isEdicao)
            {
                _compraEditando.Titulo = titulo;
                // Não atribuir Itens aqui para evitar serialização
                
                // Atualizar apenas os campos da compra no banco
                await _repository.AtualizarAsync(_compraEditando);
                
                // Deletar itens antigos e inserir novos
                await _repository.DeletarItensPorCompraAsync(_compraEditando.Id.ToString());
                
                foreach (var item in ItensTemporarios)
                {
                    item.CompraId = _compraEditando.Id;
                    item.Id = Guid.NewGuid();
                    await _repository.AdicionarItemAsync(item);
                }

                _compraEditando.Itens = new List<CompraItem>(ItensTemporarios);
                compra = _compraEditando;
            }
            else
            {
                compra = new Compra
                {
                    Id = Guid.NewGuid(),
                    Titulo = titulo,
                    DataCriacao = DateTime.UtcNow,
                    Itens = new List<CompraItem>(),
                    Concluido = false
                };

                // Inserir compra no banco (sem os itens)
                var compraInserida = await _repository.AdicionarAsync(compra);
                compra = compraInserida;

                // Inserir itens no banco
                foreach (var item in ItensTemporarios)
                {
                    item.CompraId = compra.Id;
                    item.Id = Guid.NewGuid();
                    var itemInserido = await _repository.AdicionarItemAsync(item);
                    compra.Itens.Add(itemInserido);
                }
            }

            _onCompraSalva?.Invoke(compra);
            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao salvar: {ex.Message}", "OK");
        }
    }

    private async void Cancelar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
