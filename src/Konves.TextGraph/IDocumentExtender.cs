using System;
using System.Collections.ObjectModel;
using Konves.TextGraph.Models;

namespace Konves.TextGraph
{
	public interface IDocumentExtender
	{
		ReadOnlyCollection<Type> Dependencies { get; }
		Document Extend(Document document);
	}
}
