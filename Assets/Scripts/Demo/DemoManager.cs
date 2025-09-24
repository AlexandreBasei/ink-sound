using UnityEngine;
using TMPro;

public class DemoManager : MonoBehaviour
{
    private Camera _cam;
    private PlayerMovement _player;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TextMeshProUGUI nameText;

    public SceneData SceneData;
    public GameObject levelGrid; // Référence au GameObject "Level Grid" assigné via l’inspecteur

    private int _currentLevelIndex = 0;

    private void Awake()
    {
        _cam = FindFirstObjectByType<Camera>();
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        SetSceneData(SceneData);
        SwitchLevel(0);
    }

    public void SetSceneData(SceneData data)
    {
        SceneData = data;
        _cam.backgroundColor = data.backgroundColor;
    }

    public void SwitchLevel(int index)
    {
        int childCount = levelGrid.transform.childCount;

        if (index < 0 || index >= childCount)
        {
            Debug.LogWarning("Invalid level index: " + index);
            return;
        }

        // Désactiver le niveau actuel
        for (int i = 0; i < childCount; i++)
        {
            levelGrid.transform.GetChild(i).gameObject.SetActive(false);
        }

        // Activer le nouveau niveau
        GameObject newLevel = levelGrid.transform.GetChild(index).gameObject;
        newLevel.SetActive(true);

        _player.transform.position = spawnPoint.position;
        _currentLevelIndex = index;
    }

    public static GameObject GetCurrentLevel()
    {
        GameObject levelGridObj = FindFirstObjectByType<DemoManager>().levelGrid;

        if (levelGridObj == null)
        {
            Debug.LogWarning("Level Grid not found in scene.");
            return null;
        }

        foreach (Transform child in levelGridObj.transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                return child.gameObject;
            }
        }

        Debug.LogWarning("No active level found under Level Grid.");
        return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            int nextIndex = (_currentLevelIndex + 1) % levelGrid.transform.childCount;
            SwitchLevel(nextIndex);
            Debug.Log("Switched to level: " + nextIndex);
        }
    }
}
