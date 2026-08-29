using UnityEngine;

public class RevealButton : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HiddenManager.Instance.RevealAll();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HiddenManager.Instance.HideAll();
        }
    }
}
