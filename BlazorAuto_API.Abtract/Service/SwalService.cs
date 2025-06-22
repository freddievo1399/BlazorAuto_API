using System;
using System.Threading.Tasks;

namespace BlazorAuto_API.Abstract
{
    public enum SwalType
    {
        Confirm,
        Info,
        Warning,
        Success,
        Error,
        Loadding,
        Close
    }

    public enum SwalResult
    {
        Confirmed,
        Dismissed,
        Canceled,
        Missing
    }

    public class SwalService
    {
        // KHÔNG nên static nếu dùng trong Blazor scoped (nhiều người dùng cùng lúc)
        public Func<string, string, SwalType, bool, Task<SwalResult>>? OnShow { get; set; }

        private async Task<SwalResult> Show(string title, string message, SwalType type, bool showCancel = false)
        {
            await EnsureReady();
            return OnShow != null
                ? await OnShow(title, message, type, showCancel)
                : SwalResult.Missing;
        }

        private async Task EnsureReady()
        {
            // Đợi tối đa 1 giây nếu OnShow chưa được gán
            int retry = 0;
            while (OnShow == null && retry++ < 10)
                await Task.Delay(100);
        }

        public Task<SwalResult> Confirm(string title, string message, bool showCancel = true)
            => Show(title, message, SwalType.Confirm, showCancel);

        public Task<SwalResult> Info(string title, string message)
            => Show(title, message, SwalType.Info);

        public Task<SwalResult> Warning(string title, string message)
            => Show(title, message, SwalType.Warning);

        public Task<SwalResult> Success(string title, string message)
            => Show(title, message, SwalType.Success);

        public Task<SwalResult> Error(string title, string message)
            => Show(title, message, SwalType.Error);

        public Task<SwalResult> Loading(string title = "Đang xử lý...", string message = "")
            => Show(title, message, SwalType.Loadding, false);

        public Task<SwalResult> Close()
            => Show("", "", SwalType.Close, false);
    }
}
