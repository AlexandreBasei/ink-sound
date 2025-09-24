using UnityEngine;

[CreateAssetMenu(menuName = "Tools/MatterGun/MatterCollision Data")]
public class MatterCollisionData : ScriptableObject
{
    [Header("Paint parameters")]

	public float paintRadius; //Radius of the paint effect on the wall
    public float paintSpeed;
    public Color paintColor = Color.red; //Color of the paint effect on the wall
}
