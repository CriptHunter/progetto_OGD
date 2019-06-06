﻿using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySimpleJump : EnemyBehaviour
{
    private LayerMask groundMask;
    private LayerMask playerMask;
    private LayerMask ignoredLayer = ~((1 << 2) |(1 << 9) | (1 << 10) | (1 << 13));
    private Rigidbody2D rigidbody;
    private SkeletonAnimation skeletonAnimation;
    [SerializeField] private Transform groundDetection;
    [SerializeField] private Transform forwardDetection;
    [SerializeField] private float jumpForce;
    [SerializeField] private float horizontalMovement;
    [SerializeField] private float jumpWaitingTime;
    [SerializeField] float distance;

    private float timer;
    private bool grounded;

    private void Start()
    {
        playerMask = LayerMask.NameToLayer("Player");
        groundMask = LayerMask.NameToLayer("Ground");
        rigidbody = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        ChangeDirection(movingRight);
    }

    void FixedUpdate()
    {
        if (!isServer)
            return;
        Rpc_ChangeAnimation(rigidbody.velocity.y);
        if (grounded)
        {
            RaycastHit2D forwardHit;
            RaycastHit2D downwardHit = Physics2D.Raycast(groundDetection.position + new Vector3(horizontalMovement, 0, 0), Vector2.down, jumpForce, 1<<8);
            if (movingRight)
                forwardHit = Physics2D.Raycast(forwardDetection.position, -transform.right, horizontalMovement, ignoredLayer);
            else
                forwardHit = Physics2D.Raycast(forwardDetection.position, transform.right, horizontalMovement, ignoredLayer);

            if (forwardHit.collider != null)
                this.movingRight = !movingRight;
            else if (downwardHit.collider == null)
                this.movingRight = !movingRight;

            timer += Time.deltaTime;
            if (timer > jumpWaitingTime)
            {
                Jump();
                timer = 0;
            }
        }
    }

    [ClientRpc]
    private void Rpc_ChangeAnimation(float yVelocity)
    {
        if (yVelocity > 0)
            skeletonAnimation.AnimationName = "jump_up";
        else if (yVelocity < 0)
            skeletonAnimation.AnimationName = "jump_down";
        else
            skeletonAnimation.AnimationName = "stand";
    }

    private void Jump()
    {
        rigidbody.AddForce(new Vector2(horizontalMovement, jumpForce), ForceMode2D.Impulse);
        grounded = false;
    }

    public override void ChangeDirection(bool movingRight)
    {
        if (movingRight)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            horizontalMovement = -1 * horizontalMovement;
            this.movingRight = movingRight;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            horizontalMovement = -1 * horizontalMovement;
            this.movingRight = movingRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject.layer == groundMask)
            grounded = true;
        else if (collision.transform.gameObject.layer == playerMask)
            this.movingRight = !movingRight;
    }
}
