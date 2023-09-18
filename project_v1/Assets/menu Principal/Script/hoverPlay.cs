using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverPlay :MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject background;
    private Animator backgroundAnimator;
    private Animator play;

    private void Start()
    {
        backgroundAnimator = background.GetComponent<Animator>();
        play = this.GetComponent<Animator>();
        play.Play("PlayBase");

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundAnimator.Play("PlayBackground");
        play.Play("PlayAnimation");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundAnimator.Play("Background");
        play.Play("PlayBase");
    }
    


    

    
}
