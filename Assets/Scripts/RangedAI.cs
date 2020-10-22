﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAI : MonoBehaviour
{
    private GameObject player;

    public float speed;
    public float sightDistance;

    public GameObject myAttack;
    private bool attackReady;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        attackReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(LookForPlayer()){
            if ((player.transform.position - transform.position).magnitude < sightDistance){
                Move();
            }
            else {
                MoveTo();
            }
            Rotate();
            if (attackReady){
                Attack();
            }

        }
    }

    private void Move()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Vector2 playerDirection = (transform.position - player.transform.position);
        playerDirection = playerDirection.normalized;
        Vector2 movement = playerDirection * speed * Time.deltaTime;
        transform.position = transform.position + new Vector3(movement.x,movement.y, 0);
    }

    private void MoveTo()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Vector2 playerDirection = (transform.position - player.transform.position);
        playerDirection = playerDirection.normalized;
        Vector2 movement = playerDirection * speed * Time.deltaTime * -1;
        transform.position = transform.position + new Vector3(movement.x,movement.y, 0);
    }

    private void Rotate()
    {
        Vector3 direction = (player.transform.position - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Attack()
    {
        StartCoroutine(EnemyAttacked());
        GameObject atk = Instantiate(myAttack, transform.position, transform.rotation).gameObject;
        atk.transform.Translate(Vector3.right);
    }

    private IEnumerator EnemyAttacked()
    {
        attackReady = false;
        yield return new WaitForSeconds(myAttack.GetComponent<PlayerAttack>().attackSpeed);
        attackReady = true;
    }

    private bool LookForPlayer(){
        Vector3 direction = (player.transform.position - transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
        if (hit.collider.gameObject.tag == "Player"){
            return true;
        }
        else {
            return false;
        }
    }
}
