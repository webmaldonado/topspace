namespace TopSpaceMAUI;

public partial class Title : ContentView
{
	public Title()
	{
		InitializeComponent();

        StartMemoryMonitor();

    }

    private async void StartMemoryMonitor()
    {
        while (true)
        {
            try
            {
                long memoryUsed = GC.GetTotalMemory(false);
                double memoryInMB = memoryUsed / (1024.0 * 1024.0);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    MemoryUsageLabel.Text = $"Consumo de Memória: {memoryInMB:F2} MB";
                    Util.MemoryRAM.CurrentValue = $"{memoryInMB}";
                });
            }
            catch (Exception ex)
            {
                MemoryUsageLabel.Text = $"Erro ao obter uso de memória: {ex.Message}";
            }

            await Task.Delay(1000);
        }
    }
}
