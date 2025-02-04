﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMovement : MonoBehaviour
{
    GameObject player;
    public bool movingRight;
    public LayerMask playerLayer;
    public GameObject playerObj;
    public bool playerOnLog;
    public GameObject MovePoint;
    GameObject controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Controller");
        playerOnLog = false;
        MovePoint = GameObject.FindGameObjectWithTag("MovePoint");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerOnLog = true;
            playerObj = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerOnLog = false;
            playerObj = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -36 || transform.position.x > 45)
        {
            //Outside of screen, destroy!
            Destroy(gameObject);
        }

        float extraLogSpeed = Mathf.Min(15f, Mathf.FloorToInt((transform.position.z / 100)) * 1);
        float logSpeed = 3.5f + extraLogSpeed;
        if (controller.GetComponent<GameController>().currentRain == 1)
        {
            //Log moves faster when it is raining
            logSpeed += 2f;
        }
        else if (controller.GetComponent<GameController>().currentRain == 2)
        {
            //Log moves faster when it is raining
            logSpeed += 4f;
        }

        float yValue = 1f;
        if (movingRight)
        {
            yValue = 1f;
        }
        else
        {
            yValue = -1f;
        }

        if (playerOnLog)
        {
            Vector3 movePos = MovePoint.transform.position;
            MovePoint.transform.position = Vector3.MoveTowards(movePos, new Vector3(movePos.x + yValue, movePos.y, movePos.z), Time.deltaTime * logSpeed);
        }

        Vector3 newPos = transform.position;
        newPos.x += yValue;
        transform.position = Vector3.MoveTowards(gameObject.transform.position, newPos, Time.deltaTime * logSpeed);

        // if (playerOnLog)
        // {
        //     Vector3 playerPos = playerObj.transform.position;
        //     playerPos.x += 2f;
        //     playerObj.transform.position = playerPos;
        // }
    }
}
