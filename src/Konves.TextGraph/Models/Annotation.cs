using System;

namespace Konves.TextGraph.Models
{
	public abstract class Annotation : IComparable<Annotation>
	{
		public Annotation(string documentId, int offset, int length)
		{
			if (documentId == null)
				throw new ArgumentNullException(nameof(documentId));

			if (offset < 0)
				throw new ArgumentOutOfRangeException(nameof(offset), $"{nameof(offset)} must be a non-negative integer.");

			if (length < 0)
				throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} must be a non-negative integer.");

			_documentId = documentId;
			_offset = offset;
			_length = length;
		}

		public abstract string Type { get; }
		public abstract string Subtype { get; }
		public string DocumentId { get { return _documentId; } }
		public int Offset { get { return _offset; } }
		public int Length { get { return _length; } }

		private readonly string _documentId;
		private readonly int _offset;
		private readonly int _length;

		public int CompareTo(Annotation other)
		{
			// TODO: throw if document IDs differ
			return _offset == other.Offset ? other.Length - _length : _offset - other.Offset;
		}

		public bool Intersects(Annotation other)
		{
			// TODO: consider throwing if document IDs differ
			return (_offset <= other.Offset && other.Offset <= _offset + _length - 1) || (other.Offset <= _offset && _offset <= other.Offset + other.Length - 1);
		}

		public override bool Equals(object obj)
		{
			Annotation other = obj as Annotation;

			return !ReferenceEquals(other, null) && other.DocumentId == DocumentId && other.Type == Type && other.Subtype == Subtype && other._offset == _offset && other._length == _length;
		}

		public override int GetHashCode()
		{
			return GetHashCode(Type, Subtype, DocumentId, Offset, Length);
		}

		protected int GetHashCode(params object[] fields)
		{
			if (fields == null)
				return 0;

			unchecked
			{
				int hash = 17;

				foreach (object field in fields)
					hash = hash * 31 + field?.GetHashCode() ?? 0;

				return hash;
			}
		}
	}
}
