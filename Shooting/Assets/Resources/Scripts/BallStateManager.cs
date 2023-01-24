using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallState
{
    Idle,
    Hold,
    Pushed,
    Flying,
    Dead,
}

public class BallStateManager : MonoBehaviour
{
    public BallState state;
    private GameObject _mainCam;
    private new Rigidbody rigidbody;
    private bool isReadyToDie;
    private bool isScored;
    public float forceRate;
    public float MaxForce = 6.0f;
    public float rotateSpeed = 40.0f;

    public delegate void ReturnScoreEvent(int score);
    public static event ReturnScoreEvent OnReturnScore;

    private void Start()
    {
        if (_mainCam == null)
        {
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        }
        rigidbody = this.GetComponent<Rigidbody>();
        state = BallState.Idle;
        isReadyToDie = false;
        isScored = false;
    }

    public void Update()
    {
        
    }

    private void LateUpdate()
    {
        SwitchState(state);
    }

    public void DetectScore()
    {

    }

    public void SwitchState(BallState state)
    {
        switch(state)
        {
            case BallState.Idle:
                OnBallIdle();
                break;
            case BallState.Hold:
                OnBallHolding();
                break;
            case BallState.Pushed:
                OnBallPushed();
                break;
            case BallState.Flying:
                OnBallFlying();
                break;
            case BallState.Dead:
                OnBallDead();
                break;
        }
    }

    private void OnBallDead()
    {
        Destroy(this.gameObject);
    }

    private void OnBallPushed()
    {
        rigidbody.useGravity = true;
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.AddForce(_mainCam.transform.forward * MaxForce * forceRate + _mainCam.transform.up * (MaxForce + 1.0f)* forceRate, ForceMode.Impulse);
        state = BallState.Flying;
    }

    private void OnBallFlying()
    {
        this.transform.Rotate(Vector3.right * rotateSpeed);
        rotateSpeed -= MaxForce * Time.deltaTime;
        if(rotateSpeed < 0)
        {
            rotateSpeed = 0;
        }
        RaycastHit hit;
        Debug.DrawLine(this.transform.position, this.transform.position + (-Vector3.up) * 0.2f, Color.red);
        if (Physics.Raycast(this.gameObject.transform.position, -Vector3.up ,out hit, 0.2f))
        {
            if(hit.collider.gameObject.name == "NetCollider" && !isScored)
            {
                SoundManager.PlaySound(SoundManager.SoundType.BounceWire, this.transform.position);
                if (OnReturnScore != null)
                {
                    if (this.gameObject.tag == "basketball")
                    {
                        OnReturnScore(1);
                    }
                    else if (this.gameObject.tag == "football")
                    {
                        OnReturnScore(3);
                    }
                }
                isScored = true;
            }
        }

        if (!isReadyToDie)
        {
            StartCoroutine(DeadCountDown());
            isReadyToDie = true;
        }
    }

    private void OnBallHolding()
    {
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezePosition;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        Vector3 pos = _mainCam.transform.position + (_mainCam.transform.forward) * 1.5f;
        this.transform.position = pos;
    }

    private void OnBallIdle()
    {
        rigidbody.useGravity = true;
    }

    public IEnumerator DeadCountDown()
    {
        yield return new WaitForSeconds(15.0f);
        state= BallState.Dead;
    }

    private void OnCollisionEnter(Collision collision)
    {
        string layer = LayerMask.LayerToName(collision.gameObject.layer);
        switch (layer)
        {
            case "BasketMesh":
                SoundManager.PlaySound(SoundManager.SoundType.BounceBasket, collision.gameObject.transform.position);
                break;
            case "Backboard":
                SoundManager.PlaySound(SoundManager.SoundType.BounceBackboard, collision.gameObject.transform.position);
                break;
            case "Floor":
                SoundManager.PlaySound(SoundManager.SoundType.BounceFloor, collision.gameObject.transform.position);
                break;
        }
    }
}
