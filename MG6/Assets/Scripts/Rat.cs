using System.Xml;
using UnityEngine;

public class Rat : MonoBehaviour
{
    [SerializeField] private float _shrinkAmount;
    [SerializeField] private float _spawnAmount;
    [SerializeField] private GameObject _ratPrefab;
    [SerializeField] private SpriteRenderer _explosionPrefab;

    [SerializeField] private float _KnockbackMultiplier; // I recommend values less than or equal to 100

    private float _timeSinceAttack = 0.0f;
    private bool _attacked = false;

    private Color[] _colors = new Color[3]
    {
        Color.yellow,
        Color.red,
        Color.blue
    };
    private int _colorIndex;

    private void Start()
    {
        Debug.Assert(_KnockbackMultiplier <= 50, "Having a high knockback multiplier may result in the rat clipping through the ground.");
    }

    private void Update()
    {
        if (_attacked)
        {
            _timeSinceAttack += Time.deltaTime;
        }

        if (_timeSinceAttack > 1.0f)
        {
            _attacked = false;
            _timeSinceAttack = 0.0f;
        } 

        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.transform.localScale = new Vector3(10.0f, 10.0f, gameObject.transform.localScale.z);
            gameObject.transform.position = new Vector3(1.92f, 8.66f, 0.0f);
            
            SpriteRenderer motherSprite = gameObject.GetComponent<SpriteRenderer>();
            motherSprite.enabled = true;
        }
    }

    // ------------------------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // STEP 2 -------------------------------------------------------------
        // If the Rat collides with a GameObject with the "Attack" tag,
        //      call SpawnSmallRat() _spawnAmount number of times to spawn
        //      more rats. (The spawning logic is written for you.)
        if (collision.gameObject.CompareTag("Attack") && !_attacked)
        {
            _attacked = true;

            for (int i = 0; i < _spawnAmount; i++)
            {
                SpawnSmallRat();
            }

            

            gameObject.transform.localScale = new Vector3(
                Mathf.Clamp(gameObject.transform.localScale.x - _shrinkAmount, 0, 10), 
                Mathf.Clamp(gameObject.transform.localScale.y - _shrinkAmount, 0, 10), 
                gameObject.transform.localScale.z
            );

            SpawnExplosion(_colors[_colorIndex]);
            _colorIndex = (_colorIndex + 1) % _colors.Length; // Resets the index after reaching the maximum length of the list

            ApplyKnockbackImpulse(collision.gameObject.transform.position);

            if (gameObject.transform.localScale.x == 0 || gameObject.transform.localScale.y == 0)
            {
                SpriteRenderer motherSprite = gameObject.GetComponent<SpriteRenderer>();
                motherSprite.enabled = false;
            }
        }


        // Also, shrink the original Rat by _shrinkAmount amount.
        //
        // STEP 3
        // Additionally, call SpawnExplosion(), and pass in a different color
        //      every time.
        // SPECIFICALLY, go through the colors in the array named _colors
        //      IN ORDER.
        // Each time the Rat is kicked, the explosion should be a different
        //      color: first yellow, then red, then blue, then looping back
        //      around to yellow, etc.


        // STEP 2 -------------------------------------------------------------
    }

    // ------------------------------------------------------------------------

    private void ApplyKnockbackImpulse(Vector3 playerPosition)
    {
        Rigidbody2D _motherRB = GetComponent<Rigidbody2D>();

        Vector2 _extractedPosition = new Vector2(playerPosition.x, playerPosition.y);
        Vector2 _direction = (_motherRB.position - _extractedPosition).normalized;

        _direction = -_direction;

        _motherRB.linearVelocity = _direction * _KnockbackMultiplier;
    }
    private void SpawnExplosion(Color color)
    {
        SpriteRenderer explosion = Instantiate(_explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.color = color;
        explosion.gameObject.SetActive(true);
    }

    // ------------------------------------------------------------------------
    private void SpawnSmallRat()
    {
        GameObject rat = Instantiate(_ratPrefab);

        Vector3 pos = Random.onUnitSphere;
        rat.transform.position += pos;

        // STEP 3 -------------------------------------------------------------
        // Uncomment and fix the below lines to give the small rat a random
        //      rotation correctly.
        // Your game should look exaclty like the finished example.

        float randomAngle = Random.Range(0, 360);
        Vector3 rotationAngles = new Vector3(0, 0, randomAngle);
        rat.transform.Rotate(rotationAngles);

        // STEP 3 -------------------------------------------------------------
    }
}