using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.Records.Base
{
    public abstract class RecordNode<T> : TreeNode
        where T : ISkyrimMajorRecordGetter
    {
        public T Record { get; }

        protected RecordNode(string name, T record) : base(name)
        {
            Record = record;
        }

        protected RecordNode(T record) : base(GetRecordNodeName(record))
        {
            Record = record;
        }

        private static string GetRecordNodeName(T record)
        {
            return record.EditorID ?? "UNKNOWN";
        }
    }
}