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

        public TreeNode? Parent { get; private set; }
        public ObservableCollection<TreeNode> Children { get; } = new();

        private float _value;

        protected TreeNode(string name)
        {
            Name = name;
        }

        public void AddNode(TreeNode node)
        {
            node.Parent = this;
            Children.Add(node);
        }

        public void ClearNode()
        {
            foreach (var node in Children)
            {
                node.ClearNode();
                node.Parent = null;
            }

            Children.Clear();
        }

        public bool HasNode(TreeNode node) => Children.Contains(node);

        public bool TryGetNodeByName(string name, [MaybeNullWhen(false)] out TreeNode treeNode)
        {
            treeNode = Children.FirstOrDefault(x => x.Name.Equals(name));
            return treeNode != null;
        }

        public virtual void UpdateValuesToDepth()
        {
            foreach (var treeNode in Children)
                treeNode.UpdateValuesToDepth();

            _value = Children.Sum(x => x.Value);
            OnPropertyChanged(nameof(Value));
        }

        public virtual void UpdateValuesFromDepth()
        {
            _value = Children.Sum(x => x.Value);
            OnPropertyChanged(nameof(Value));

            Parent?.UpdateValuesFromDepth();
        }
    }
}