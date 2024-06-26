using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{ 
    public bool TestMode;

    //Player Gameplay Variables
    //private int _lives;
    //public int lives
    //{
    //    get => _lives;
    //    set 
    //    {
    //        if (value <= 0) GameOver();
    //        if (value < _lives) Respawn();
    //        if (value > maxLives) value = maxLives;
    //        _lives = value;

    //        Debug.Log($"Lives have been set to {_lives}");
    //        //broadcast can happen here
    //    }
    //}

    [SerializeField] private int maxLives = 5;

    //Movement Variables
    [SerializeField] private int speed;
    [SerializeField] private int jumpForce = 3;

    //Groundcheck
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask isGroundLayer;
    [SerializeField] private float groundCheckRadius;

    //Audio Clips
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip stompClip;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private AudioSource audioSource;

    private Coroutine jumpForceChange = null;
    private Coroutine speedChange = null;

    public void PowerupValueChange(Pickup.PickupType type)
    {
        if (type == Pickup.PickupType.PowerupSpeed)
            FillSpecificCoroutineVar(ref speedChange,ref speed, type);

        if (type == Pickup.PickupType.PowerupJump)
            FillSpecificCoroutineVar(ref jumpForceChange,ref jumpForce, type);
    }

    void FillSpecificCoroutineVar(ref Coroutine inVar, ref int varToChange, Pickup.PickupType type)
    {
        if (inVar != null)
        {
            StopCoroutine(inVar);
            inVar = null;
            varToChange /= 2;
            inVar = StartCoroutine(ValueChangeCoroutine(type));
            return;
        }

        inVar = StartCoroutine(ValueChangeCoroutine(type));
    }
    IEnumerator ValueChangeCoroutine(Pickup.PickupType type)
    {
        if (type == Pickup.PickupType.PowerupSpeed)
            speed *= 2;
        if (type == Pickup.PickupType.PowerupJump)
            jumpForce *= 2;
        
        yield return new WaitForSeconds(2.0f);

        if (type == Pickup.PickupType.PowerupSpeed)
        {
            speed /= 2;
            speedChange = null;
        }
        if (type == Pickup.PickupType.PowerupJump)
        {
            jumpForce /= 2;
            jumpForceChange = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (speed <= 0)
        {
            speed = 5;
            if (TestMode) Debug.Log("Speed Set To Default Value");
        }

        if (jumpForce <= 0) 
        {
            jumpForce = 3;
            if (TestMode) Debug.Log("Jump Force Set To Default Value");
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.02f;
            if (TestMode) Debug.Log("Ground Check Radius Set To Default Value");
        }

        if (groundCheck == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("GroundCheck");
            if (obj != null)
            {
                groundCheck = obj.transform;
                return;
            }
            GameObject newObj = new GameObject();
            newObj.transform.SetParent(transform);
            newObj.transform.localPosition = Vector3.zero;
            newObj.name = "GroundCheck";
            newObj.tag = newObj.name;
            groundCheck = newObj.transform;
            if (TestMode) Debug.Log("Ground Check Transform Created via Code - Did you forget to assign it in the inspector?");
        } 

        
    }
    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);
        float xInput = Input.GetAxis("Horizontal");
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        //animation checks for physics
        if (curPlayingClips.Length > 0)
        {
            if (curPlayingClips[0].clip.name == "Attack")
                rb.velocity = Vector2.zero;
            else
            {
                Vector2 moveDirection = new Vector2(xInput * speed, rb.velocity.y);
                rb.velocity = moveDirection;

                //Input check for attack happens here in order to prevent fire from retriggering in the attack animation
                if (Input.GetButtonDown("Fire1"))
                    anim.SetTrigger("Attack");
            }
        }

        //Input checks
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            audioSource.PlayOneShot(jumpClip);
        }

        if (Input.GetButtonDown("Jump") && !isGrounded)
            anim.SetTrigger("JumpAttack");

        

        //Sprite Flipping
        if (xInput != 0) sr.flipX = (xInput < 0);

        //setting specific animation variables
        anim.SetFloat("speed", Mathf.Abs(xInput));
        anim.SetBool("isGrounded", isGrounded);
    }

    public void IncreaseGravity()
    {
        rb.gravityScale = 10;
    }

    private void GameOver()
    {
        Debug.Log("GameOver goes here");
    }

    private void Respawn()
    {
        Debug.Log("Respawn goes here");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Squish"))
        {
            Destroy(collision.gameObject);
            audioSource.PlayOneShot(stompClip);
            collision.transform.parent.GetComponent<Enemy>().TakeDamage(9999);
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
