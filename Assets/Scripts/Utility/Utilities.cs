namespace Utility
{
	public class Utilities
	{
		public static string StripNonLatinLetters(string name)
		{
			string newName = "";
			foreach (char c in name)
			{
				int code = (int)c;

				// Only english numbers, letters, symbols
				if ((code >= 32 && code <= 127))
				{
					newName += c;
				}
			}

			return newName;
		}

	}
}