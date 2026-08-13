/* Disabled for headless CI: renamed to .disabled to exclude from compilation */
// Original file moved to .disabled by automation to avoid UnityPurchasing demo compile errors.

#if UNITY_EDITOR || DEVELOPMENT_BUILD
#if !UNITY_PURCHASING
#warning "Unity IAP plugin is installed, but Unity IAP is not enabled. Please enable Unity IAP in the Services window."
#endif
public static class PurchasingCheck { public static bool IsIAPEnabled() { return false; } }
#else
public static class PurchasingCheck { public static bool IsIAPEnabled() { return false; } }
#endif
