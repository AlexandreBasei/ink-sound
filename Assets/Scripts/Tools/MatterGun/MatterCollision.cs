using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class MatterCollision : MonoBehaviour
{
    public Sprite paintSprite;          // Le sprite représentant la peinture
    public MatterCollisionData Data; // Données de collision pour la peinture

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Ground,NonReflective")) // Vérifie si le mur a le tag "Ground"
        {
            // Récupère la position de la collision
            Vector3 collisionPos = collision.contacts[0].point;

            // Convertir la position de la collision en coordonnées de la Tilemap
            Vector3Int tilePos = collision.collider.GetComponent<Tilemap>().WorldToCell(collisionPos);

            // Peindre sur la tilemap active
            PaintOnTilemaps(tilePos);

            // Détruire le projectile après la collision
            Destroy(gameObject); // Suppression du projectile
        }
    }

    // Peindre sur toutes les Tilemaps avec le tag "Ground"
    private void PaintOnTilemaps(Vector3Int tilePos)
    {
        GameObject currentLevel = DemoManager.GetCurrentLevel();
        if (currentLevel == null) return;

        Debug.Log("Current level: " + currentLevel.name);

        // Récupérer les Tilemaps enfants Reflective et NonReflective
        Tilemap reflectiveTilemap = currentLevel.transform.Find("Reflective")?.GetComponent<Tilemap>();
        Tilemap nonReflectiveTilemap = currentLevel.transform.Find("NonReflective")?.GetComponent<Tilemap>();

        if (reflectiveTilemap == null || nonReflectiveTilemap == null)
        {
            Debug.LogWarning("Reflective or NonReflective tilemap not found under active level.");
            return;
        }

        bool isReflective = this.CompareTag("Matter,Reflective");
        bool isNonReflective = this.CompareTag("Matter,NonReflective");

        Tilemap sourceTilemap = null;
        Tilemap targetTilemap = null;
        TileBase originalTile = null;

        // Déterminer sur quelle tilemap on a touché une tuile
        if (reflectiveTilemap.HasTile(tilePos))
        {
            sourceTilemap = reflectiveTilemap;
            originalTile = reflectiveTilemap.GetTile(tilePos);
            targetTilemap = isReflective ? reflectiveTilemap : nonReflectiveTilemap;
        }
        else if (nonReflectiveTilemap.HasTile(tilePos))
        {
            sourceTilemap = nonReflectiveTilemap;
            originalTile = nonReflectiveTilemap.GetTile(tilePos);
            targetTilemap = isNonReflective ? nonReflectiveTilemap : reflectiveTilemap;
        }

        if (originalTile is Tile original && sourceTilemap != null && targetTilemap != null)
        {
            // Supprimer la tuile d'origine
            sourceTilemap.SetTile(tilePos, null);

            // Créer une nouvelle tuile
            Tile copiedTile = ScriptableObject.CreateInstance<Tile>();
            copiedTile.sprite = paintSprite != null ? paintSprite : original.sprite;
            copiedTile.color = isReflective ? Color.blue : Color.red;
            copiedTile.colliderType = original.colliderType;
            copiedTile.flags = original.flags;

            // Placer la nouvelle tuile sur la tilemap cible
            targetTilemap.SetTile(tilePos, copiedTile);
            targetTilemap.RefreshTile(tilePos);
        }
    }
}
