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

    internal abstract class ResultBase : IInternalResult
    {
        internal const long CancelDialogCode = 4201;
        internal const string ErrorCodeKey = "error_code";
        internal const string ErrorMessageKey = "error_message";

        internal ResultBase(ResultContainer result)
        {
            string error = ResultBase.GetErrorValue(result.ResultDictionary);
            bool cancelled = ResultBase.GetCancelledValue(result.ResultDictionary);
            string callbackId = ResultBase.GetCallbackId(result.ResultDictionary);

            this.Init(result, error, cancelled, callbackId);
        }

        internal ResultBase(ResultContainer result, string error, bool cancelled)
        {
            this.Init(result, error, cancelled, null);
        }

        public virtual string Error { get; protected set; }

        public virtual IDictionary<string, object> ResultDictionary { get; protected set; }

        public virtual string RawResult { get; protected set; }

        public virtual bool Cancelled { get; protected set; }

        public virtual string CallbackId { get; protected set; }

        protected long? CanvasErrorCode { get; private set; }

        public override string ToString()
        {
            return Utilities.FormatToString(
                base.ToString(),
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "Error", this.Error },
                    { "RawResult", this.RawResult },
                    { "Cancelled", this.Cancelled.ToString() },
                });
        }

        protected void Init(ResultContainer result, string error, bool cancelled, string callbackId)
        {
            this.RawResult = result.RawResult;
            this.ResultDictionary = result.ResultDictionary;
            this.Cancelled = cancelled;
            this.Error = error;
            this.CallbackId = callbackId;

            if (this.ResultDictionary != null)
            {
                long errorCode;
                if (this.ResultDictionary.TryGetValue(ResultBase.ErrorCodeKey, out errorCode))
                {
                    this.CanvasErrorCode = errorCode;
                    if (errorCode == ResultBase.CancelDialogCode)
                    {
                        this.Cancelled = true;
                    }
                }

                string errorMessage;
                if (this.ResultDictionary.TryGetValue(ResultBase.ErrorMessageKey, out errorMessage))
                {
                    this.Error = errorMessage;
                }
            }
        }

        private static string GetErrorValue(IDictionary<string, object> result)
        {
            if (result == null)
            {
                return null;
            }

            string error;
            if (result.TryGetValue<string>("error", out error))
            {
                return error;
            }

            return null;
        }

        private static bool GetCancelledValue(IDictionary<string, object> result)
        {
            if (result == null)
            {
                return false;
            }

            // Check for cancel string
            object cancelled;
            if (result.TryGetValue("cancelled", out cancelled))
            {
                bool? cancelBool = cancelled as bool?;
                if (cancelBool != null)
                {
                    return cancelBool.HasValue && cancelBool.Value;
                }

                string cancelString = cancelled as string;
                if (cancelString != null)
                {
                    return Convert.ToBoolean(cancelString);
                }

                int? cancelInt = cancelled as int?;
                if (cancelInt != null)
                {
                    return cancelInt.HasValue && cancelInt.Value != 0;
                }
            }

            return false;
        }

        private static string GetCallbackId(IDictionary<string, object> result)
        {
            if (result == null)
            {
                return null;
            }

            // Check for cancel string
            string callbackId;
            if (result.TryGetValue<string>(Constants.CallbackIdKey, out callbackId))
            {
                return callbackId;
            }

            return null;
        }
    }
}
