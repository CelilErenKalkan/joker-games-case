using Game_Management;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dice
{
    public class DiceController : MonoBehaviour
    {
        private GameManager _gameManager;
        private Rigidbody _rigidbody;
        private MeshCollider _meshCollider;

        private Vector3 initPosition;
        private Quaternion initRotation;
        private bool _isHolding;

        private bool IsDiceStill()
        {
            Vector3 velocity;
            return (velocity = _rigidbody.velocity).x < 0.05f && velocity.y < 0.05f &&
                   velocity.x > -0.05f && velocity.y > -0.05f
                   || transform.position.y <= -5;
        }
        
        // Checks if the dice is grounded.
        public bool IsDiceGrounded() => _rigidbody.velocity.y == 0;
        
        // Set variables of the dice
        private void SetDice()
        {
            _gameManager = GameManager.Instance;
            if (TryGetComponent(out Rigidbody rb)) _rigidbody = rb;
            if (TryGetComponent(out MeshCollider meshCollider)) _meshCollider = meshCollider;
            initPosition = transform.localPosition;
            initRotation = transform.localRotation;
        }
        
        // Start is called before the first frame update
        private void Start()
        {
            SetDice();
            Throw();
        }

        // Detects the result of the dice by using cross and dot product
        private void DetectResult()
        {
            if (transform.position.y < 0) return;
            Vector3 position = transform.position;
            position.y = 0;
            if (  Vector3.Cross(Vector3.up, transform.right).magnitude < 0.5f) //x axis a.b.sin theta < 45
            {
                if (Vector3.Dot(Vector3.up, transform.right) > 0) // 1st Face
                {
                    
                }
                else // 3rd Face
                {
                    
                }
            }
            else if ( Vector3.Cross(Vector3.up, transform.up).magnitude < 0.5f) //y axis
            {
                if (Vector3.Dot(Vector3.up, transform.up) > 0) // 0th Face
                {
                    
                }
                else // 2nd Face
                {
                    
                }
            }
            else if ( Vector3.Cross(Vector3.up, transform.forward).magnitude < 0.5f) //z axis
            {
                if (Vector3.Dot(Vector3.up, transform.forward) > 0) // 4th Face
                {
                    
                }
                else // 5th Face
                {
                    
                }
            }
        }
        
        // Sets the dice sitting on the platform with a random rotation considering faces.
        private void SetRandomFaceOnPlatform()
        {
            int[] angles = new [] { -90, 0, 90, 180 };
            
            int randomX = Random.Range(0, 4);
            int x = angles[randomX];

            transform.localEulerAngles = new Vector3(x, 0, 0);
        }
        
        // Throws the dice with a random rotation and calculated parabolic movement
        private void Throw()
        {
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;

            float dirX = Random.Range(100, 1000); // random x
            float dirY = Random.Range(100, 1000); // random y
            float dirZ = Random.Range(50, 100); // random z

            _rigidbody.AddRelativeTorque(dirX, dirY, dirZ);
            _rigidbody.AddForce(transform.up * Random.Range(-600, -300));
            _rigidbody.AddForce(transform.forward * Random.Range(300, 600));
        }
        
        // Resets dice variables before relaunch
        private void Reset()
        {
            // Prevents activating dice after the game ends.
            if (_gameManager.isPlayable)
                DetectResult();
            
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
            _meshCollider.enabled = true;
            transform.localPosition = initPosition;
            transform.localRotation = initRotation;
            SetRandomFaceOnPlatform();
        }

        // Update is called once per frame
        private void Update()
        {
            
        }
    }
}
