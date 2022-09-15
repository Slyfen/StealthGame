using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSlider : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Image>().fillAmount < 1)
            GetComponent<Image>().fillAmount += Time.deltaTime * .5f;
        else
            GetComponent<Image>().fillAmount = 1;

    }
}
