using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TopSpaceMAUI.ViewModel
{
	public partial class AdminExplorerCacheViewModel: ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> items;

        [ObservableProperty]
        private string currentPath;

        private string rootPath;

        public AdminExplorerCacheViewModel()
        {
            rootPath = Microsoft.Maui.Storage.FileSystem.CacheDirectory;
            CurrentPath = rootPath;
            Items = new ObservableCollection<string>();
            LoadItems();
        }

        [RelayCommand]
        public void LoadItems()
        {
            Items.Clear();

            if (CurrentPath != rootPath)
            {
                Items.Add(".. (Voltar)");
            }

            //foreach (var dir in Directory.GetDirectories(CurrentPath))
            //{
            //    Items.Add($"📂 {Path.GetFileName(dir)}");
            //}

            foreach (var file in Directory.GetFiles(CurrentPath))
            {
                Items.Add($"📄 {Path.GetFileName(file)}");
            }
        }


        [RelayCommand]
        public void Navigate(string selectedItem)
        {
            if (selectedItem == ".. (Voltar)")
            {
                if (Directory.GetParent(CurrentPath) != null)
                {
                    CurrentPath = Directory.GetParent(CurrentPath).FullName;
                }
            }
            else if (selectedItem.EndsWith("jpg"))
            {
                //string caminhoImagem = Path.Combine(CurrentPath, selectedItem);
                //ImageSource imageSource = ImageSource.FromFile(caminhoImagem);
                //var popup = new ImagePopup(imageSource);
                //this.ShowPopup(popup);
            }
            else if (selectedItem.StartsWith("📂")) // Apenas diretórios
            {
                string folderName = selectedItem.Substring(2).Trim(); // Remove o emoji "📂 "
                string newPath = Path.Combine(CurrentPath, folderName);

                if (Directory.Exists(newPath))
                {
                    CurrentPath = newPath;
                }
            }

            LoadItems();
        }

    }
}

