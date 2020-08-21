using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOverShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ShowandHideThisText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowandHideThisText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShowandHideThisText.SetActive(false);
    }
}

