using UnityEngine;
using UnityEngine.UI;

public class ResolutionHandler : MonoBehaviour
{
    public static ResolutionHandler Active;

    private readonly Vector2 ScreenMatchXTablet = new Vector2(1500, 1920);
    private readonly Vector2 ScreenMatchXPhone = new Vector2(1080, 1920);

    public ResolutionHandler()
    {
        Active = this;
    }

    private void Start()
    {
        SetFieldOfView();
    }

    public void SetFieldOfView()
    {
        float screenRatio = ((float)Screen.height) / ((float)Screen.width);
        if(screenRatio < 1.5f)
        {
            UIManager.Active.canvasScaler.referenceResolution = ScreenMatchXTablet;
        }
        else
        {
            UIManager.Active.canvasScaler.referenceResolution = ScreenMatchXPhone;
        }
    }
}
