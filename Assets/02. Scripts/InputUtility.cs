using UnityEngine;
using UnityEngine.InputSystem;


public static class InputUtility
{
    // 좌/우 입력 처리
    public static float GetSteering()
    {
        if (Keyboard.current == null)
        {
            return 0f;
        }

        float steer = 0f;
        if (Keyboard.current.leftArrowKey.isPressed == true)
        {
            steer -= 1f;
        }
        if (Keyboard.current.rightArrowKey.isPressed == true)
        {
            steer += 1f;
        }

        return steer;
    }

    // 전/후 입력 처리
    public static float GetThrottle()
    {
        if (Keyboard.current == null)
        {
            return 0f;
        }

        float throttle = 0f;
        if (Keyboard.current.wKey.isPressed == true)
        {
            throttle += 1f;
        }
        if (Keyboard.current.sKey.isPressed == true)
        {
            throttle -= 1f;
        }

        return throttle;
    }

    // 브레이크 입력 처리
    public static float GetBrake()
    {
        if (Keyboard.current == null)
        {
            return 0;
        }

        return Keyboard.current.spaceKey.isPressed ? 1f : 0f;
    }
}
