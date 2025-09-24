using UnityEngine;
using UnityEngine.InputSystem;

public class SoundEmitter : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public SoundEmitterData Data; // Reference to the SoundEmitterData scriptable object

    #region SOUND PREFABS
    public GameObject soundPrefab; // Reference to the sound prefab object
    #endregion

    #region INPUTS
    private InputSystem_Actions _inputActions;
    #endregion

    #region SHOOT VARIABLES
    private Vector2 _aimInput;
    private Vector3 lastAimDirection = Vector3.right;
    #endregion


    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Look.performed += ctx => _aimInput = ctx.ReadValue<Vector2>();
        _inputActions.Player.Look.canceled += ctx => _aimInput = Vector2.zero;

        _inputActions.Player.Look.performed += ctx => _aimInput = ctx.ReadValue<Vector2>();

        _inputActions.Player.Attack.performed += ctx => Shoot();
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
    }

    public void Shoot()
    {
        GameObject soundInstance = Instantiate(soundPrefab, transform.position, transform.rotation);
        soundInstance.SetActive(true); // Activate the sound instance

        // Get the Rigidbody2D component of the sound instance
        Rigidbody2D rb = soundInstance.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = transform.right * Data.soundSpeed; // Applique une vitesse dans la direction du tir
        }


    }
}
