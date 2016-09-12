using Konves.TextGraph.Models;

namespace Konves.TextGraph.Annotations
{
	public sealed class BlockQuote : Annotation
	{
		public BlockQuote(int offset, int length) : base(offset, length)
		{
		}

		public override string Subtype
		{
			get
			{
				return "blockquote";
			}
		}

		public override string Type
		{
			get
			{
				return "structure";
			}
		}
	}
}
