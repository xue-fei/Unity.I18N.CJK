using System;
using System.Text;

namespace I18N.CJK
{
	[Serializable]
    public class CP936 : DbcsEncoding
	{
		private const int GB2312_CODE_PAGE = 936;

		public override string BodyName => "gb2312";

		public override string EncodingName => "Chinese Simplified (GB2312)";

		public override string HeaderName => "gb2312";

		public override bool IsBrowserDisplay => true;

		public override bool IsBrowserSave => true;

		public override bool IsMailNewsDisplay => true;

		public override bool IsMailNewsSave => true;

		public override string WebName => "gb2312";

		public CP936()
			: base(936)
		{
		}

		internal override DbcsConvert GetConvert()
		{
			return DbcsConvert.Gb2312;
		}

		protected int GetBytesInternal(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			int num = byteIndex;
			int num2 = charIndex + charCount;
			int num3 = (bytes != null) ? bytes.Length : 0;
			DbcsConvert convert = GetConvert();
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			int num4 = charIndex;
			while (num4 < num2)
			{
				char c = chars[num4];
				if (c <= '\u0080' || c == 'ÿ')
				{
					int num6 = byteIndex++;
					if (bytes != null)
					{
						bytes[num6] = (byte)c;
					}
				}
				else
				{
					byte b = convert.u2n[c * 2 + 1];
					byte b2 = convert.u2n[c * 2];
					if (b == 0 && b2 == 0)
					{
						base.HandleFallback(ref encoderFallbackBuffer, chars, ref num4, ref charCount, bytes, ref byteIndex, ref num3, null);
					}
					else if (bytes != null)
					{
						bytes[byteIndex++] = b;
						bytes[byteIndex++] = b2;
					}
					else
					{
						byteIndex += 2;
					}
				}
				num4++;
				charCount--;
			}
			return byteIndex - num;
		}

		public override int GetByteCount(char[] chars, int index, int count)
		{
			return GetBytes(chars, index, count, null, 0);
		}

		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			return GetBytesInternal(chars, charIndex, charCount, bytes, byteIndex);
		}

		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			return GetDecoder().GetCharCount(bytes, index, count);
		}

		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			return GetDecoder().GetChars(bytes, byteIndex, byteCount, chars, charIndex);
		}

		public override Decoder GetDecoder()
		{
			return new CP936Decoder(GetConvert());
		}
	}
}
