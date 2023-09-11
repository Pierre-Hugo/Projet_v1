using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverOption :MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    private Animator option;
    public GameObject MainScreen;
    private bool boutonCache;

    private void Start()
    {
        option = this.GetComponent<Animator>();
        option.Play("BaseOption");
        boutonCache = false;
    }
    private void update()
    {
        if(!MainScreen.activeSelf)
        {
            Debug.Log("Menu caché!");
            option.Play("BaseOption");
        }

        if(boutonCache  && MainScreen.activeSelf) 
        {
            boutonCache = false;
            option.Play("Option");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        option.Play("Option");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        option.Play("BaseOption");
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        boutonCache = true;
        option.Play("BaseOption");
    }


}
