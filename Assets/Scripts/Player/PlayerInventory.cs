using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject toolsObject;
    private GameObject[] tools; //Array of tools to be used
    private int currentToolIndex = 0;
    private GameObject currentTool;
    private InputSystem_Actions _inputActions;
    private Vector2 _aimInput;

    void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Previous.performed += ctx => OnPreviousInput();
        _inputActions.Player.Next.performed += ctx => OnNextInput();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }
    private void OnDisable()
    {
        _inputActions.Disable();
    }
    void Start()
    {
        // Récupérer tous les enfants de toolsObject et les stocker dans le tableau tools
        int childCount = toolsObject.transform.childCount;
        tools = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            tools[i] = toolsObject.transform.GetChild(i).gameObject;
            //Désactiver tous les outils au départ
            tools[i].SetActive(false);
        }

        currentTool = tools[currentToolIndex];
        currentTool.SetActive(true); //Activer le premier outil
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnPreviousInput()
    {
        currentTool.SetActive(false);
        currentToolIndex--;
        if (currentToolIndex < 0) currentToolIndex = tools.Length - 1;
        currentTool = tools[currentToolIndex];
        currentTool.SetActive(true);
    }

    private void OnNextInput()
    {
        currentTool.SetActive(false);
        currentToolIndex++;
        if (currentToolIndex >= tools.Length) currentToolIndex = 0;
        currentTool = tools[currentToolIndex];
        currentTool.SetActive(true);
    }

}