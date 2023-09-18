using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBack : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator back;

    private void Start()
    {
        back = this.GetComponent<Animator>();
        back.Play("BaseBack");
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        back.Play("BackAnimation");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        back.Play("BaseBack");
    }
}
