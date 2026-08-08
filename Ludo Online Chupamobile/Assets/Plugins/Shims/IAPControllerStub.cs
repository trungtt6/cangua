#if !UNITY_PURCHASING
using System;
using UnityEngine;

// Minimal IAPController stub so code referencing it compiles when Unity IAP is not installed.
public class IAPController : MonoBehaviour
{
    public void Purchase(string id) { Debug.Log("IAPController.Purchase stub called: " + id); }
}
#endif
