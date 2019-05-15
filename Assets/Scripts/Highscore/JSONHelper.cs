using UnityEngine;
using System.Collections;
using System.Linq;

public class JSONHelper
{
	/// <summary>
	/// Formats the JSON downloaded from Dreamlo so it starts with the 'entry' tag instead of the 'dreamlo' tag.
	/// 'n' is how far down the JSON tree it should go.
	/// </summary>
	public static string RemoveNJsonFields(string source, int n)
	{
		n++;
		int index = source.TakeWhile(c => (n -= (c == '{' ? 1 : 0)) > 0).Count();
		return source.Substring(index, source.Length - (index + 2 - n));
	}
}
