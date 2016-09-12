using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Konves.TextGraph.Models;
using System.Linq;
using Konves.TextGraph.Annotations;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Konves.TextGraph.HtmlParser.UnitTests
{
	[TestClass]
	public class HtmlDocumentParserTestFixture
	{
		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_Heading()
		{
			DoParseTest(
				sourceDocumentId: "asdf",
				sourceDocumentText: "<h2>header text</h2> <h2>header text</h2>",
				expectedDocumentText: "header text header text",
				expectedAnnotations: new List<Annotation> { new Heading(0, 11, 2), new Heading(12, 11, 2) });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_Emphasis()
		{
			DoParseTest(
				sourceDocumentId: "asdf",
				sourceDocumentText: "<em>emphasized text</em>",
				expectedDocumentText: "emphasized text",
				expectedAnnotations: new List<Annotation> { new Emphasis(0, 15) });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_Link()
		{
			DoParseTest(
				sourceDocumentId: "asdf",
				sourceDocumentText: "<a HrEf='http://example.com/path?query=value#hash'>link text</a>",
				expectedDocumentText: "link text",
				expectedAnnotations: new List<Annotation> { new Link(0, 9, "http://example.com/path?query=value#hash") });

			DoParseTest(
				sourceDocumentId: "asdf",
				sourceDocumentText: "<a href=''>link text</a>",
				expectedDocumentText: "link text",
				expectedAnnotations: new List<Annotation> { new Link(0, 9, string.Empty) });

			DoParseTest(
				sourceDocumentId: "asdf",
				sourceDocumentText: "<a>link text</a>",
				expectedDocumentText: "link text",
				expectedAnnotations: new List<Annotation> { new Link(0, 9, null) });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_Paragraph()
		{
			DoParseTest(
				sourceDocumentId: "asdf",
				sourceDocumentText: $@"<p>paragraph
text</p>",
				expectedDocumentText: "paragraph text",
				expectedAnnotations: new List<Annotation> { new Paragraph(0, 14) });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_LineBreak()
		{
			DoParseTest(
				sourceDocumentId: "asdf",
				sourceDocumentText: "some text<br /> more text",
				expectedDocumentText: "some text more text",
				expectedAnnotations: new List<Annotation> { new LineBreak(9) });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_BlockQuote()
		{
			DoParseTest(
				sourceDocumentId: "asdf",
				sourceDocumentText: "<blockquote>some block quoted text</blockquote>",
				expectedDocumentText: "some block quoted text",
				expectedAnnotations: new List<Annotation> { new BlockQuote(0, 22) });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_OrderedList()
		{
			DoParseTest(
				sourceDocumentId: "asdf",
				sourceDocumentText: "<ol><li>item one</li> <li>item two</li></ol>",
				expectedDocumentText: "item one item two",
				expectedAnnotations: new List<Annotation> { new ListItem(0, 8), new ListItem(9, 8), new List(0, 17, true) });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_UnorderedList()
		{
			DoParseTest(
				sourceDocumentId: "asdf",
				sourceDocumentText: "<ul><li>item one</li> <li>item two</li></ul>",
				expectedDocumentText: "item one item two",
				expectedAnnotations: new List<Annotation> { new ListItem(0, 8), new ListItem(9, 8), new List(0, 17, false) });
		}

		private void DoParseTest(string sourceDocumentId, string sourceDocumentText, string expectedDocumentText, IEnumerable<Annotation> expectedAnnotations)
		{
			// Arrange
			Stream stream = ToStream(sourceDocumentText);
			HtmlDocumentParser sut = new HtmlDocumentParser();

			// Act
			Document result = sut.Parse(sourceDocumentId, stream);

			// Assert
			Assert.AreEqual(sourceDocumentId, result.Id);
			Assert.AreEqual(expectedDocumentText, result.Text);
			CollectionAssert.AreEquivalent(expectedAnnotations.ToList(), result.Annotations);
		}

		private static Stream ToStream(string s)
		{
			Stream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);

			writer.Write(s);
			writer.Flush();
			writer.BaseStream.Position = 0;

			return stream;
		}
	}
}
