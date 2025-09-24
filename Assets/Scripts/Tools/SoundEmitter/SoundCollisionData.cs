using UnityEngine;

[CreateAssetMenu(menuName = "Tools/SoundEmitter/SoundCollision Data")]
public class SoundCollisionData : ScriptableObject
{
    [Header("Sound parameters")]
    public float lifetime = 5f; // Lifetime of the sound in seconds
    public float soundSpeed = 10f; // vitesse minimale après rebond
}
