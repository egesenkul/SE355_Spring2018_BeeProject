﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    private GameObject bee;

    public bool isFirstClick = false;
    public bool isTransferring = false;
    public int transferInterval = 1; //1 second
    private float nextTransferTime = 0;

    public GameObject underMouseObject;
    public GameObject initialObject; //first selected
    public GameObject targetObject; //second selected 

    void Update()
    {
        Swipe();
        underMouseObject = OnTargetOver();

        //hedef node'u belirle
        if (isFirstClick == true && underMouseObject != initialObject)
        {
            targetObject = underMouseObject;
            //kosullar uygunsa ari gonder

            //Base'den node'a gondermeyi dene
            if (initialObject.GetComponent<BaseNode>() != null && initialObject.GetComponent<BaseNode>().concurrentBee > 0 /*&& isTransferring == false*/)
            {
                if (targetObject != null && targetObject.GetComponent<Node>() != null)
                {
                    if (targetObject.GetComponent<Node>().NodeType1 == Node.NodeType.Control)
                    {
                        //StartCoroutine(TransferBee(initialObject.GetComponent<BaseNode>().concurrentSoldierBee, targetObject.GetComponent<Node>().concurentBee));
                        if (Time.time > nextTransferTime)
                        {
                            nextTransferTime = Time.time + transferInterval;
                            initialObject.GetComponent<BaseNode>().concurrentSoldierBee--;
                            targetObject.GetComponent<Node>().concurentBee++;
                            Debug.Log("Transferred Soldier Bee");
                        }
                        //isTransferring = true;
                        //Debug.Log("Transferring Soldier Bee");
                    }
                    else if (targetObject.GetComponent<Node>().NodeType1 == Node.NodeType.Resource)
                    {
                        //StartCoroutine(TransferBee(initialObject.GetComponent<BaseNode>().concurrentWorkerBee, targetObject.GetComponent<Node>().concurentBee));
                        //isTransferring = true;
                        //Debug.Log("Transferring Worker Bee");
                    }   
                }
            }
            else
            {
                StopCoroutine("TransferBee");
                isTransferring = false;
            }
        }


        // Debug.Log("There is a " + underMouseObject + " under your cursor");
        //ClickSelect();
        //SetTarget();
    }

    IEnumerator TransferBee(int decrease, int increase)
    {
        decrease--;
        yield return new WaitForSecondsRealtime(1);
        increase++;
    }



    public void Swipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            //firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);

            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if (hit)
            {
                Debug.Log("Seçtim");
                initialObject = hit.transform.gameObject; //ilk tıkladın elinde bu var
                isFirstClick = true;

            }
        }

        if (Input.GetMouseButtonUp(0))
        {


            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if (hit)
            {
                Debug.Log("Bıraktım");
            }
            
            //reset values
            isFirstClick = false;
            initialObject = null;
            targetObject = null;

        }
    }

    //If there is an object under the cursor, return that game object
    GameObject OnTargetOver()
    {
        Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
        Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
        
        if (hit == true)
        {
            return hit.transform.gameObject;
        }
        else
        {
            return null;
        }
    }


    void ClickSelect()
    {

        if (Input.GetMouseButtonDown(0) && !isFirstClick)
        {
            //Converting Mouse Pos to 2D (vector2) World Pos
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if (hit)
            {
                Debug.Log(hit.transform.name);
                isFirstClick = true;
                bee = hit.transform.gameObject;
            }

        }
        else if (Input.GetMouseButtonDown(0) && isFirstClick)
        {

            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if (hit)
            {
                while ((bee.transform.position - hit.transform.position).magnitude < 0.1f)
                {
                    Vector2 direction = (hit.transform.position - bee.transform.position);
                    direction.Normalize();
                    bee.transform.Translate(direction * Time.deltaTime);
                }

            }
            initialObject = null;
        }
    }
}
