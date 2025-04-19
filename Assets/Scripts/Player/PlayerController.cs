using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    private Vector3 move;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1; // 1 for middle lane
    public float laneDistance = 2.5f;

    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public float jumpForce;
    public float gravity = -12f;
    public Vector3 velocity;

    public Animator animator;
    private bool isSliding = false;

    bool toggle = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Time.timeScale = 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
            return;


        // Increases speed
        // if (forwardSpeed < maxSpeed)
        //     forwardSpeed += 0.1f * Time.deltaTime;

        animator.SetBool("isGameStarted", true);
        move.z = forwardSpeed;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.17f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);
        if (isGrounded && velocity.y < 0)
            velocity.y = -1f;

        if (isGrounded)
        {
            // if(Input.GetKeyDown(KeyCode.UpArrow))
            if (SwipeManager.swipeUp)
                Jump();

            if (SwipeManager.swipeDown && !isSliding)
                StartCoroutine(Slide());
        } else
        {
            velocity.y += gravity * TileManager.deltaTime;
            if (SwipeManager.swipeDown && !isSliding)
            {
                StartCoroutine(Slide());
                velocity.y = -10;
            }
        }
        controller.Move(velocity * Time.deltaTime);



    //     if(controller.isGrounded)
    //     {
    //         direction.y = -2;
    //         // if(Input.GetKeyDown(KeyCode.UpArrow))
    //         if(SwipeManager.swipeUp && Input.GetKeyDown(KeyCode.UpArrow))
    //         {
    //             Jump();
    //         }
    //     } else
    //     {
    //         direction.y += gravity * Time.deltaTime;
    //     }

    //     if (SwipeManager.swipeDown && !isSliding)
    //     {
    //         StartCoroutine(Slide());
    //     }

    //     // if(Input.GetKeyDown(KeyCode.RightArrow))
    //     if(SwipeManager.swipeRight && Input.GetKeyDown(KeyCode.UpArrow))
    //     {
    //         desiredLane++;
    //         if(desiredLane == 3)
    //             desiredLane = 2;
    //     }

    //     // if(Input.GetKeyDown(KeyCode.LeftArrow))
    //     if(SwipeManager.swipeLeft && Input.GetKeyDown(KeyCode.UpArrow))
    //     {
    //         desiredLane--;
    //         if(desiredLane == -1)
    //             desiredLane = 0;
    //     }


    //     Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
    
    //     if (desiredLane == 0)
    //     {
    //         targetPosition += Vector3.left * laneDistance;
    //     } else if (desiredLane == 2)
    //     {
    //         targetPosition += Vector3.right * laneDistance;
    //     }

    // if (transform.position == targetPosition)
    //     return;
    // Vector3 diff = targetPosition - transform.position;
    // Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
    // if (moveDir.sqrMagnitude < diff.magnitude)
    //     controller.Move(moveDir);
    // else
    //     controller.Move(diff);
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
            return;

        if (toggle)
        {
            toggle = false;
            if (forwardSpeed < maxSpeed)
                forwardSpeed += 0.2f * TileManager.fixedDeltaTime;
        }
        else
        {
            toggle = true;
            if (Time.timeScale < 2f)
                TileManager.timeScale += 0.005f * Time.fixedDeltaTime;
        }
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
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0, -1f, 0);
        controller.height = 1;

        yield return new WaitForSeconds(1.3f);
        
        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        animator.SetBool("isSliding", false);
        isSliding = false;
    }
}
