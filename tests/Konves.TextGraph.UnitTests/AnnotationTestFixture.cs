using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Konves.TextGraph.Models;

namespace Konves.TextGraph.UnitTests
{
	[TestClass]
	public class AnnotationTestFixture
	{
		[TestCategory("Annotation.Intersects")]
		[TestMethod]
		public void TestMultiCharOverlap()
		{
			TestIntersects(0, 5, 3, 5, true);
			TestIntersects(3, 5, 0, 5, true);
		}

		[TestCategory("Annotation.Intersects")]
		[TestMethod]
		public void TestSingleCharOverlap()
		{
			TestIntersects(0, 5, 4, 5, true);
			TestIntersects(4, 5, 0, 5, true);
		}

		[TestCategory("Annotation.Intersects")]
		[TestMethod]
		public void TestFullOverlap()
		{
			TestIntersects(0, 10, 3, 5, true);
			TestIntersects(3, 5, 0, 10, true);
		}

		[TestCategory("Annotation.Intersects")]
		[TestMethod]
		public void TestAdjacent()
		{
			TestIntersects(0, 5, 5, 5, false);
			TestIntersects(5, 5, 0, 5, false);
		}

		[TestCategory("Annotation.CompareTo")]
		[TestMethod]
		public void TestSameOffset()
		{
			TestCompareTo(0, 5, 0, 5, 0);
			TestCompareTo(0, 5, 0, 6, 1);
			TestCompareTo(0, 5, 0, 4, -1);
		}

		[TestCategory("Annotation.CompareTo")]
		[TestMethod]
		public void TestDifferentOffset()
		{
			TestCompareTo(1, 5, 0, 5, 1);
			TestCompareTo(0, 5, 1, 5, -1);
		}

		[TestCategory("Annotation.Ctor")]
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestOffsetOutOfRange()
		{
			new TestAnnotation(-1, 5);
		}

		[TestCategory("Annotation.Ctor")]
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestLengthOutOfRange()
		{
			new TestAnnotation(5, -1);
		}

		[TestCategory("Annotation.Equals")]
		[TestMethod]
		public void TestEquals()
		{
			DoTestEquals(new TestAnnotation(5, 7), new TestAnnotation(5, 7), true);
			DoTestEquals(new TestAnnotation(5, 3), new TestAnnotation(5, 7), false);
			DoTestEquals(new TestAnnotation(5, 3), null, false);
			DoTestEquals(new TestAnnotation(5, 7), new TestAnnotationB(5, 7), false);
		}

		private void DoTestEquals(Annotation a, Annotation b, bool expectedResult)
		{
			// Arrange

			// Act
			bool result = a.Equals(b);

			// Assert
			Assert.AreEqual(expectedResult, result);
		}

		[TestCategory("Annotation.GetHAshCode")]
		[TestMethod]
		public void TestGetHashCode()
		{
			// Arrange
			Annotation sut = new TestAnnotation(5, 7);

			// Act
			int result = sut.GetHashCode();

			// Assert
			Assert.AreNotEqual(0, result);
		}

		[TestCategory("Annotation.GetHAshCode")]
		[TestMethod]
		public void TestGetHashCode_Base()
		{
			// Arrange
			TestAnnotationForHashCodes sut = new TestAnnotationForHashCodes();

			// Act
			int result = sut.PublicGetHashCode(null);

			// Assert
			Assert.AreEqual(0, result);
		}

		private void TestIntersects(int offset1, int length1, int offset2, int length2, bool expectedResult)
		{
			// Arrange
			Annotation a = new TestAnnotation(offset1, length1);
			Annotation b = new TestAnnotation(offset2, length2);

			// Act
			bool result = a.Intersects(b);

			// Assert
			Assert.AreEqual(expectedResult, result);
		}

		private void TestCompareTo(int offset1, int length1, int offset2, int length2, int exectedSign)
		{
			// Arrange
			IComparable<Annotation> a = new TestAnnotation(offset1, length1);
			Annotation b = new TestAnnotation(offset2, length2);

			// Act
			int result = a.CompareTo(b);

			// Assert
			Assert.AreEqual(exectedSign, Math.Sign(result));
		}

		private void TestCtor(int offset, int length)
		{
			// Arrange

			// Act
			Annotation a = new TestAnnotation(offset, length);

			// Assert
		}

		private class TestAnnotation : Annotation
		{
			public TestAnnotation(int offset, int length) : base(offset, length) { }

			public override string Subtype
			{
				get
				{
					return "Subtype";
				}
			}

			public override string Type
			{
				get
				{
					return "Type";
				}
			}
		}

		private class TestAnnotationB : Annotation
		{
			public TestAnnotationB(int offset, int length) : base(offset, length) { }

			public override string Subtype
			{
				get
				{
					return "Subtype";
				}
			}

			public override string Type
			{
				get
				{
					return "TypeB";
				}
			}
		}

		private class TestAnnotationForHashCodes : Annotation
		{
			public TestAnnotationForHashCodes() : base(0, 0)
			{
			}

			public int PublicGetHashCode(params object[] fields)
			{
				return GetHashCode(fields);
			}

			public override string Subtype
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public override string Type
			{
				get
				{
					throw new NotImplementedException();
				}
			}
		}
	}
}
