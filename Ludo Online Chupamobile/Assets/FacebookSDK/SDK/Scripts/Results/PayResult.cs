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
    using System.Collections.Generic;

    internal class PayResult : ResultBase, IPayResult
    {
        internal const long CancelPaymentFlowCode = 1383010;

        internal PayResult(ResultContainer resultContainer) : base(resultContainer)
        {
            if (this.CanvasErrorCode != null && this.CanvasErrorCode.Value == PayResult.CancelPaymentFlowCode)
            {
                this.Cancelled = true;
            }
        }

        public long ErrorCode
        {
            get
            {
                return this.CanvasErrorCode.GetValueOrDefault();
            }
        }

        public override string ToString()
        {
            return Utilities.FormatToString(
                base.ToString(),
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "ErrorCode", this.ErrorCode.ToString() },
                });
        }
    }
}
