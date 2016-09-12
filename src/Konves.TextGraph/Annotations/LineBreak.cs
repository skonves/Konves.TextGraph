using Konves.TextGraph.Models;

namespace Konves.TextGraph.Annotations
{
	public sealed class LineBreak : Annotation
	{
		public LineBreak(int offset) : base(offset, 0)
		{
		}

		public override string Subtype
		{
			get
			{
				return "linebreak";
			}
		}

		public override string Type
		{
			get
			{
				return "formatting";
			}
		}
	}
}
