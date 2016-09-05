using System;

namespace Konves.TextGraph.Models
{
	public abstract class Annotation : IComparable<Annotation>
	{
		public Annotation(int offset, int length)
		{
			if (offset < 0)
				throw new ArgumentOutOfRangeException(nameof(offset), $"{nameof(offset)} must be a non-negative integer.");

			if (length < 0)
				throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} must be a non-negative integer.");

			_offset = offset;
			_length = length;
		}

		public abstract string Type { get; }
		public abstract string Subtype { get; }
		public int Offset { get { return _offset; } }
		public int Length { get { return _length; } }

		private readonly int _offset;
		private readonly int _length;

		public int CompareTo(Annotation other)
		{
			return _offset == other.Offset ? other.Length - _length : _offset - other.Offset;
		}

		public bool Intersects(Annotation other)
		{
			return (_offset <= other.Offset && other.Offset <= _offset + _length - 1) || (other.Offset <= _offset && _offset <= other.Offset + other.Length - 1);
		}
	}
}
