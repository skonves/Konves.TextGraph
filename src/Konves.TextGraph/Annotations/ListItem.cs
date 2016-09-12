using Konves.TextGraph.Models;

namespace Konves.TextGraph.Annotations
{
	public sealed class ListItem : Annotation
	{
		public ListItem(int offset, int length) : base(offset, length) { }

		public override string Subtype { get { return "listitem"; } }

		public override string Type { get { return "structure"; } }
	}
}
