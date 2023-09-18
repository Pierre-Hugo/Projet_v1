using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverOption :MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Animator option;

    private void Start()
    {
        option = this.GetComponent<Animator>();
        option.Play("BaseOption");
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        option.Play("Option");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        option.Play("BaseOption");
    }

   


}
