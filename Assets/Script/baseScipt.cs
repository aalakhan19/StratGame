using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class baseScipt : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI baseName;
    public TextMeshProUGUI unitsCounter;
    public TextMeshProUGUI attackButton;
    public TextMeshProUGUI reinforceButton;

    public bool ally;
    public bool enemy;
    public bool neutral;

    public int maxCap;
    public int prodRatePerSecond;
    public int units;

    public GameObject[] connectedTo;
    private int index;
    void Awake()
    {
        unitsCounter.SetText(units.ToString());
        connectedTo = new GameObject[50];
        index = 0;

    }

    // Update is called once per frame
    void Update()
    {
        unitsCounter.SetText(units.ToString() + "/" + maxCap);

        if (ally)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (enemy)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else    //neutral
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }

    }

    public void changeRelationTo(int i)
    {
        // neutral
        // ally
        // enemy
        if (i == 0)
        {
            neutral = true;
            ally = false;
            enemy = false;
            StopCoroutine("Produce");
        }
        if (i == 1)
        {
            neutral = false;
            ally = true;
            enemy = false;
            StopCoroutine("Produce");
            StartCoroutine("Produce");
        }
        if (i == 2)
        {
            neutral = false;
            ally = false;
            enemy = true;
            StopCoroutine("Produce");
            StartCoroutine("Produce");
        }
    }

    public void addInArray(GameObject x)
    {
        connectedTo[index] = x;
        index++;
    }

    public bool isConnected(GameObject go)
    {
        for (int i = 0; i < index; i++)
        {
            if (connectedTo[i] == go)
            {
                return true;
            }
        }
        return false;
    }

    public void printArray()
    {
        for (int i = 0; i < index; i++)
        {
            Debug.Log(connectedTo[i].transform.position);
        }
    }

    IEnumerator Produce()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (units < maxCap)
            {
                if (units + prodRatePerSecond > maxCap)
                {
                    units = maxCap;
                }
                else
                {
                    units += prodRatePerSecond;
                }
            }
        }

    }



}
