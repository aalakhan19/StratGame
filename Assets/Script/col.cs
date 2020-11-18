using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class col : MonoBehaviour
{
    private Transform body;
    private Camera myCamera;

    int round;
    float movespeed;
    private Animator animator;
    void Start()
    {

        myCamera = Camera.main;
        body = GetComponent<Transform>();
        round = 2;
        movespeed = 15f;
        animator = GetComponentInParent<Animator>();
    }

    void FixedUpdate()
    {


    }

    void moveCamToBaseOnClick()
    {
        //if clicked
        myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, body.position, Time.deltaTime * movespeed);
        if (Math.Round(myCamera.transform.position.x, round) == Math.Round(body.position.x, round) && Math.Round(myCamera.transform.position.y, round) == Math.Round(body.position.y, round))
        {
            Debug.Log("done");
            myCamera.transform.position = body.position;
            //clicked = false;
        }

    }
    void OnMouseOver()
    {
        animator.SetTrigger("hoverTrigger");
        animator.ResetTrigger("deHoverTrigger");
    }




    void OnMouseExit()
    {
        animator.ResetTrigger("hoverTrigger");
        animator.SetTrigger("deHoverTrigger");
    }


}
