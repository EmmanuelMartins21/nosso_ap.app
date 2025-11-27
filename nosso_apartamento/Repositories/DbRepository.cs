using nosso_apartamento.Models;
using Supabase;
using Supabase.Postgrest.Models;


namespace nosso_apartamento.Repositories
{
    public class DbRepository
    {
        private readonly Client _client;

        public DbRepository(Client client)
        {
            _client = client;
        }

        // ====== COMPRAS ======

        public async Task<List<Compra>> ObterTodasAsync()
        {
            var response = await _client
                .From<Compra>()
                .Select("*, compra_itens(*)")
                .Get();

            return response.Models;
        }

        public async Task<Compra?> ObterPorIdAsync(string id)
        {
            var response = await _client
                .From<Compra>()
                .Select("*, compra_itens(*)")
                .Where(x => x.Id == Guid.Parse(id))
                .Get();

            return response.Models.FirstOrDefault();
        }

        public async Task<Compra> AdicionarAsync(Compra compra)
        {
            compra.Id = Guid.NewGuid();
            compra.DataCriacao = DateTime.Now;

            var response = await _client
                .From<Compra>()
                .Insert(compra);

            return response.Models.FirstOrDefault() ?? compra;
        }

        public async Task<Compra> AtualizarAsync(Compra compra)
        {
            compra.DataCriacao = compra.DataCriacao == DateTime.MinValue ? DateTime.Now : compra.DataCriacao;

            var response = await _client
                .From<Compra>()
                .Update(compra);

            return response.Models.FirstOrDefault() ?? compra;
        }

        public async Task DeletarAsync(string id)
        {
            await _client
                .From<Compra>()
                .Where(x => x.Id == Guid.Parse(id))
                .Delete();
        }

        // ====== COMPRA ITENS ======

        public async Task<List<CompraItem>> ObterItensPorCompraAsync(string compraId)
        {
            var response = await _client
                .From<CompraItem>()
                .Where(x => x.CompraId == Guid.Parse(compraId))
                .Get();

            return response.Models;
        }

        public async Task<CompraItem> AdicionarItemAsync(CompraItem item)
        {
            item.Id = Guid.NewGuid();
            item.DataCriacao = DateTime.Now;

            var response = await _client
                .From<CompraItem>()
                .Insert(item);

            return response.Models.FirstOrDefault() ?? item;
        }

        public async Task<CompraItem> AtualizarItemAsync(CompraItem item)
        {
            var response = await _client
                .From<CompraItem>()
                .Update(item);

            return response.Models.FirstOrDefault() ?? item;
        }

        public async Task DeletarItemAsync(string itemId)
        {
            await _client
                .From<CompraItem>()
                .Where(x => x.Id == Guid.Parse(itemId))
                .Delete();
        }

        public async Task DeletarItensPorCompraAsync(string compraId)
        {
            await _client
                .From<CompraItem>()
                .Where(x => x.CompraId == Guid.Parse(compraId))
                .Delete();
        }
    }
}
