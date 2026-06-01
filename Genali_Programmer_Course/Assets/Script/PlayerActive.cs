using UnityEngine;

public class PlayerActive : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float moveSpeed;
    //[SerializeField] Rigidbody myRigidbody;
    [SerializeField] Transform myTransform;
    [SerializeField] CharacterController characterController;
    [SerializeField] bool isShooting;

    [Header("Weapon")]
    [SerializeField] int maxAmmo;
    [SerializeField] int currentAmmo;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawn;

    [Header("Camera")]
    [SerializeField] Transform cameraPivot;
    [SerializeField] Camera gameCamera;
    [SerializeField] float mouseSensitivity = 2f;
    [SerializeField] float minPitch = -30f;
    [SerializeField] float maxPitch = 60f;
    //[SerializeField] float cameraSensitivity;

    [Header("Gravity")]
    public float gravity = -9.81f;

    [Header("Reference")]
    [SerializeField] GameManager gameManager;
    [SerializeField] Animator playerAnimator;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        //cameraTransform = Camera.main.transform;
        //myRigidbody = GetComponent<Rigidbody>();
        myTransform = GetComponent<Transform>();
        characterController = GetComponent<CharacterController>();
        gameCamera = Camera.main;
        playerAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        currentAmmo = maxAmmo;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private float yVelocity;
    private float yaw;
    private float pitch;


    void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Rotate pivot (kamera orbit)
        cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    Vector3 moveDirection;
    void Movement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = gameCamera.transform.forward;
        Vector3 right = gameCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        moveDirection = forward * v + right * h;

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (moveDirection.magnitude > 0.1f && !isShooting)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            playerAnimator.SetBool("IsWalking", true);
        }
        else
        {
            playerAnimator.SetBool("IsWalking", false);
        }

        Invoke(nameof(ResetShootState), 0.1f);
    }

    void ResetShootState()
    {
        isShooting = false;
    }

    void ApplyGravity()
    {
        if (characterController.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 gravityMove = new Vector3(0, yVelocity, 0);
        characterController.Move(gravityMove * Time.deltaTime);
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentAmmo = maxAmmo;
        }
    }

    [SerializeField] float fireRate = 0.5f;

    float nextFireTime;

    void Shoot()
    {
        ShootingAnimation();
        gameManager._ammoHUD.text = currentAmmo + " / " + maxAmmo;

        if (Input.GetMouseButton(0) && currentAmmo > 0 && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Vector3 lookDirection = gameCamera.transform.forward;
            lookDirection.y = 0f;

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = targetRotation;
            }

            Recoil();
            bulletSpawn.rotation = cameraPivot.rotation;
            currentAmmo--;
            Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
        }
    }

    void ShootingAnimation()
    {
        if (Input.GetMouseButton(0))
        {
            playerAnimator.SetBool("IsShooting", true);
        }
        else
        {
            playerAnimator.SetBool("IsShooting", false);
        }
    }

    void Recoil()
    {
        pitch -= 2f;

        yaw += Random.Range(-1f, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            if (currentAmmo <= maxAmmo)
            {
                currentAmmo++;
            }
            Destroy(other.gameObject);
        }

    }

    void Update()
    {
        CameraRotation();
        Movement();
        Reload();
        Shoot();
        ApplyGravity();
    }
}
