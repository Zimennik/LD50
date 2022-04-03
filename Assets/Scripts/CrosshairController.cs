using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private Image _crosshair;
    [SerializeField] private Image _crosshairProgress;


    public void SetCroshairVisibility(bool isVisible)
    {
        _crosshair.gameObject.SetActive(isVisible);
    }

    public void SetProgress(float progress)
    {
        _crosshairProgress.fillAmount = progress;
    }

    public void SetProgressVisibility(bool isVisible)
    {
        _crosshairProgress.gameObject.SetActive(isVisible);
    }
}