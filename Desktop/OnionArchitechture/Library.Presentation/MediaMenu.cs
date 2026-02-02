using Library.Service;
using Library.Core.Domain; // Cần cái này để dùng List<MediaFile>
using System.Text;
using System.Diagnostics;

namespace Library.Presentation;

public class MediaMenu
{
    private readonly MediaPlayerService _service;

    public MediaMenu(MediaPlayerService service)
    {
        _service = service;
    }


    private void OpenBrowser(string url)
    {
        try
        {
            // Kiểm tra nếu url rỗng thì thôi
            if (string.IsNullOrWhiteSpace(url)) return;

            // Lệnh này ép Windows dùng trình duyệt mặc định để mở link
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LOI] Khong the mo trinh duyet: {ex.Message}");
        }
    }
    public async Task RunAsync()
    {
        Console.OutputEncoding = Encoding.UTF8;
        while (true)
        {
            Console.Clear(); 
            Console.WriteLine("\n===  MEDIA MANAGER ===");

            // Hiển thị danh sách ngay trang chủ luôn cho dễ nhìn
            var currentList = await ShowListAndGetMapping();

            Console.WriteLine("---------------------------");
            Console.WriteLine("1. Thêm Video");
            Console.WriteLine("2. Đổi tên Video ");
            Console.WriteLine("3. Phát Video ");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn chức năng: ");

            var key = Console.ReadLine();
            switch (key)
            {
                case "1": await AddFlow(); break;
                case "2": await UpdateFlow(currentList); break;
                case "3":
                    await PlayFlowCorrected(currentList); // Gọi hàm tách riêng cho gọn
                    break;
                case "0": return;
            }
        }
    }

    private async Task<List<MediaFile>> ShowListAndGetMapping()
    {
        var list = (await _service.GetLibraryAsync()).ToList(); 

        if (list.Count == 0)
        {
            Console.WriteLine(" (Danh sách trống)");
        }
        else
        {
            Console.WriteLine("DANH SÁCH VIDEO:");
            for (int i = 0; i < list.Count; i++)
            {
                // In ra: 1. Video Nhạc Trẻ (Đang phát)
                var status = list[i].IsPlaying ? "[ĐANG PHÁT...]" : "";
                Console.WriteLine($"  {i + 1}. {list[i].Title} {status}");
            }
        }
        return list;
    }

    private async Task AddFlow()
    {
        Console.WriteLine("\n--- THÊM VIDEO ---");
        Console.Write("Tên video: "); var title = Console.ReadLine() ?? "";
        Console.Write("Đường dẫn: "); var path = Console.ReadLine() ?? "";
        await _service.AddVideoAsync(title, path);
        Console.WriteLine("=> Thêm xong! Bấm Enter để quay lại...");
        Console.ReadLine();
    }

    private async Task UpdateFlow(List<MediaFile> list)
    {
        if (list.Count == 0) return;

        Console.Write("\nNhập Số Thứ Tự (STT) muốn sửa: ");
        if (int.TryParse(Console.ReadLine(), out int stt) && stt > 0 && stt <= list.Count)
        {
            
            var selectedItem = list[stt - 1];

            Console.Write($"Nhập tên mới cho '{selectedItem.Title}': ");
            var newTitle = Console.ReadLine() ?? "";

           
            await _service.ChangeVideoTitleAsync(selectedItem.Id, newTitle);
            Console.WriteLine("=> Đã sửa!");
        }
        else
        {
            Console.WriteLine("=> Số thứ tự không hợp lệ!");
        }
        Console.ReadLine();
    }

    private async Task PlayFlowCorrected(List<MediaFile> list)
    {
        if (list.Count == 0)
        {
            Console.WriteLine("Danh sách trống, không có gì để phát!");
            Console.ReadLine();
            return;
        }

        Console.Write("\nNhập Số Thứ Tự (STT) muốn phát: ");
        // Kiểm tra xem người dùng nhập có đúng số không
        if (int.TryParse(Console.ReadLine(), out int stt) && stt > 0 && stt <= list.Count)
        {
            // 1. Lấy video từ danh sách dựa trên số thứ tự nhập vào
            var video = list[stt - 1];

            Console.WriteLine($"\n[INFO] Đang mở trình duyệt: {video.Title}...");
            Console.WriteLine($"Link: {video.FilePath}");

            // 2. Mở trình duyệt (Hàm này bạn đã viết đúng rồi)
            OpenBrowser(video.FilePath);

            // 3. Gọi Service để lưu trạng thái "Đang phát" vào Database
            // (Service chỉ lo việc Database, còn UI lo việc mở trình duyệt)
            await _service.PlayAsync(video.Id);

            Console.WriteLine("=> Đã cập nhật trạng thái đang phát!");
        }
        else
        {
            Console.WriteLine("=> Số thứ tự không hợp lệ!");
        }

        Console.WriteLine("Bấm Enter để quay lại...");
        Console.ReadLine();
    }
}
