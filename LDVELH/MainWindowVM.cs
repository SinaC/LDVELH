using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace LDVELH
{
    public class MainWindowVM : BindableBase
    {
        public MainWindowVM()
        {
            Reset();

            // add Layout Algorithm Types
            LayoutAlgorithmTypes = new List<string> { "BoundedFR", "Circular", "CompoundFDP", "EfficientSugiyama", "FR", "ISOM", "KK", "LinLog", "Tree" };

            // pick a default Layout Algorithm Type
            LayoutAlgorithmType = "LinLog";
        }

        private bool _isRawLinksFocused;
        public bool IsRawLinksFocused
        {
            get => _isRawLinksFocused;
            set => SetProperty(ref _isRawLinksFocused, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private ObservableCollection<PageVM> _pages;
        public ObservableCollection<PageVM> Pages
        {
            get => _pages;
            protected set => SetProperty(ref _pages, value);
        }

        private PageVM _currentPage;
        public PageVM CurrentPage
        {
            get => _currentPage;
            set
            {
                SetProperty(ref _currentPage, value);
                IsRawLinksFocused = true;
            }
        }

        private ICommand _resetCommand;
        public ICommand ResetCommand => _resetCommand = _resetCommand ?? new DelegateCommand(Reset);
        private void Reset()
        {
            CurrentPage = null;
            Pages = new ObservableCollection<PageVM>();
            GoToPage(1);
        }

        private ICommand _loadCommand;
        public ICommand LoadCommand => _loadCommand = _loadCommand ?? new DelegateCommand(Load);
        private void Load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var filename = openFileDialog.FileName;

                CurrentPage = null;
                var lines = File.ReadAllLines(filename);
                Name = lines[0];
                var pages = lines.Skip(1).Select(x => x.Split(':')).Select(x => new PageVM(Convert.ToInt32(x[0]), x[1], x[2])).ToList();
                Pages = new ObservableCollection<PageVM>(pages);
                GoToPage(Pages.Max(x => x.PageNumber));
            }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand = _saveCommand ?? new DelegateCommand(Save);
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return;
            var lines = new[] { Name }.Concat(Pages.Select(x => $"{x.PageNumber}:{x.RawLinks}:{x.Notes}"));
            File.WriteAllLines($@"C:\PRJ\Personal\LDVELH\books\ldvelh_{Name}.txt", lines);
            IsRawLinksFocused = true;
        }

        private ICommand _goToPageCommand;
        public ICommand GoToPageCommand => _goToPageCommand = _goToPageCommand ?? new DelegateCommand<int>(GoToPage);
        private void GoToPage(int pageNumber)
        {
            var page = Pages.SingleOrDefault(x => x.PageNumber == pageNumber);
            if (page == null)
            {
                page = new PageVM(pageNumber);
                Pages.Add(page);
            }
            CurrentPage = page;
        }

        private ICommand _goToPreviousPageCommand;
        public ICommand GoToPreviousPageCommand => _goToPreviousPageCommand = _goToPreviousPageCommand ?? new DelegateCommand(GoToPreviousPage);
        private void GoToPreviousPage()
        {
            if (CurrentPage.PageNumber == 1)
                return;
            var previousPageNumber = CurrentPage.PageNumber - 1;
            var previousPage = Pages.SingleOrDefault(x => x.PageNumber == previousPageNumber);
            if (previousPage == null)
            {
                previousPage = new PageVM(previousPageNumber);
                Pages.Insert(0, previousPage);
            }
            CurrentPage = previousPage;
        }

        private ICommand _goToNextPageCommand;
        public ICommand GoToNextPageCommand => _goToNextPageCommand = _goToNextPageCommand ?? new DelegateCommand(GoToNextPage);
        private void GoToNextPage()
        {
            var NextPageNumber = CurrentPage.PageNumber + 1;
            var nextPage = Pages.SingleOrDefault(x => x.PageNumber == NextPageNumber);
            if (nextPage == null)
            {
                nextPage = new PageVM(NextPageNumber);
                Pages.Add(nextPage);
            }
            CurrentPage = nextPage;
        }

        //
        private PageGraph _graph;
        public PageGraph Graph
        {
            get => _graph;
            protected set => SetProperty(ref _graph, value);
        }

        private List<string> _layoutAlgorithmTypes;
        public List<string> LayoutAlgorithmTypes
        {
            get => _layoutAlgorithmTypes;
            protected set => SetProperty(ref _layoutAlgorithmTypes, value);
        }

        private string _layoutAlgorithmType;
        public string LayoutAlgorithmType
        {
            get => _layoutAlgorithmType;
            set => SetProperty(ref _layoutAlgorithmType, value);
        }

        private ICommand _refreshGraphCommand;
        public ICommand RefreshGraphCommand => _refreshGraphCommand = _refreshGraphCommand ?? new DelegateCommand(RefreshGraph);
        private void RefreshGraph()
        {
            Graph = new PageGraph(Pages, true);
        }
    }

    internal class MainWindowVMDesignData : MainWindowVM
    {
        public MainWindowVMDesignData()
        {
            Pages = new ObservableCollection<PageVM>(Enumerable.Range(0,10).Select(x => new PageVM(x, $"{(x+1)%10},-1", x%2 == 0 ? $"note:{x}" : null)));
            Graph = new PageGraph(Pages, false);
        }
    }
}
