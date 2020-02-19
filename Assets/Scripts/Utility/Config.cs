using UnityEngine;
using SimpleJSON;

namespace Utility
{
	/// <summary>
	/// Access config as JSON via this component. <para/>
	/// 
	/// E.g. : <code>string key = Config.GetConfig()["apiKeys"]["keyName"]</code>
	/// </summary>
	public class Config : MonoBehaviour
	{
		[SerializeField] private TextAsset configFile;

		private JSONNode root;

		public static Config instance;

		private void Awake()
		{
			root = JSON.Parse(configFile.text);

			if (instance)
			{
				DestroyImmediate(gameObject);
			}
			else
			{
				DontDestroyOnLoad(gameObject);
				instance = this;
			}
		}

		public JSONNode GetConfig()
		{
			return root;
		}
	}
}