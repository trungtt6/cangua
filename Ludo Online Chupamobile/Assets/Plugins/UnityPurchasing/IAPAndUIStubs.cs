using System;
using UnityEngine;

namespace UnityEngine.Purchasing {
    public enum PurchaseProcessingResult { Complete = 0, Pending = 1 }
    public enum PurchaseFailureReason { Unknown = 0 }
    public enum InitializationFailureReason { Unknown = 0 }

    public enum ProductType { Consumable, NonConsumable, Subscription }

    public class ProductDefinition {
        public string id;
        public ProductDefinition(string id) { this.id = id; }
    }

    public class Product {
        public ProductDefinition definition;
        public Product(ProductDefinition def) { definition = def; }
    }

    public class PurchaseEventArgs {
        public Product purchasedProduct;
        public PurchaseEventArgs(Product p) { purchasedProduct = p; }
    }

    public interface IStoreListener {
        void OnInitialized(IStoreController controller, IExtensionProvider extensions);
        void OnInitializeFailed(InitializationFailureReason error);
        PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e);
        void OnPurchaseFailed(Product product, PurchaseFailureReason reason);
    }

    public interface IStoreController {
        void InitiatePurchase(string productId);
        void InitiatePurchase(Product product);
    }

    public interface IExtensionProvider { }

    public class StandardPurchasingModule {
        public static StandardPurchasingModule Instance() { return new StandardPurchasingModule(); }
    }

    public class ConfigurationBuilder {
        public static ConfigurationBuilder Instance(StandardPurchasingModule module) { return new ConfigurationBuilder(); }
        public void AddProduct(string id, ProductType type) { }
    }

    public static class UnityPurchasing {
        public static void Initialize(IStoreListener listener, ConfigurationBuilder builder) { }
    }
}

// Minimal Unity Ads stubs (used by project scripts)
namespace UnityEngine.Advertisements {
    public enum ShowResult { Failed = 0, Skipped = 1, Finished = 2 }

    public class ShowOptions {
        public Action<ShowResult> resultCallback;
    }

    public static class Advertisement {
        public static bool IsReady(string placementId = null) { return false; }
        public static void Show() { }
        public static void Show(string placementId) { }
        public static void Show(string placementId, ShowOptions options) {
            options?.resultCallback?.Invoke(ShowResult.Finished);
        }
    }
}

namespace UnityEngine.UI {
    public class Selectable : MonoBehaviour { }
    public class Button : Selectable { }
    public class Dropdown : Selectable { }
    public class Text : MonoBehaviour { }
}
