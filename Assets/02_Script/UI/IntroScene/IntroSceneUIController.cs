using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IntroSceneUIController : MonoBehaviour
{

    [SerializeField] private Transform buttonsRoot;

    private void Start()
    {

        for (int i = 0; i < buttonsRoot.childCount; i++)
        {

            buttonsRoot.GetChild(i).transform.localPosition += new Vector3(-1100, 0, 0);

        }

        StartCoroutine(ButtonTweenCo());

    }

    private IEnumerator ButtonTweenCo()
    {

        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < buttonsRoot.childCount; i++)
        {

            buttonsRoot.GetChild(i).DOLocalMoveX(-550, 0.3f).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(0.1f);

        }

    }

}
