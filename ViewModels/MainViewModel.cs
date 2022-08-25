using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshMkhariPROG7312Game.Core;
using JoshMkhariPROG7312Game.ViewModels;

namespace JoshMkhariPROG7312Game
{
    class MainViewModel: ObservableObjects
    {

        public RelayCOmmand HomeViewCommand { get; set; }
        public RelayCOmmand ReplaceViewCommand { get; set; }

        private object _currentView;

        public HomeViewModel HomeVm { get; set; }
        public ReplaceBooksViewModel ReplaceVm { get; set; }
        public object CurrentView
        {
            get { return _currentView; }
            set { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            HomeVm = new HomeViewModel();
            ReplaceVm = new ReplaceBooksViewModel();
            CurrentView = HomeVm;

            HomeViewCommand = new RelayCOmmand(o =>
            {
                CurrentView = HomeVm;
            });            
            
            ReplaceViewCommand = new RelayCOmmand(o =>
            {
                CurrentView = ReplaceVm;
            });
        }
    }
}
