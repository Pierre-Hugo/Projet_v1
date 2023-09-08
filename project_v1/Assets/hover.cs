using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hover :MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject background;
    private Animator backgroundAnimator;

    private void Start()
    {
        backgroundAnimator = background.GetComponent<Animator>();
       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundAnimator.Play("PlayBackground");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundAnimator.Play("Background");
    }
}
