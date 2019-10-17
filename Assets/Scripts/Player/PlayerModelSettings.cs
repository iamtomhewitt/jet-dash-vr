using UnityEngine;

namespace Player
{
	public class PlayerModelSettings : MonoBehaviour
	{
		[SerializeField] private GameObject[] models;

		private Transform model;
		private Quaternion originalRotation;

		private float z = 0f;

		/// <summary>
		/// Rotates the ship based on the mobile accelerometer.
		/// </summary>
		public void RotateBasedOnMobileInput(Transform t, float limit)
		{
			z = Input.acceleration.x * 30f;
			z = Mathf.Clamp(z, -limit, limit);

			t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y, -z);
		}

		public void RotateBaseOnKeyboardInput(Transform t, float limit)
		{
			z = Input.GetAxis("Horizontal") * 30f;
			z = Mathf.Clamp(z, -limit, limit);
			t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y, -z);
		}

		/// <summary>
		/// Activates a random ship at the start of the game.
		/// </summary>
		public void SelectRandomShip()
		{
			for (int i = 0; i < models.Length; i++)
			{
				models[i].SetActive(false);
			}

			int j = Random.Range(0, models.Length);
			model = models[j].transform;

			model.gameObject.SetActive(true);
		}

		public void SetOriginalRotation()
		{
			originalRotation = model.rotation;
		}

		public Transform GetModel()
		{
			return model;
		}
	}
}