using SkyrimActorValueEditor.Models.ActorValues;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.Npcs;
using System.Collections.ObjectModel;

namespace SkyrimActorValueEditor.ViewModels.Collections
{
    public class ActorValueTreeView : ObservableCollection<TreeNode>
    {
        public NpcModel? SelectedNpc
        {
            get => _selectedActor;
            set
            {
                if (value != null && _selectedActor != value)
                {
                    _selectedActor = value;
                    ActorValueNodeBuilder.RebuildActorValues(value);
                }
            }
        }

        private NpcModel? _selectedActor;

        public ActorValueTreeView()
        {
            foreach (var node in ActorValueNodeBuilder.LoadActorValues())
                Add(node);
        }
    }
}