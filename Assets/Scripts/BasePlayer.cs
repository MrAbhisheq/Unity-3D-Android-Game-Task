using System;
using System.Collections;
using UnityEngine;

public abstract class BasePlayer : MonoBehaviour
{
    public static BasePlayer Instance;

    public float speed = 3;
    public float jumpForce = 5;
    public float fallPositionY = -10f;

    private Rigidbody rb;
    private Collider playerCollider;
    private BlockChecker blockChecker;
    protected Vector2 input;
    private bool canMove = true;

    private bool isJumping;
    protected Vector3 moveDir;

    private bool isGrounded;

    private Vector3 initialPos;
    private Vector3 initialRot;

    public Action OnPlayerFallen;
    public bool isFallen;

    private void Awake()
    {
        Instance = this;
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        blockChecker = GetComponentInChildren<BlockChecker>(true);

        initialPos = transform.position;
        initialRot = transform.eulerAngles;
    }

    protected virtual void FixedUpdate()
    {
        if (!canMove)
            return;

        CheckGround();

        Vector3 move = new Vector3(input.x, 0f, input.y);
        //moveDir = move.normalized;

        if (input.sqrMagnitude > 0.01f)
        {
            moveDir = new Vector3(input.x, 0f, input.y).normalized;

            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * move);
            rb.MoveRotation(Quaternion.LookRotation(moveDir, Vector3.up));
        }

        //rb.MovePosition(
        //    rb.position + speed * Time.fixedDeltaTime * moveDir
        //);


        if (!isJumping && isGrounded && moveDir != Vector3.zero)
        {
            if(!blockChecker.hasBlock)
                Jump();
        }

        if (isGrounded)
            isJumping = false;

        DetectPlayerFall();
    }

    void DetectPlayerFall()
    {
        if (!isGrounded)
        {
            if(transform.position.y < fallPositionY)
                OnPlayerFall();
        }
    }

    protected virtual void OnPlayerFall()
    {
        gameObject.SetActive(false);
        OnPlayerFallen?.Invoke();
        isFallen = true;
    }

    public void SetInput(Vector2 value)
    {
        input = value;
    }

    public void Jump()
    {
        if (!canMove || isJumping)
            return;

        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;
        rb.linearVelocity = velocity;

        isJumping = true;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void CheckGround()
    {
        Vector3 start = playerCollider.bounds.center;
        start.y = playerCollider.bounds.min.y + 0.05f;

        isGrounded = Physics.Raycast(
            start,
            Vector3.down,
            0.2f
        );
    }

    public void ToggleMovement(bool state)
    {
        canMove = state;
        input = Vector2.zero;
    }

    public IEnumerator ToggleMovementWhenGrounded(bool state)
    {
        yield return new WaitUntil(() => isGrounded);
        ToggleMovement(state);
    }

    public void ResetPlayer()
    {
        ToggleMovement(false);

        isFallen = false;
        input = Vector2.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;
        rb.linearVelocity = velocity;

        transform.position = initialPos;
        transform.eulerAngles = initialRot;
    }
}
