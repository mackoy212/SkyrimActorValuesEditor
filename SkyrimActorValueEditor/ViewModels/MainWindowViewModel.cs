using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.Models.ActorValues;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.Npcs;
using SkyrimActorValueEditor.ViewModels.Base;
using SkyrimActorValueEditor.ViewModels.Commands;
using SkyrimActorValueEditor.ViewModels.Enums;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SkyrimActorValueEditor.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<NpcModel> FilteredNpcs
        {
            get => _npcs;
            set
            {
                if (Set(ref _npcs, value))
                {
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<TreeNode> ActorValueTreeNodes { get; } = new();

        public NpcModel? SelectedNpc
        {
            get => _selectedActor;
            set
            {
                if (Set(ref _selectedActor, value) && value != null)
                {
                    ActorValueNodeBuilder.UpdateActorValues(value);
                }
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                ApplyFilter(_filterText);
            }
        }
        public FilterOption SelectedFilterOption
        {
            get => _selectedFilterOption;
            set
            {
                _selectedFilterOption = value;
                ApplyFilter(_filterText);
            }
        }
        public Array FilterOptions { get; } = Enum.GetValues(typeof(FilterOption));

        public ICommand CopyFormKeyCommand { get; }
        public ICommand CopyEditorIDCommand { get; }
        public ICommand CopyNameCommand { get; }

        public ICommand ApplyFilterCommand { get; }
        public ICommand SaveChangesCommand { get; }
        //public ICommand LoadTemplatesCommand { get; }

        private ObservableCollection<NpcModel> _npcs = new();
        private NpcModel? _selectedActor;
        private string _filterText = string.Empty;
        FilterOption _selectedFilterOption = FilterOption.EditorID;

        public MainWindowViewModel()
        {
            ActorValueNodeBuilder.LoadActorValues(ActorValueTreeNodes);

            foreach (var npc in NpcService.NpcModels.Values)
                FilteredNpcs.Add(npc);

            CopyFormKeyCommand = new RelayCommand(_ =>
            {
                if (SelectedNpc?.FormKey != null)
                    Clipboard.SetText(SelectedNpc.FormKey);
            });

            CopyEditorIDCommand = new RelayCommand(_ =>
            {
                if (SelectedNpc?.EditorID != null)
                    Clipboard.SetText(SelectedNpc.EditorID);
            });

            CopyNameCommand = new RelayCommand(_ =>
            {
                if (SelectedNpc?.Name != null)
                    Clipboard.SetText(SelectedNpc.Name);
            });

            ApplyFilterCommand = new RelayCommand(arg =>
            {
                FilterText = arg as string ?? string.Empty;
            });

            SaveChangesCommand = new RelayCommand(_ => 
            {
                GameContext.SaveChanges();
            });

            //LoadTemplatesCommand = new LoadTemplatesCommand();
        }

        private void ApplyFilter(string filter)
        {
            var allNpcs = NpcService.NpcModels.Values;

            var filteredNpcs = SelectedFilterOption switch
            {
                FilterOption.EditorID =>
                    allNpcs.Where(x => x.EditorID?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false),
                FilterOption.Name =>
                    allNpcs.Where(x => x.Name?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false),
                FilterOption.FormKey =>
                    allNpcs.Where(x => x.FormKey?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false),
                _ => allNpcs,
            };

            FilteredNpcs.Clear();

            foreach (var npc in filteredNpcs)
                FilteredNpcs.Add(npc);
        }
    }
}