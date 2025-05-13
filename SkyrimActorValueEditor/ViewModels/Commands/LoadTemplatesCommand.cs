using SkyrimActorValueEditor.Models.Npcs;
using Syncfusion.UI.Xaml.TreeGrid;
using System.Windows.Input;

namespace SkyrimActorValueEditor.ViewModels.Commands
{
    class LoadTemplatesCommand : ICommand
    {
        public bool CanExecute(object? parameter)
        {
            if (parameter is TreeNode treeNode
                && treeNode.Item is NpcModel npcModel)
            {
                return npcModel.HasTemplates;
            }

            return false;
        }

        public void Execute(object? parameter)
        {
            if (parameter is TreeNode treeNode
                && treeNode.Item is NpcModel npcModel)
            {
                treeNode.PopulateChildNodes(npcModel.Templates);
            }
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
