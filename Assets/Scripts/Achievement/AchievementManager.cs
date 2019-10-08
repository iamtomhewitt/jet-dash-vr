using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
	private void Start()
	{
		Achievement[] a = new Achievement[1];
		Achievement b = new Achievement();
		b.id = 1;
		a[0] = b;

		string json = AchievementJsonHelper.ToJson(a);

		print(json);

		a = AchievementJsonHelper.FromJson(json);
		foreach (Achievement ac in a)
		{
			print(ac.id);
		}
	}
}
