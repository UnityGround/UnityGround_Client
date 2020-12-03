using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameManager gameManager;
    Rigidbody2D rb;
    public float maxSpeed;
    public float jumpPower;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;

    // 오디오
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioYeah;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();

        gameManager = new GameManager();
    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "jump":
                audioSource.PlayOneShot(audioJump);
                break;
            case "attack":
                audioSource.PlayOneShot(audioAttack);
                break;
            case "item":
                audioSource.PlayOneShot(audioItem);
                break;
            case "die":
                audioSource.PlayOneShot(audioDie);
                break;
            case "finish":
                audioSource.PlayOneShot(audioYeah);
                break;
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        // 점프하기
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJump"))    // spaceBar
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
            PlaySound("jump");
        }

        // 캐릭터 멈추기
        if (Input.GetButtonUp("Horizontal"))
        {
            rb.velocity = new Vector2(rb.velocity.normalized.x * 0.5f, rb.velocity.y);
        }

        // 방향전환
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // 애니메이션
        if (Mathf.Abs(rb.velocity.normalized.x) < 0.2)
            anim.SetBool("isWalk", false);
        else
            anim.SetBool("isWalk", true);
    }

    

    void FixedUpdate()
    {
        // 캐릭터 움직이기
        float h = Input.GetAxisRaw("Horizontal");
        rb.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // 스피드 조정
        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        else if (rb.velocity.x < maxSpeed * (-1))
        {
            rb.velocity = new Vector2(maxSpeed * (-1), rb.velocity.y);
        }

        // 바닥에
        if (rb.velocity.y < 0)
        {
            Debug.DrawRay(rb.position, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                    anim.SetBool("isJump", false);
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 몬스터와 충돌했을때
        if(collision.gameObject.tag == "Enemy")
        {
            if (rb.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
                GameManager.Point += 100;
                GameManager.Kill++;
                Debug.Log("kill: " + GameManager.Kill);
                PlaySound("attack");
            }
            else
                OnDamaged(collision.transform.position);
        }

        // 깃발에 충돌햇을때
        if (collision.gameObject.tag == "Finish")
        {
            gameManager.NextStage();
            Debug.Log("Finsih");
            PlaySound("finish");
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 코인과 충돌했을때
        if (collision.gameObject.tag == "Item")
        {
            // 코인 없어지기
            collision.gameObject.SetActive(false);

            bool isBronze = collision.gameObject.name.Contains("BCoin");
            bool isSilver = collision.gameObject.name.Contains("SCoin");
            bool isGold = collision.gameObject.name.Contains("GCoin");

            if (isBronze)
            {
                GameManager.Point += 50;
            }
            else if (isSilver)
            {
                GameManager.Point += 100;
            }
            else if (isGold)
            {
                GameManager.Point += 150;
            }
            PlaySound("item");
        }
    }

    // 몬스터 죽이기
    void OnAttack(Transform enemy)
    {
        rb.AddForce(Vector2.up * 4, ForceMode2D.Impulse);

        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }

    // 데미지 받을때
    void OnDamaged(Vector2 targetPos)
    {
        Debug.Log("죽어랏!!!!");
        gameManager.HealthDown();
        gameObject.layer = 11;

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rb.AddForce(new Vector2(dirc, 1) * 6, ForceMode2D.Impulse);

        // 애니메이션
        anim.SetTrigger("DoDamaged");

        // 무적 지속 시간
        Invoke("OffDamaged", 2);
    }

    // 무적설정
    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    // 플레이어 죽을때
    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        capsuleCollider.enabled = false;

        rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        GameManager.RestartBtn.gameObject.SetActive(true);
        PlaySound("die");
        Invoke("TimeScaleZero", 2);
    }

    public void TimeScaleZero()
    {
        Time.timeScale = 0;
        Debug.Log("TimeScaleZero");
    }
}
