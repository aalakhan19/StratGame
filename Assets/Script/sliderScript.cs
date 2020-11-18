using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class sliderScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject usingBase;
    GameObject attackingBase;

    public GameObject topBar;

    float sliderValue;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        topBar.GetComponent<TextMeshProUGUI>().text = "Attacking " + attackingBase.GetComponent<baseScipt>().baseName.text + " from " + usingBase.GetComponent<baseScipt>().baseName.text + " with " + sliderValue;
    }

    public void SetSliderValue(float f)
    {
        sliderValue = f;
    }

    public void setUsingBase(GameObject ub)
    {
        usingBase = ub;
    }

    public void setAttackingBase(GameObject ab)
    {
        attackingBase = ab;
    }
}
