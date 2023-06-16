namespace I18N.CJK
{
	internal sealed class CP936Decoder : DbcsEncoding.DbcsDecoder
	{
		private int last_byte_count;

		private int last_byte_bytes;

		public CP936Decoder(DbcsConvert convert)
			: base(convert)
		{
		}

		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			return GetCharCount(bytes, index, count, false);
		}

		public override int GetCharCount(byte[] bytes, int index, int count, bool refresh)
		{
			base.CheckRange(bytes, index, count);
			int num = last_byte_count;
			last_byte_count = 0;
			int num2 = 0;
			while (count-- > 0)
			{
				int num5 = bytes[index++];
				if (num == 0)
				{
					if (num5 <= 128 || num5 == 255)
					{
						num2++;
					}
					else
					{
						num = num5;
					}
				}
				else
				{
					num2++;
					num = 0;
				}
			}
			if (num != 0)
			{
				if (refresh)
				{
					num2++;
					last_byte_count = 0;
				}
				else
				{
					last_byte_count = num;
				}
			}
			return num2;
		}

		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			return GetChars(bytes, byteIndex, byteCount, chars, charIndex, false);
		}

		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, bool refresh)
		{
			base.CheckRange(bytes, byteIndex, byteCount, chars, charIndex);
			int num = charIndex;
			int num2 = last_byte_bytes;
			last_byte_bytes = 0;
			while (byteCount-- > 0)
			{
				int num5 = bytes[byteIndex++];
				if (num2 == 0)
				{
					if (num5 <= 128 || num5 == 255)
					{
						chars[charIndex++] = (char)num5;
					}
					else if (num5 >= 129 && num5 < 255)
					{
						num2 = num5;
					}
				}
				else
				{
					int num7 = ((num2 - 129) * 191 + num5 - 64) * 2;
					char c = (num7 >= 0 && num7 < base.convert.n2u.Length) ? ((char)(base.convert.n2u[num7] + base.convert.n2u[num7 + 1] * 256)) : '\0';
					if (c == '\0')
					{
						chars[charIndex++] = '?';
					}
					else
					{
						chars[charIndex++] = c;
					}
					num2 = 0;
				}
			}
			if (num2 != 0)
			{
				if (refresh)
				{
					chars[charIndex++] = '?';
					last_byte_bytes = 0;
				}
				else
				{
					last_byte_bytes = num2;
				}
			}
			return charIndex - num;
		}
	}
}
