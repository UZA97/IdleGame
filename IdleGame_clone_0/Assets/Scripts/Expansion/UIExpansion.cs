using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class UIExpansion
{
    public static void SetEvent(this Button target, UnityAction call = null)
    {
        target.onClick.RemoveAllListeners();
        target.onClick.AddListener(call);
    }
}
