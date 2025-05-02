using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Interfaces
{
    public interface ITreeNodeBuilder<TSource>
    {
        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(TSource source);
    }
}