using System.Text;
using System;

namespace I18N.Common
{
    [Serializable]
    public abstract class MonoSafeEncoding : Encoding
    {
        private readonly int win_code_page;

        public override int WindowsCodePage
        {
            get
            {
                if (win_code_page == 0)
                {
                    return base.WindowsCodePage;
                }

                return win_code_page;
            }
        }

        public MonoSafeEncoding(int codePage)
            : this(codePage, 0)
        {
        }

        public MonoSafeEncoding(int codePage, int windowsCodePage)
            : base(codePage)
        {
            win_code_page = windowsCodePage;
        }

        protected virtual int GetBytesInternal(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush, object state)
        {
            throw new NotImplementedException("Statefull encoding is not implemented (yet?) by this encoding class.");
        }

        public void HandleFallback(ref EncoderFallbackBuffer buffer, char[] chars, ref int charIndex, ref int charCount, byte[] bytes, ref int byteIndex, ref int byteCount, object state)
        {
            if (buffer == null)
            {
                buffer = base.EncoderFallback.CreateFallbackBuffer();
            }

            if (charCount > 1 && char.IsSurrogate(chars[charIndex]) && char.IsSurrogate(chars[charIndex + 1]))
            {
                buffer.Fallback(chars[charIndex], chars[charIndex + 1], charIndex);
                charIndex++;
                charCount--;
            }
            else
            {
                buffer.Fallback(chars[charIndex], charIndex);
            }

            char[] array = new char[buffer.Remaining];
            int num = 0;
            while (buffer.Remaining > 0)
            {
                array[num++] = buffer.GetNextChar();
            }

            int num2 = ((state == null) ? GetBytes(array, 0, array.Length, bytes, byteIndex) : GetBytesInternal(array, 0, array.Length, bytes, byteIndex, flush: true, state));
            byteIndex += num2;
            byteCount -= num2;
        }
    }
}