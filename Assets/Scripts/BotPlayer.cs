using UnityEngine;

public class BotPlayer : BasePlayer
{
    private Vector2 targetInput;
    private float changeTime = 0f;
    private float holdTime = 0f;

    private void Update()
    {
        if (holdTime > 0f)
        {
            holdTime -= Time.deltaTime;
            input = Vector2.zero;
            return;
        }

        changeTime -= Time.deltaTime;

        if (changeTime <= 0f)
        {
            targetInput = Random.insideUnitCircle.normalized;
            changeTime = Random.Range(1, 3);
            holdTime = Random.Range(0, 1.5f);
        }



        input = Vector2.Lerp(input, targetInput, Time.deltaTime * 2f);
    }

    //protected override void OnPlayerFall()
    //{
    //    base.OnPlayerFall();

    //    gameObject.SetActive(false);
    //}
}
