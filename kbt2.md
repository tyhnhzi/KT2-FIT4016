BÀI KIỂM TRA 02
Môn học: Lập trình Web với ASP.NET Core (Razor Pages)

Công nghệ: ASP.NET Core Razor Pages, Entity Framework Core, Identity, SQL Server.

TÊN ĐỀ TÀI: HỆ THỐNG QUẢN LÝ CLB PICKLEBALL "VỢT THỦ PHỐ NÚI" (PCM)
I. MÔ TẢ BÀI TOÁN (CHI TIẾT NGHIỆP VỤ)

1. Bối cảnh & Văn hóa CLB

CLB "Vợt Thủ Phố Núi" hoạt động dựa trên tinh thần "Vui - Khỏe - Có Thưởng". Hệ thống PCM
cần quản lý sâu sát các hoạt động đặc thù sau:

Văn hóa "Kèo" (Challenges):
o Đây là lời thách đấu hoặc lời rủ rê được tạo ra trước khi thi đấu.
o Kèo Thách đấu (Duel): Thường là 1vs1 hoặc 2vs2. Phần thưởng là hiện vật
mang tính vui vẻ do người chơi tự thỏa thuận (Ví dụ: "Kèo 20 quả trứng gà ta" ,
"Kèo 1 block nước Revive" , "Kèo bao tiền sân" ).
o Kèo Giải đấu nhỏ (Mini-game): Dạng "King of the Court" hoặc "Đánh xoay
vòng" cho nhiều người. Mỗi người tham gia tự đóng một khoản phí nhỏ (VD:
20k/người). Tổng số tiền thu được (Total Pot) sẽ dùng để mua giải thưởng lớn
(Ví dụ: "Góp tiền làm nồi lẩu" , "Mua thùng bia" ).
Trận đấu thực tế (Matches):
o Là kết quả diễn ra trên sân. Hệ thống phải ghi nhận chính xác: Đây là trận Đơn
hay Đôi? Ai cặp với ai? Cặp nào thắng cặp nào?
o Kết quả trận đấu ảnh hưởng trực tiếp đến điểm trình độ (Rank DUPR).
Tài chính minh bạch:
o Quản lý các khoản thu chi phức tạp (Tiền sân, bóng, nước, quỹ tháng...). Danh
mục thu chi cần linh hoạt để Admin có thể thêm mới khi có phát sinh (Ví dụ:
thêm mục "Tiền phạt đi trễ" ).
II. THIẾT KẾ CƠ SỞ DỮ LIỆU (DATABASE SCHEMA)
Sinh viên thực hiện Code First với các bảng sau ( Lưu ý: Tên bảng nghiệp vụ phải bắt đầu bằng
3 số cuối MSSV ).

1. Nhóm Quản trị & Hội viên

- [xxx]\_Members:
  o Id, IdentityUserId (FK).
  o FullName, DOB (Ngày sinh), PhoneNumber, JoinDate.
  o RankLevel (Double): Mặc định 1.0.
  o Status: (Active/Inactive).

2. Nhóm Tài chính (Treasury) - Nâng cấp

- [xxx]\_TransactionCategories: (Danh mục động - Cho phép Admin thêm/sửa)
  o Id, CategoryName (VD: Tiền sân, Mua bóng, Quỹ tháng, Tài trợ...).
  o Type: (Thu hoặc Chi).
- [xxx]\_Transactions:
  o Id, TransactionDate, Amount (Số tiền), Description.
  o CategoryId (FK): Liên kết tới bảng Category ở trên.

3. Nhóm Thi đấu (Sports Core) - Quan trọng

- [xxx]\_Challenges (Kèo đấu):
  o Id, CreatorId (FK), Title, Description.
  o Mode: Enum (0: Singles, 1: Doubles, 2: MiniGame).
  o RewardDescription: String (Mô tả phần thưởng: "20 trứng", "Nồi lẩu").
  o EntryFee: Decimal (Lệ phí tham gia cho Mini-game, mặc định 0).
  o Status: (Open - Đang nhận / Closed - Đã chốt / Completed - Đã trao giải).
- [xxx]\_Participants: (Ai đã tham gia kèo?)
  o Id, ChallengeId (FK), MemberId (FK).
- [xxx]\_Matches (Lịch sử đấu):
  o Id, MatchDate.

o ChallengeId (FK - Nullable): Trận này thuộc kèo nào?
o Format: Enum (Singles - Đơn / Doubles - Đôi).
o IsRanked (Bool): Có tính điểm Rank không.
o Thông tin đấu thủ (Cụ thể):
▪ Winner1Id (FK): Người thắng 1 (Bắt buộc).
▪ Winner2Id (FK): Người thắng 2 (Chỉ có nếu đánh Đôi).
▪ Loser1Id (FK): Người thua 1 (Bắt buộc).
▪ Loser2Id (FK): Người thua 2 (Chỉ có nếu đánh Đôi). 4. Tiện ích khác

- [xxx]\_News: Id, Title, Content, PostedDate.
- [xxx]\_Bookings: Id, MemberId, StartTime, EndTime.

III. CÁC PHÂN HỆ CHỨC NĂNG (MODULES)
Module 1: Quản lý Hội viên (Membership)

Admin: Quản lý danh sách, xem chi tiết hồ sơ.
Logic hiển thị:
o Rank > 5.0: Hiển thị badge vàng [PRO].
o Rank < 2.0: Hiển thị badge xanh [Newbie].
o Thông tin hiển thị phải đầy đủ: Họ tên, Ngày tham gia, Trình độ hiện tại.
Module 2: Quản lý Tài chính Động (Dynamic Treasury)

Quản lý Danh mục (Category CRUD): Admin có quyền thêm/sửa/xóa các loại danh
mục thu chi (Ví dụ: Thêm danh mục mới "Tiền phạt văng tục" ).
Quản lý Giao dịch:
o Ghi nhận Thu/Chi dựa trên danh mục đã tạo.
o Hiển thị Tổng Quỹ Hiện Tại.
o Cảnh báo: Nếu Quỹ < 0 → Hiện thông báo đỏ rực: "CẢNH BÁO: QUỸ ĐANG ÂM!
Cần bổ sung nguồn tiền ngay."
Module 3: Sàn đấu Kèo & Giải trí (Challenges)

Tạo Kèo (Member):
o Nhập: Tiêu đề, Chế độ (Đơn/Đôi/Mini-game).
o Nhập Phần thưởng (Reward) : Cho phép nhập text tự do (VD: "Chầu cafe", "
quả trứng").
o Nếu là Mini-game: Nhập Lệ phí (EntryFee) (VD: 20.000đ).
Hưởng ứng (Member): Nút "Tham gia".
Logic Tính thưởng Mini-game:
o Hệ thống tự động tính: Tổng giải thưởng (Pot) = Số người tham gia \* EntryFee.
o Hiển thị tại trang chi tiết để khích lệ hội viên (VD: "Quỹ thưởng hiện tại:
500.000đ - Đủ làm nồi lẩu rồi!" ).
Module 4: Trọng tài & Ghi nhận Trận đấu (Referee & Matches)

Chức năng: Chỉ Admin mới được nhập kết quả.
Quy trình nhập liệu (Form Create Match):
Chọn Kèo (hoặc để trống nếu đánh giao hữu).
Chọn Thể thức (Đơn/Đôi).
Chọn Người chơi (Rất quan trọng):
▪ Nếu chọn Đơn: Dropdown chọn Winner 1 vs Dropdown chọn Loser 1.
▪ Nếu chọn Đôi: Hệ thống hiển thị 4 Dropdown (Winner 1, Winner 2 vs
Loser 1, Loser 2).
▪ Validate: Đảm bảo 1 người không thể xuất hiện 2 lần trong cùng 1 trận.
Checkbox IsRanked (Tính điểm).
Xử lý sau khi lưu:
o Nếu là trận Ranked: Winner (+0.1), Loser (-0.1).
o Nếu thuộc Kèo Mini-game: Hệ thống tự động đóng Kèo và cập nhật người thắng
cuộc nhận giải.
Module 5: Tin tức & Thông báo

Admin đăng các thông báo về lịch hoạt động, đóng quỹ, hoặc bài viết vinh danh các
tay vợt lên Rank.
Hiển thị danh sách tin tức mới nhất ở Trang chủ.
IV. YÊU CẦU GIAO DIỆN & TRẢI NGHIỆM (UI/UX)
Sử dụng Razor Pages kết hợp Bootstrap.
Dashboard: Hiển thị các chỉ số quan trọng (Số dư quỹ, Số kèo đang mở, BXH Top 5).
Sàn đấu: Thiết kế dạng thẻ (Card), làm nổi bật Phần thưởng và Số tiền thưởng Mini-
game.
V. ĐIỂM CỘNG (BONUS FEATURES)
Báo cáo tài chính (Export): Xuất lịch sử Thu/Chi ra file Excel hoặc PDF để báo cáo
cuối tháng.
Top Ranking: Widget hiển thị Top 5 cao thủ có Rank cao nhất bên phải trang chủ.
Validate Tuổi: Khi nhập DOB (Ngày sinh), hệ thống tự tính tuổi. Nếu < 10 tuổi hoặc >
80 tuổi thì hiện cảnh báo vui: "Cẩn thận chấn thương nhé!".
VI. QUY ĐỊNH NỘP BÀI
Cách thức nộp bài:

Bước 1: Đẩy toàn bộ Source Code lên Github (Chế độ Public). Đảm bảo file .gitignore
hoạt động tốt (không upload thư mục bin, obj).
Bước 2: Copy đường link Repository vừa tạo.
Bước 3: Gửi link vào nhóm Zalo môn học với cú pháp tin nhắn:
NỘP BÀI THI: [MSSV] - [HỌ VÀ TÊN] - [LINK GITHUB]
