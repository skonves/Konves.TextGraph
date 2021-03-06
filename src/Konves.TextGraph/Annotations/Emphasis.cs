﻿using Konves.TextGraph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konves.TextGraph.Annotations
{
	public sealed class Emphasis : Annotation
	{
		public Emphasis(int offset, int length) : base(offset, length)
		{
		}

		public override string Type
		{
			get
			{
				return "formatting";
			}
		}

		public override string Subtype
		{
			get
			{
				return "emphasis";
			}
		}
	}
}
