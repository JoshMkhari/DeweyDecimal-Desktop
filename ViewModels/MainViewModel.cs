using JoshMkhariPROG7312Game.Core;

namespace JoshMkhariPROG7312Game.ViewModels
{
    internal class MainViewModel : ObservableObjects
    {
        private object _currentView;

        public MainViewModel()
        {
            HomeVm = new HomeViewModel();
            ReplaceVm = new ReplaceBooksViewModel();
            CurrentView = HomeVm;

            HomeViewCommand = new RelayCommand(o => { CurrentView = HomeVm; });

            ReplaceViewCommand = new RelayCommand(o => { CurrentView = ReplaceVm; });
        }

        public RelayCommand HomeViewCommand { get; }
        public RelayCommand ReplaceViewCommand { get; }

        private HomeViewModel HomeVm { get; }
        private ReplaceBooksViewModel ReplaceVm { get; }

        public object CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
    }
}