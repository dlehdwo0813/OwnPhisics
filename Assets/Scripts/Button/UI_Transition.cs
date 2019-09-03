using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Transition : MonoBehaviour {

    Animator animator;
    public bool isTransitionStart = false;
    public bool startWithTransition = false;
    public float animationTimer = 1;
    string from;
    string to;

    void Start() {
        animator = GetComponent<Animator>();
        if (startWithTransition)
        {
            SetType((int)transitionType);
            TransitionStart();
        }
    }



    public void SetFrom(string f)
    {
        from = f;
    }
    public void SetTo(string t)
    {
        to = t;
    }


    public void PopUp()
    {
    }

    [System.Serializable]
    public enum TransitionType
    {
        Canvas, Scene, PopUp
    }

    public TransitionType transitionType = TransitionType.Canvas;

    public void SetType(int t)
    {
        transitionType = (TransitionType)t;
        animator.SetInteger("TransitionType", (int)transitionType);
    }

    public void TransitionStart()
    {
        gameObject.SetActive(true);
        animator.SetBool("isTransitionStart", true);

        switch (transitionType)
        {
            case TransitionType.Canvas:
                {
                    break;
                }
            case TransitionType.Scene:
                {
                    break;
                }
            case TransitionType.PopUp:
                {
                    if (to != null)
                        GameManager.Instance.CanvasEnable(to);
                    break;
                }

        }



    }


    public void TransitionEnd()
    {
        transform.localScale = new Vector2(0, 0);
        switch (transitionType)
        {
            case TransitionType.Canvas:
                {
                    if (from != null && to != null)
                    {
                        GameManager.Instance.CanvasChange(from, to);
                        gameObject.SetActive(false);
                    }
                    break;
                }
            case TransitionType.Scene:
                {
                    if (to != null)
                        GameManager.Instance.LoadScene(to);
                    break;
                }
            case TransitionType.PopUp:
                {
                    if (to != null)
                        GameManager.Instance.CanvasDisable(to);
                    break;
                }

        }

        from = to = null;
        animator.SetBool("isTransitionStart", false);


    }


}
