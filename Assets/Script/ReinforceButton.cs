using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReinforceButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject gm = GameObject.Find("GameMaster");
        GetComponent<Button>().onClick.AddListener(gm.GetComponent<GameMaster>().StartReinforce);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
