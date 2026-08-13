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

    // Commented out to prevent conflict with Stores.dll
    /*
    public class StandardPurchasingModule {
        public static StandardPurchasingModule Instance() { return new StandardPurchasingModule(); }
    }
    */

    public class ConfigurationBuilder {
        // Updated to not use the excluded StandardPurchasingModule directly
        public static ConfigurationBuilder Instance(object module) { return new ConfigurationBuilder(); }
        public void AddProduct(string id, ProductType type) { }
    }

    public static class UnityPurchasing {
        public static void Initialize(IStoreListener listener, ConfigurationBuilder builder) { }
    }
}
