using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldManager : MonoBehaviour
{
    const int timeDifference = 6;

    [SerializeField]
    SpriteRenderer blueSky;
    [SerializeField]
    SpriteRenderer redSky;
    [SerializeField]
    SpriteRenderer blackSky;
    [SerializeField]
    Light2D light;
    [SerializeField]
    AnimationCurve lightCurve;
    public enum Weather
    {
        sunny,
        cloudy,
        snowy,
        stormy
    }

    public void Initialize()
    {
        int time = (SaveDataManager._mainSaveData.time + timeDifference)  % 24;
        BlueSkyBehaviour(time);
        RedSkyBehaviour(time);
        LightBehaviour(time);
        Debug.Log($"blue:{blueSky.color.a} + red:{redSky.color.a} + time:{time}");
    }

    void BlueSkyBehaviour(float time)
    {
        if (time < 3 || 16 <= time)
        {
            blueSky.color = new Color(blueSky.color.r, blueSky.color.g, blueSky.color.b, 0);
        }
        else if (time <= 6)
        {
            blueSky.color = new Color(blueSky.color.r, blueSky.color.g, blueSky.color.b, (time - 2) / 5);
        }
        else if (14 <= time)
        {
            blueSky.color = new Color(blueSky.color.r, blueSky.color.g, blueSky.color.b, (17 - time) / 4);
        }
        else
        {
            blueSky.color = new Color(blueSky.color.r, blueSky.color.g, blueSky.color.b, 1);
        }
    }

    void RedSkyBehaviour(float time)
    {
        if (6 < time && time < 17)
        {
            redSky.color = new Color(redSky.color.r, redSky.color.g, redSky.color.b, 1);
        }
        else if (17 <= time && time <= 19)
        {
            redSky.color = new Color(redSky.color.r, redSky.color.g, redSky.color.b, (20 - time) / 4);
        }
        else
        {
            redSky.color = new Color(redSky.color.r, redSky.color.g, redSky.color.b, 0);
        }
    }
    void LightBehaviour(float time)
    {
        light.intensity = lightCurve.Evaluate(time);
    }
}
