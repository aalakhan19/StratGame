using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject gm = GameObject.Find("GameMaster");
        GetComponent<Button>().onClick.AddListener(gm.GetComponent<GameMaster>().StartAttack);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        Debug.Log("You have clicked the button!");
    }
}
