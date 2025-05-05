using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using RestSharp;

namespace TopSpaceMAUI.ViewModel
{
    public partial class SyncBetaViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Tabela> tabelasParaSincronizar = new();

        public SyncBetaViewModel()
        {
            TabelasParaSincronizar = new ObservableCollection<Tabela>
            {
                new TabelaBrand(),
                new TabelaSku(),
                new TabelaPOS(),
            };
        }

        [RelayCommand]
        public async Task SincronizarTabelasAsync()
        {
            var myToken = ObterTokenAsync().Result;
            var tasks = TabelasParaSincronizar.Select(tabela => tabela.SincronizarAsync(myToken));
            await Task.WhenAll(tasks);
        }

        private async Task<string> ObterTokenAsync()
        {
            RestClient client = null;
            RestRequest request = null;
            RestResponse<Model.Token> response = null;

            client = new RestClient(Config.URL_API_BASE);
            request = new RestRequest(Config.URL_API_REQUEST_TOKEN, Method.Post);
            request.AddParameter(Config.URL_API_PARAMETER_USERNAME, "EFYUZ");
            request.AddParameter(Config.URL_API_PARAMETER_PASSWORD, "teste");
            response = client.Execute<Model.Token>(request);

            var token = response.Data?.TokenID;

            return token;
        }
    }

    public partial class Tabela : ObservableObject
    {
        [ObservableProperty]
        private string nome;

        [ObservableProperty]
        private float porcentagemConcluida;

        public virtual Task SincronizarAsync(string token)
        {
            return Task.CompletedTask;
        }
    }

    public class TabelaBrand : Tabela
    {
        public TabelaBrand()
        {
            Nome = "Brand";
        }

        public override async Task SincronizarAsync(string token)
        {
            PorcentagemConcluida = 0;

            var db = DAL.Database.GetNewConnection();

            RestClient client = null;
            RestRequest request = null;
            RestResponse<List<Model.Brand>> response = null;

            client = new RestClient(Config.URL_API_BASE);
            var url = String.Format(Config.URL_API_REQUEST_BRANDS, DAL.Token.Current.Username);
            request = new RestRequest(url);
            request.AddHeader(Config.API_HEADER_TOKEN, token);
            request.RequestFormat = DataFormat.Json;
            request.Timeout = TimeSpan.FromMilliseconds(300000);
            response = await client.ExecuteAsync<List<Model.Brand>>(request);

            var brands = response.Data;
            
            db.Table<Model.Brand>().Delete(w => w.BrandID > 0);

            db.InsertAll(brands);
            
            PorcentagemConcluida = 100;
        }
    }

    public class TabelaSku : Tabela
    {
        public TabelaSku()
        {
            Nome = "SKU";
        }

        public override async Task SincronizarAsync(string token)
        {
            PorcentagemConcluida = 0;

            var db = DAL.Database.GetNewConnection();

            RestClient client = null;
            RestRequest request = null;
            RestResponse<List<Model.SKU>> response = null;

            client = new RestClient(Config.URL_API_BASE);
            var url = String.Format(Config.URL_API_REQUEST_SKUS, DAL.Token.Current.Username);
            request = new RestRequest(url);
            request.AddHeader(Config.API_HEADER_TOKEN, token);
            request.RequestFormat = DataFormat.Json;
            request.Timeout = TimeSpan.FromMilliseconds(300000);
            response = await client.ExecuteAsync<List<Model.SKU>>(request);

            var skus = response.Data;

            db.Table<Model.SKU>().Delete(w => w.SKUID > 0);

            db.InsertAll(skus);

            PorcentagemConcluida = 100;
        }
    }

    public class TabelaPOS : Tabela
    {
        public TabelaPOS()
        {
            Nome = "POS";
        }

        public override async Task SincronizarAsync(string token)
        {
            PorcentagemConcluida = 0;

            var db = DAL.Database.GetNewConnection();

            RestClient client = null;
            RestRequest request = null;
            RestResponse<List<Model.POS>> response = null;

            client = new RestClient(Config.URL_API_BASE);
            var url = String.Format(Config.URL_API_REQUEST_POSS, DAL.Token.Current.Username);
            request = new RestRequest(url);
            request.AddHeader(Config.API_HEADER_TOKEN, token);
            request.RequestFormat = DataFormat.Json;
            request.Timeout = TimeSpan.FromMilliseconds(300000);
            response = await client.ExecuteAsync<List<Model.POS>>(request);

            var poss = response.Data;

            db.Table<Model.POS>().Delete();

            //db.Table<Model.POS>().Delete(w => w.POSCode != string.Empty);

            //db.InsertAll(poss);

            PorcentagemConcluida = 100;
        }
    }
}
