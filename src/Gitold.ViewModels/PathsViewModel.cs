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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        protected override async Task<IEnumerable<PathViewModel>> GetItems() {
            return Properties.Settings.Default.LocalRepoPaths
                .Cast<string>()
                .Select(s => new PathViewModel
                {
                    Caption = s
                });
        }
    }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}
