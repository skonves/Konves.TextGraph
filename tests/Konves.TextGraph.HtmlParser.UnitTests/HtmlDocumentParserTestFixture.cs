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
			// Arrange
			Stream stream = ToStream("<h2>header text</h2> <h2>header text</h2>");

			string docId = "abc";
			string expectedText = "header text header text";
			List<Annotation> expectedAnnotations = new List<Annotation> { new Heading(0, 11, 2), new Heading(12, 11, 2) };

			HtmlDocumentParser sut = new HtmlDocumentParser();

			// Act
			var result = sut.Parse(docId, stream);

			// Assert
			AssertDocuemnt(result, expectedText, expectedAnnotations);
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_Emphasis()
		{
			// Arrange
			Stream stream = ToStream("<em>emphasized text</em>");

			string docId = "abc";
			string expectedText = "emphasized text";
			List<Annotation> expectedAnnotations = new List<Annotation> { new Emphasis(0, 15) };

			HtmlDocumentParser sut = new HtmlDocumentParser();

			// Act
			var result = sut.Parse(docId, stream);

			// Assert
			AssertDocuemnt(result, expectedText, expectedAnnotations);
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_Link()
		{
			// Arrange
			string docId = "abc";
			string expectedText = "link text";
			string expectedUri = "http://example.com/path?query=value#hash";

			Stream stream = ToStream($"<a HrEf='{expectedUri}'>{expectedText}</a>");

			List<Annotation> expectedAnnotations = new List<Annotation> { new Link(0, 9, expectedUri) };

			HtmlDocumentParser sut = new HtmlDocumentParser();

			// Act
			var result = sut.Parse(docId, stream);

			// Assert
			AssertDocuemnt(result, expectedText, expectedAnnotations);
		}

		[TestCategory(nameof(HtmlDocumentParser))]
		[TestMethod]
		public void ParseTest_Paragraph()
		{
			// Arrange
			string docId = "abc";
			string expectedText = "paragraph text";

			Stream stream = ToStream($@"<p>paragraph
text</p>");

			List<Annotation> expectedAnnotations = new List<Annotation> { new Paragraph(0, 14) };

			HtmlDocumentParser sut = new HtmlDocumentParser();

			// Act
			var result = sut.Parse(docId, stream);

			// Assert
			AssertDocuemnt(result, expectedText, expectedAnnotations);
		}

		private void AssertDocuemnt(ICollection<Document> result, string expectedText, IEnumerable<Annotation> expectedAnnotations)
		{
			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());

			Document document = result.Single();

			Assert.AreEqual(expectedText, document.Text);
			CollectionAssert.AreEquivalent(expectedAnnotations.ToList(), document.Annotations);
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
