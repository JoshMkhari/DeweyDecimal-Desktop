using JoshMkhariPROG7312Game.Core;

namespace JoshMkhariPROG7312Game.ViewModels
{
    internal class MainViewModel : ObservableObjects
    {
        private object _currentView;

        public MainViewModel()
        {
            HomeVm = new HomeViewModel();
            ReplaceVm = new ReplaceBooksViewModel(0);
            IdentifyingAreasVm = new IdentifyingAreasViewModel();
            FindCallNumsVM = new FindCallNumbersViewModel();
            
            
            
            CurrentView = HomeVm;

            HomeViewCommand = new RelayCommand(o => { CurrentView = HomeVm; });

            ReplaceViewCommand = new RelayCommand(o => { CurrentView = ReplaceVm; });
            
            IdentifyingAreasCommand = new RelayCommand(o => { CurrentView = IdentifyingAreasVm; });
            
            FindCallNumsCommand = new RelayCommand(o => { CurrentView = FindCallNumsVM; });
        }

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand ReplaceViewCommand { get; set; }
        public RelayCommand IdentifyingAreasCommand { get; set; }
        public RelayCommand FindCallNumsCommand { get; set; }

        public HomeViewModel HomeVm { get; set; }
        public ReplaceBooksViewModel ReplaceVm { get; set; }
        public IdentifyingAreasViewModel IdentifyingAreasVm { get; set; }
        public FindCallNumbersViewModel FindCallNumsVM { get; set; }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
    }
}