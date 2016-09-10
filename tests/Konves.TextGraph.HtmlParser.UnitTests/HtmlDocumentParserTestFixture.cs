using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Konves.TextGraph.Models;
using System.Linq;
using Konves.TextGraph.Annotations;
using System.Collections.Generic;

namespace Konves.TextGraph.HtmlParser.UnitTests
{
	[TestClass]
	public class HtmlDocumentParserTestFixture
	{
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
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);

			Document document = result.Single();

			Assert.AreEqual(expectedText, document.Text);
			CollectionAssert.AreEquivalent(expectedAnnotations, document.Annotations);
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
