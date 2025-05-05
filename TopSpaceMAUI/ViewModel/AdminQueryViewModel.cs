using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Data;
using SQLite;

namespace TopSpaceMAUI.ViewModel
{
	public partial class AdminQueryViewModel: ObservableObject
    {
        private readonly SQLiteAsyncConnection _database;

        public AdminQueryViewModel()
		{
            var dbPath = DAL.Database.DbName;
            _database = new SQLiteAsyncConnection(dbPath);

            LoadTablesAsync().ConfigureAwait(false);
        }

        [ObservableProperty]
        private ObservableCollection<string> _tables = new();

        [ObservableProperty]
        private DataTable _selectedTableData = new();

        [ObservableProperty]
        private string _selectedTable = string.Empty;

        [ObservableProperty]
        private ObservableCollection<string> _tableRows = new();

        private async Task LoadTablesAsync()
        {
            var query = "SELECT name FROM sqlite_master WHERE type='table'";
            var result = await _database.QueryAsync<TableInfo>(query);
            Tables = new ObservableCollection<string>(result.Select(t => t.Name));
        }

        [RelayCommand]
        private async Task ConsultarAsync()
        {
            if (string.IsNullOrEmpty(SelectedTable))
                return;
            

            var myConnection = DAL.Database.GetNewConnection();

            var table_info = myConnection.TableMappings.Where(w => w.TableName == SelectedTable).FirstOrDefault();

            var query = $"SELECT * FROM {SelectedTable}";

            var result = myConnection.Query(table_info, query);

            foreach (var item in result)
            {
                
            }
        }


        private class TableInfo
        {
            public string Name { get; set; }
        }
    }
}

