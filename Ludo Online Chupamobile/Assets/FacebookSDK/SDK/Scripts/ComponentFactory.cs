/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace Facebook.Unity
{
    using UnityEngine;

    internal class ComponentFactory
    {
        public const string GameObjectName = "UnityFacebookSDKPlugin";

        private static GameObject facebookGameObject;

        internal enum IfNotExist
        {
            AddNew,
            ReturnNull
        }

        private static GameObject FacebookGameObject
        {
            get
            {
                if (facebookGameObject == null)
                {
                    facebookGameObject = new GameObject(GameObjectName);
                }

                return facebookGameObject;
            }
        }

        /**
         * Gets one and only one component.  Lazy creates one if it doesn't exist
         */
        public static T GetComponent<T>(IfNotExist ifNotExist = IfNotExist.AddNew) where T : MonoBehaviour
        {
            var facebookGameObject = FacebookGameObject;

            T component = facebookGameObject.GetComponent<T>();
            if (component == null && ifNotExist == IfNotExist.AddNew)
            {
                component = facebookGameObject.AddComponent<T>();
            }

            return component;
        }

        /**
         * Creates a new component on the Facebook object regardless if there is already one
         */
        public static T AddComponent<T>() where T : MonoBehaviour
        {
            return FacebookGameObject.AddComponent<T>();
        }
    }
}
