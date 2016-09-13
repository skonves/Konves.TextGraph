using Konves.TextGraph.Annotations;
using Konves.TextGraph.Models;
using Konves.TextGraph.UnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Konves.TextGraph.HtmlParser.UnitTests
{
	[TestClass]
	public class HtmlDocumentParserTestFixture
	{
		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_Entities()
		{
			DoParseTest(
				sourceDocumentText: "some &amp; text &lt;br/&gt;",
				expectedDocumentText: "some & text <br/>",
				expectedAnnotations: new List<Annotation> { });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_Heading()
		{
			DoParseTest(
				sourceDocumentText: " <h2> header  text </h2> <h2> header  text </h2> ",
				expectedDocumentText: "header text header text",
				expectedAnnotations: new List<Annotation> { new Heading(0, 11, 2), new Heading(12, 11, 2) });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_Emphasis()
		{
			DoParseTest(
				sourceDocumentText: " <em> emphasized  text </em> ",
				expectedDocumentText: "emphasized text",
				expectedAnnotations: new List<Annotation> { new Emphasis(0, 15) });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_Link()
		{
			DoParseTest(
				sourceDocumentText: " <a HrEf='http://example.com/path?query=value#hash'> link  text </a> ",
				expectedDocumentText: "link text",
				expectedAnnotations: new List<Annotation> { new Link(0, 9, "http://example.com/path?query=value#hash") });

			DoParseTest(
				sourceDocumentText: " <a href=''> link  text </a> ",
				expectedDocumentText: "link text",
				expectedAnnotations: new List<Annotation> { new Link(0, 9, string.Empty) });

			DoParseTest(
				sourceDocumentText: " <a> link  text </a> ",
				expectedDocumentText: "link text",
				expectedAnnotations: new List<Annotation> { new Link(0, 9, null) });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_Paragraph()
		{
			DoParseTest(
				sourceDocumentText: @" <p> paragraph
text </p> ",
				expectedDocumentText: "paragraph text",
				expectedAnnotations: new List<Annotation> { new Paragraph(0, 14) });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_LineBreak()
		{
			DoParseTest(
				sourceDocumentText: " some text <br /> more text ",
				expectedDocumentText: "some text more text",
				expectedAnnotations: new List<Annotation> { new LineBreak(9) });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_BlockQuote()
		{
			DoParseTest(
				sourceDocumentText: " <blockquote> ab </blockquote> <blockquote>  cd  </blockquote>  ",
				expectedDocumentText: "ab cd",
				expectedAnnotations: new List<Annotation> { new BlockQuote(0, 2), new BlockQuote(3, 2) });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_OrderedList()
		{
			DoParseTest(
				sourceDocumentText: @"
<ol>
	<li>item one</li>
	<li>item two</li>
</ol>",
				expectedDocumentText: "item one item two",
				expectedAnnotations: new List<Annotation> { new ListItem(0, 8), new ListItem(9, 8), new List(0, 17, true) });

			DoParseTest(
				sourceDocumentText: "<ol><li>item one</li><li>item two</li></ol>",
				expectedDocumentText: "item oneitem two",
				expectedAnnotations: new List<Annotation> { new ListItem(0, 8), new ListItem(8, 8), new List(0, 16, true) });

			DoParseTest(
				sourceDocumentText: " <ol> <li> item  one </li> <li> item  two </li> </ol> ",
				expectedDocumentText: "item one item two",
				expectedAnnotations: new List<Annotation> { new ListItem(0, 8), new ListItem(9, 8), new List(0, 17, true) });
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_UnorderedList()
		{
			DoParseTest(
				sourceDocumentText: @"
<ul>
	<li>item one</li>
	<li>item two</li>
</ul>",
				expectedDocumentText: "item one item two",
				expectedAnnotations: new List<Annotation> { new ListItem(0, 8), new ListItem(9, 8), new List(0, 17, false) });

			DoParseTest(
				sourceDocumentText: "<ul><li>item one</li><li>item two</li></ul>",
				expectedDocumentText: "item oneitem two",
				expectedAnnotations: new List<Annotation> { new ListItem(0, 8), new ListItem(8, 8), new List(0, 16, false) });

			DoParseTest(
				sourceDocumentText: " <ul> <li> item  one </li> <li> item  two </li> </ul> ",
				expectedDocumentText: "item one item two",
				expectedAnnotations: new List<Annotation> { new ListItem(0, 8), new ListItem(9, 8), new List(0, 17, false) });
		}

		private void DoParseTest(string sourceDocumentText, string expectedDocumentText, IEnumerable<Annotation> expectedAnnotations)
		{
			// Arrange
			string sourceDocumentId = "asdf";
			Stream stream = ToStream(sourceDocumentText);
			HtmlDocumentParser sut = new HtmlDocumentParser();

			// Act
			Document result = sut.Parse(sourceDocumentId, stream);

			// Assert
			new DocumentValidator().Run(result);

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
