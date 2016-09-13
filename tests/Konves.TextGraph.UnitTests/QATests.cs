using Konves.TextGraph.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konves.TextGraph.UnitTests
{
	public class DocumentValidator
	{
		public void Run(Document document)
		{
			// Assert
			Assert.IsNotNull(document);
			Assert.IsNotNull(document.Id, "Document shall have a globally unique, non-null identifier");
			Assert.IsNotNull(document.Text);
			Assert.IsNotNull(document.Annotations, "Docment shall have a non-null collection of zero or more annotations");

			Assert.IsFalse(char.IsWhiteSpace(document.Text[0]), "Document text shall not begin with whitespace.");
			Assert.IsFalse(char.IsWhiteSpace(document.Text[document.Text.Length - 1]), "Document text shall not end with whitespace.");

			for (int i = 0; i < document.Text.Length; i++)
			{
				if (char.IsWhiteSpace(document.Text[i]))
				{
					Assert.AreEqual((char)0x20, document.Text[i], "All whitespace characters shall be a space (0x20).");

					if (i > 0 && char.IsWhiteSpace(document.Text[i - 1]))
						Assert.Fail("Document text shall not contain multiple subsequent whitespace");
				}
			}

			foreach (Annotation annotation in document.Annotations)
			{
				Assert.IsTrue(annotation.Offset + annotation.Length <= document.Text.Length, "Annotation range shall be a subset of the document text range");

				string text = document.Text.Substring(annotation.Offset, annotation.Length);

				if (text.Length > 0)
				{
					Assert.IsFalse(char.IsWhiteSpace(text[0]), "Annotation text shall not begin with whitespace.");
					Assert.IsFalse(char.IsWhiteSpace(text[text.Length - 1]), "Annotation text shall not end with whitespace.");
				}
			}
		}
	}
}
