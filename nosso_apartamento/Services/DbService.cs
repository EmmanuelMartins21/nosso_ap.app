using nosso_apartamento.Utils;
using Supabase;

namespace nosso_apartamento.Services
{
    public class DbService
    {
        private Client? _client;

        private TaskCompletionSource<bool> _initializationTcs = new();

        public DbService()
        {
        }

        public async Task InitializeAsync()
        {
            try
            {
                await ConfiguracaoApp.CarregarConfiguracaoAsync();
                var options = new SupabaseOptions
                {
                    AutoConnectRealtime = true
                };

                _client = new Client(ConfiguracaoApp.SupabaseUrl, ConfiguracaoApp.SupabaseAnonKey, options);
                await _client.InitializeAsync();
                _initializationTcs.SetResult(true);
            }
            catch (Exception ex)
            {
                _initializationTcs.SetException(ex);
            }
        }

        public async Task<Client> GetClientAsync()
        {
            await _initializationTcs.Task;
            return _client ?? throw new InvalidOperationException("Supabase não foi inicializado");
        }
    }
}

