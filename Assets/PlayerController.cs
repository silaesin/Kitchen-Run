using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float hareketHizi = 12f; 
    public float jumpForce = 25f; 

    [Header("Eğilme Ayarları")]
    public Vector2 egilmeSize = new Vector2(0.9f, 1.2f); 
    public Vector2 egilmeOffset = new Vector2(0f, -0.6f); 

    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D boxCollider;
    
    private bool isGrounded;
    private Vector3 baslangicBoyutu; 
    private Vector2 normalSize;
    private Vector2 normalOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        
        baslangicBoyutu = transform.localScale;
        rb.freezeRotation = true;

        if (boxCollider != null)
        {
            normalSize = boxCollider.size;
            normalOffset = boxCollider.offset;
        }
    }

    void Update()
    {
        float yatayHareket = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(yatayHareket * hareketHizi, rb.velocity.y);

        if (yatayHareket > 0) transform.localScale = baslangicBoyutu;
        else if (yatayHareket < 0) transform.localScale = new Vector3(-baslangicBoyutu.x, baslangicBoyutu.y, baslangicBoyutu.z);

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (animator != null) animator.SetBool("Egiliyor", true);
            if (boxCollider != null)
            {
                boxCollider.size = egilmeSize;
                boxCollider.offset = egilmeOffset;
            }
        }
        else 
        {
            if (animator != null) animator.SetBool("Egiliyor", false);
            if (boxCollider != null)
            {
                boxCollider.size = normalSize;
                boxCollider.offset = normalOffset;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col) 
    { 
        if (col.gameObject.CompareTag("Floor") || col.gameObject.CompareTag("Engel")) isGrounded = true; 
    }

    private void OnTriggerEnter2D(Collider2D diger)
    {
        GameManager kontrolcu = GameObject.FindFirstObjectByType<GameManager>();
        if (kontrolcu == null) return;

        if (diger.CompareTag("Yumurta") || diger.CompareTag("Seker") || diger.CompareTag("Sut") || 
            diger.CompareTag("Yag") || diger.CompareTag("Un") || diger.CompareTag("KabartmaTozu"))
        {
            if (diger.enabled) 
            {
                diger.enabled = false; 
                kontrolcu.MalzemeToplandi(diger.tag);
                Destroy(diger.gameObject);
            }
        }
        
        // --- İŞTE BURASI DEĞİŞTİ ---
        // Artık "MikseriKontrolEt" değil, "FirinaGidildi" çağırıyoruz.
        if (diger.CompareTag("Firin") || diger.CompareTag("Mikser")) 
        {
            kontrolcu.FirinaGidildi();
        }
    }
}