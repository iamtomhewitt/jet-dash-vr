using Manager;

namespace LevelManagers
{
	/// <summary>
	/// Handles everything on the Shop menu.
	/// </summary>
	public class ShopLevelManager : LevelManager
	{
		/// <summary>
		/// Called from a Unity button.
		/// </summary>
		public void BuyShip(string name)
		{
			ShopManager.instance.PurchaseShip(name);
		}
	}
}