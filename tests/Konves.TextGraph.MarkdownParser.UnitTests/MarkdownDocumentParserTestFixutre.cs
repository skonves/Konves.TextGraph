using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Konves.TextGraph.MarkDownParser;
using System.IO;
using Konves.TextGraph.Models;
using System.Collections.ObjectModel;

namespace Konves.TextGraph.MarkdownParser.UnitTests
{
	[TestClass]
	public class MarkdownDocumentParserTestFixutre
	{
		[TestMethod]
		public void ParseTest()
		{
			// Arrange
			string markdown = @"
# header *bold* asdf

Some body text with *bold stuff* in it

> Block quote
> of some text
> that is quoted

```
some code
```

";

			Stream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);

			writer.Write(markdown);
			writer.Flush();
			writer.BaseStream.Position = 0;

			MarkdownDocumentParser sut = new MarkdownDocumentParser();

			// Act
			Document result = sut.Parse("asdf", stream);

			// Assert

		}
	}
}
