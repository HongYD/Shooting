using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public enum PlayerState 
    {
        Empty = 0,
        HoldingBall,
        ShootingBall,
    }


    public class PlayerStateManager : MonoBehaviour
    {
        private StarterAssetsInputs _input;
        private CharacterController _controller;
        private FirstPersonController _firstPersonController;
        private GameObject _mainCam;
        private RaycastHit hit;
        private GameObject currentBall;
        public LayerMask ballLayer;
        public PlayerState playerState;
        public int frameCount = 0;

        public delegate void PlayerHoldingBallEvent();
        public static event PlayerHoldingBallEvent OnPlayerHoldBall;

        public delegate float PlayerShootingBallEvent();
        public static event PlayerShootingBallEvent OnPlayerShootBall;

        private void OnEnable()
        {
            GameManager.OnGameEndEvent += this.OnGameEndEvent;
        }

        private void OnDisable()
        {
            GameManager.OnGameEndEvent -= this.OnGameEndEvent;
        }

        private void Awake()
        {
            if(_mainCam == null)
            {
                _mainCam = GameObject.FindGameObjectWithTag("MainCamera");
            }
            CursorManager.instance.SetCursorType(CursorType.CursorNormal);
            CursorManager.instance.SetCursorLockState(CursorLockMode.Locked);
        }

        // Start is called before the first frame update
        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
            _firstPersonController = GetComponent<FirstPersonController>();
            playerState = PlayerState.Empty;
            SoundManager.Init();
        }

        // Update is called once per frame
        void Update()
        {

            //Debug.DrawLine(_mainCam.transform.position, _mainCam.transform.position + (_mainCam.transform.forward) * 2.5f, Color.red);
            HandleSoundEffect();
            switchState(playerState);
            frameCount++;
        }

        public void switchState(PlayerState playerState)
        {
            switch (playerState)
            {
                case PlayerState.Empty:
                    OnEmpty();
                    break;
                case PlayerState.HoldingBall:
                    OnHoldingBall();
                    break;
                case PlayerState.ShootingBall:
                    break;
            }
        }

        public void OnEmpty()
        {
            CursorManager.instance.SetCursorType(CursorType.CursorNormal);
            Ray ray = _mainCam.GetComponent<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit, 2.5f, ballLayer))
            {

                if (hit.collider.gameObject.tag == "football" || hit.collider.gameObject.tag == "basketball")
                {
                    CursorManager.instance.SetCursorType(CursorType.CursorGrab);
                    if (Mouse.current.leftButton.wasReleasedThisFrame && playerState == PlayerState.Empty && currentBall == null)
                    {
                        currentBall = hit.collider.gameObject;
                        currentBall.GetComponent<BallStateManager>().state = BallState.Hold;
                        playerState = PlayerState.HoldingBall;
                        OnPlayerHoldBall();
                    }
                }
            }
        }

        public void OnHoldingBall()
        {          
            if(Mouse.current.leftButton.wasReleasedThisFrame && currentBall.GetComponent<BallStateManager>().state == BallState.Hold
                && playerState == PlayerState.HoldingBall)
            {
                currentBall.GetComponent<BallStateManager>().state = BallState.Pushed;
                float power = OnPlayerShootBall();
                currentBall.GetComponent<BallStateManager>().forceRate = power;
                currentBall = null;
                playerState = PlayerState.Empty;
            }
        }

        public void OnShootingBall()
        {

        }

        public void HandleSoundEffect()
        {
            if (_input.move != Vector2.zero && _firstPersonController.Grounded)
            {
                SoundManager.PlaySoundWithTimer(SoundManager.SoundType.Step, 0.4f);
            }
            if (_input.isJumpValueChanged && _firstPersonController.Grounded && _input.jump)
            {
                SoundManager.PlaySound(SoundManager.SoundType.Jump, this.transform.position);
                _input.isJumpValueChanged = false;
            }
        }

        public void OnGameEndEvent(int score, int highestScore)
        {
            _input.cursorInputForLook= false;
            _input.cursorLocked = false;
            CursorManager.instance.SetCursorLockState(CursorLockMode.None);
        }
    }
}
