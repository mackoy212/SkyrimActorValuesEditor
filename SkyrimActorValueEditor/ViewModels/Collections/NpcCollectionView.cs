using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.Models.Npcs;
using SkyrimActorValueEditor.ViewModels.Enums;
using System.Collections.ObjectModel;

namespace SkyrimActorValueEditor.ViewModels.Collections
{
    public class NpcCollectionView : ObservableCollection<NpcModel>
    {
        public string FilterText
        {
            get => _filterText;
            set
            {
                if (_filterText != value)
                {
                    _filterText = value;
                    Refresh();
                }
            }
        }
        public FilterOption FilterOption
        {
            get => _filterOption;
            set
            {
                if (_filterOption != value)
                {
                    _filterOption = value;
                    Refresh();
                }
            }
        }

        private string _filterText = string.Empty;
        private FilterOption _filterOption = FilterOption.EditorID;

        public NpcCollectionView()
        {
            Refresh();
        }

        public void Refresh()
        {
            ClearItems();

            foreach (var filteredNpc in NpcService.Npcs.Where(x => PassesFilter(x)))
                Add(filteredNpc);
        }

        private bool PassesFilter(NpcModel npc)
        {
            if (string.IsNullOrEmpty(FilterText))
                return true;

            return FilterOption switch
            {
                FilterOption.EditorID => npc.EditorID?.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ?? false,
                FilterOption.Name => npc.Name?.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ?? false,
                FilterOption.FormKey => npc.FormKey?.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ?? false,
                _ => true,
            };
        }
    }
}