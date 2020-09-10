using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    public int fps = 60;

    private void Start()
    {
        Application.targetFrameRate = fps;
    }
}
