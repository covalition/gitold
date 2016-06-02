using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igorary.ViewModels;

namespace Gitold.ViewModels
{
    public class PathsViewModel : GenericListEditViewModel<PathViewModel>
    {
        private MainViewModel _mainViewModel;

        public PathsViewModel(MainViewModel mainViewModel) {
            _mainViewModel = mainViewModel;
        }
    }
}
