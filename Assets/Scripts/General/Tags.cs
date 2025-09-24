using UnityEngine;

public class Tags
{
    public static bool CompareTags(string tag, GameObject obj)
    {
        // Extraction des tags
        string[] tabTags = obj.tag.Split(',');

        //Comparaison
        foreach (string t in tabTags)
        {
            if (t.Trim() == tag) return true; // Si un des tags correspond, on retourne true
        }
        return false; // Si aucun tag ne correspond, on retourne false
    }
}
