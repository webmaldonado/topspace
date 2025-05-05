using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SQLite;

namespace TopSpaceMAUI.ViewModel
{
    public partial class VisitDataSKUViewModel : ObservableObject
    {
        public ObservableCollection<int> ValueOptions { get; set; }

        public VisitDataSKUViewModel()
        {
            ValueOptions = new ObservableCollection<int>();
            for (int i = 0; i <= 100; i++)
            {
                ValueOptions.Add(i);
            }
        }

        [ObservableProperty]
        private int _MetricID;

        [ObservableProperty]
        private int _SKUID;

        [ObservableProperty]
        private string _Name;

        [ObservableProperty]
        private int _BrandID;

        [ObservableProperty]
        private string _BrandName;

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
        private Color _LineColor;

        partial void OnValueChanged(int oldValue, int newValue)
        {
            Visit.VisitDataSKUSaveInMemory();
            Visit.RefreshScore();
        }

        [RelayCommand]
        private void Add()
        {
            if (Value == 100) return;
            Value++;
        }

        [RelayCommand]
        private void Remove()
        {
            if (Value == 0) return;
            Value--;
        }
    }
}
