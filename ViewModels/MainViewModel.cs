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
            
            CurrentView = HomeVm;

            HomeViewCommand = new RelayCOmmand(o => { CurrentView = HomeVm; });

            ReplaceViewCommand = new RelayCOmmand(o => { CurrentView = ReplaceVm; });
            
            IdentifyingAreasCommand = new RelayCOmmand(o => { CurrentView = IdentifyingAreasVm; });
        }

        public RelayCOmmand HomeViewCommand { get; set; }
        public RelayCOmmand ReplaceViewCommand { get; set; }
        public RelayCOmmand IdentifyingAreasCommand { get; set; }

        public HomeViewModel HomeVm { get; set; }
        public ReplaceBooksViewModel ReplaceVm { get; set; }
        public IdentifyingAreasViewModel IdentifyingAreasVm { get; set; }

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