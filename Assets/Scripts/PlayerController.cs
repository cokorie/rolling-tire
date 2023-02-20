using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;

    private int desiredLane = 1; // 1 for middle lane
    public float laneDistance = 2.5f;

    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public float jumpForce;
    public float Gravity = -20;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;

        direction.z = forwardSpeed;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);

        if(controller.isGrounded)
        {
            direction.y = -2;
            // if(Input.GetKeyDown(KeyCode.UpArrow))
            if(SwipeManager.swipeUp)
            {
                Jump();
            }
        } else
        {
            direction.y += Gravity * Time.deltaTime;
        }

        // if(Input.GetKeyDown(KeyCode.RightArrow))
        if(SwipeManager.swipeRight)
        {
            desiredLane++;
            if(desiredLane == 3)
                desiredLane = 2;
        }

        // if(Input.GetKeyDown(KeyCode.LeftArrow))
        if(SwipeManager.swipeLeft)
        {
            desiredLane--;
            if(desiredLane == -1)
                desiredLane = 0;
        }


        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
    
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        } else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

    if (transform.position == targetPosition)
        return;
    Vector3 diff = targetPosition - transform.position;
    Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
    if (moveDir.sqrMagnitude < diff.magnitude)
        controller.Move(moveDir);
    else
        controller.Move(diff);    
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
            return;

        controller.Move(direction * Time.fixedDeltaTime);
    } 

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }
}
