using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konves.TextGraph.Models
{
	public sealed class Document
	{
		public Document(string id, string text, IEnumerable<Annotation> annotations)
		{
			Id = id;
			Text = text;
			Annotations = annotations is ReadOnlyCollection<Annotation>
				? (ReadOnlyCollection<Annotation>)annotations
				: new ReadOnlyCollection<Annotation>(
					annotations is IList<Annotation>
					? (IList<Annotation>)annotations
					: annotations.ToList());
		}

		public string Id { get; }
		public string Text { get; }
		public ReadOnlyCollection<Annotation> Annotations { get; }
	}
}
