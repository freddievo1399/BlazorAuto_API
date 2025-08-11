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
            int retry = 0;
            while (OnShow == null && retry++ < 10)
                await Task.Delay(100);
        }

        public Task<SwalResult> Confirm(string message, string title = "Xác nhận", bool showCancel = true)
            => Show(title, message, SwalType.Confirm, showCancel);

        public Task<SwalResult> Info(string message, string title = "Thông báo")
            => Show(title, message, SwalType.Info);

        public Task<SwalResult> Warning(string message, string title = "Cảnh báo")
            => Show(title, message, SwalType.Warning);

        public Task<SwalResult> Success(string message, string title = "Thành công")
            => Show(title, message, SwalType.Success);

        public Task<SwalResult> Error(string message, string title = "Báo lỗi")
            => Show(title, message, SwalType.Error);

        public Task<SwalResult> Loading(string message = "", string title = "Đang xử lý...")
            => Show(title, message, SwalType.Loadding, false);

        public Task<SwalResult> Close()
            => Show("", "", SwalType.Close, false);
    }
}
