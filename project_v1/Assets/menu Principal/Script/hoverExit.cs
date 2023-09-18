using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverExit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator exit;

    private void Start()
    {
        exit = this.GetComponent<Animator>();
        exit.Play("ExitBase");
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        exit.Play("ExitAnimation");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        exit.Play("ExitBase");
    }
}
