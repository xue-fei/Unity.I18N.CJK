using System;
using System.IO;
using UnityEngine;

namespace I18N.CJK
{
    public class CodeTable : IDisposable
    {
        private Stream stream;

        public CodeTable(string name)
        {
            Debug.Log(name);
            TextAsset textAsset = Resources.Load("Table/" + name) as TextAsset;
            stream = new MemoryStream(textAsset.bytes);
            if (stream != null)
            {
                Debug.Log(name +" "+ textAsset.bytes.Length);
                return;
            }
            Debug.Log(name + " " + textAsset.bytes.Length);
            throw new NotSupportedException("NotSupp_MissingCodeTable " +name);
            //stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
        }

        public void Dispose()
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }
        }

        public byte[] GetSection(int num)
        {
            if (stream == null)
            {
                return null;
            }
            long num2 = 0L;
            long length = stream.Length;
            byte[] array = new byte[8];
            int num4;
            for (; num2 + 8 <= length; num2 += 8 + num4)
            {
                stream.Position = num2;
                if (stream.Read(array, 0, 8) != 8)
                {
                    break;
                }
                int num3 = array[0] | array[1] << 8 | array[2] << 16 | array[3] << 24;
                num4 = (array[4] | array[5] << 8 | array[6] << 16 | array[7] << 24);
                if (num3 == num)
                {
                    byte[] array2 = new byte[num4];
                    if (stream.Read(array2, 0, num4) != num4)
                    {
                        break;
                    }
                    return array2;
                }
            }
            return null;
        }
    }
}