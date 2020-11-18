using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject basePrefab;
    GameObject[] bases;
    private RaycastHit2D currentHit;
    private TextMeshProUGUI topBarText;
    public GameObject LineRendererPrefab;
    GameObject usingBase;
    GameObject attackingBase;
    public GameObject attackPopUpObject;
    public GameObject ReinforcePopUpObject;
    private string userInputAttack;
    private int unitAttackCount;

    private GameObject userLineObject;


    void Start()
    {
        topBarText = GameObject.Find("UI").GetComponentInChildren<TextMeshProUGUI>();

        GenerateBaha1v1Map();

        bases = GameObject.FindGameObjectsWithTag("base");

        makeConnections();

        int x = 1;
        foreach (GameObject item in bases)
        {
            baseScipt itemScript = item.GetComponent<baseScipt>();
            itemScript.baseName.text = "beta" + x;
            item.name = "beta" + x;
            itemScript.maxCap = 200;
            itemScript.prodRatePerSecond = 2;
            itemScript.units = 0;
            itemScript.changeRelationTo(0);
            x++;
        }

        findBaseAtPos(new Vector3(1, 5, 0)).GetComponent<baseScipt>().changeRelationTo(1);
        findBaseAtPos(new Vector3(17, 5, 0)).GetComponent<baseScipt>().changeRelationTo(2);

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
                        usingBase.GetComponent<baseScipt>().printArray();
                        showAttackButton(usingBase);
                        showReinforceButton(usingBase);
                        topBarText.text = "Selected " + usingBase.GetComponent<baseScipt>().baseName.text;
                    }
                }
            }
        }

        if (Input.GetKeyDown("escape"))
        {
            attackPopUpObject.SetActive(false);
            ReinforcePopUpObject.SetActive(false);
            hideAttackButton();
            hideReinforceButton();
            DestroyUserLine();
            topBarText.text = "Selected nothing ";
        }

    }

    public void StartAttack()
    {
        attackingBase = currentHit.collider.transform.parent.parent.gameObject;
        DrawUserLine(usingBase.transform.position, attackingBase.transform.position, Color.red);
        topBarText.text = usingBase.GetComponent<baseScipt>().baseName.text + " -> " + attackingBase.GetComponent<baseScipt>().baseName.text;
        attackPopUpObject.SetActive(true);

    }

    public void StartReinforce()
    {
        attackingBase = currentHit.collider.transform.parent.parent.gameObject;
        DrawUserLine(usingBase.transform.position, attackingBase.transform.position, Color.green);
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
            DestroyUserLine();
            return;
        }
        else
        {
            attackingBase.GetComponent<baseScipt>().units -= unitAttackCount;
            usingBase.GetComponent<baseScipt>().units -= unitAttackCount;
            if (attackingBase.GetComponent<baseScipt>().units <= 0)
            {
                attackingBase.GetComponent<baseScipt>().changeRelationTo(1);
                attackingBase.GetComponent<baseScipt>().units = 0;
            }
        }
        attackPopUpObject.SetActive(false);
        topBarText.text = "Awaiting Command...";
        unitAttackCount = 0;
        DestroyUserLine();
    }

    public void ConfirmReinforce()
    {
        if (usingBase.GetComponent<baseScipt>().units < int.Parse(userInputAttack))
        {
            topBarText.text = "not enough units";
            ReinforcePopUpObject.SetActive(false);
            DestroyUserLine();
            return;
        }
        else
        {
            attackingBase.GetComponent<baseScipt>().units += unitAttackCount;
            usingBase.GetComponent<baseScipt>().units -= unitAttackCount;
            if (attackingBase.GetComponent<baseScipt>().units >= attackingBase.GetComponent<baseScipt>().maxCap)
            {
                attackingBase.GetComponent<baseScipt>().units = attackingBase.GetComponent<baseScipt>().maxCap;
            }
        }

        ReinforcePopUpObject.SetActive(false);
        topBarText.text = "Awaiting Command...";
        unitAttackCount = 0;
        DestroyUserLine();
    }



    void showAttackButton(GameObject go)
    {
        foreach (GameObject item in bases)
        {
            if (item != go)
            {
                if (!item.GetComponent<baseScipt>().ally && item.GetComponent<baseScipt>().isConnected(go))
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
                if (item.GetComponent<baseScipt>().ally && item.GetComponent<baseScipt>().isConnected(go))
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
        //int[,] array2D = new int[15, 2] { { 1, 5 }, { 3, 3 }, { 5, 5 }, { 5, 7 }, { 5, 1 }, { 7, 5 }, { 9, 7 }, { 9, 5 }, { 9, 1 }, { 11, 5 }, { 13, 7 }, { 13, 5 }, { 13, 1 }, { 15, 3 }, { 17, 5 } };
        //Spawner(array2D);

        Instantiate(basePrefab).transform.position = new Vector3(1, 5, 0);
        Instantiate(basePrefab).transform.position = new Vector3(3, 3, 0);
        Instantiate(basePrefab).transform.position = new Vector3(5, 5, 0);
        Instantiate(basePrefab).transform.position = new Vector3(5, 7, 0);
        Instantiate(basePrefab).transform.position = new Vector3(5, 1, 0);
        Instantiate(basePrefab).transform.position = new Vector3(7, 5, 0);
        Instantiate(basePrefab).transform.position = new Vector3(9, 7, 0);
        Instantiate(basePrefab).transform.position = new Vector3(9, 5, 0);
        Instantiate(basePrefab).transform.position = new Vector3(9, 1, 0);
        Instantiate(basePrefab).transform.position = new Vector3(11, 5, 0);
        Instantiate(basePrefab).transform.position = new Vector3(13, 7, 0);
        Instantiate(basePrefab).transform.position = new Vector3(13, 5, 0);
        Instantiate(basePrefab).transform.position = new Vector3(13, 1, 0);
        Instantiate(basePrefab).transform.position = new Vector3(15, 3, 0);
        Instantiate(basePrefab).transform.position = new Vector3(17, 5, 0);

        DrawLine(new Vector3(1, 5, 0), new Vector3(5, 7, 0));
        DrawLine(new Vector3(1, 5, 0), new Vector3(3, 3, 0));
        DrawLine(new Vector3(5, 7, 0), new Vector3(5, 5, 0));
        DrawLine(new Vector3(5, 7, 0), new Vector3(7, 5, 0));
        DrawLine(new Vector3(3, 3, 0), new Vector3(5, 5, 0));
        DrawLine(new Vector3(3, 3, 0), new Vector3(5, 1, 0));
        DrawLine(new Vector3(5, 5, 0), new Vector3(7, 5, 0));
        DrawLine(new Vector3(5, 1, 0), new Vector3(7, 5, 0));
        DrawLine(new Vector3(5, 1, 0), new Vector3(9, 1, 0));
        DrawLine(new Vector3(7, 5, 0), new Vector3(9, 1, 0));
        DrawLine(new Vector3(7, 5, 0), new Vector3(9, 7, 0));
        DrawLine(new Vector3(9, 7, 0), new Vector3(9, 5, 0));
        DrawLine(new Vector3(9, 5, 0), new Vector3(9, 1, 0));
        DrawLine(new Vector3(9, 7, 0), new Vector3(11, 5, 0));
        DrawLine(new Vector3(9, 1, 0), new Vector3(11, 5, 0));
        DrawLine(new Vector3(9, 1, 0), new Vector3(13, 1, 0));
        DrawLine(new Vector3(11, 5, 0), new Vector3(13, 1, 0));
        DrawLine(new Vector3(11, 5, 0), new Vector3(13, 5, 0));
        DrawLine(new Vector3(11, 5, 0), new Vector3(13, 7, 0));
        DrawLine(new Vector3(13, 7, 0), new Vector3(13, 5, 0));
        DrawLine(new Vector3(13, 1, 0), new Vector3(15, 3, 0));
        DrawLine(new Vector3(13, 5, 0), new Vector3(15, 3, 0));
        DrawLine(new Vector3(15, 3, 0), new Vector3(17, 5, 0));
        DrawLine(new Vector3(13, 7, 0), new Vector3(17, 5, 0));

        //ArrayList vArray = new ArrayList();
        //vArray.Add(new Vector2(5, 7));

    }

    private void makeConnections()
    {
        GameObject[] linesToConnect = GameObject.FindGameObjectsWithTag("lineRenderer");

        foreach (GameObject item in linesToConnect)
        {
            Vector3 pos1 = item.GetComponent<LineRenderer>().GetPosition(0);
            Vector3 pos2 = item.GetComponent<LineRenderer>().GetPosition(1);
            GameObject sBase = findBaseAtPos(pos1);
            GameObject cBase = findBaseAtPos(pos2);
            sBase.GetComponent<baseScipt>().addInArray(cBase);
            cBase.GetComponent<baseScipt>().addInArray(sBase);
        }
    }

    private GameObject findBaseAtPos(Vector3 pos1)
    {
        foreach (GameObject item in bases)
        {
            if (item.transform.position == pos1)
            {
                return item;
            }
        }
        return null;
    }

    private void Spawner(int[,] a)
    {
        for (int i = 0; i < a.Length / 2; i++)
        {
            GameObject temp = Instantiate(basePrefab);
            temp.transform.position = new Vector3(a[i, 0], a[i, 1], 0);
        }
    }


    void DrawLine(Vector3 p1, Vector3 p2)
    {
        GameObject temp = Instantiate(LineRendererPrefab);
        temp.transform.parent = transform;
        LineRenderer tempLineRenderer = temp.GetComponent<LineRenderer>();
        tempLineRenderer.positionCount = 2;
        tempLineRenderer.SetPosition(0, p1);
        tempLineRenderer.SetPosition(1, p2);
    }
    void DrawUserLine(Vector3 p1, Vector3 p2, Color c)
    {
        GameObject temp = Instantiate(LineRendererPrefab);
        userLineObject = temp;
        temp.transform.parent = transform;
        LineRenderer tempLineRenderer = temp.GetComponent<LineRenderer>();
        tempLineRenderer.positionCount = 2;
        tempLineRenderer.name = "testLine";
        tempLineRenderer.startColor = c;
        tempLineRenderer.endColor = c;
        tempLineRenderer.SetPosition(0, p1);
        tempLineRenderer.SetPosition(1, p2);
    }

    void DestroyUserLine()
    {
        Destroy(userLineObject);
    }
}
