# Pickleball Club Management System (CPM)

Hệ thống quản lý câu lạc bộ Pickleball, bao gồm quản lý thành viên, đặt sân, tài chính, tin tức và các giải đấu/kèo đấu.

## 🚀 Tính năng Chính

### 1. Quản lý Tin tức (News Management)

- **Người dùng**: Xem danh sách tin tức.
- **Admin**: Thêm, Sửa, Xóa tin tức.

### 2. Quản lý Đặt sân (Booking Management)

- **Hội viên**: Đặt sân, xem lịch sử đặt sân của mình.
- **Admin**:
  - Xem toàn bộ lịch đặt sân.
  - Đặt sân hộ hội viên.
  - Duyệt (Confirm) hoặc Hủy (Cancel) yêu cầu đặt sân.
  - Chỉnh sửa thông tin, xóa đặt sân.

### 3. Sàn đấu Kèo & Giải (Challenges)

- **Hội viên**:
  - Xem danh sách các kèo đấu (Open, Ongoing, Finished).
  - Tham gia kèo đấu (trạng thái Open).
- **Admin**:
  - Tạo kèo mới (Giải đấu, Kèo giao lưu...).
  - Duyệt kèo (nếu thành viên tạo - tính năng mở rộng).
  - Bắt đầu kèo (chuyển sang Ongoing).
  - Kết thúc kèo.

### 4. Quản lý Kết quả Trận đấu (Matches)

- **Referee/Admin**:
  - Ghi nhận kết quả trận đấu (Singles/Doubles).
  - Chọn kèo đấu liên quan (để tính điểm cho kèo).
  - Chọn người thắng cuộc.
- **Hệ thống**:
  - Tự động cập nhật Rank (trình độ) cho người chơi nếu trận đấu có tính điểm (Ranked).
  - Tự động thống kê số trận thắng/thua.

### 5. Quản lý Tài chính (Treasury) - Admin Only

- Theo dõi tổng quỹ CLB.
- Ghi nhận Thu/Chi (Income/Expense).
- Quản lý danh mục thu chi.
- Cảnh báo khi quỹ âm.

---

## 🛠️ Cài đặt & Chạy ứng dụng

### Yêu cầu hệ thống

- .NET 8.0 SDK trở lên.
- SQL Server (hoặc LocalDB).

### Các bước cài đặt

1. **Clone Repository**

   ```bash
   git clone https://github.com/tyhnhzi/KT2-FIT4016.git
   cd KT2-FIT4016
   ```

2. **Cấu hình Database**
   - Mở file `appsettings.json`.
   - Cập nhật chuỗi kết nối `DefaultConnection` phù hợp với SQL Server của bạn.

3. **Cập nhật Database (Migrations)**

   ```bash
   cd PickleballClub
   dotnet ef database update
   ```

4. **Chạy ứng dụng**
   ```bash
   dotnet run
   ```
   Truy cập: `https://localhost:7xxx` (theo cấu hình launchSettings).

---

## 🔑 Tài khoản Mặc định (Seed Data)

Khi chạy lần đầu, hệ thống có thể tạo các tài khoản mẫu (nếu đã cấu hình trong `DbInitializer`):

- **Admin**: `admin@pcm.com` / `Admin@123!`
- **Member**: `member@pcm.com` / `Member@123!`

---

## 📂 Cấu trúc Dự án

- **Pages/**: Các trang Razor Pages (News, Bookings, Challenges...).
- **Models/**: Các thực thể dữ liệu (Booking, Match, Member...).
- **Data/**: DbContext và Migrations.
- **Services/**: Các dịch vụ xử lý logic (nếu có).

---

## 📝 Lưu ý

- Kiểm tra `appsettings.json` để đảm bảo kết nối CSDL đúng.
- Sử dụng tài khoản Admin để truy cập đầy đủ các tính năng quản lý (nút Edit/Delete sẽ hiện ra).

---

**FIT4016 - Bài Kiểm Tra 2**
