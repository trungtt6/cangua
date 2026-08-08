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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Contains the access token and related information.
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessToken"/> class.
        /// </summary>
        /// <param name="tokenString">Token string of the token.</param>
        /// <param name="userId">User identifier of the token.</param>
        /// <param name="expirationTime">Expiration time of the token.</param>
        /// <param name="permissions">Permissions of the token.</param>
        /// <param name="lastRefresh">Last Refresh time of token.</param>
        internal AccessToken(
            string tokenString,
            string userId,
            DateTime expirationTime,
            IEnumerable<string> permissions,
            DateTime? lastRefresh)
        {
            if (string.IsNullOrEmpty(tokenString))
            {
                throw new ArgumentNullException("tokenString");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            if (expirationTime == DateTime.MinValue)
            {
                throw new ArgumentException("Expiration time is unassigned");
            }

            if (permissions == null)
            {
                throw new ArgumentNullException("permissions");
            }

            this.TokenString = tokenString;
            this.ExpirationTime = expirationTime;
            this.Permissions = permissions;
            this.UserId = userId;
            this.LastRefresh = lastRefresh;
        }

        /// <summary>
        /// Gets the current access token.
        /// </summary>
        /// <value>The current access token.</value>
        public static AccessToken CurrentAccessToken { get; internal set; }

        /// <summary>
        /// Gets the token string.
        /// </summary>
        /// <value>The token string.</value>
        public string TokenString { get; private set; }

        /// <summary>
        /// Gets the expiration time.
        /// </summary>
        /// <value>The expiration time.</value>
        public DateTime ExpirationTime { get; private set; }

        /// <summary>
        /// Gets the list of permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public IEnumerable<string> Permissions { get; private set; }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserId { get; private set; }

        /// <summary>
        /// Gets the last refresh.
        /// </summary>
        /// <value>The last refresh.</value>
        public DateTime? LastRefresh { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Facebook.Unity.AccessToken"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Facebook.Unity.AccessToken"/>.</returns>
        public override string ToString()
        {
            return Utilities.FormatToString(
                null,
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "ExpirationTime", this.ExpirationTime.TotalSeconds().ToString() },
                    { "Permissions", this.Permissions.ToCommaSeparateList() },
                    { "UserId", this.UserId.ToStringNullOk() },
                    { "LastRefresh", this.LastRefresh.ToStringNullOk() },
                });
        }

        internal string ToJson()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary[LoginResult.PermissionsKey] = string.Join(",", this.Permissions.ToArray());
            dictionary[LoginResult.ExpirationTimestampKey] = this.ExpirationTime.TotalSeconds().ToString();
            dictionary[LoginResult.AccessTokenKey] = this.TokenString;
            dictionary[LoginResult.UserIdKey] = this.UserId;
            if (this.LastRefresh != null)
            {
                dictionary[LoginResult.LastRefreshKey] = this.LastRefresh.Value.TotalSeconds().ToString();
            }

            return MiniJSON.Json.Serialize(dictionary);
        }
    }
}
