using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    public float axisChangeChance = 0.25f;
    public int hitsToFix = 1;
    public AudioClip hitClip;
    public AudioClip fixedClip;

    Rigidbody2D rigidbody2d;
    Animator animator;
    float timer;
    int direction = 1;
    bool broken = true;
    int hitsLeft;
    SpriteRenderer spriteRenderer;
    Color baseColor;
    float flashTimer;
    AudioSource audioSource;

    public bool isBroken { get { return broken; } }

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;
        hitsLeft = hitsToFix;
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0)
            {
                spriteRenderer.color = baseColor;
            }
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;

            if (Random.value < axisChangeChance)
            {
                vertical = !vertical;
            }
        }
    }

    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }

        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2d.MovePosition(position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        hitsLeft--;
        if (hitsLeft > 0)
        {
            // Влучання зараховано, але ворог ще зламаний — коротко блимає
            spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0.35f);
            flashTimer = 0.15f;
            PlaySound(hitClip);
            return;
        }

        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        UIHandler.instance.AddFixedEnemy();
        PlaySound(fixedClip);
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
