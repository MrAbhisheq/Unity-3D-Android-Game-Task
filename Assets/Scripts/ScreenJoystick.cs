using UnityEngine;

public class ScreenJoystick : MonoBehaviour
{
    public MainPlayer player;
    public RectTransform joystickHandle;

    private Vector2 joystickPos;
    private Vector2 lastPos;
    private float distanceMoved;
    private bool isPressed;
    private bool wasPressed;

    private void Update()
    {
        if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                isPressed = true;
                joystickPos = Input.GetTouch(0).position;
            }
            else
            {
                isPressed = false;
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                isPressed = true;
                joystickPos = (Vector2)Input.mousePosition;
            }
            else
            {
                isPressed = false;
            }
        }

        if(isPressed)
        {
            if (!wasPressed)
            {
                lastPos = joystickPos;
            }

            distanceMoved = wasPressed ? Vector2.Distance(lastPos, joystickPos) : 0f;

            joystickHandle.position = new Vector3(joystickPos.x, joystickPos.y, 0f);
        }
        else
        {
            distanceMoved = 0f;
            lastPos = Vector2.zero;
            joystickPos = Vector2.zero;
        }

        wasPressed = isPressed;

        if(player != null)
            player.SetInput(GetMovementValue());
    }

    Vector2 GetMovementValue()
    {
        Vector2 dir = (joystickPos - lastPos) / 100;

        Vector2 inputVal = isPressed ? Vector2.ClampMagnitude(dir, 1f) : Vector2.zero;

        //Debug.Log($"Joystick Input: {inputVal}");

        return inputVal;
    }
}
