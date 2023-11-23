using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverOption :MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject background;
    private Animator backgroundAnimator;
    private Animator option;

    private void Start()
    {
        backgroundAnimator = background.GetComponent<Animator>();
        option = this.GetComponent<Animator>();
        option.Play("BaseOption");
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        AnimatorStateInfo backgroundState = backgroundAnimator.GetCurrentAnimatorStateInfo(0);

        // Calculer la proportion de progression pour commencer à la même frame que Background
        float normalizedTime = (float)(backgroundState.normalizedTime)%1 ;

        // Lancer l'animation OptionBackground à partir de la même frame que Background
        backgroundAnimator.Play("OptionBackground", 0, Mathf.Clamp01(normalizedTime));

        option.Play("Option");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AnimatorStateInfo backgroundState = backgroundAnimator.GetCurrentAnimatorStateInfo(0);

        // Calculer la proportion de progression pour commencer à la même frame que Background
        float normalizedTime = (float)(backgroundState.normalizedTime)%1;

        // Lancer l'animation OptionBackground à partir de la même frame que Background
        backgroundAnimator.Play("Background", 0, Mathf.Clamp01(normalizedTime));
        option.Play("BaseOption");
    }

}
