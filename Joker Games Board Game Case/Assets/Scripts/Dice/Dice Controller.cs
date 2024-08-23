using System;
using Game_Management;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dice
{
    public class DiceController : MonoBehaviour
    {
        private GameManager _gameManager;
        private Rigidbody _rigidbody;
        private bool _once;

        private void OnEnable()
        {
            _once = true;
            SetDice(); // Initialize dice settings
            Throw();   // Throw the dice with random force and torque
            _once = false;
        }

        // Set variables of the dice and assign components
        private void SetDice()
        {
            _gameManager = GameManager.Instance;
            if (TryGetComponent(out Rigidbody rb)) _rigidbody = rb;
            SetRandomFaceOnPlatform(); // Set a random starting face on the platform
        }

        #region Dice Mechanics

        // Checks if the dice has stopped moving
        private bool IsDiceStill()
        {
            Vector3 velocity = _rigidbody.velocity;
            return Mathf.Abs(velocity.x) < 0.05f && Mathf.Abs(velocity.y) < 0.05f;
        }

        // Checks if the dice is grounded (not moving vertically)
        private bool IsDiceGrounded() => _rigidbody.velocity.y == 0;

        // Detects the result of the dice by checking its orientation
        private void DetectResult()
        {
            int result = 1;

            // Determine the result based on the dice's orientation using cross and dot products
            if (Vector3.Cross(Vector3.up, transform.right).magnitude < 0.5f)
            {
                result = Vector3.Dot(Vector3.up, transform.right) > 0 ? 6 : 1;
            }
            else if (Vector3.Cross(Vector3.up, transform.up).magnitude < 0.5f)
            {
                result = Vector3.Dot(Vector3.up, transform.up) > 0 ? 3 : 4;
            }
            else if (Vector3.Cross(Vector3.up, transform.forward).magnitude < 0.5f)
            {
                result = Vector3.Dot(Vector3.up, transform.forward) > 0 ? 5 : 2;
            }

            Actions.DiceResult?.Invoke(result);
        }

        // Sets the dice on the platform with a random face facing up
        private void SetRandomFaceOnPlatform()
        {
            int[] angles = { -90, 0, 90, 180 };
            transform.localEulerAngles = new Vector3(angles[Random.Range(0, 4)], 0, 0);
        }

        // Throws the dice with random force and rotation
        private void Throw()
        {
            _rigidbody.AddRelativeTorque(
                Random.Range(-1000, 1000),
                Random.Range(-1000, 1000),
                Random.Range(-1000, 1000)
            );

            _rigidbody.AddForce(Vector3.up * Random.Range(-120, -80), ForceMode.Impulse);
            _rigidbody.AddForce(Vector3.forward * Random.Range(50, 30), ForceMode.Impulse);
        }

        #endregion

        #region Game Flow

        // Resets the dice and checks for game state
        private void Reset()
        {
            if (_gameManager.isPlayable) DetectResult();
        }

        private void Update()
        {
            if (IsDiceStill() && IsDiceGrounded() && !_once)
            {
                _once = true;
                Reset(); // Detect result and reset for next throw
            }
        }

        #endregion

        #region Dice Collisions

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out DiceController diceController))
                Actions.DiceToDiceCollision?.Invoke();
            else
                Actions.DiceToFloorCollision?.Invoke();
        }

        #endregion
    }
}
