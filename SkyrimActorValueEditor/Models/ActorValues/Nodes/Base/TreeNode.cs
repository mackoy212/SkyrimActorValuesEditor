using SkyrimActorValueEditor.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.Base
{
    public abstract class TreeNode : BaseViewModel
    {
        public string Name { get; }

        public virtual float Value
        {
            get => _value;
            set { }
        }

        public virtual bool IsEditable => false;

        public ObservableCollection<TreeNode> Children { get; } = new();

        private float _value;

        protected TreeNode(string name)
        {
            Name = name;
        }

        public void AddNode(TreeNode node)
        {
            Children.Add(node);
        }

        public void ClearNode()
        {
            foreach (var node in Children)
                node.ClearNode();

            Children.Clear();
        }

        public bool HasNode(TreeNode node) => Children.Contains(node);

        public bool TryGetNodeByName(string name, [MaybeNullWhen(false)] out TreeNode treeNode)
        {
            treeNode = Children.FirstOrDefault(x => x.Name.Equals(name));
            return treeNode != null;
        }

        public void UpdateValue()
        {
            foreach (var treeNode in Children)
                treeNode.UpdateValue();

            _value = Children.Sum(x => x.Value);
            OnPropertyChanged(nameof(Value));
        }
    }
}