using Game_Management;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dice
{
    public class DiceController : MonoBehaviour
    {
        private GameManager _gameManager;
        private Rigidbody _rigidbody;
        private BoxCollider _boxCollider;

        private Vector3 initPosition;
        private Quaternion initRotation;
        private bool _once;

        private bool IsDiceStill()
        {
            Vector3 velocity;
            return (velocity = _rigidbody.velocity).x < 0.05f && velocity.y < 0.05f &&
                   velocity.x > -0.05f && velocity.y > -0.05f;
        }
        
        // Checks if the dice is grounded.
        public bool IsDiceGrounded() => _rigidbody.velocity.y == 0;
        
        // Set variables of the dice
        private void SetDice()
        {
            _gameManager = GameManager.Instance;
            if (TryGetComponent(out Rigidbody rb)) _rigidbody = rb;
            if (TryGetComponent(out BoxCollider boxCollider)) _boxCollider = boxCollider;
            initPosition = transform.localPosition;
            initRotation = transform.localRotation;
        }
        
        private void OnEnable()
        {
            SetDice();
            Throw();
            _once = false;
        }

        // Detects the result of the dice by using cross and dot product
        private void DetectResult()
        {
            int result = 1;
            if (  Vector3.Cross(Vector3.up, transform.right).magnitude < 0.5f) //x axis a.b.sin theta < 45
            {
                if (Vector3.Dot(Vector3.up, transform.right) > 0) // 6th Face
                {
                    result = 6;
                }
                else // 1st Face
                {
                    result = 1;
                }
            }
            else if ( Vector3.Cross(Vector3.up, transform.up).magnitude < 0.5f) //y axis
            {
                if (Vector3.Dot(Vector3.up, transform.up) > 0) // 4th Face
                {
                    result = 4;
                }
                else // 3rd Face
                {
                    result = 3;
                }
            }
            else if ( Vector3.Cross(Vector3.up, transform.forward).magnitude < 0.5f) //z axis
            {
                if (Vector3.Dot(Vector3.up, transform.forward) > 0) // 5th Face
                {
                    result = 5;
                }
                else // 2nd Face
                {
                    result = 2;
                }
            }
            
            Debug.Log(result);
            Actions.DiceResult?.Invoke(result);
            
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

            float dirX = Random.Range(500, 1000); // random x
            float dirY = Random.Range(500, 1000); // random y
            float dirZ = Random.Range(500, 1000); // random z

            _rigidbody.AddRelativeTorque(dirX, dirY, dirZ);
            _rigidbody.AddForce(Vector3.up * Random.Range(-800, -500));
            _rigidbody.AddForce(Vector3.forward * Random.Range(500, 800));
        }
        
        // Resets dice variables before relaunch
        private void Reset()
        {
            // Prevents activating dice after the game ends.
            if (_gameManager.isPlayable)
                DetectResult();
            
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
            _boxCollider.enabled = true;
            transform.localPosition = initPosition;
            transform.localRotation = initRotation;
            SetRandomFaceOnPlatform();
            Pool.Instance.DeactivateObject(gameObject, PoolItemType.Dice, 0.1f);
        }

        // Update is called once per frame
        private void Update()
        {
            if (IsDiceStill() && IsDiceGrounded() && !_once)
            {
                _once = true;
                Reset();
            }
        }
    }
}
