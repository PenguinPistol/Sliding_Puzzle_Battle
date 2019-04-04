using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeChanger
{
    public static float Calculate(RectTransform _canvas)
    {
        float ratio = Display.main.systemHeight / _canvas.rect.height;
        float banner = AdsManager.Instance.BannerHeight / ratio;

        //Debug.Log("canvas : " + _canvas.rect.height);
        //Debug.Log("banner : " + banner);
        //Debug.Log("cal : " + (_canvas.rect.height - banner));

        return _canvas.rect.height - banner;
    }
}
