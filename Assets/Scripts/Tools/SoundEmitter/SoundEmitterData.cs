using UnityEngine;

[CreateAssetMenu(menuName = "Tools/SoundEmitter/SoundEmitter Data")]
public class SoundEmitterData : ScriptableObject
{
    [Header("Space parameters")]
    public float holdDistance; //Distance from the player to hold the sound emitter

    [Space(20)]
    [Header("Propagation parameters")]
    public float soundSpeed;
    private void OnValidate()
    {
        // Ensure the hold distance is always positive
        if (holdDistance < 0)
        {
            holdDistance = 0;
        }

        // Ensure the sound speed is always positive
        if (soundSpeed < 0)
        {
            soundSpeed = 0;
        }
    }
}