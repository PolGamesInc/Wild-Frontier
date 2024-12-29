using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float SpeedMovePlayer = 5f;
    [SerializeField] private float JumpStrong = 300f;
    [SerializeField] private float SpeedSprintPlayer = 10f;
    [SerializeField] private float SpeedSitPlayer = 2.5f;
    [SerializeField] private float MaxStamina;
    [SerializeField] private float StaminaDepletion = 10f;
    [SerializeField] private float StaminaRecovery = 5f;
    [SerializeField] private Transform Camera;
    [SerializeField] private Transform PlayerModel;
    private Rigidbody RigidbodyPlayer;
    private bool CheckGround;
    private CapsuleCollider PlayerCollider;
    private float OriginalHeightPlayerCollider;
    private Vector3 OriginalCenterPlayerCollider;
    private Vector3 OriginalScalePlayer;
    private bool isSit = false;
    private bool isSprinting = false;

    private void Start()
    {
        RigidbodyPlayer = GetComponent<Rigidbody>();
        PlayerCollider = GetComponent<CapsuleCollider>();

        OriginalHeightPlayerCollider = PlayerCollider.height;
        OriginalCenterPlayerCollider = PlayerCollider.center;
        OriginalScalePlayer = PlayerModel.localScale;

        MaxStamina = 100f;
    }

    private void Update()
    {
        Move();
        CheckGroundForJump();
        RegenerateStamina();

        if(Input.GetKeyDown(KeyCode.C))
        {
            isSit = !isSit;
        }

        if(isSit == true)
        {
            Crouch(true);
        }
        else
        {
            Crouch(false);
        }

        if(CheckGround == true && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Move()
    {
        float MoveHorizontal = Input.GetAxis("Horizontal");
        float MoveVertical = Input.GetAxis("Vertical");

        Vector3 Forward = Camera.forward;
        Vector3 Right = Camera.right;

        Forward.y = 0f;
        Right.y = 0f;

        Forward.Normalize();
        Right.Normalize();

        Vector3 Movement = (Forward * MoveVertical + Right * MoveHorizontal).normalized;
        if(Input.GetKey(KeyCode.LeftShift) && StaminaDepletion > 0f)
        {
            isSprinting = true;
            MaxStamina -= StaminaDepletion * Time.deltaTime;
        }
        else
        {
            isSprinting = false;
        }
        float CurrentSpeed;
        if(MaxStamina <= 0f)
        {
            isSprinting = false;
            CurrentSpeed = SpeedMovePlayer;
        }
        else
        {
            if(Input.GetKey(KeyCode.LeftShift) && MaxStamina > 0f)
            {
                isSprinting = true;
                MaxStamina -= StaminaDepletion * Time.deltaTime;
            }
            else
            {
                isSprinting = false;
            }
            CurrentSpeed = isSprinting ? SpeedSprintPlayer : SpeedMovePlayer;
        }

        if(isSit == true)
        {
            CurrentSpeed = SpeedSitPlayer;
        }
        else
        {
            CurrentSpeed = isSprinting ? SpeedSprintPlayer : SpeedMovePlayer;
        }
        RigidbodyPlayer.MovePosition(transform.position + Movement * CurrentSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        RigidbodyPlayer.AddForce(Vector3.up * JumpStrong);
    }

    private void CheckGroundForJump()
    {
        CheckGround = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void Crouch(bool isCrouching)
    {
        if(isCrouching == true)
        {
            PlayerCollider.height = OriginalHeightPlayerCollider / 2f;
            PlayerCollider.center = new Vector3(0, -0.5f, 0);

            PlayerModel.localScale = new Vector3(OriginalScalePlayer.x, OriginalScalePlayer.y / 2f, OriginalScalePlayer.z);
        }
        else
        {
            PlayerCollider.height = OriginalHeightPlayerCollider;
            PlayerCollider.center = OriginalCenterPlayerCollider;

            PlayerModel.localScale = OriginalScalePlayer;
        }
    }

    private void RegenerateStamina()
    {
        if(isSprinting == false && MaxStamina < 100f)
        {
            MaxStamina += StaminaRecovery * Time.deltaTime;
            MaxStamina = Mathf.Clamp(MaxStamina, 0f, 100f);
        }

        if(MaxStamina > 0f)
        {
            isSprinting = false;
        }
    }
}
