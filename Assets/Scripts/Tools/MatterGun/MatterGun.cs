using UnityEngine;
using UnityEngine.InputSystem;

public class MatterGun : MonoBehaviour
{
    public Transform player;
    public MatterGunData Data;
    [SerializeField] private InkSound inkSound;
    #region MATTER PREFABS
    public GameObject matterPrefabsObject;
    //Créer un tableau de GameObject pour les matterPrefabs en récupérant les enfants de matterPrefabsObject
    private GameObject[] matterPrefabs; //Array of matter prefabs to be used
    private int matterIndex = 0; //Index of the current matter prefab to be used
    private GameObject matterPrefab; //Prefab of the current matter to be used
    #endregion

    #region INPUTS
    private InputSystem_Actions _inputActions;
    private Vector2 _aimInput;
    #endregion

    #region SHOOT VARIABLES
    private Vector3 lastAimDirection = Vector3.right;

    private bool isShooting = false;
    private float shootTimer = 0f;
    #endregion

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();

        _inputActions.Player.Look.performed += ctx => _aimInput = ctx.ReadValue<Vector2>();
        _inputActions.Player.Look.canceled += ctx => _aimInput = Vector2.zero;

        _inputActions.Player.Attack.started += ctx => isShooting = true;
        _inputActions.Player.Attack.canceled += ctx => isShooting = false;

        #region CHANGE MATTER PREFAB BY SCROLLING (MOUSE)
        _inputActions.Player.Scroll.performed += ctx =>
        {
            Vector2 scrollValue = ctx.ReadValue<Vector2>();
            //Utiliser le scroll de la souris pour changer de prefab de matière
            if (scrollValue.y > 0f)
                matterIndex = (matterIndex + 1) % matterPrefabs.Length;
            else if (scrollValue.y < 0f)
                matterIndex = (matterIndex - 1 + matterPrefabs.Length) % matterPrefabs.Length;

            matterPrefab = matterPrefabs[matterIndex];
        };
        #endregion

        #region CHANGE MATTER PREFAB BY SCROLLING (GAMEPAD)
        _inputActions.Player.Scroll1.performed += ctx =>
        {
            float scrollValue = ctx.ReadValue<float>();

            if (scrollValue > 0f)
                matterIndex = (matterIndex + 1) % matterPrefabs.Length;
            else if (scrollValue < 0f)
                matterIndex = (matterIndex - 1 + matterPrefabs.Length) % matterPrefabs.Length;
            matterPrefab = matterPrefabs[matterIndex];

            // PlayVibration();
        };
        #endregion
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Récupérer tous les enfants de matterPrefabsObject et les stocker dans le tableau matterPrefabs
        int childCount = matterPrefabsObject.transform.childCount;
        matterPrefabs = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            matterPrefabs[i] = matterPrefabsObject.transform.GetChild(i).gameObject;
            //Désactiver tous les prefabs de matière au départ
            matterPrefabs[i].SetActive(false);
        }

        //Initialiser le prefab de matière avec le premier prefab du tableau
        matterPrefab = matterPrefabs[matterIndex];
    }

    // Update is called once per frame
    void Update()
    {
        #region AIMING

        Vector3 direction = lastAimDirection;

        bool hasNewInput = false;

        // Souris
        if (Mouse.current != null && Mouse.current.delta.ReadValue().sqrMagnitude > 0f)
        {
            Vector3 aimWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            aimWorldPos.z = 0f;
            direction = (aimWorldPos - player.position).normalized;
            hasNewInput = true;
        }
        // Stick droit
        else if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.rightStick.ReadValue();
            if (stick.sqrMagnitude > 0.1f)
            {
                direction = stick.normalized;
                hasNewInput = true;
            }
        }

        if (hasNewInput)
        {
            lastAimDirection = direction;
        }


        // Positionner l'arme à une certaine distance du joueur
        Vector3 offset = direction * Data.holdDistance;
        transform.position = player.position + offset;

        // Appliquer la rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        #endregion

        #region SHOOTING
        // Tir automatique si la touche est maintenue
        if (isShooting)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= 1f / Data.fireRate)
            {
                shootTimer = 0f;
                Shoot();
            }
        }
        #endregion
    }


    public void Shoot()
    {
        // Créer la matière (peinture) à la position de l'arme
        GameObject matter = Instantiate(matterPrefab, transform.position, transform.rotation);
        //Activer le prefab de la matière
        matter.SetActive(true);

        // Ajouter une force pour "tirer" la matière
        Rigidbody2D rb = matter.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.right * Data.shootForce; // Applique une vitesse dans la direction du tir
        }
    }

    public void PlayVibration(float freq1 = 0.5f, float freq2 = 1.0f, float duration = 0.2f)
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(freq1, freq2); // (low freq, high freq)
            StartCoroutine(StopVibration(duration)); // vibration pendant 0.2 secondes
        }
    }
    public System.Collections.IEnumerator StopVibration(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
    }
}