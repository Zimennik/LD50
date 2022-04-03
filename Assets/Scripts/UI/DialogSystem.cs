using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;


//Show text on screen with TMP_Text component
public class DialogSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text message;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject _pressAnyKey;

    bool awaitingInput = false;

    void Update()
    {
        if (Input.anyKeyDown && awaitingInput)
        {
            awaitingInput = false;
        }
    }

    public async UniTask Show(List<string> textToShow)
    {
        gameObject.SetActive(true);
        awaitingInput = false;

        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f);
        canvasGroup.blocksRaycasts = true;

        foreach (string t in textToShow)
        {
            _pressAnyKey.SetActive(false);
            message.text = t;
            message.maxVisibleCharacters = 0;
            for (int i = 0; i < t.Length; i++)
            {
                message.maxVisibleCharacters++;
                await UniTask.Delay(5);
            }

            awaitingInput = true;
            _pressAnyKey.SetActive(true);
            await UniTask.WaitUntil(() => !awaitingInput);
        }


        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
}