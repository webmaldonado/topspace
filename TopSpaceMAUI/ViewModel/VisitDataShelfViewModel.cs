using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System.IO;
using System.Threading.Tasks;
using TopSpaceMAUI.Util;
using TopSpaceMAUI.Views;
using CommunityToolkit.Maui.Views;
using System.Reflection.Emit;

namespace TopSpaceMAUI.ViewModel
{
    public partial class VisitDataShelfViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _BrandID;
        [ObservableProperty]
        private string _BrandName;

        [ObservableProperty]
        private int _MetricID;

        [ObservableProperty]
        private decimal _Objective;

        [ObservableProperty]
        private decimal _Score;

        [ObservableProperty]
        private int? _Grade;

        [ObservableProperty]
        private decimal? _GradeWeight;

        [ObservableProperty]
        private ImageSource _Photo;

        [ObservableProperty]
        private bool _ShowScore;

        private ImageSource oldPhoto;

        [ObservableProperty]
        private string _PhotoPath;


        //[RelayCommand]
        //private async Task OpenCamera()
        //{
        //    this.oldPhoto = Photo;
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

        //        await Task.Delay(700);
        //        Score = GradeWeight != null ? GradeWeight.Value : 0;
        //        PictureShelfViewModel model = new PictureShelfViewModel
        //        {
        //            Objective = Objective,
        //            Name = BrandName,
        //            Score = 0,
        //            BrandID = BrandID,
        //            MetricID = MetricID,
        //            VisitDate = DateTime.Now,
        //            Source = Photo
        //        };

        //        var view = new PictureShelfView(model);
        //        await view.OpenAsModal(updateScore);
        //    }
        //}


        //[RelayCommand]
        //private async Task OpenCamera()
        //{
        //    await MainThread.InvokeOnMainThreadAsync(async () =>
        //    {
        //        await Task.Delay(500); // Aguarda 500ms antes de abrir a câmera

        //        this.oldPhoto = Photo;
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

        //            await Task.Delay(700);
        //            Score = GradeWeight != null ? GradeWeight.Value : 0;
        //            PictureShelfViewModel model = new PictureShelfViewModel
        //            {
        //                Objective = Objective,
        //                Name = BrandName,
        //                Score = 0,
        //                BrandID = BrandID,
        //                MetricID = MetricID,
        //                VisitDate = DateTime.Now,
        //                Source = Photo
        //            };

        //            var view = new PictureShelfView(model);
        //            await view.OpenAsModal(updateScore);
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
                await Task.Delay(1000);

                this.oldPhoto = Photo;

                var result = await MediaPicker.Default.CapturePhotoAsync();
                //var result = await Camera.TakePictureAsync();

                if (result == null)
                {
                    TopSpaceMAUI.Util.LogUnhandledException.LogError(new Exception("Ponto Natural : Câmera cancelada ou erro desconhecido."));
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

                // Aguarda 700 ms para abrir a pop-up de edicao (bugfix, sem isso, a edicao nao abre)
                await Task.Delay(700);

                Score = GradeWeight != null ? GradeWeight.Value : 0;
                PictureShelfViewModel model = new PictureShelfViewModel
                {
                    Objective = Objective,
                    Name = BrandName,
                    Score = 0,
                    BrandID = BrandID,
                    MetricID = MetricID,
                    VisitDate = DateTime.Now,
                    Source = Photo,
                    PhotoPath = PhotoPath
                };

                var view = new PictureShelfView(model);
                await view.OpenAsModal(updateScore);

            }
            catch (Exception ex)
            {
                TopSpaceMAUI.Util.LogUnhandledException.LogError(ex);
            }
        }


        public PictureShelfViewModel updateScore(PictureShelfViewModel returnedModel)
        {
            if (returnedModel != null)
            {
                Score = returnedModel.Score;
                //_photoBytes = returnedModel.PhotoStream;
                //Photo = ImageSource.FromStream(() => new MemoryStream(_photoBytes));
                Photo = ImageSource.FromFile(PhotoPath);

                Visit.VisitDataShelfSaveInMemory();
                Visit.RefreshScore();
            }
            else
            {
                //_photoBytes = null;
                Photo = this.oldPhoto;
            }

            return returnedModel;
        }



        //[RelayCommand]
        //private async Task OpenCamera()
        //{
        //    var oldPhoto = Photo;
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

        //        Score = GradeWeight != null ? GradeWeight.Value : 0;

        //        PictureShelfViewModel model = new PictureShelfViewModel();
        //        model.Objective = Objective;
        //        model.Name = BrandName;
        //        model.Score = Score;
        //        model.BrandID = BrandID;
        //        model.MetricID = MetricID;
        //        model.Objective = Objective;
        //        model.VisitDate = DateTime.Now;
        //        model.Source = Photo;

        //        PictureShelfView view = new PictureShelfView(model);
        //        Page page = ((AppShell)Application.Current.MainPage).CurrentPage;
        //        //model = (PictureShelfViewModel)await page.ShowPopupAsync(view);

        //        MainThread.BeginInvokeOnMainThread(() =>
        //        {
        //            var popup = new VisitObjectivesPop("06626253021078");
        //            popup.CanBeDismissedByTappingOutsideOfPopup = true;
        //            var page = (AppShell)Application.Current.MainPage;
        //            page.ShowPopup(popup);
        //        });

        //        //var popup = new VisitObjectivesPop("06626253021078");
        //        //popup.CanBeDismissedByTappingOutsideOfPopup = true;
        //        //page.ShowPopup(popup);

        //        if (model != null)
        //        {
        //            Score = model.Score;

        //            _photoBytes = model.PhotoStream;
        //            Photo = ImageSource.FromStream(() => new MemoryStream(_photoBytes));
        //            Visit.VisitDataShelfSaveInMemory();
        //            Visit.RefreshScore();
        //        }
        //        else
        //        {
        //            _photoBytes = null;
        //            Photo = oldPhoto;
        //        }
        //    }
        //    // var imagePath = "test_photo.jpg";
        //    // Photo = ImageSource.FromFile(imagePath);
        //}

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
        //    }
        //    // var imagePath = "test_photo.jpg";
        //    // Photo = ImageSource.FromFile(imagePath);
        //    Score = GradeWeight != null ? GradeWeight.Value : 0;
        //    Visit.VisitDataShelfSaveInMemory();
        //    Visit.RefreshScore();
        //}

        [RelayCommand]
        private async Task ClearPhoto()
        {
            PhotoPath = null;
            Photo = null;
            //_photoBytes = null;
            Score = 0;
            Visit.VisitDataShelfSaveInMemory();
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
                    PhotoPath = null; // Remove a referência para liberar memória
                }
                catch (Exception ex)
                {
                    TopSpaceMAUI.Util.LogUnhandledException.LogError(ex);
                }
            }
        }
    }
}
