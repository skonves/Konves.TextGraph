using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konves.TextGraph.Models;
using System.IO;
using Konves.TextGraph.HtmlParser;
using System.Collections.ObjectModel;

namespace Konves.TextGraph.MarkDownParser
{
	public class MarkdownDocumentParser
	{
		public Document Parse(string id, Stream documentStream)
		{
			using (StreamReader reader = new StreamReader(documentStream))
			using (MemoryStream htmlStream = new MemoryStream())
			using (StreamWriter writer = new StreamWriter(htmlStream))
			{
				string html = new Markdown().Transform(reader.ReadToEnd());

				writer.Write(html);

				writer.Flush();
				htmlStream.Position = 0;

				return new HtmlDocumentParser().Parse(id, htmlStream);
			}
		}
	}
}
