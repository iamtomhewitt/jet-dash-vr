using SimpleJSON;
using UnityEngine;

namespace Utility
{
	/// <summary>
	/// Access config as JSON via this component. <para/>
	/// 
	/// E.g. : <code>string key = Config.GetConfig()["apiKeys"]["keyName"]</code>
	/// </summary>
	public class Config : MonoBehaviour
	{
		private JSONNode root;

		public static Config instance;

		private void Awake()
		{
			if (instance)
			{
				DestroyImmediate(gameObject);
			}
			else
			{
				DontDestroyOnLoad(gameObject);
				instance = this;
			}

			TextAsset configFile = Resources.Load<TextAsset>("config");

			if (configFile == null)
			{
				Debug.LogError("ERROR: Could not load config from Resources/config.json");
			}

			root = JSON.Parse(configFile.text);
		}

		public JSONNode GetConfig()
		{
			return root;
		}
	}
}