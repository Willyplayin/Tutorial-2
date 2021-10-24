using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;

    public Text win;

    public Text lives;

    public Text lose;

    private int scoreValue = 0;

    private int livesValue = 3;

    private int gameOver = 0;

    private int alreadyTeleported = 0;

    public AudioSource musicSource;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    private bool facingRight = true;

    Animator anim;

    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        win.enabled = false;
        lose.enabled = false;
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) && isOnGround && !Input.GetKey(KeyCode.W))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKey(KeyCode.D) && isOnGround && !Input.GetKey(KeyCode.W))
        {
            anim.SetInteger("State", 1);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if (scoreValue == 8 && livesValue != 0 && gameOver == 0)
            {
                win.enabled = true;
                musicSource.loop = false;
                musicSource.clip = musicClipTwo;
                musicSource.Play();
                gameOver = 1;
            }

            else if (scoreValue == 4 && alreadyTeleported == 0)
            {
                livesValue = 3;
                lives.text = "Lives: " + livesValue.ToString();
                alreadyTeleported = 1;
                transform.position = new Vector2(50.0f, 0.0f);
            }
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);

            if (livesValue == 0)
            {
                lose.enabled = true;
                win.enabled = false;
                Destroy(gameObject);
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (!Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.D))
            {
                anim.SetInteger("State", 0);
            }
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
                anim.SetInteger("State", 2);
            }
        }
    }

    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }
}