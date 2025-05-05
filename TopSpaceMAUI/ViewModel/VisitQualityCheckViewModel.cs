using System;
using System.Reflection.Emit;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.ViewModel;

public partial class VisitQualityCheckViewModel: ObservableObject
{

    [ObservableProperty]
    private int _SKUID;

    [ObservableProperty]
    private string _Name;

    [ObservableProperty]
    private int _BrandID;

    [ObservableProperty]
    private string _BrandName;

    [ObservableProperty]
    private ImageSource _Photo;

    private byte[] _photoBytes;

    [ObservableProperty]
    private Boolean _IsPhotoOK;

    [ObservableProperty]
    private Color _BackColor;

    //[RelayCommand]
    //private async Task OpenCamera()
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

    //        IsPhotoOK = true;
    //        BackColor = Color.FromHex("#e5f2e5");
    //    }

    //    //var imagePath = "test_photo.jpg";
    //    //Photo = ImageSource.FromFile(imagePath);
    //    //IsPhotoOK = true;
    //    //BackColor = Color.FromHex("#e5f2e5");
    //}

    //[RelayCommand]
    //private async Task OpenCamera()
    //{
    //    await MainThread.InvokeOnMainThreadAsync(async () =>
    //    {
    //        await Task.Delay(500); // Aguarda 500ms antes de abrir a câmera

    //        var result = await MediaPicker.Default.CapturePhotoAsync();

    //        if (result != null)
    //        {
    //            using var stream = await result.OpenReadAsync();
    //            using (var memoryStream = new MemoryStream())
    //            {
    //                await stream.CopyToAsync(memoryStream);
    //                _photoBytes = memoryStream.ToArray();
    //            }

    //            Photo = ImageSource.FromStream(() => new MemoryStream(_photoBytes));

    //            IsPhotoOK = true;
    //            BackColor = Color.FromHex("#e5f2e5");
    //        }

    //        // Libera memória manualmente
    //        GC.Collect();
    //        GC.WaitForPendingFinalizers();

    //    });
    //}

    [RelayCommand]
    private async Task OpenCamera()
    {
        try
        {
            await Task.Delay(1000); // Aguarda 1000ms antes de abrir a câmera

            //var result = await MediaPicker.Default.CapturePhotoAsync();
            var result = await Camera.TakePictureAsync();

            if (result == null)
            {
                TopSpaceMAUI.Util.LogUnhandledException.LogError(new Exception("Quality Check : Câmera cancelada ou erro desconhecido."));
                return; // O usuário cancelou a captura ou houve erro
            }

            using var stream = await result.OpenReadAsync();
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                _photoBytes = memoryStream.ToArray();
            }

            Photo = ImageSource.FromStream(() => new MemoryStream(_photoBytes));

            IsPhotoOK = true;
            BackColor = Color.FromHex("#e5f2e5");
        }
        catch (Exception ex)
        {
            TopSpaceMAUI.Util.LogUnhandledException.LogError(ex);
        }
    }

    [RelayCommand]
    private async Task ClearPhoto()
    {
        Photo = null;
        _photoBytes = null;
        IsPhotoOK = false;
        BackColor = Color.FromHex("#ffe5e5");
    }


    public async Task<string> SavePhotoAsync(string pos_code, string data_visita)
    {
        var ret = string.Empty;

        if (_photoBytes != null)
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var photoName = String.Format(Config.PHOTO_NAME_QUALITY_CHECK, pos_code, data_visita);
            var filePath = Path.Combine(folderPath, photoName);

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await fileStream.WriteAsync(_photoBytes, 0, _photoBytes.Length);
            }

            ret = photoName;
        }

        return ret;
    }
}

