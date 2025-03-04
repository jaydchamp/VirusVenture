 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidbody2D;

    //[SerializeField] PolygonCollider2D standCollider;

    //JORGE MODS BELOW
    [SerializeField] BoxCollider2D standCollider;
    [SerializeField] BoxCollider2D slideCollider;
    //JORGE MODS ABOVE


    [SerializeField] Dialogue preChallengeDialogue;
    [SerializeField] Dialogue winningChallengeDialogue;

    public Transform groundChecker1;
    public Transform groundChecker2;
    public LayerMask pointerToGroundLayer;
    public Transform playerSprite;
    private SpriteRenderer spriteRend;

    float horizontal;
    //float vertical;
    private bool onGround;
    private bool isFacingRight;
    private bool isInvincible;

    public float IFrameDurationSeconds = 3.0f;
    private float IFrameDeltaTime = 0.3f;

    public Animator movementAnimator;

    //momentum data
    public float baseSpeed = 2.0f;
    public float maxSpeed = 10.0f;
    public float speedDownChange = 0.5f;
    public float speedUpChange = 0.5f;
    public float currentSpeed;

    //slide data
    public float slideForce = 20;
    private bool isSlide = false;

    //jumping data
    private bool isChargingJump;
    public float currentJumpPressure;
    [SerializeField] private float minJumpPress = 2.0f;
    [SerializeField] private float maxJumpPress = 8.0f;
    public Color chargingColor;
    public Color normalColor;

    //stomp data
    public GameObject stompPrefab;
    private Stomp currentStomp;

    public bool canMove = true;
    private bool inDetectionRange = false;
    private bool insideChallenge = false;
    private bool inNextLevelBox = false;

    private bool chargingJumpSoundStatus;
    public AudioSource chargingJumpSound; //Sound for CHARGING the jump -> VVJumpChargeSound
    public AudioSource chargedJumpSound; // sound for performing a charged jump -> Assign VirusVentureChargedJumpSound.wav
    public AudioSource normalJumpSound; // Assign VirusVenturenormalJumpSound.wav
    public AudioSource ScanSound; // Assign VirusVentureAttackSound.wav
    public AudioSource slideGroundSound; // Assign VirusVenture(G)SlideSound.wav
    public AudioSource slideAirSound; // Assign VirusVenture(A)SlideSound.wav

    void Start()
    {
        //JORGE MOD BELOW
        slideCollider.enabled = false;
        //JORGE MOD ABOVE
        chargingJumpSoundStatus = false;
        onGround = true;
        isChargingJump = false;
        isInvincible = false;
        isSlide = false;
        currentJumpPressure = 0.0f;

        spriteRend = GetComponent<SpriteRenderer>();
        isFacingRight = false;
        currentSpeed = baseSpeed;
        Collider2D groundCheckCol = transform.GetChild(0).GetComponent<Collider2D>();

    }
     
    // Update is called once per frame
    void Update()
    {
        OnGroundCheck();
        //Debug.Log("Current Challenge Status: " + insideChallenge);
        //Debug.Log(TestManager.Instance.testStatus);

        //only used in tutorial:
        if (inNextLevelBox && TestManager.Instance.GetTestPassValue()) 
        {
            inNextLevelBox = false;
            TestManager.Instance.SetTestPassValue(false);
            CustomSceneManager.Instance.MoveToNextScene();
        }

        //all player movement + powerups:
        if (canMove)
        {
            horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left 1 is right

            float tempVelocity = Mathf.Abs(horizontal);
            movementAnimator.SetFloat("Velocity", tempVelocity);

            if (onGround)
            {
                if (TestManager.Instance.GetPowerupStatus((int)PlayerMoves.Stomp))// && Input.GetKeyDown(KeyCode.Space))
                {
                    //player starts stomp
                    if (Input.GetKeyDown(KeyCode.Space) && currentStomp == null && !isSlide && !isChargingJump)
                    {
                        //+ turn off player movement
                        horizontal = 0;
                        this.rigidbody2D.velocity = Vector3.zero;
                        canMove = false;

                        //create new stomp
                        GameObject newStomp = Instantiate(stompPrefab, transform.position, Quaternion.identity);
                        currentStomp = newStomp.GetComponent<Stomp>();

                        movementAnimator.SetBool("IsScanning", true);

                        //Scan Attack/Stomp Sound 
                        ScanSound.Play();

                        //subscribe to StompFinished Event from current stomp instance and wait for it
                        currentStomp.StompFinished += () =>
                        {
                            //turn movement back on and unsub from current instance events
                            canMove = true;
                            movementAnimator.SetBool("IsScanning", false);

                            currentStomp.StompFinished -= () => { };
                            currentStomp = null;
                        };
                    }
                }
                if (TestManager.Instance.GetPowerupStatus((int)PlayerMoves.Jump))// && Input.GetKeyDown(KeyCode.UpArrow))
                {
                    //player charging jump
                    if (Input.GetKey(KeyCode.UpArrow) && onGround && currentStomp == null && !isSlide) // && onGround //
                    {
                        //Debug.Log("JUMP CALLED");
                        //if the charging jump sound is not already playing
                        if (!chargingJumpSoundStatus)
                        {
                            chargingJumpSound.Play();
                            chargingJumpSoundStatus = true;

                            //make player slightly shint
                        }

                        if (currentJumpPressure < maxJumpPress)
                        {
                            currentJumpPressure += Time.deltaTime * 10.0f;
                            movementAnimator.SetBool("IsJumping", true);
                            //spriteRend.color = chargingColor;
                        }
                        else
                        {
                            currentJumpPressure = maxJumpPress;
                        }
                        isChargingJump = true;
                        rigidbody2D.velocity = Vector2.zero;
                        //Debug.Log(currentJumpPressure);
                    }
                    else
                    {
                        if (currentJumpPressure > 0.0f && onGround)
                        {
                            currentJumpPressure = currentJumpPressure + minJumpPress;
                            rigidbody2D.velocity = new Vector2(0f, currentJumpPressure);

                            movementAnimator.SetBool("IsJumping", false);

                            chargingJumpSound.Stop();
                            chargingJumpSoundStatus = false;

                            //Play the appropriate jump sound based on the jump force
                            if (currentJumpPressure >= maxJumpPress)
                            {
                                chargedJumpSound.Play(); // Play the VirusVentureChargedJumpSound.wav
                            }
                            else
                            {
                                normalJumpSound.Play(); // Play the VirusVenturenormalJumpSound.wav
                            }

                            currentJumpPressure = 0f;
                            isChargingJump = false;

                            //spriteRend.color = normalColor;
                            //Debug.Log("Jump goes off");
                        }
                        else if (!onGround)
                        {
                            currentJumpPressure = 0f;
                            isChargingJump = false;
                            //Debug.Log("Attempted to perfom jump mid air");
                        }

                        movementAnimator.SetBool("IsJumping", false);
                    }
                }
                //basic small jump
                else if (Input.GetKey(KeyCode.UpArrow) && onGround && currentStomp == null && !isSlide)
                {
                    normalJumpSound.Play();
                    rigidbody2D.velocity = new Vector2(0f, minJumpPress);
                }
            }

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, speedUpChange * Time.deltaTime);
            }
            else 
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, baseSpeed, speedDownChange * Time.deltaTime);
            }

            rigidbody2D.velocity = new Vector2(horizontal * currentSpeed, rigidbody2D.velocity.y);

            flip();
        }
        if(insideChallenge && TestManager.Instance.GetTestPassValue())
        {
            //Debug.Log("GAME WONNNNNNNNNNNNNN");
            TurnOffTest();
            canMove = false;
            MusicManager.Instance.SwitchToPlatformingMusic();
            StartCoroutine(DialogueManager.Instance.ShowDialogue(winningChallengeDialogue));

            //after winning dialogue is finished, turn movement and platforming music back on
            DialogueManager.Instance.OnCloseText += () =>
            {
                //Debug.Log("TEXT CLOSED MUSIC SWITCHED");
                if(!insideChallenge)
                {
                    //Debug.Log("Player inverts music playing");
                    //MusicManager.Instance.SwitchToPlatformingMusic();
                    //MusicManager.Instance.SwitchMusic();
                    canMove = true;
                }
            };
        }
        //player has forcibly quit the challenge
        if (insideChallenge && Input.GetKeyDown(KeyCode.Space)) 
        {
            //Debug.Log("Player sets music to platforming");
            MusicManager.Instance.SwitchToPlatformingMusic();
            //MusicManager.Instance.SwitchMusic();
            TurnOffTest();
        }
        //player has started test
        if (inDetectionRange && Input.GetKeyDown(KeyCode.Return) &&!insideChallenge && !TestManager.Instance.GetTestPassValue())
        {
            //Debug.Log("Player sets mnusic to challenge musci");
            MusicManager.Instance.SwitchToChallengeMusic();
            //MusicManager.Instance.SwitchMusic();
            TurnOnTest();
            StartCoroutine(DialogueManager.Instance.ShowDialogueAndLoadTest(preChallengeDialogue));
        }
    }

    void FixedUpdate()
    {
        if (canMove && !isChargingJump)
        {
            rigidbody2D.velocity = new Vector2(horizontal * currentSpeed, rigidbody2D.velocity.y);//vertical * runSpeed);
        }

        if (TestManager.Instance.GetPowerupStatus((int)PlayerMoves.Slide))
        {
            if (canMove && Input.GetKeyDown(KeyCode.DownArrow) && currentStomp == null && !isChargingJump)
            {
                //Debug.Log("Slide Input Detected");
                canMove = false;
                movementAnimator.SetBool("IsSliding", true);
                isSlide = true;

                slideCollider.enabled = true;
                standCollider.enabled = false;

                //groundcheck and slidesounds
                if (onGround) {
                    slideGroundSound.Play();
                    //Debug.Log("Slide Ground");
                }
                else {
                    slideAirSound.Play();
                    //Debug.Log("Slide Air");
                }

                Vector2 changeForce = new Vector2(slideForce * horizontal, 0);
                if (changeForce.x == 0)
                {
                    //Debug.Log("Not Moving");
                    if (isFacingRight)
                    {
                        changeForce.x = slideForce;
                        //Debug.Log("Push Right");
                    }
                    else
                    {
                        changeForce.x = slideForce * -1;
                        //Debug.Log("Push Left");
                    }
                }
                rigidbody2D.AddForce(changeForce, ForceMode2D.Impulse);
                StartCoroutine(StopSlide());
            }
        }
    }

    IEnumerator StopSlide()
    {
        yield return new WaitForSeconds(1.0f);
        //Debug.Log("Stopping Slide");
        movementAnimator.SetBool("IsSliding", false);
        isSlide = false;

        //JORGE MOD BELOW
        standCollider.enabled = true;
        slideCollider.enabled = false;
        //JORGE MOD ABOVE
        canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            //INSERT I FRAMES
            StartCoroutine(BecomeInvisTemp());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("detectionBox"))
        {
            inDetectionRange = true;
        }

        if (other.CompareTag("FinishLevel"))
        {
            inNextLevelBox = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("detectionBox"))
        {
            inDetectionRange = false;
        }

        if (other.CompareTag("FinishLevel"))
        {
            inNextLevelBox = false;
        }
    }

    public void TurnOffTest()
    {
        //Debug.Log("Test QUIT");
        insideChallenge = false;
        canMove = true;
        TestManager.Instance.TurnOffChallenge();
    }

    public void TurnOnTest()
    {
        //Debug.Log("Hello?");
        canMove = false;
        insideChallenge = true;
        rigidbody2D.velocity = Vector2.zero;
    }

    private void flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    public void OnGroundCheck()
    {
        Collider2D col = Physics2D.OverlapCircle(groundChecker1.position, 0.01f, pointerToGroundLayer);
        Collider2D col2 = Physics2D.OverlapCircle(groundChecker2.position, 0.01f, pointerToGroundLayer);

        if (col == null && col2 == null)
        {
            onGround = false;
            currentJumpPressure = 0f;
            isChargingJump = false;
            //spriteRend.color = normalColor;
            //Debug.Log("NO LONGER GROUNDed");
        }
        else
        {
            onGround = true;
            //Debug.Log("ON GROUND");
        }
    }

    //https://www.aleksandrhovhannisyan.com/blog/invulnerability-frames-in-unity/
    private IEnumerator BecomeInvisTemp() //
    {
        //Debug.Log("INVIS");
        isInvincible = true;

        for (float i = 0; i < IFrameDurationSeconds; i += IFrameDeltaTime)
        {
            //ANY LOGIC INSERT HERE
            yield return new WaitForSeconds(IFrameDeltaTime);
        }

        //Debug.Log("NO LONGER INVIS");
        isInvincible = false;
    }

    //on collision take a bit of I frames 
}