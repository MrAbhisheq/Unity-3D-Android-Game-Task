using UnityEngine;

public class BlockChecker : MonoBehaviour
{
    public bool hasBlock;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlatformBlock"))
        {
            hasBlock = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlatformBlock"))
        {
            hasBlock = false;
        }
    }
}
