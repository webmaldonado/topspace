using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI;

public partial class AdminPageQuery : ContentPage
{
    public AdminQueryViewModel myViewModel { get; set; } = new();

    public AdminPageQuery()
	{
		InitializeComponent();
        BindingContext = myViewModel;
    }

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        var SelectedTable = myViewModel.SelectedTable;

        if (string.IsNullOrEmpty(SelectedTable))
            return;

        var myConnection = DAL.Database.GetNewConnection();
        var table_info = myConnection.TableMappings.Where(w => w.TableName == SelectedTable).FirstOrDefault();

        // Criando o Grid para o cabeçalho
        var header_layout = new Grid
        {
            BackgroundColor = Colors.LightGray,
            Padding = new Thickness(20),
            ColumnSpacing = 10, // Espaçamento entre as colunas
        };

        if (table_info == null)
        {
            ListResultQuery.ItemsSource = null;
            return;
        }

        // Definindo as colunas no Grid
        for (int i = 0; i < table_info.Columns.Count(); i++)
        {
            header_layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        }

        for (int i = 0; i < table_info.Columns.Count(); i++)
        {
            var myLabel = new Label
            {
                FontSize = 14,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(5),
                Text = table_info.Columns[i].Name
            };

            // Definindo a coluna onde o Label será adicionado
            Grid.SetColumn(myLabel, i);

            // Adicionando o Label ao Grid
            header_layout.Children.Add(myLabel);
        }

        ListResultQuery.Header = header_layout;

        // Criando o ItemTemplate do ListView
        ListResultQuery.ItemTemplate = new DataTemplate(() =>
        {
            var gridLayout = new Grid
            {
                ColumnSpacing = 10, // Espaçamento entre as colunas
            };

            for (int i = 0; i < table_info.Columns.Count(); i++)
            {
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            for (int i = 0; i < table_info.Columns.Count(); i++)
            {
                var myLabel = new Label
                {
                    FontSize = 14,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Padding = new Thickness(5)
                };

                myLabel.SetBinding(Label.TextProperty, table_info.Columns[i].Name);

                // Definindo a coluna onde o Label será adicionado
                Grid.SetColumn(myLabel, i);

                // Adicionando o Label ao Grid
                gridLayout.Children.Add(myLabel);
            }

            return new ViewCell { View = gridLayout };
        });

        var query = $"SELECT * FROM {SelectedTable}";

        List<object> result = new();
        if (table_info != null)
        {
            result = myConnection.Query(table_info, query);
        }

        ListResultQuery.ItemsSource = result;
    }
}
