using UnityEngine;

// Disabled to avoid demo compile requirements.
public static class IAPConfigurationHelper { }
/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// Disabled to avoid compile errors when Unity Purchasing package isn't available
#if false
#if UNITY_PURCHASING
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace UnityEngine.Purchasing
{
    public static class IAPConfigurationHelper {
        /// Populate a ConfigurationBuilder with products from a ProductCatalog
        public static void PopulateConfigurationBuilder(ref ConfigurationBuilder builder, ProductCatalog catalog)
        {
            foreach (var product in catalog.allProducts) {
                IDs ids = null;

                if (product.allStoreIDs.Count > 0) {
                    ids = new IDs();
                    foreach (var storeID in product.allStoreIDs) {
                        ids.Add(storeID.id, storeID.store);
                    }
                }

                #if UNITY_2017_2_OR_LATER

                var payoutDefinitions = new List<PayoutDefinition>();
                foreach (var payout in product.Payouts) {
                    payoutDefinitions.Add(new PayoutDefinition(payout.typeString, payout.subtype, payout.quantity, payout.data));
                }
                builder.AddProduct(product.id, product.type, ids, payoutDefinitions.ToArray());

                #else

                builder.AddProduct(product.id, product.type, ids);

                #endif
            }
        }
    }
}
#endif
#endif
