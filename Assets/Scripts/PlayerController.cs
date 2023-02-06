using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int loopCount;
    [SerializeField] float jumpSpeed;
    [SerializeField] float multiplier;
    private Vector3 playerVel;
    private GameManager gm;

    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    private string swiped;
    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
        gm = FindObjectOfType<GameManager>();
    }
    void Start()
    {

    }


    void Update()
    {
        if (!gm.isOnGround)
        {
            playerVel = GetComponent<Rigidbody>().velocity;
            playerVel.y -= 25 * Time.deltaTime;
            GetComponent<Rigidbody>().velocity = playerVel;
        }
        float posX = transform.position.x;
        switch (Swipe())
        {
            case "up":
                if (gm.isOnGround)
                {
                    gameObject.GetComponent<Animator>().SetTrigger("Jump");
                    rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                    gm.isOnGround = false;
                }
                break;
            case "down":
                if (!gm.isOnGround)
                {
                    rb.AddForce(Vector3.down * jumpSpeed, ForceMode.Impulse);
                }
                break;
            case "left":
                if (-4f < posX && posX <= 10f)
                {
                    Vector3 posL = 4.5f * Vector3.left;
                    transform.position += posL;
                }
                break;
            case "right":
                if (-10f <= posX && posX < 4f)
                {
                    Vector3 posR = 4.5f * Vector3.right;
                    transform.position += posR;

                }
                break;
            default:
                break;
        }

        if (gm.playing)
        {
            gameObject.GetComponent<Animator>().SetBool("Run", true);
        }
        else
        {
            gameObject.GetComponent<Animator>().SetInteger("Idle", Random.Range(1, 4));
        }
        if (gm.gameOver)
        {
            loopCount++;
            Crash(loopCount);
            gameObject.GetComponent<Animator>().SetTrigger("Dead");
        }
    }

    private void Crash(int count)
    {
        if (count == 1)
        {
            Vector3 oldPos = transform.position;
            transform.position = new Vector3(oldPos.x, oldPos.y, (oldPos.z - 10));
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            gm.isOnGround = true;
        }
        if (collision.transform.CompareTag("Car"))
        {
            gm.gameOver = true;
            gm.playing = false;
        }
        if (collision.transform.CompareTag("Food"))
        {
            if (gm)
            {
                gm.score += 50;
                gm.SpeedUp();
                gm.RefreshScore();
                gm.foodList.Remove(collision.gameObject);
            }
            Destroy(collision.gameObject);
        }
    }

    private string Swipe()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                swiped = "wait";
                //save began touch 2d point
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }
            if (t.phase == TouchPhase.Ended)
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(t.position.x, t.position.y);

                //create vector from the two points
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //normalize the 2d vector
                currentSwipe.Normalize();

                //swipe upwards
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    swiped = "up";
                }
                //swipe down
                if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    swiped = "down";
                }
                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    swiped = "left";
                }
                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    swiped = "right";
                }
            }
            return swiped;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                swiped = "up";
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                swiped = "down";
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                swiped = "left";
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                swiped = "right";
            }
            else
            {
                swiped = "no-touch";
            }
            return swiped;
        }
    }
}
