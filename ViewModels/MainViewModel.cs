﻿using JoshMkhariPROG7312Game.Core;
using JoshMkhariPROG7312Game.ViewModels;

namespace JoshMkhariPROG7312Game
{
    internal class MainViewModel : ObservableObjects
    {
        private object _currentView;

        public MainViewModel()
        {
            HomeVm = new HomeViewModel();
            ReplaceVm = new ReplaceBooksViewModel();
            CurrentView = HomeVm;

            HomeViewCommand = new RelayCOmmand(o => { CurrentView = HomeVm; });

            ReplaceViewCommand = new RelayCOmmand(o => { CurrentView = ReplaceVm; });
        }

        public RelayCOmmand HomeViewCommand { get; set; }
        public RelayCOmmand ReplaceViewCommand { get; set; }

        public HomeViewModel HomeVm { get; set; }
        public ReplaceBooksViewModel ReplaceVm { get; set; }

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