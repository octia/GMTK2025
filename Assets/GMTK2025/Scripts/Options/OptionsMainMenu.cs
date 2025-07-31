using UnityEngine;
using PrimeTween;

public class OptionsMainMenu : MonoBehaviour
{
    private bool bIsAnimating = false;
    [SerializeField] private GameObject goFrootLoopUI;
    [SerializeField] private UnityEngine.UI.Image imBackground;
    public void OptionsAnim(bool bIsOnOptions)
    {
        if(!bIsAnimating)
        {
            bIsAnimating = true;
            if (bIsOnOptions)
            {
                Sequence.Create()
                    .Insert(atTime: 0f, Tween.Color(imBackground, new Color32(0,0,0,0), duration: 0.1f))
                    .Group(Tween.PositionY(goFrootLoopUI.transform, endValue: 1.159f, duration: 1f))
                    .Group(Tween.PositionX(goFrootLoopUI.transform, endValue: 1.076f, duration: 1f))
                    .Group(Tween.PositionZ(goFrootLoopUI.transform, endValue: 1.613f, duration: 1f))
                    .Group(Tween.Rotation(goFrootLoopUI.transform, endValue: Quaternion.Euler(0, -195, 0), duration: 1f))
                    .Chain(Tween.Rotation(goFrootLoopUI.transform, endValue: Quaternion.Euler(0f, 0f, 0f), duration: 0.5f))
                    .OnComplete(() => bIsAnimating = false); 
            }
            else
            {
                Sequence.Create()
                    .Chain(Tween.Rotation(goFrootLoopUI.transform, endValue: Quaternion.Euler(0, -195, 0), duration: 1f))
                    .Chain(Tween.PositionY(goFrootLoopUI.transform, endValue: 1.437f, duration: 0.5f))
                    .Group(Tween.PositionX(goFrootLoopUI.transform, endValue: 0f, duration: 0.5f))
                    .Group(Tween.PositionZ(goFrootLoopUI.transform, endValue: 1.154f, duration: 0.5f))
                    .Group(Tween.Rotation(goFrootLoopUI.transform, endValue: Quaternion.Euler(0.016f, 115.767f, 15.926f), duration: 0.5f))
                    .Insert(atTime: 1.4f, Tween.Color(imBackground, new Color32(0, 0, 0, 200), duration: 0.3f))
                    .OnComplete(() => bIsAnimating = false);
            }
        }
    }
}
