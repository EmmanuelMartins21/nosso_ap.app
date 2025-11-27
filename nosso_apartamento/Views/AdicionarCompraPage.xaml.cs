using nosso_apartamento.Models;
using System.Collections.ObjectModel;

namespace nosso_apartamento.Views;

public partial class AdicionarCompraPage : ContentPage
{
	public ObservableCollection<CompraItem> ItensTemporarios { get; set; } = new ObservableCollection<CompraItem>();
    private Action<Compra> _onCompraSalva;
    private Compra _compraEditando;
    private bool _isEdicao;

    public AdicionarCompraPage(Action<Compra> onCompraSalva = null, Compra compraParaEditar = null)
	{
		InitializeComponent();
        _onCompraSalva = onCompraSalva;
        _compraEditando = compraParaEditar;
        _isEdicao = compraParaEditar != null;

        if (_isEdicao)
        {
            Title = "Editar Compra";
            TituloEntry.Text = _compraEditando.Titulo;
            
            foreach (var item in _compraEditando.Itens)
            {
                ItensTemporarios.Add(item);
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

        Compra compra;

        if (_isEdicao)
        {
            _compraEditando.Titulo = titulo;
            _compraEditando.Itens = new List<CompraItem>(ItensTemporarios);
            compra = _compraEditando;
        }
        else
        {
            compra = new Compra
            {
                Titulo = titulo,
                DataCriacao = DateTime.UtcNow,
                Itens = new List<CompraItem>(ItensTemporarios),
                Concluido = false
            };
        }

        _onCompraSalva?.Invoke(compra);
        await Navigation.PopModalAsync();
    }

    private async void Cancelar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
