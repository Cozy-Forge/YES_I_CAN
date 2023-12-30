using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.UI;

public class IntroSceneUIController : MonoBehaviour
{

    [Header("TitleUI")]
    [SerializeField] private Transform titleButtonsRoot;
    [SerializeField] private TMP_Text[] titleArr;

    [Space]
    [Header("OptionUI")]
    [SerializeField] private Transform optionPanel;
    [SerializeField] private Transform optionButtonRoot;
    [SerializeField] private Transform optionExitButton;
    [SerializeField] private Transform optionVoluemPanel;
    [SerializeField] private Transform optionSfxVoluemPanel;

    private Slider optionVolumeSlider;
    private Slider optionSfxVolumeSlider;
    private bool isControlled;

    private void Awake()
    {

        for (int i = 0; i < optionButtonRoot.childCount; i++)
        {

            optionButtonRoot.GetChild(i).transform.localPosition += new Vector3(-1100, 0, 0);

        }

        optionVoluemPanel.position += new Vector3(1100, 0, 0);
        optionSfxVoluemPanel.position += new Vector3(1100, 0, 0);
        optionExitButton.position -= new Vector3(1100, 0, 0);
        optionPanel.position -= new Vector3(1100, 0, 0);

        var buttons = titleButtonsRoot.GetComponentsInChildren<CustomButton>();

        foreach(var button in buttons)
        {

            button.OnButtonClickEvent += HandleIntroButtonClick;

        }

        optionSfxVolumeSlider = optionSfxVoluemPanel.GetComponentInChildren<Slider>();
        optionVolumeSlider = optionVoluemPanel.GetComponentInChildren<Slider>();

        optionVolumeSlider.onValueChanged.AddListener(HandleBGValueChanged);
        optionSfxVolumeSlider.onValueChanged.AddListener(HandleSFXValueChanged);

        optionSfxVolumeSlider.value = PlayerPrefs.GetFloat("SFX", 0.5f);
        optionVolumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);


    }

    private void HandleSFXValueChanged(float arg0)
    {

        SoundManager.Instance.SFXVolume(arg0);
        PlayerPrefs.SetFloat("SFX", arg0);

    }

    private void HandleBGValueChanged(float arg0)
    {

        SoundManager.Instance.BGSoundVolume(arg0);
        PlayerPrefs.SetFloat("Volume", arg0);

    }

    private void HandleIntroButtonClick()
    {

        StartCoroutine(ButtonUnShowTweenCo());

    }

    private void Start()
    {

        TitleUIShow(1);

    }

    public void TitleUIShow(float waitTime)
    {

        for (int i = 0; i < titleButtonsRoot.childCount; i++)
        {

            titleButtonsRoot.GetChild(i).transform.localPosition += new Vector3(-1100, 0, 0);

        }

        foreach (var text in titleArr)
        {

            var old = text.color;
            old.a = 0f;
            text.color = old;

        }

        StartCoroutine(ButtonShowTweenCo(waitTime));

    }

    public void OptionUIShow()
    {

        StartCoroutine(OptionUIShowCo());

    }

    public void OptionUIUnShow()
    {

        StartCoroutine(OptionUIUnShowCo());

    }

    private IEnumerator OptionUIUnShowCo()
    {

        optionPanel.DOLocalMoveX(-510 -1100, 0.4f).SetEase(Ease.OutSine);
        optionExitButton.DOLocalMoveX(-741 -1100, 0.4f).SetEase(Ease.OutSine);
        optionVoluemPanel.DOLocalMoveX(406 +1100, 0.4f).SetEase(Ease.OutSine);

        yield return new WaitForSeconds(0.1f);

        optionSfxVoluemPanel.DOLocalMoveX(406 + 1100, 0.4f).SetEase(Ease.OutSine);

        for (int i = 0; i < optionButtonRoot.childCount; i++)
        {

            optionButtonRoot.GetChild(i).DOLocalMoveX(-646 -1100, 0.3f).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(0.1f);

        }

    }

    private IEnumerator OptionUIShowCo()
    {

        optionPanel.DOLocalMoveX(-610, 0.4f).SetEase(Ease.OutSine);
        optionExitButton.DOLocalMoveX(-741, 0.4f).SetEase(Ease.OutSine);
        optionVoluemPanel.DOLocalMoveX(406, 0.4f).SetEase(Ease.OutSine);

        yield return new WaitForSeconds(0.1f);

        optionSfxVoluemPanel.DOLocalMoveX(406, 0.4f).SetEase(Ease.OutSine);

        for (int i = 0; i < optionButtonRoot.childCount; i++)
        {

            optionButtonRoot.GetChild(i).DOLocalMoveX(-646, 0.3f).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(0.1f);

        }

    }

    private IEnumerator ButtonShowTweenCo(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);

        foreach (var text in titleArr)
        {

            text.DOFade(1, 0.3f).SetEase(Ease.OutSine);

        }

        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < titleButtonsRoot.childCount; i++)
        {

            titleButtonsRoot.GetChild(i).DOLocalMoveX(-550, 0.3f).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(0.1f);

        }

    }

    private IEnumerator ButtonUnShowTweenCo()
    {


        foreach (var text in titleArr)
        {

            text.DOFade(0, 0.3f).SetEase(Ease.OutSine);

        }

        for (int i = 0; i < titleButtonsRoot.childCount; i++)
        {

            titleButtonsRoot.GetChild(i).DOLocalMoveX(-1650, 0.3f).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(0.1f);

        }


    }

}