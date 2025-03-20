using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    
    public int maxHealth = 5;
    
    public float timeInvincible = 2.0f;

    public int Health => _currentHealth;

    private int _currentHealth;
    
    private bool _isInvincible;
    
    private float _invincibleTimer;
    
    private Rigidbody2D _rigidbody2d;
    
    private float _horizontal;
    
    private float _vertical;
    
    Animator animator;
    
    Vector2 lookDirection = new Vector2(1,0);
    
    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        var move = new Vector2(_horizontal, _vertical);
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        
        if (_isInvincible)
        {
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer < 0)
                _isInvincible = false;
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = _rigidbody2d.position;
        position.x += speed * _horizontal * Time.deltaTime;
        position.y += speed * _vertical * Time.deltaTime;

        _rigidbody2d.MovePosition(position);
    }
    
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (_isInvincible)
                return;
            
            _isInvincible = true;
            _invincibleTimer = timeInvincible;
        }
        
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
        Debug.Log(_currentHealth + "/" + maxHealth);
    }
}
