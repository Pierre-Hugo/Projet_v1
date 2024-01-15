using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class StartScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator start;
    public Button boutonStart;
    

    private void Start()
    {
        start = GetComponent<Animator>();
        

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (boutonStart.interactable)
        {
            start.Play("PlayAnimation");
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (boutonStart.interactable)
        {
            start.Play("PlayBase");
        }
    }
}
