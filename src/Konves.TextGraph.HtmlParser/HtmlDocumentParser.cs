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
	public class HtmlDocumentParser
	{
		public Document Parse(string id, Stream htmlDocumentStream)
		{
			HtmlDocument htmlDocument = new HtmlDocument();

			htmlDocument.Load(htmlDocumentStream);

			ParseState state = new ParseState(id);
			state.Traverse(htmlDocument.DocumentNode);

			return state.ToDocument();
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
						offset = char.IsWhiteSpace(_text[offset]) ? offset + 1 : offset;
						_annotations.Add(new Link(offset, _text.Length - offset, node.Attributes["href"]?.Value));
						break;
					case "#text":
						AddText(node.InnerText);
						break;
					case "em":
						Traverse(node.ChildNodes);
						offset = char.IsWhiteSpace(_text[offset]) ? offset + 1 : offset;
						_annotations.Add(new Emphasis(offset, _text.Length - offset));
						break;
					case "p":
						Traverse(node.ChildNodes);
						offset = char.IsWhiteSpace(_text[offset]) ? offset + 1 : offset;
						_annotations.Add(new Paragraph(offset, _text.Length - offset));
						break;
					case "br":
						_annotations.Add(new LineBreak(offset));
						break;
					case "blockquote":
						Traverse(node.ChildNodes);
						offset = char.IsWhiteSpace(_text[offset]) ? offset + 1 : offset;
						_annotations.Add(new BlockQuote(offset, _text.Length - offset));
						break;
					case "ul":
						Traverse(node.ChildNodes);
						offset = char.IsWhiteSpace(_text[offset]) ? offset + 1 : offset;
						_annotations.Add(new List(offset, _text.Length - offset, isOrdered: false));
						break;
					case "ol":
						Traverse(node.ChildNodes);
						offset = char.IsWhiteSpace(_text[offset]) ? offset + 1 : offset;
						_annotations.Add(new List(offset, _text.Length - offset, isOrdered: true));
						break;
					case "li":
						Traverse(node.ChildNodes);
						offset = char.IsWhiteSpace(_text[offset]) ? offset + 1 : offset;
						_annotations.Add(new ListItem(offset, _text.Length - offset));
						break;
					case "h1":
					case "h2":
					case "h3":
					case "h4":
					case "h5":
					case "h6":
						Traverse(node.ChildNodes);
						offset = char.IsWhiteSpace(_text[offset]) ? offset + 1 : offset;
						_annotations.Add(new Heading(offset, _text.Length - offset, int.Parse(node.Name.Substring(1, 1))));
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

			internal void AddText(string text)
			{
				string s = HtmlEntity.DeEntitize(text);

				for (int i = 0; i < s.Length; i++)
				{
					if (char.IsWhiteSpace(s[i]))
					{
						if (_text.Length > 0)
							_isSpaceQueued = true;
					}
					else
					{
						if (_isSpaceQueued)
						{
							_text.Append((char)0x20);
							_isSpaceQueued = false;
						}

						_text.Append(s[i]);
					}
				}
			}

			bool _isSpaceQueued = false;

			StringBuilder _text = new StringBuilder();

			List<Annotation> _annotations = new List<Annotation>();

			readonly string _documentId;
		}
	}
}
