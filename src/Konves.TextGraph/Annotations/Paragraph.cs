using Konves.TextGraph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konves.TextGraph.Annotations
{
	public sealed class Paragraph : Annotation
	{
		public Paragraph(int offset, int length) : base(offset, length)
		{
		}

		public override string Type
		{
			get
			{
				return "structure";
			}
		}

		public override string Subtype
		{
			get
			{
				return "paragraph";
			}
		}
	}
}
