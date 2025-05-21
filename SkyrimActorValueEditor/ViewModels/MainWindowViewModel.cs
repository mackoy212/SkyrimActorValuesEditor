using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.Models.Npcs;
using SkyrimActorValueEditor.ViewModels.Base;
using SkyrimActorValueEditor.ViewModels.Collections;
using SkyrimActorValueEditor.ViewModels.Commands;
using SkyrimActorValueEditor.ViewModels.Enums;
using System.Windows;
using System.Windows.Input;

namespace SkyrimActorValueEditor.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public NpcCollectionView FilteredNpcs { get; } = new();
        public ActorValueTreeView ActorValueTree { get; } = new();

        public NpcModel? SelectedNpc
        {
            get => ActorValueTree.SelectedNpc;
            set
            {
                ActorValueTree.SelectedNpc = value;
                OnPropertyChanged();
            }
        }

        public string FilterText
        {
            get => FilteredNpcs.FilterText;
            set => FilteredNpcs.FilterText = value;
        }
        public FilterOption SelectedFilterOption
        {
            get => FilteredNpcs.FilterOption;
            set => FilteredNpcs.FilterOption = value;
        }
        public Array FilterOptions { get; } = Enum.GetValues(typeof(FilterOption));

        public ICommand CopyNpcPropretyCommand { get; }
        public ICommand SetFilterTextCommand { get; }
        public ICommand SaveChangesCommand { get; }

        //public ICommand LoadTemplatesCommand { get; }

        public MainWindowViewModel()
        {
            CopyNpcPropretyCommand = new RelayCommand(arg =>
            {
                if (arg is string text && !string.IsNullOrEmpty(text))
                    Clipboard.SetText(text);
            });

            SetFilterTextCommand = new RelayCommand(arg => FilterText = arg as string ?? string.Empty);

            SaveChangesCommand = new RelayCommand(_ => GameContext.SaveChanges());

            //LoadTemplatesCommand = new LoadTemplatesCommand();
        }
    }
}