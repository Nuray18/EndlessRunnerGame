using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    public float speed = 10f;
    public float jumpForce = 10f;
    private float laneDistance = 3f;
    private float laneChangeSpeed = 10f;
    private float gravity = 30f;

    private int currentLane = 1;
    private Vector3 velocity;
    private Vector3 targetPosition;

    private CharacterController controller;
    private bool isSliding = false;
    private float horizontalInputCooldown = 0.25f;
    private float lastInputTime = 0f;

    private float slideSpeed = 15f; // Kayma sırasında hız
    private float slideDuration = 1f; // Kayma süresi
    private float slideTime = 0f; // Kayma zamanlayıcı

    private float normalHeight = 1.5f; // Normal boy
    private float slidingHeight = 1f; // Kayma sırasında boy
    private Vector3 normalScale = new Vector3(1f, 1f, 1f); // Normal scale
    private Vector3 slidingScale = new Vector3(1f, 0.5f, 1f); // Kayma sırasında scale

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleInput();
        MovePlayer();
    }

    private void HandleInput()
    {
        // Moving left and right
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal < 0 && currentLane > 0 && Time.time - lastInputTime > horizontalInputCooldown)
        {
            currentLane--;
            lastInputTime = Time.time;
        }
        else if (horizontal > 0 && currentLane < 2 && Time.time - lastInputTime > horizontalInputCooldown)
        {
            currentLane++;
            lastInputTime = Time.time;
        }

        // Jumping
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
        }

        // Sliding
        if (Input.GetKeyDown(KeyCode.S) && !isSliding)
        {
            StartCoroutine(Slide());
        }

        float targetX = (currentLane - 1) * laneDistance;
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    private void MovePlayer()
    {
        Vector3 move = Vector3.zero;

        // Kayma sırasında hız artırılır (Kayma hareketi ekleniyor)
        if (isSliding)
        {
            move.z = slideSpeed; // Kayma sırasında daha hızlı hareket
            slideTime -= Time.deltaTime;
        }
        else
        {
            move.z = speed; // Normal hareket
        }

        // Horizontal movement (smooth lane switching)
        float diff = targetPosition.x - transform.position.x;
        move.x = diff * laneChangeSpeed;

        // Apply gravity
        if (!IsGrounded())
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -1f; // to keep grounded
        }

        move.y = velocity.y;

        // Move character
        controller.Move(move * Time.deltaTime);

        // Kayma süresi bittiğinde eski yüksekliğe dön
        if (slideTime <= 0 && isSliding)
        {
            EndSlide();
        }
    }

    public bool IsGrounded()
    {
        return controller.isGrounded;
    }

    private System.Collections.IEnumerator Slide()
    {
        isSliding = true;
        slideTime = slideDuration;

        // Kayma sırasında karakterin boyutlarını küçültme
        transform.localScale = slidingScale; // Kayma sırasında scale küçülür
        transform.position = new Vector3(transform.position.x, slidingHeight, transform.position.z); // Y'yi kaydır

        yield return new WaitForSeconds(slideDuration); // Kayma süresi

        EndSlide();
    }

    private void EndSlide()
    {
        // Kayma bittiğinde normal boyutlara dön
        transform.localScale = normalScale; // Scale geri alınır
        transform.position = new Vector3(transform.position.x, normalHeight, transform.position.z); // Y tekrar eski haline gelir

        isSliding = false;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
