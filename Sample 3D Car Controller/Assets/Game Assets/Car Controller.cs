using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement Values")]

    public float acceleration;
    public float steering;

    [Header("Physics Values")]

    public float drag;
    public float angularDrag;
    public float moveForce;
    private float inputForward;
    private float inputSteering;
    private float maxSpeed = 30f;

    [Header("Grounded Checks")]

    private bool isGrounded;
    private float groundedCheck = 1f;
    private int getLayerMask;

    private void Start()
    {
        acceleration = 0;
        getLayerMask = LayerMask.GetMask("GroundLayer");
        if(getLayerMask < 0)
        {
            Debug.LogError("Ground layer not found!");
            return;
        }

        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
        rb.angularDrag = angularDrag;
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }

    private void FixedUpdate()
    {
        InputMovement();
    }

    void InputMovement()
    {
        inputForward = Input.GetAxis("Vertical");
        inputSteering = Input.GetAxis("Horizontal");

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, groundedCheck, getLayerMask))
        {
            isGrounded = true;
            MoveForward();
            HandleSpeed();
            MoveSteering();
            LimitMovement();
        }

        else
        {
            isGrounded = false;
        }
    }

    void MoveForward()
    {
        Vector3 forward = inputForward * acceleration * transform.forward;
        rb.AddForce(forward * moveForce, ForceMode.Acceleration);
    }

    void HandleSpeed()
    {
        if(inputForward > 0f)
        {
            acceleration += Time.deltaTime * 10f;

            if(acceleration > maxSpeed)
            {
                acceleration = maxSpeed;
            }
        }

        else
        {
            acceleration -= Time.deltaTime * 10f;
        }
    }

    void MoveSteering()
    {
        float rotateAmnt = inputSteering * steering;
        Quaternion rot = Quaternion.Euler(0f, rotateAmnt, 0f); // representing rotation values
        rb.MoveRotation(rb.rotation * rot);
    }

    void LimitMovement()
    {
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}