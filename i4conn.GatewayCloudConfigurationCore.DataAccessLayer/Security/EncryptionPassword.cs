using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Security
{
    public static class EncryptionPassword
    {
        public static string Crypt(string value, int len)
        {
			string sChiave = "Tesar S.p.A.";
			string sS;
			string sS2 = string.Empty;
			int j = 0;
			int i;
			string sC;
			int c2i;
			int c1i;
			string sAlfabeto = string.Empty;

			for (i = 32; i <= 126; i++)
			{
				sAlfabeto += (char)i;
			}

			// Compongo la chiave di criptazione
			//sChiave = (char)84 + (char)101 + (char)115 + (char)97 + (char)114 + (char)32 + (char)83 + (char)46 + (char)112 + (char)46 + (char)65 + (char)46;
			sS = value.PadRight(len);

			for (i = 0; i < sS.Length; i++)
			{
				sC = sS.Substring(i, 1);
				c2i = sAlfabeto.IndexOf(sC) + 1;
				if (c2i > -1)
				{
					c1i = sAlfabeto.IndexOf(sChiave.Substring(j % sChiave.Length, 1)) + 1;
					j++;
					sC = ((char)(((c1i + c2i - 1) % 94) + 32)).ToString();
				}
				sS2 = sS2 + sC;
			}

			return sS2;
		}

		public static string Decrypt(string value, int len)
		{
			string sChiave = "Tesar S.p.A.";
			string sS;
			string sS2 = string.Empty;
			int j = 0;
			int i;
			string sC;
			int c2i;
			int c1i;
			string sAlfabeto = string.Empty;

			for (i = 32; i <= 126; i++)
			{
				sAlfabeto += (char)i;
			}

			//sChiave = (char)84 + (char)101 + (char)115 + (char)97 + (char)114 + (char)32 + (char)83 + (char)46 + (char)112 + (char)46 + (char)65 + (char)46;
			sS = value.PadRight(len);

			for (i = 0; i < sS.Length; i++)
			{
				sC = sS.Substring(i, 1);
				c2i = sAlfabeto.IndexOf(sC) + 1;
				if (c2i > -1)
				{
					c1i = sAlfabeto.IndexOf(sChiave.Substring(j % sChiave.Length, 1)) + 1;
					j++;
					sC = sAlfabeto.Substring((c2i - c1i + 94) % 94 - 1, 1);
				}
				sS2 = sS2 + sC;
			}

			return sS2;
		}

		public static string Decrypt(string strToDecrypt)
		{
			string chiave = "Tesar S.p.A.";
			string s = string.Empty;
			string s2 = string.Empty;
			int j = 0;
			int i = 0;
			string c = string.Empty;
			int c2i = 0;
			int c1i = 0;
			string alfabeto = string.Empty;


			for (i = 32; i < 127; i++)
			{
				alfabeto = alfabeto + Convert.ToChar(i);
			}

			s = strToDecrypt.PadRight(strToDecrypt.Length);

			for (i = 0; i < s.Length; i++)
			{
				c = s.Substring(i, 1);
				c2i = alfabeto.IndexOf(c) + 1;
				if (c2i > 0)
				{
					c1i = alfabeto.IndexOf(chiave.Substring(j % chiave.Length, 1)) + 1;
					j++;
					c = alfabeto.Substring((c2i - c1i + 94) % 94 - 1, 1);
				}
				s2 = s2 + c;
			}

			return s2;
		}
	}
}
