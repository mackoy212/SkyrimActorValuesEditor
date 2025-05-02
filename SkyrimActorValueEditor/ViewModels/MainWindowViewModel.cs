using SkyrimActorValueEditor.Core.Services.GameData;
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
        public ObservableCollection<NpcModel> FilteredActors { get; } = new();
        public ObservableCollection<TreeNode> ActorValueTreeNodes { get; } = new();

        public NpcModel? SelectedActor
        {
            get => _selectedActor;
            set
            {
                if (Set(ref _selectedActor, value) && value != null)
                {
                    ActorValueNodeBuilder.UpdateActorValues(value.NPC);
                }
            }
        }

        public Array FilterOptions { get; } = Enum.GetValues(typeof(FilterOption));
        public FilterOption SelectedFilterOption { get; set; } = FilterOption.EditorID;

        public ICommand ApplyFilterCommand { get; }
        public ICommand SaveChanges { get; }
        public ICommand CopyFormKey { get; }
        public ICommand CopyEditorID { get; }
        public ICommand CopyName { get; }

        private NpcModel? _selectedActor;
        private List<NpcModel> _allActors = new();

        public MainWindowViewModel()
        {
            GameReader.LoadNPCs(_allActors);
            ActorValueNodeBuilder.LoadActorValues(ActorValueTreeNodes);

            foreach (var actor in _allActors)
                FilteredActors.Add(actor);

            ApplyFilterCommand = new RelayCommand(param =>
            {
                string filter = param as string ?? string.Empty;
                ApplyFilter(filter);
            });
            SaveChanges = new RelayCommand(_ => GameWriter.SaveChanges());
            CopyFormKey = new RelayCommand(_ => CopyToClipboard(SelectedActor?.FormKey));
            CopyEditorID = new RelayCommand(_ => CopyToClipboard(SelectedActor?.EditorID));
            CopyName = new RelayCommand(_ => CopyToClipboard(SelectedActor?.Name));
        }

        private void ApplyFilter(string filter)
        {
            var filteredActors = SelectedFilterOption switch
            {
                FilterOption.EditorID =>
                    _allActors.Where(x => x.EditorID?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false),
                FilterOption.Name =>
                    _allActors.Where(x => x.Name?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false),
                FilterOption.FormKey =>
                    _allActors.Where(x => x.FormKey?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false),
                _ => _allActors,
            };

            FilteredActors.Clear();
            foreach (var actor in filteredActors)
                FilteredActors.Add(actor);
        }

        private void CopyToClipboard(string? text)
        {
            Clipboard.SetText(text);
        }
    }
}