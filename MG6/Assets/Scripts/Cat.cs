using Unity.Mathematics;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField] private GameObject _attackLeft;
    [SerializeField] private GameObject _attackRight;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _speed;
    [SerializeField] private float _jump;
    [SerializeField] private float _attackCooldown;

    private bool _facingLeft;
    private bool _isGrounded = true;
    private float _attackTime;

    // ------------------------------------------------------------------------
    private void Update()
    {
        // Detect input to move player left/right
        _rigidbody.linearVelocity = new Vector2(
            Input.GetAxis("Horizontal") * _speed,
            _rigidbody.linearVelocity.y
        );

        // Detect input to jump
        if (Input.GetKey(KeyCode.Space) && _isGrounded)
        {
            _isGrounded = false;

            _rigidbody.linearVelocity = new Vector2(
                _rigidbody.linearVelocity.x,
                _jump
            );
        }

        _attackTime -= Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift) && _attackTime <= 0.0f)
        {
            Attack();
        }

        // Change character facing 
        if (Input.GetAxis("Horizontal") < 0 && !_facingLeft)
        {
            _spriteRenderer.flipX = true;
            _facingLeft = true;
        }
        else if (Input.GetAxis("Horizontal") > 0 && _facingLeft)
        {
            _spriteRenderer.flipX = false;
            _facingLeft = false;
        }

        // Set animation state parameters
        _animator.SetBool("walking", _rigidbody.linearVelocity.x != 0.0f);

        // STEP 1 -------------------------------------------------------------
        // Add code here to allow the player to pass through the underside of
        //      platforms.
        // Check if the player is moving upwards, and if so, diable their
        //      collider.
        // You might want to check the Rigidbody2D scripting documentation
        //      for a useful variable :)

        if (_rigidbody.linearVelocityY > 0)
        {
            _collider.enabled = false;
        } else
        {
            _collider.enabled = true;
        }

        if (_rigidbody.position.y >= 14 || _rigidbody.position.y <= -3.2 || _rigidbody.position.x < -6.5 || _rigidbody.position.x > 10.5)
        {
            _rigidbody.transform.position = new Vector3(2f, 2.63f, 0);
        }

        
        // STEP 1 -------------------------------------------------------------
    }

    // ------------------------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals("Ground") || tag.Equals("Platform"))
        {
            _isGrounded = true;
        }
    }

    // ------------------------------------------------------------------------
    private void Attack ()
    {
        _attackTime = _attackCooldown;
        _animator.SetTrigger("attack");

        if (_facingLeft)
        {
            _attackLeft.SetActive(true);
        }
        else
        {
            _attackRight.SetActive(true);
        }
    }

}