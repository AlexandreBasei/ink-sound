using UnityEngine;

[CreateAssetMenu(menuName = "Scene Data")]
public class SceneData : ScriptableObject
{
    [Header("Camera")]

    [Header("Colors")]
    public Color backgroundColor;
    public Color foregroundColor;
}
