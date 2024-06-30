using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialiserMusic : MonoBehaviour
{
   public string namePlayerPrefToGet;
    void Start()
    {
        GetComponent<Slider>().value = PlayerPrefs.GetFloat(namePlayerPrefToGet);

    }


}
