﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.ViewModel;

public partial class VisitDataMerchandisingViewModel: ObservableObject
{
    public ObservableCollection<int> ValueOptions { get; set; }

    public ObservableCollection<Model.ExecutionContext> ExecutionOptions { get; set; }


    public VisitDataMerchandisingViewModel()
    {
        ValueOptions = new ObservableCollection<int>();
        for (int i = 0; i <= 100; i++)
        {
            ValueOptions.Add(i);
        }
    }

    [ObservableProperty]
    private bool _IsItemSelected;

    [ObservableProperty]
    private int _MetricID;

    [ObservableProperty]
    private string _MetricType;

    [ObservableProperty]
    private string _Name;

    [ObservableProperty]
    private int _BrandID;

    [ObservableProperty]
    private string _BrandName;

    [ObservableProperty]
    private decimal? _Objective;

    [ObservableProperty]
    private double _ImageEditOpacity;

    [ObservableProperty]
    private bool _ImageEditIsVisible;

    [ObservableProperty]
    private int _Value;

    [ObservableProperty]
    private float _Score;

    [ObservableProperty]
    private float _Grade;

    [ObservableProperty]
    private decimal _GradeWeight;

    [ObservableProperty]
    private bool _ShowScore;

    [ObservableProperty]
    private ImageSource _Photo;

    //public byte[] _photoBytes;

    [ObservableProperty]
    private string _PhotoPath;

    [RelayCommand]
    private void Add()
    {
        if (Value == 100) return;
        Value++;
        Visit.VisitDataMerchandisingSaveInMemory();
        Visit.RefreshScore();
    }

    [RelayCommand]
    private void Remove()
    {
        if (Value == 0) return;
        Value--;
        Visit.VisitDataMerchandisingSaveInMemory();
        Visit.RefreshScore();
    }

    //[RelayCommand]
    //private async Task OpenCamera(VisitDataMerchandisingViewModel vm)
    //{
    //    var result = await MediaPicker.Default.CapturePhotoAsync();

    //    if (result != null)
    //    {
    //        var stream = await result.OpenReadAsync();
    //        using (var memoryStream = new MemoryStream())
    //        {
    //            await stream.CopyToAsync(memoryStream);
    //            _photoBytes = memoryStream.ToArray();
    //        }

    //        Photo = ImageSource.FromStream(() => new MemoryStream(_photoBytes));
    //    }

    //    //var imagePath = "test_photo.jpg";
    //    //Photo = ImageSource.FromFile(imagePath);
    //    Visit.VisitDataMerchandisingSaveInMemory();
    //}

    //[RelayCommand]
    //private async Task OpenCamera(VisitDataMerchandisingViewModel vm)
    //{
    //    await MainThread.InvokeOnMainThreadAsync(async () =>
    //    {
    //        await Task.Delay(500); // Aguarda 500ms antes de abrir a câmera

    //        var result = await MediaPicker.Default.CapturePhotoAsync();

    //        if (result != null)
    //        {
    //            using var stream = await result.OpenReadAsync();
    //            using var memoryStream = new MemoryStream();
    //            await stream.CopyToAsync(memoryStream);
    //            _photoBytes = memoryStream.ToArray();

    //            Photo = ImageSource.FromStream(() => new MemoryStream(_photoBytes));
    //        }

    //        Visit.VisitDataMerchandisingSaveInMemory();
    //        Visit.RefreshScore();

    //        // Libera memória manualmente
    //        GC.Collect();
    //        GC.WaitForPendingFinalizers();
    //    });
    //}

    //[RelayCommand]
    //private async Task OpenCamera(VisitDataMerchandisingViewModel vm)
    //{
    //    try
    //    {
    //        await Task.Delay(1000); // Pequeno atraso antes de abrir a câmera

    //        var result = await MediaPicker.Default.CapturePhotoAsync();

    //        if (result == null)
    //        {
    //            TopSpaceMAUI.Util.LogUnhandledException.LogError(new Exception("Merchandising : Câmera cancelada ou erro desconhecido."));
    //            return; // O usuário cancelou a captura ou houve erro
    //        }

    //        await using var stream = await result.OpenReadAsync();
    //        await using var memoryStream = new MemoryStream();

    //        await stream.CopyToAsync(memoryStream);
    //        _photoBytes = memoryStream.ToArray();

    //        Photo = ImageSource.FromStream(() => new MemoryStream(_photoBytes));

    //        Visit.VisitDataMerchandisingSaveInMemory();
    //        Visit.RefreshScore();
    //    }
    //    catch (Exception ex)
    //    {
    //        TopSpaceMAUI.Util.LogUnhandledException.LogError(ex);
    //    }
    //}

    //[RelayCommand]
    //private async Task OpenCamera(VisitDataMerchandisingViewModel vm)
    //{
    //    try
    //    {
    //        await Task.Delay(1000);

    //        var result = await Camera.TakePictureAsync(); 

    //        if (result == null)
    //        {
    //            TopSpaceMAUI.Util.LogUnhandledException.LogError(new Exception("Merchandising : Câmera cancelada ou erro desconhecido."));
    //            return;
    //        }

    //        await using var stream = await result.OpenReadAsync();
    //        await using var memoryStream = new MemoryStream();

    //        await stream.CopyToAsync(memoryStream);
    //        _photoBytes = memoryStream.ToArray();

    //        Photo = ImageSource.FromStream(() => new MemoryStream(_photoBytes));

    //        Visit.VisitDataMerchandisingSaveInMemory();
    //        Visit.RefreshScore();
    //    }
    //    catch (Exception ex)
    //    {
    //        TopSpaceMAUI.Util.LogUnhandledException.LogError(ex);
    //    }
    //}
    [RelayCommand]
    private async Task OpenCamera(VisitDataMerchandisingViewModel vm)
    {
        try
        {
            await Task.Delay(1000);

            var result = await Camera.TakePictureAsync();
            
            if (result == null)
            {
                TopSpaceMAUI.Util.LogUnhandledException.LogError(new Exception("Merchandising : Câmera cancelada ou erro desconhecido."));
                return;
            }

            // Caminho temporário para salvar a imagem
            var tempFilePath = Path.Combine(Microsoft.Maui.Storage.FileSystem.CacheDirectory, $"{Guid.NewGuid()}.jpg");

            await using var stream = await result.OpenReadAsync();
            await using var fileStream = File.Create(tempFilePath);
            await stream.CopyToAsync(fileStream);

            // Liberar memória imediatamente
            stream.Dispose();
            fileStream.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Armazena apenas o caminho da imagem
            PhotoPath = tempFilePath;
            Photo = ImageSource.FromFile(PhotoPath);

            Visit.VisitDataMerchandisingSaveInMemory();
            Visit.RefreshScore();
        }
        catch (Exception ex)
        {
            TopSpaceMAUI.Util.LogUnhandledException.LogError(ex);
        }
    }





    [RelayCommand]
    private async Task ClearPhoto(VisitDataMerchandisingViewModel vm)
    {
        PhotoPath = null;
        Photo = null;
        //_photoBytes = null;
        Visit.VisitDataMerchandisingSaveInMemory();
        Visit.RefreshScore();
    }

    //public async Task SavePhotoAsync(string photosDirectory)
    //{
    //    if (_photoBytes != null)
    //    {
    //        var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    //        var photoName = string.Format(Config.PHOTO_NAME, BrandID, MetricID);
    //        var filePath = Path.Combine(folderPath, photosDirectory, photoName);

    //        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
    //        {
    //            await fileStream.WriteAsync(_photoBytes, 0, _photoBytes.Length);
    //        }
    //    }
    //}

    public async Task SavePhotoAsync(string photosDirectory)
    {
        if (!string.IsNullOrEmpty(PhotoPath) && File.Exists(PhotoPath))
        {
            try
            {
                var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var photoName = string.Format(Config.PHOTO_NAME, BrandID, MetricID);
                var destinationPath = Path.Combine(folderPath, photosDirectory, photoName);

                // Garante que o diretório existe antes de salvar
                var directoryPath = Path.GetDirectoryName(destinationPath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Copia a imagem do local temporário para o destino final
                File.Copy(PhotoPath, destinationPath, true);

                // Limpa a imagem temporária para liberar espaço
                File.Delete(PhotoPath);
                PhotoPath = null;
            }
            catch (Exception ex)
            {
                TopSpaceMAUI.Util.LogUnhandledException.LogError(ex);
            }
        }
    }


    [ObservableProperty]
    private bool _IsPickerExecutionVisible;

    [ObservableProperty]
    private Model.ExecutionContext _ExecutionSelectedValue;

    [RelayCommand]
    private async Task OpenPickerAsync(Picker picker)
    {
        if (picker != null)
        {
            picker.Focus();
        }
    }

}

