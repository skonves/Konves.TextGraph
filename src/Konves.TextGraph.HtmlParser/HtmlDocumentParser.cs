using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konves.TextGraph.Models;
using HtmlAgilityPack;
using System.Collections.ObjectModel;
using Konves.TextGraph.Annotations;

namespace Konves.TextGraph.HtmlParser
{
	public class HtmlDocumentParser : IDocumentParser
	{
		public ReadOnlyCollection<Document> Parse(string id, Stream documentStream)
		{
			HtmlDocument htmlDocument = new HtmlDocument();

			htmlDocument.Load(documentStream);

			ParseState state = new ParseState(id);
			state.Traverse(htmlDocument.DocumentNode);

			Document document = state.ToDocument();

			return new ReadOnlyCollection<Document>(new[] { document });
		}

		private class ParseState
		{
			public ParseState(string documentId)
			{
				_documentId = documentId;
			}

			public void Traverse(IEnumerable<HtmlNode> nodes)
			{
				foreach (HtmlNode node in nodes ?? Enumerable.Empty<HtmlNode>())
					Traverse(node);
			}

			public void Traverse(HtmlNode node)
			{
				int offset = _text.Length;

				switch (node.Name)
				{
					case "a":
						Traverse(node.ChildNodes);
						_annotations.Add(new Link(_documentId, offset, _text.Length - offset, node.Attributes["href"]?.Value));
						break;
					case "#text":
						AddText(node.InnerText);
						break;
					case "em":						
						Traverse(node.ChildNodes);
						_annotations.Add(new Emphasis(_documentId, offset, _text.Length - offset));
						break;
					case "p":
						Traverse(node.ChildNodes);
						_annotations.Add(new Paragraph(_documentId, offset, _text.Length - offset));
						break;
					case "h1":
					case "h2":
					case "h3":
					case "h4":
					case "h5":
					case "h6":
						Traverse(node.ChildNodes);
						_annotations.Add(new Heading(_documentId, offset, _text.Length - offset, int.Parse(node.Name.Substring(1,1))));
						break;
					default:
						Traverse(node.ChildNodes);
						break;
				}
			}

			public Document ToDocument()
			{
				return new Document(_documentId, _text.ToString(), _annotations);
			}

			internal void AddText(string s)
			{
				for (int i = 0; i < s.Length; i++)
				{
					if (char.IsWhiteSpace(s[i]))
					{
						if (!_previousCharIsWhitespace)
						{
							_text.Append(" ");
						}

						_previousCharIsWhitespace = true;
					}
					else
					{
						_previousCharIsWhitespace = false;
						_text.Append(s[i]);
					}
				}
			}

			bool _previousCharIsWhitespace = false;

			StringBuilder _text = new StringBuilder();

			List<Annotation> _annotations = new List<Annotation>();

			readonly string _documentId;
		}
	}
}
