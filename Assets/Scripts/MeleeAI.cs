﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAI : MonoBehaviour
{
    private GameObject player;

    public float speed;
    public float sightDistance;
    public float stun;
    public bool zMode;

    private Vector3 playerPosition;
    public Vector3 checkpoint;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        playerPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (stun <= 0)
        {
            if (LookForPlayer())
            {
                MoveTo();
                playerPosition = player.transform.position;
                Rotate();

            }
            else
            {
                if (playerPosition != Vector3.zero)
                    MoveToLast();
                else if(zMode)
                {
                    MoveToCheckpoint();
                    RotateToCheckpoint();
                }
            }
        }
        else
        {
            stun -= Time.deltaTime;
        }
    }

    private void MoveTo()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Vector2 playerDirection = (transform.position - player.transform.position);
        playerDirection = playerDirection.normalized;
        Vector2 movement = playerDirection * speed * Time.deltaTime * -1;
        transform.position = transform.position + new Vector3(movement.x, movement.y, 0);
    }

    private void MoveToLast()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Vector2 playerDirection = (transform.position - playerPosition);
        playerDirection = playerDirection.normalized;
        Vector2 movement = playerDirection * speed * Time.deltaTime * -1;
        transform.position = transform.position + new Vector3(movement.x, movement.y, 0);
    }

    private void MoveToCheckpoint()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Vector2 playerDirection = (transform.position - checkpoint);
        playerDirection = playerDirection.normalized;
        Vector2 movement = playerDirection * speed * Time.deltaTime * -1;
        transform.position = transform.position + new Vector3(movement.x, movement.y, 0);
    }

    private void Rotate()
    {
        Vector3 direction = (player.transform.position - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void RotateToCheckpoint()
    {
        Vector3 direction = (checkpoint - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private bool LookForPlayer()
    {
        if (player != null)
        {
            int layer_mask = LayerMask.GetMask("Default");
            Vector3 direction = (player.transform.position - transform.position);

            RaycastHit2D hit;
            if (!zMode)
                hit = Physics2D.Raycast(transform.position, direction);
            else
                hit = Physics2D.Raycast(transform.position, direction, 100, layer_mask);

            if (hit.collider.gameObject.tag == "Player")
            {
                playerPosition = player.transform.position;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }

    private void OnDestroy()
    {
        if (zMode && FindObjectOfType<ZController>() != null)
            FindObjectOfType<ZController>().zombiesKilled++;
    }
}
