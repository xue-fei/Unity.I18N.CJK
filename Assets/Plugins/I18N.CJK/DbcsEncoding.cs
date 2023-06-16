using I18N.Common;
using System;
using System.Text;

namespace I18N.CJK
{
	[Serializable]
	public abstract class DbcsEncoding : MonoSafeEncoding
	{
		internal abstract class DbcsDecoder : Decoder
		{
			protected DbcsConvert convert;

			public DbcsDecoder(DbcsConvert convert)
			{
				this.convert = convert;
			}

			internal void CheckRange(byte[] bytes, int index, int count)
			{
				if (bytes == null)
				{
					throw new ArgumentNullException("bytes");
				}
				if (index >= 0 && index <= bytes.Length)
				{
					if (count >= 0 && count <= bytes.Length - index)
					{
						return;
					}
					throw new ArgumentOutOfRangeException("count", "ArgRange_Array");
				}
				throw new ArgumentOutOfRangeException("index", "ArgRange_Array");
			}

			internal void CheckRange(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
			{
				if (bytes == null)
				{
					throw new ArgumentNullException("bytes");
				}
				if (chars == null)
				{
					throw new ArgumentNullException("chars");
				}
				if (byteIndex >= 0 && byteIndex <= bytes.Length)
				{
					if (byteCount >= 0 && byteIndex + byteCount <= bytes.Length)
					{
						if (charIndex >= 0 && charIndex <= chars.Length)
						{
							return;
						}
						throw new ArgumentOutOfRangeException("charIndex", "ArgRange_Array");
					}
					throw new ArgumentOutOfRangeException("byteCount", "ArgRange_Array");
				}
				throw new ArgumentOutOfRangeException("byteIndex", "ArgRange_Array");
			}
		}

		public override bool IsBrowserDisplay => true;

		public override bool IsBrowserSave => true;

		public override bool IsMailNewsDisplay => true;

		public override bool IsMailNewsSave => true;

		public DbcsEncoding(int codePage)
			: this(codePage, 0)
		{
		}

		public DbcsEncoding(int codePage, int windowsCodePage)
			: base(codePage, windowsCodePage)
		{
		}

		internal abstract DbcsConvert GetConvert();

		public override int GetByteCount(char[] chars, int index, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (index >= 0 && index <= chars.Length)
			{
				if (count >= 0 && index + count <= chars.Length)
				{
					byte[] bytes = new byte[count * 2];
					return GetBytes(chars, index, count, bytes, 0);
				}
				throw new ArgumentOutOfRangeException("count", "ArgRange_Array");
			}
			throw new ArgumentOutOfRangeException("index", "ArgRange_Array");
		}

		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (index >= 0 && index <= bytes.Length)
			{
				if (count >= 0 && index + count <= bytes.Length)
				{
					char[] chars = new char[count];
					return GetChars(bytes, index, count, chars, 0);
				}
				throw new ArgumentOutOfRangeException("count", "ArgRange_Array");
			}
			throw new ArgumentOutOfRangeException("index", "ArgRange_Array");
		}

		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (byteIndex >= 0 && byteIndex <= bytes.Length)
			{
				if (byteCount >= 0 && byteIndex + byteCount <= bytes.Length)
				{
					if (charIndex >= 0 && charIndex <= chars.Length)
					{
						return 0;
					}
					throw new ArgumentOutOfRangeException("charIndex", "ArgRange_Array");
				}
				throw new ArgumentOutOfRangeException("byteCount", "ArgRange_Array");
			}
			throw new ArgumentOutOfRangeException("byteIndex", "ArgRange_Array");
		}

		public override int GetMaxByteCount(int charCount)
		{
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount", "ArgRange_NonNegative");
			}
			return charCount * 2;
		}

		public override int GetMaxCharCount(int byteCount)
		{
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", "ArgRange_NonNegative");
			}
			return byteCount;
		}
	}
}
