using System.Collections.Generic;
using UnityEngine;


public class HiddenManager : MonoBehaviour
{
    public static HiddenManager Instance { get; private set; }

    [Header("Hidden Elements")]
    [SerializeField] GameObject hiddenContainer;

    private HiddenController[] hiddenElements;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        hiddenElements = hiddenContainer.GetComponentsInChildren<HiddenController>(true);
    }

    public void HideAll()
    {
        foreach (HiddenController hidden in hiddenElements)
        {
            hidden.HideTruth();
        }
    }

    public void RevealAll()
    {
        foreach (HiddenController hidden in hiddenElements)
        {
            if (hidden != null)
            {
                hidden.RevealTruth();
            }
        }
    }
}
