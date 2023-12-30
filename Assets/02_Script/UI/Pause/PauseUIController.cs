using DG.Tweening;
using FD.Dev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIController : MonoBehaviour
{

    [SerializeField] private Transform buttonsRoot;
    [SerializeField] private Transform settingRoot;
    [SerializeField] private Transform pausePanel;
    [SerializeField] private Image bg;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sfxSlider;

    private bool isPause = false;

    private void Awake()
    {
        
        for(int i = 0; i < buttonsRoot.childCount; i++)
        {

            buttonsRoot.GetChild(i).localPosition -= new Vector3(1100, 0, 0);

        }

        for (int i = 0; i < settingRoot.childCount; i++)
        {

            settingRoot.GetChild(i).localPosition += new Vector3(1100, 0, 0);

        }

        pausePanel.transform.localPosition -= new Vector3(1100, 0, 0);

        var set = bg.color;
        set.a = 0;
        bg.color = set;

        volumeSlider.onValueChanged.AddListener(HandleBGValueChanged);
        sfxSlider.onValueChanged.AddListener(HandleSFXValueChanged);

        volumeSlider.value = PlayerPrefs.GetFloat("SFX", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);

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

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {

            if (isPause)
            {

                Resume();

            }
            else
            {

                Pause();

            }

            isPause = !isPause;

        }

    }

    public void Resume()
    {

        Time.timeScale = 1f;
        StartCoroutine(PauseUIUnShowCo());

    }

    public void Pause()
    {

        Time.timeScale = 0f;
        StartCoroutine(PauseUIShowCo());

    }

    private void OnDestroy()
    {

        Time.timeScale = 1.0f;

    }

    private IEnumerator PauseUIShowCo()
    {

        pausePanel.DOLocalMoveX(-560, 0.3f).SetEase(Ease.OutSine).SetUpdate(true);
        bg.DOFade(0.7f, 0.4f).SetUpdate(true); 

        for (int i = 0; i < buttonsRoot.childCount; i++)
        {

            buttonsRoot.GetChild(i).DOLocalMoveX(-646, 0.3f).SetEase(Ease.OutSine).SetUpdate(true);
            yield return new WaitForSecondsRealtime(0.1f);

        }

        for (int i = 0; i < settingRoot.childCount; i++)
        {

            settingRoot.GetChild(i).DOLocalMoveX(406, 0.3f).SetEase(Ease.OutSine).SetUpdate(true);
            yield return new WaitForSecondsRealtime(0.1f);

        }


        yield return null;

    }

    private IEnumerator PauseUIUnShowCo()
    {
        pausePanel.DOLocalMoveX(-560 - 1100, 0.3f).SetEase(Ease.OutSine).SetUpdate(true);
        bg.DOFade(0f, 0.4f).SetUpdate(true);

        for (int i = 0; i < buttonsRoot.childCount; i++)
        {

            buttonsRoot.GetChild(i).DOLocalMoveX(-646 - 1100, 0.3f).SetEase(Ease.OutSine).SetUpdate(true);
            yield return new WaitForSecondsRealtime(0.1f);

        }

        for (int i = 0; i < settingRoot.childCount; i++)
        {

            settingRoot.GetChild(i).DOLocalMoveX(406 + 1100, 0.3f).SetEase(Ease.OutSine).SetUpdate(true);
            yield return new WaitForSecondsRealtime(0.1f);

        }


        yield return null;


    }


}
