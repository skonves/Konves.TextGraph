using Konves.TextGraph.Models;

namespace Konves.TextGraph.Annotations
{
	public sealed class List : Annotation
	{
		public List(int offset, int length, bool isOrdered) : base(offset, length)
		{
			IsOrdered = isOrdered;
		}

		public bool IsOrdered { get; }

		public override string Subtype { get { return "list"; } }

		public override string Type { get { return "structure"; } }

		public override bool Equals(object obj)
		{
			return base.Equals(obj) && (obj as List).IsOrdered == IsOrdered;
		}

		public override int GetHashCode()
		{
			return GetHashCode(GetHashCode(), IsOrdered);
		}
	}
}
