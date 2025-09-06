using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player
{
    [RequireComponent(typeof(PlayerMovment))]
    [RequireComponent(typeof(PlayerJump))]
    [RequireComponent(typeof(PlayerAnimator))]
    public class Player : MonoBehaviour
    {
        private PlayerMovment _movementPlayer;
        private PlayerJump _jumpPlayer;
        private PlayerAnimator _animator;

        private void Awake()
        {
            _movementPlayer = GetComponent<PlayerMovment>();
            _jumpPlayer = GetComponent<PlayerJump>();
            _animator = GetComponent<PlayerAnimator>();
        }

        private void FixedUpdate()
        {
            _movementPlayer.SetDirection(Input.GetAxisRaw("Horizontal"));

            if (Input.GetButtonDown("Jump"))
                _jumpPlayer.TryJump();
        }
    }
}