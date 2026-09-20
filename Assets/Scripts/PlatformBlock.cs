using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlatformBlock : MonoBehaviour
{
    public bool isInitialPlatform;
    public float stayTime = 2;
    public float pressDistance = -0.15F;

    [Header("Platform Colors")]
    public Color initialColor = Color.black;
    public Color endColor = Color.black;

    private Renderer cube;
    private Material platformMat;
    private bool isVisited;
    private BasePlayer currentPlayer;

    public bool isOnPlatform;

    private void Awake()
    {
        cube = GetComponentInChildren<Renderer>();
        platformMat = cube.material;
        initialColor = platformMat.color;
    }

    public void SetBlockColor(Color color)
    {
        initialColor = color;
        platformMat.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Bot"))
        {
            if (!other.TryGetComponent<BasePlayer>(out var player))
                return;

            currentPlayer = player;

            if (isInitialPlatform)
                return;

            isOnPlatform = true;

            if (!isVisited)
            {
                //if (isInitialPlatform)
                //    StartCoroutine(HideInitialPlatformAfterTimeout(other.GetComponent<BasePlayer>()));
                //else
                    StartCoroutine(HidePlatformAfterTimeout(currentPlayer));
            }
        }
    }

    IEnumerator HidePlatformAfterTimeout(BasePlayer player)
    {
        isVisited = true;

        cube.transform.DOKill();
        cube.transform.DOLocalMoveY(pressDistance, 0.2F)
            .OnComplete(() => cube.transform.DOLocalMoveY(0, 0.2F));

        yield return new WaitForSeconds(stayTime/2);
        platformMat.DOColor(endColor, 0.05F);
        yield return new WaitForSeconds(stayTime/2);

        if(isOnPlatform)
            player.Jump();

        gameObject.SetActive(false);
    }

    public IEnumerator HideInitialPlatform()
    {
        isVisited = true;

        platformMat.DOColor(endColor, 0.05F);
        yield return new WaitForSeconds(0.25F);

        if (isOnPlatform)
        {
            currentPlayer.Jump();
        }

        gameObject.SetActive(false);
    }

    public void ResetPlatform()
    {
        isVisited = false;
        isOnPlatform = false;
        platformMat.color = initialColor;
        gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        isOnPlatform = false;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (!other.gameObject.CompareTag("Player"))
    //        return;

    //    if (!isVisited)
    //        StartCoroutine(HidePlatformAfterTimeout());
    //}


    //private void OnTriggerExit(Collider other)
    //{
    //    if (!other.gameObject.CompareTag("Player"))
    //        return;

    //    cube.transform.DOKill();
    //    cube.transform.DOLocalMoveY(0, 0.15F).SetDelay(0.5F);
    //}

}
