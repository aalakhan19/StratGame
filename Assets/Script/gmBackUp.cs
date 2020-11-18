using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameMasterBackUp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject basePrefab;
    GameObject[] bases;
    private RaycastHit2D currentHit;
    private TextMeshProUGUI topBarText;
    public GameObject LineRendererPrefab;
    GameObject usingBase;
    GameObject attackingBase;
    private bool lines;
    public GameObject attackPopUpObject;
    public GameObject ReinforcePopUpObject;
    private string userInputAttack;
    private int unitAttackCount;


    void Start()
    {
        topBarText = GameObject.Find("UI").GetComponentInChildren<TextMeshProUGUI>();

        GenerateBaha1v1Map();

        bases = GameObject.FindGameObjectsWithTag("base");

        int x = 1;
        foreach (GameObject item in bases)
        {
            baseScipt itemScript = item.GetComponent<baseScipt>();
            itemScript.baseName.text = "beta" + x;
            itemScript.maxCap = 200;
            itemScript.prodRatePerSecond = 2;
            itemScript.units = 20;
            itemScript.StartCoroutine("Produce");
            x++;

            if (itemScript.ally)
            {
                item.GetComponent<SpriteRenderer>().color = Color.green;
            }
            else if (itemScript.enemy)
            {
                item.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else    //neutral
            {
                item.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }



    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            currentHit = GetMouseClick();
            if (currentHit)
            {
                if (currentHit.collider.tag == "baseCollider")
                {
                    if (currentHit.collider.transform.parent.gameObject.GetComponent<baseScipt>().ally)
                    {
                        usingBase = currentHit.collider.transform.parent.gameObject;
                        foreach (GameObject item in bases)
                        {
                            DrawLine(usingBase.transform.position, item.transform.position);
                        }
                        showAttackButton(usingBase);
                        showReinforceButton(usingBase);
                        lines = true;
                        topBarText.text = "Selected " + usingBase.GetComponent<baseScipt>().baseName.text;
                    }
                }
            }
        }

        if (Input.GetKeyDown("escape"))
        {
            DestroyLines();
            attackPopUpObject.SetActive(false);
            ReinforcePopUpObject.SetActive(false);
            topBarText.text = "Selected nothing ";
        }

    }

    public void StartAttack()
    {
        attackingBase = currentHit.collider.transform.parent.parent.gameObject;
        DestroyLines();
        DrawLine(usingBase.transform.position, attackingBase.transform.position, Color.red);
        lines = true;
        topBarText.text = usingBase.GetComponent<baseScipt>().baseName.text + " -> " + attackingBase.GetComponent<baseScipt>().baseName.text;
        attackPopUpObject.SetActive(true);

    }

    public void StartReinforce()
    {
        attackingBase = currentHit.collider.transform.parent.parent.gameObject;
        DestroyLines();
        DrawLine(usingBase.transform.position, attackingBase.transform.position, Color.green);
        lines = true;
        topBarText.text = usingBase.GetComponent<baseScipt>().baseName.text + " -> " + attackingBase.GetComponent<baseScipt>().baseName.text;
        ReinforcePopUpObject.SetActive(true);

    }

    public void SetAttackUserInput(string s)
    {
        userInputAttack = s;
        unitAttackCount = int.Parse(userInputAttack);
        ConfirmAttack();

    }

    public void SetReinforceUserInput(string s)
    {
        userInputAttack = s;
        unitAttackCount = int.Parse(userInputAttack);
        ConfirmReinforce();

    }

    public void ConfirmAttack()
    {
        if (usingBase.GetComponent<baseScipt>().units < int.Parse(userInputAttack))
        {
            topBarText.text = "not enough units";
            attackPopUpObject.SetActive(false);
            DestroyLines();
            return;
        }
        else
        {
            attackingBase.GetComponent<baseScipt>().units -= unitAttackCount;
            usingBase.GetComponent<baseScipt>().units -= unitAttackCount;
            if (attackingBase.GetComponent<baseScipt>().units <= 0)
            {
                Destroy(attackingBase);
            }
        }
        attackPopUpObject.SetActive(false);
        DestroyLines();
        topBarText.text = "Awaiting Command...";
        unitAttackCount = 0;
    }

    public void ConfirmReinforce()
    {
        if (usingBase.GetComponent<baseScipt>().units < int.Parse(userInputAttack))
        {
            topBarText.text = "not enough units";
            ReinforcePopUpObject.SetActive(false);
            DestroyLines();
            return;
        }
        else
        {
            Debug.Log("reinforce in progress");
            attackingBase.GetComponent<baseScipt>().units += unitAttackCount;
            usingBase.GetComponent<baseScipt>().units -= unitAttackCount;
            if (attackingBase.GetComponent<baseScipt>().units >= attackingBase.GetComponent<baseScipt>().maxCap)
            {
                attackingBase.GetComponent<baseScipt>().units = attackingBase.GetComponent<baseScipt>().maxCap;
            }
        }

        ReinforcePopUpObject.SetActive(false);
        DestroyLines();
        topBarText.text = "Awaiting Command...";
        unitAttackCount = 0;
    }



    void showAttackButton(GameObject go)
    {
        foreach (GameObject item in bases)
        {
            if (item != go)
            {
                if (!item.GetComponent<baseScipt>().ally)
                {
                    item.GetComponent<baseScipt>().attackButton.enabled = true;
                    item.GetComponent<baseScipt>().attackButton.transform.gameObject.SetActive(true);
                }
            }
        }
    }

    void showReinforceButton(GameObject go)
    {
        foreach (GameObject item in bases)
        {
            if (item != go)
            {
                if (item.GetComponent<baseScipt>().ally)
                {
                    item.GetComponent<baseScipt>().reinforceButton.enabled = true;
                    item.GetComponent<baseScipt>().reinforceButton.transform.gameObject.SetActive(true);

                }
            }
        }
    }

    void hideAttackButton()
    {
        foreach (GameObject item in bases)
        {
            item.GetComponent<baseScipt>().attackButton.enabled = false;
            item.GetComponent<baseScipt>().attackButton.transform.gameObject.SetActive(false);
        }
    }

    void hideReinforceButton()
    {
        foreach (GameObject item in bases)
        {
            item.GetComponent<baseScipt>().reinforceButton.enabled = false;
            item.GetComponent<baseScipt>().reinforceButton.transform.gameObject.SetActive(false);
        }
    }

    RaycastHit2D GetMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        return hit;
    }
    private void GenerateBaha1v1Map()
    {
        int[,] array2D = new int[15, 2] { { 1, 5 }, { 3, 3 }, { 5, 5 }, { 5, 7 }, { 5, 1 }, { 7, 5 }, { 9, 7 }, { 9, 5 }, { 9, 1 }, { 11, 5 }, { 13, 7 }, { 13, 5 }, { 13, 1 }, { 15, 3 }, { 17, 5 } };
        Spawner(array2D);

    }

    private void Spawner(int[,] a)
    {
        for (int i = 0; i < a.Length / 2; i++)
        {
            GameObject temp = Instantiate(basePrefab);
            temp.transform.position = new Vector3(a[i, 0], a[i, 1], 0);
        }
    }
    private void MakeConnection()
    {

    }


    void DrawLine(Vector3 p1, Vector3 p2)
    {
        DestroyLines();
        GameObject temp = Instantiate(LineRendererPrefab);
        temp.transform.parent = transform;
        LineRenderer tempLineRenderer = temp.GetComponent<LineRenderer>();
        tempLineRenderer.positionCount = 2;
        tempLineRenderer.SetPosition(0, p1);
        tempLineRenderer.SetPosition(1, p2);
    }
    void DrawLine(Vector3 p1, Vector3 p2, Color c)
    {
        DestroyLines();
        GameObject temp = Instantiate(LineRendererPrefab);
        temp.transform.parent = transform;
        LineRenderer tempLineRenderer = temp.GetComponent<LineRenderer>();
        tempLineRenderer.positionCount = 2;
        tempLineRenderer.startColor = c;
        tempLineRenderer.endColor = c;
        tempLineRenderer.SetPosition(0, p1);
        tempLineRenderer.SetPosition(1, p2);
    }

    void DestroyLines()
    {
        hideAttackButton();
        hideReinforceButton();
        if (lines == true)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            lines = false;
        }
    }

    void DestroyLines(GameObject go)
    {
        hideAttackButton();
        hideReinforceButton();
        if (lines == true)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            lines = false;
        }
    }
}
