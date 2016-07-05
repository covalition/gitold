using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Gitold.Application;
using Gitold.ViewModels.Properties;

namespace Gitold.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel() {
            //Path = Properties.Settings.Default.LocalRepoPath;
        }

        #region Refresh command

        private RelayCommand _refresh;

        public RelayCommand Refresh {
            get {
                return _refresh ?? (_refresh = new RelayCommand(refreshAction, refreshCanExecute));
            }
        }

        bool _refreshing = false;

        public bool Refreshing {
            get {
                return _refreshing;
            }
            set {
                if (value != _refreshing) {
                    _refreshing = value;
                    Refresh.RaiseCanExecuteChanged();
                }
            }
        }

        private const int ALL_DAYS = 7;
        private const int ALL_HOURS = 24;

        private async void refreshAction() {
            Refreshing = true;
            try {
                string[] paths = Paths.Where(p => p.IsSelected).Select(p => p.Caption).ToArray();
                int[,] _values = await DomainFacade.GetCommitCounts(paths, Authors[SelectedAuthor], AllDates? null: DateFrom, AllDates? null: DateTo);
                int maxValue = _values.Cast<int>().Max();
                int maxSumH = 0, maxSumD = 0;
                for (int d = 0; d < 8; d++) 
                    maxSumD = updateDay(_values, maxValue, maxSumD, d);

                for (int d = 0; d < 7; d++)
                    DayViewModels[d, ALL_HOURS].Percent = maxSumD != 0 ? (double)DayViewModels[d, ALL_HOURS].Value / maxSumD : 0.0;

                for (int h = 0; h < 24; h++)
                    maxSumH = Math.Max(maxSumH, DayViewModels[ALL_DAYS, h].Value);

                for (int h = 0; h < 24; h++)
                    DayViewModels[ALL_DAYS, h].Percent = maxSumH != 0 ? (double)DayViewModels[ALL_DAYS, h].Value / maxSumH : 0.0;
               
            }
            catch (Exception ex) {
                // MessageBox.Show(ex.Message);
                Messenger.Default.Send(new RepoExceptionMessage(ex));
            }
            finally {
                Refreshing = false;
            }
        }

        private int updateDay(int[,] _values, int maxValue, int maxSumD, int day) {
            DayViewModels[day, ALL_HOURS].Value = 0; // clear sum

            for (int h = 0; h < 25; h++)
                updateHour(_values, maxValue, day, h);

            maxSumD = Math.Max(maxSumD, DayViewModels[day, ALL_HOURS].Value);
            return maxSumD;
        }

        private void updateHour(int[,] _values, int maxValue, int day, int hour) {
            if (day == 0)
                DayViewModels[ALL_DAYS, hour].Value = 0; // clear sum

            DayViewModel dayViewModel = DayViewModels[day, hour];

            if (day < 7 && hour < 24) {
                dayViewModel.Value = _values[day, hour];
                DayViewModels[day, ALL_HOURS].Value += _values[day, hour];
                DayViewModels[ALL_DAYS, hour].Value += _values[day, hour];
                dayViewModel.Percent = maxValue != 0 ? (double)dayViewModel.Value / maxValue : 0.0;
            }
        }

        private bool refreshCanExecute() {
            return !_refreshing;
        }

        #endregion

        private bool _isFilterRefreshing;

        public bool IsFilterRefreshing {
            get {
                return _isFilterRefreshing;
            }
            set {
                if (value != _isFilterRefreshing) {
                    _isFilterRefreshing = value;
                    RaisePropertyChanged(nameof(IsFilterRefreshing));
                }
            }
        }

        internal void PathsChanged() {
            _paths = null;
            RaisePropertyChanged(nameof(Paths));
        }

        private List<PathItemViewModel> _paths;

        public List<PathItemViewModel> Paths {
            get {
                if(_paths == null) {
                    _paths = Properties.Settings.Default.LocalRepoPaths
                        .Cast<string>()
                        .Select(s => new PathItemViewModel { Caption = s })
                        .ToList();
                    readDatesAndAuthors();
                }
                return _paths;
            }
        }

        private async void readDatesAndAuthors() {
            IsFilterRefreshing = true;
            try {
                Details details = await DomainFacade.GetRepoDetails(Settings.Default.LocalRepoPaths.Cast<string>().ToArray());
                DateFrom = details.DateFrom;
                DateTo = details.DateTo;
                // details.Commiters.Insert(0, string.Empty);
                Authors = details.Commiters;
            }
            finally {
                IsFilterRefreshing = false;
            }
        }

        DayViewModel[,] _dayViewModels;

        public DayViewModel[,] DayViewModels {
            get {
                if (_dayViewModels == null) {
                    _dayViewModels = new DayViewModel[8, 25];
                    // initialize the array
                    for (int d = 0; d < 8; d++)
                        for (int h = 0; h < 25; h++) {
                            _dayViewModels[d, h] = new DayViewModel();
                            if (d == 7 || h == 24)
                                _dayViewModels[d, h].IsSummary = true;
                        }
                }
                return _dayViewModels;
            }
        }

        private DateTime? _dateFrom;

        public DateTime? DateFrom {
            get {
                return _dateFrom;
            }
            set {
                if (value != _dateFrom) {
                    _dateFrom = value;
                    RaisePropertyChanged(nameof(DateFrom));
                }
            }
        }

        private DateTime? _dateTo;

        public DateTime? DateTo {
            get {
                return _dateTo;
            }
            set {
                if (value != _dateTo) {
                    _dateTo = value;
                    RaisePropertyChanged(nameof(DateTo));
                }
            }
        }

        private bool _allDates;

        public bool AllDates {
            get {
                return _allDates;
            }
            set {
                if (value != _allDates) {
                    _allDates = value;
                    RaisePropertyChanged(nameof(AllDates));
                }
            }
        }

        private int _selectedAuthor;

        public int SelectedAuthor {
            get {
                return _selectedAuthor;
            }
            set {
                if (value != _selectedAuthor) {
                    _selectedAuthor = value;
                    RaisePropertyChanged(nameof(SelectedAuthor));
                }
            }
        }

        private List<string> _authors = new List<string>();

        public List<string> Authors {
            get {
                if(_authors.FirstOrDefault() != string.Empty)
                    _authors.Insert(0, string.Empty);
                return _authors;
            }
            set {
                if (value != _authors) {
                    _authors = value;
                    RaisePropertyChanged(nameof(Authors));
                }
            }
        }

        #region Settings

        private PathsViewModel _pathSettings;

        public PathsViewModel PathSettings {
            get {
                return _pathSettings?? (_pathSettings = new PathsViewModel(this));
            }
        }

        #endregion

    }
}
