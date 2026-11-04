# Bài 1 - Tổng quan hệ thống và MEP
Họ tên: Hoàng Xuân Đoàn    MSSV: dtc245200226    Lớp: K23E

## 1. Cài đặt
- Đã cài đặt thành công .NET SDK và Git trên môi trường Windows.
- Kiểm tra phiên bản trên terminal:
  - `dotnet --version` -> `10.0.403`
  - `git --version` -> `2.56.0.windows.1`

## 2. MepDemo (Bước 7)
Chạy thử nghiệm thành công 5 mô hình trao đổi thông điệp (MEP) qua console:
- **1. Request-Response (`dotnet run -- 1`)**: Gửi yêu cầu chấm bài cho `SV001` và nhận phản hồi `8.5 điểm`; mô phỏng timeout khi quá thời gian chờ 1 giây ở `SV002`.
- **2. One-way (`dotnet run -- 2`)**: Gửi log đi liên tục (`log #1`, `#2`, `#3`) mà không chờ kết quả nghiệp vụ; `AuditLog` tiếp nhận và ghi nhận danh sách đăng nhập.
- **3. Queue-based (`dotnet run -- 3`)**: Đẩy các job vào hàng đợi (`sub-1` đến `sub-4`), các worker (`Worker-A`, `Worker-B`) lần lượt lấy job ra xử lý bất đồng bộ song song.
- **4. Publish-Subscribe (`dotnet run -- 4`)**: Khi worker chấm xong, phát sự kiện `SubmissionGraded` tới nhiều bên đăng ký đồng thời (`ScoreService`, `NotificationService`, `StatsService`).
- **5. Streaming / Push (`dotnet run -- 5`)**: Nhận cập nhật tiến trình liên tục từ trạng thái `received` đến `processing 30%`, `70%` và kết quả hoàn thành `graded: 8.5`.

## 3. BasicWebApi (Bước 8)
- Build và chạy thành công dự án `BasicWebApi` trên cổng `http://localhost:5081`.
- Kiểm tra các HTTP request trả về mã trạng thái chuẩn:
  - `200 OK`: Lấy danh sách dữ liệu thành công.
  - `201 Created`: Tạo mới dữ liệu thành công.
  - `400 Bad Request`: Xử lý lỗi dữ liệu không hợp lệ từ client.

## 4. MiniDistributedSystem (Bước 9)
- Chạy đồng thời hệ thống phân tán nhỏ gồm 2 tiến trình độc lập trên HTTP:
  - **`Submission.Api`**: Lắng nghe tại cổng `http://localhost:5000`, xử lý các endpoint `/api/assignments` và `/api/submissions`.
  - **`Submission.Client`**: Gọi HTTP request tới API để lấy danh sách bài tập và gửi bài nộp với mã sinh viên `dtc245200226`, nhận về mã định danh Guid và trạng thái `received`.

## 5. Thí nghiệm lỗi (Bước 10)
| # | Dự đoán | Thực tế | Đúng/Sai | Giải thích |
|---|---|---|---|---|
| 1 | Lỗi kết nối khi API chưa bật | Bắt được ngoại lệ `HttpRequestException` ở Client | Đúng | Client không thể thiết lập kết nối TCP tới cổng API đang tắt |

## 6. Dự án EduSubmit tự làm (Bước 11-12)
- Khởi tạo solution `EduSubmit` với MSSV `dtc245200226`, cấu trúc gồm `Submission.Api` và `Submission.Client`.
- **Luồng thông điệp:**
  - `Client --(HTTP GET /api/assignments)--> Api` : Kiểu `request-response` (gửi yêu cầu lấy danh sách bài tập và nhận về mảng dữ liệu JSON).
  - `Client --(HTTP POST /api/submissions)--> Api` : Kiểu `request-response` (gửi thông tin nộp bài chứa mã sinh viên và nhận về mã định danh `Guid` cùng trạng thái xử lý).

## 7. Khó khăn gặp phải và cách giải quyết
- **Khó khăn:** Ban đầu gặp lỗi phân quyền khi tạo thư mục dự án trên ổ D (`Access is denied`) và lỗi đường dẫn remote git khi cấu hình sai tên tài khoản.
- **Cách giải quyết:** Chuyển thư mục làm việc sang phân vùng cá nhân (`C:\Users\Admin\Code_demo`) và cấu hình lại `git remote set-url origin` trúng repository chính xác trên GitHub.