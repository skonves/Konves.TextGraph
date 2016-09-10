using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konves.TextGraph.Models;

namespace Konves.TextGraph
{
	public interface IDocumentParser
	{
		ReadOnlyCollection<Document> Parse(string id, Stream documentStream);
	}
}
