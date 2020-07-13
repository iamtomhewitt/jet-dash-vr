using UnityEngine.UI;
using UnityEngine;
using Utility;

namespace UI
{
	public class Version : MonoBehaviour
	{
		private void Start()
		{
			GetComponent<Text>().SetText(Ui.VERSION(Application.version));
		}
	}
}