using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;


public class TitleCmd : MonoBehaviour
{
    const char DOT = '.';
    [SerializeField] Image dim;
    [SerializeField] Button startButton;
    [SerializeField] TextMeshProUGUI contentText;
    [SerializeField] string[] contentStrings;

    private Sequence sequence;
    private Coroutine dotCoroutine;
    private void Awake()
    {
        sequence = DOTween.Sequence();
        SetButtonEvent();
    }
    private void Start()
    {
        startButton.interactable = false;
        sequence.Append(contentText.DOFade(0, 1));
        sequence.Append(contentText.DOFade(1, 1));
        sequence.SetAutoKill(false);
        sequence.Pause();

        DimStart();
        SetContentString(AppLoadState.end);
    }

    private void SetButtonEvent()
    {
        startButton.SetEvent(() =>
        {
            Debug.Log($"<color=red>씬이동</color>");
        });
    }

    #region Splash
    private void DimStart()
    {
        dim.gameObject.SetActive(true);
        dim.DOFade(0, 1).onComplete = DimEnd;
    }
    private void DimEnd()
    {
        dim.gameObject.SetActive(false);
    }
    #endregion

    #region Event
    private void SetContentString(AppLoadState state)
    {
        if (dotCoroutine != null)
            StopCoroutine(dotCoroutine);
        switch (state)
        {
            case AppLoadState.userCheck:
            case AppLoadState.versionCheck:
            case AppLoadState.download:
                dotCoroutine = StartCoroutine(ContentTextDotCoroutine(contentStrings[(int)state]));
                break;
            case AppLoadState.end:
                startButton.interactable = true;
                sequence.Play().SetLoops(-1);
                break;
            default:
                Debug.LogError($"SetContentString : 해당되는 State 없음");
                break;
        }
    }

    private IEnumerator ContentTextDotCoroutine(string str)
    {
        int countMax = 5, count = 0;
        string text = str;
        while (true)
        {
            count++;
            if (count < countMax) text += DOT;
            else
            {
                count = 0;
                text = str;
            }
            contentText.text = text;
            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion

}
