using Konves.TextGraph.Models;

namespace Konves.TextGraph.Annotations
{
	public sealed class Heading : Annotation
	{
		public Heading(int offset, int length, int level) : base(offset, length)
		{
			Level = level;
		}

		public override string Subtype
		{
			get
			{
				return "heading";
			}
		}

		public override string Type
		{
			get
			{
				return "structure";
			}
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj) && (obj as Heading).Level == Level;
		}

		public override int GetHashCode()
		{
			return GetHashCode(GetHashCode(), Level);
		}

		public int Level { get; }
	}
}
