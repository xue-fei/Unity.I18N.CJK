namespace I18N.CJK
{
	public class DbcsConvert
	{
		public byte[] n2u;

		public byte[] u2n;

		internal static readonly DbcsConvert Gb2312 = new DbcsConvert("gb2312.table");

		internal static readonly DbcsConvert Big5 = new DbcsConvert("big5.table");

		internal static readonly DbcsConvert KS = new DbcsConvert("ks.table");

		internal DbcsConvert(string fileName)
		{
			using (CodeTable codeTable = new CodeTable(fileName))
			{
				n2u = codeTable.GetSection(1);
				u2n = codeTable.GetSection(2);
			}
		}
	}
}
