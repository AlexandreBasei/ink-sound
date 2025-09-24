using UnityEngine;

[CreateAssetMenu(menuName = "Tools/MatterGun/MatterGun Data")] //Create a new matter gun Data object by right clicking in the Project Menu then Create/Player/Player Data and drag onto the matter gun
public class MatterGunData : ScriptableObject
{
	[Header("Space parameters")]

	public float holdDistance = 0.5f; //Distance from the player to hold the matter gun

	[Space(20)]
	[Header("Shoot parameters")]

	public float shootForce = 10f; //Speed of the matter projectile
	public float fireRate = 40f; // Fire rate of the matter gun

	//Unity Callback, called when the inspector updates
	private void OnValidate()
	{
		// Ensure the hold distance is always positive
		if (holdDistance < 0)
		{
			holdDistance = 0;
		}

		// Ensure the shoot force is always positive
		if (shootForce < 0)
		{
			shootForce = 0;
		}

		if (fireRate < 0)
		{
			fireRate = 0;
		}
	}
}