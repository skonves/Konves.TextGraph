using Konves.TextGraph.Models;

namespace Konves.TextGraph.Annotations
{
	public sealed class Link : Annotation
	{
		public Link(int offset, int length, string uri) : base(offset, length)
		{
			Uri = uri;
		}

		public string Uri { get; }

		public override string Subtype { get { return "link"; } }

		public override string Type { get { return "navigation"; } }
	}
}
