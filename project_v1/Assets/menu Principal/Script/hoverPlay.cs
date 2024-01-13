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
        play = GetComponent<Animator>();
        play.Play("PlayBase");

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AnimatorStateInfo backgroundState = backgroundAnimator.GetCurrentAnimatorStateInfo(0);

        // Calculer la proportion de progression pour commencer à la même frame que Background
        float normalizedTime = (float)(backgroundState.normalizedTime) % 1;

        // Lancer l'animation OptionBackground à partir de la même frame que Background
        backgroundAnimator.Play("PlayBackground", 0, Mathf.Clamp01(normalizedTime));
        play.Play("PlayAnimation");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AnimatorStateInfo backgroundState = backgroundAnimator.GetCurrentAnimatorStateInfo(0);

        // Calculer la proportion de progression pour commencer à la même frame que Background
        float normalizedTime = (float)(backgroundState.normalizedTime) % 1;

        // Lancer l'animation OptionBackground à partir de la même frame que Background
        backgroundAnimator.Play("Background", 0, Mathf.Clamp01(normalizedTime));
        play.Play("PlayBase");
    }
    


    

    
}
