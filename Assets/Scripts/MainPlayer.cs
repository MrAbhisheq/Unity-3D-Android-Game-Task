using UnityEngine;

public class MainPlayer : BasePlayer
{
    public bool useKeyboard = true;

    protected override void Start()
    {
        base.Start();

        moveDir = Vector3.back;
    }

    //protected override void OnPlayerFall()
    //{
    //    base.OnPlayerFall();

    //    gameObject.SetActive(false);
    //}

    void Update()
    {

#if UNITY_EDITOR
        if (useKeyboard)
        {
            input = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            );

            //Debug.Log($"Keyboard Move: {input}");
        }
#endif
    }
}