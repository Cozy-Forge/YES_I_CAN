using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    [Header("Hover")]
    [SerializeField] private Color originColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Vector3 originSize;
    [SerializeField] private Vector3 hoverSize;
    [SerializeField] private float hoveringSpeed;

    [Space]
    [Header("Click")]
    [SerializeField] private UnityEvent clickEvent;

    private TMP_Text buttonText;
    private float percent = 0;
    private bool isHover;

    public event Action OnButtonClickEvent;

    private void Awake()
    {

        buttonText = transform.Find("ButtonText").GetComponent<TMP_Text>();

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        clickEvent?.Invoke();
        OnButtonClickEvent?.Invoke();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        isHover = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        isHover = false;

    }

    private void Update()
    {

        if (isHover)
        {

            percent += Time.deltaTime * hoveringSpeed;

        }
        else
        {

            percent -= Time.deltaTime * hoveringSpeed;

        }

        percent = Mathf.Clamp01(percent);

        buttonText.color = Color.Lerp(originColor, hoverColor, percent);
        transform.localScale = Vector3.Lerp(originSize, hoverSize, percent);

    }

}
