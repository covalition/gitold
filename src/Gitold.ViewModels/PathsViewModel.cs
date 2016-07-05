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

        protected override Task<IEnumerable<PathViewModel>> LoadItems() {
            return Task.FromResult(Properties.Settings.Default.LocalRepoPaths
                .Cast<string>()
                .Select(s => new PathViewModel
                {
                    Caption = s
                }));
        }

        protected override Task<List<LabeledFieldViewModel>> LoadFields() {
            List<LabeledFieldViewModel> res = new List<LabeledFieldViewModel>();
            FolderPathFieldViewModel folderPath = new FolderPathFieldViewModel("Folder", this);
            if (SelectedItemIndex >= 0 && SelectedItemIndex < Properties.Settings.Default.LocalRepoPaths.Count)
                folderPath.Value = Properties.Settings.Default.LocalRepoPaths[SelectedItemIndex];
            res.Add(folderPath);
            return Task.FromResult(res);
        }

        protected override Task Delete() {
            return Task.Run(() =>
            {
                Properties.Settings.Default.LocalRepoPaths.RemoveAt(SelectedItemIndex);
                Properties.Settings.Default.Save();
                _mainViewModel.PathsChanged();
            });
        }

        protected override Task<int> Save() {
            return Task.Run(() =>
            {
                string path = (Fields[0] as FolderPathFieldViewModel).Value;
                if (IsNew)
                    Properties.Settings.Default.LocalRepoPaths.Add(path);
                else
                    Properties.Settings.Default.LocalRepoPaths[SelectedItemIndex] = path;
                Properties.Settings.Default.Save();
                _mainViewModel.PathsChanged();
                return IsNew ? Properties.Settings.Default.LocalRepoPaths.Count - 1 : SelectedItemIndex;
            });
        }

        protected override void OnException(Exception ex) {
            // TODO:
        }
    }
}
