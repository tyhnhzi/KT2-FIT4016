BÀI KIỂM TRA 02
Môn học: Lập trình Web với ASP.NET Core (Razor Pages)
Công nghệ: ASP.NET Core Razor Pages, Entity Framework Core, Identity, SQL Server.

TÊN ĐỀ TÀI: HỆ THỐNG QUẢN LÝ CLB PICKLEBALL "VỢT THỦ PHỐ NÚI"
(PCM)
PHẦN 1: MÔ TẢ BÀI TOÁN & NGHIỆP VỤ (FULL SCENARIO)
CLB "Vợt Thủ Phố Núi" hoạt động dựa trên tinh thần "Vui - Khỏe - Có Thưởng". Hệ thống PCM cần quản lý
sâu sát các hoạt động đặc thù sau:

1. Quản trị Nội bộ (Operations) - "Xương sống của CLB"
   Quản lý Hội viên:
   Mỗi thành viên có một Hồ sơ số (Digital Profile) gồm thông tin cá nhân và Rank DUPR (Trình độ)
   biến động theo kết quả thi đấu.
   Mỗi hội viên phải có tài khoản Identity để đăng nhập hệ thống. Thông tin trong bảng Members
   (Email, PhoneNumber) có thể lấy từ Identity User hoặc lưu riêng để bổ sung.
   Quản lý Tài chính (Treasury):
   Quản lý dòng tiền Thu/Chi minh bạch. Mỗi giao dịch phải có danh mục (Category) để phân loại.
   Admin có thể thêm mới các Danh mục Thu/Chi (Ví dụ: Thêm mục "Tiền sửa đèn", "Tiền tất niên").
   Hệ thống phải Cảnh báo đỏ ngay lập tức nếu Quỹ bị âm (tổng Thu - tổng Chi < 0).
   Tin tức & Thông báo: Đăng tải thông báo đóng quỹ, lịch nghỉ, vinh danh thành viên... Tin có thể được
   ghim (IsPinned) để hiển thị ưu tiên.
2. Hoạt động Thường nhật (Daily Activities)
   Đặt sân (Booking):
   Thành viên xem lịch theo từng sân (Court), chọn khung giờ (StartTime - EndTime) và đặt sân.
   Hệ thống chặn trùng lịch trên cùng sân: Không cho phép hai booking có thời gian giao nhau
   trên cùng một Court.
   Ví dụ: Nếu Sân 1 đã có booking từ 8:00-9:00, thì không thể đặt Sân 1 từ 8:30-9:30 (trùng lịch).
   Booking cũng có thể mang ý nghĩa là lời hẹn nhau xuống thi đấu (khung giờ) để cho mọi người
   biết.
   Trận đấu Giao hữu (Daily Matches):
   Đây là hoạt động diễn ra nhiều nhất. Hội viên ra sân đánh Đơn (1vs1) hoặc Đôi (2vs2).
   Kết quả trận đấu sẽ làm tăng/giảm điểm Rank DUPR của người chơi (nếu IsRanked = true).
3. Sàn đấu & Sự kiện (Challenges & Tournaments)
   Kèo Thách đấu (Duel):
   Một người thách đấu người khác (1vs1 hoặc 2vs2).
   Phần thưởng vui vẻ (Nước, Trứng) - không có Entry Fee hoặc PrizePool lớn.
   Type = Duel, GameMode = None.
   Giải đấu Mini (Mini-game):
   Nhóm hội viên (10-20 người) góp tiền (Entry Fee) tạo giải.
   Entry Fee: Số tiền mỗi người phải đóng để tham gia (VD: 50.000đ/người).
   PrizePool: Tổng quỹ thưởng = Entry Fee × Số người tham gia (tự động tính hoặc admin nhập).
   Thể thức Team Battle:
   Chia 2 phe A-B. Có thể phân chia tự động theo Rank để cân sức (người rank cao nhất →
   Team A, rank thứ 2 → Team B, rank thứ 3 → Team A...).
   Đánh chạm mốc thắng: Phe nào thắng đủ số trận (Config_TargetWins, VD: 5 trận) trước thì
   ăn trọn quỹ.
   Type = MiniGame, GameMode = TeamBattle.
   Thể thức Vòng tròn (RoundRobin):
   Đánh xoay vòng tích điểm cá nhân. Mỗi người đấu với nhiều người khác, tích điểm theo
   kết quả.
   Xếp hạng theo tổng điểm, top 1/2/3 nhận thưởng.
   Type = MiniGame, GameMode = RoundRobin.
   Lưu ý: Dù là giải đấu kiểu gì, thì các trận đấu con bên trong (Matches) vẫn phải tuân thủ luật
   Đơn/Đôi.
   PHẦN 2: THIẾT KẾ CƠ SỞ DỮ LIỆU (DATABASE SCHEMA)
   Sinh viên sử dụng Code First và Migration. Tên bảng nghiệp vụ bắt đầu bằng 3 số cuối MSSV.

4. Nhóm Quản trị (Operations)
   [xxx]\_Members:
   Cơ bản: Id, FullName, JoinDate, RankLevel (Double), Status (hoặc IsActive - bool).
   Bổ sung (bắt buộc):
   UserId (FK → AspNetUsers) - Liên kết với tài khoản Identity để đăng nhập.
   Email, PhoneNumber - Có thể lấy từ Identity hoặc lưu riêng để bổ sung thông tin.
   DateOfBirth (DateTime?, nullable) - Ngày sinh.
   Khuyến nghị: TotalMatches (int), WinMatches (int), CreatedDate, ModifiedDate.
   [xxx]\_News:
   Id, Title, Content, IsPinned (bool) - Tin được ghim sẽ hiển thị ưu tiên.
   Khuyến nghị thêm CreatedDate.
   [xxx]\_TransactionCategories:
   Id, Name (VD: "Tiền sân", "Nước", "Quỹ tháng", "Phạt"...), Type (Enum: Thu / Chi).
   [xxx]\_Transactions:
   Id, Date (DateTime), Amount (decimal), Description (string), CategoryId (FK →
   TransactionCategories).
   Khuyến nghị: CreatedBy (FK → Members), CreatedDate.
5. Bảng Sân bóng (Courts) – BẮT BUỘC
   [xxx]\_Courts:
   Id, Name (string, VD: "Sân 1", "Sân 2"), IsActive (bool), Description (string, nullable).
6. Nhóm Thi đấu (Sports Core) – QUAN TRỌNG
   [xxx]\_Bookings:
   Id, CourtId (FK → Courts) - Bắt buộc phải chọn sân, MemberId (FK → Members), StartTime
   (DateTime), EndTime (DateTime).
   Bổ sung:
   Status (Enum: Pending / Confirmed / Cancelled) - Trạng thái booking.
   Notes (string, nullable) - Ghi chú.
   CreatedDate.
   [xxx]\_Challenges (Sự kiện/Giải đấu):
   Id, Title (string), Type (Enum: Duel / MiniGame).
   GameMode (Enum: None / TeamBattle / RoundRobin) - None cho Duel, TeamBattle/RoundRobin
   cho MiniGame.
   Status (Enum: Open / Ongoing / Finished) - Open: Đang mở đăng ký, Ongoing: Đang diễn ra,
   Finished: Đã kết thúc.
   Config_TargetWins (int, nullable) - Cho TeamBattle: Số trận thắng cần đạt (VD: 5).
   CurrentScore_TeamA (int), CurrentScore_TeamB (int) - Điểm hiện tại của 2 phe (cho TeamBattle).
   Bổ sung (bắt buộc):
   EntryFee (decimal) - Số tiền mỗi người đóng để tham gia.
   PrizePool (decimal) - Tổng quỹ thưởng (có thể tự động = EntryFee × số participants hoặc
   admin nhập).
   CreatedBy (FK → Members) - Người tạo challenge.
   StartDate, EndDate (DateTime?, nullable) - Thời gian bắt đầu/kết thúc.
   CreatedDate, ModifiedDate.
   [xxx]\_Matches (Bảng lưu trữ trận đấu – Thiết kế chặt chẽ):
   Id, Date (DateTime) - Ngày giờ trận đấu.
   IsRanked (bool) - Quan trọng: Nếu = true thì trận này sẽ ảnh hưởng đến Rank DUPR. Thường thì
   Daily Matches và Challenge Matches đều có thể ranked.
   ChallengeId (FK → Challenges, nullable) - Null nếu là trận giao hữu, có giá trị nếu thuộc
   Challenge.
   MatchFormat: Enum (Singles = 1vs1, Doubles = 2vs2).
   ĐỘI HÌNH 1 (TEAM 1):
   Team1_Player1Id (FK → Members, bắt buộc).
   Team1_Player2Id (FK → Members, nullable) - Chỉ có dữ liệu khi MatchFormat = Doubles.
   ĐỘI HÌNH 2 (TEAM 2):
   Team2_Player1Id (FK → Members, bắt buộc).
   Team2_Player2Id (FK → Members, nullable) - Chỉ có dữ liệu khi MatchFormat = Doubles.
   KẾT QUẢ:
   WinningSide: Enum (None - Chưa đấu, Team1, Team2).
   [xxx]\_Participants:
   Id, ChallengeId (FK → Challenges), MemberId (FK → Members).
   Team (Enum: TeamA / TeamB / None) - Cho TeamBattle: thuộc phe nào. None cho RoundRobin
   hoặc Duel.
   EntryFeePaid (bool) - Đã đóng Entry Fee chưa.
   EntryFeeAmount (decimal) - Số tiền đã đóng (thường = Challenge.EntryFee, nhưng có thể khác
   nếu có giảm giá/ưu đãi).
   JoinedDate (DateTime) - Ngày tham gia.
   Status (Enum: Pending / Confirmed / Withdrawn) - Pending: Chờ xác nhận, Confirmed: Đã xác
   nhận, Withdrawn: Đã rút.
7. Bảng bổ sung (Khuyến nghị / Điểm cộng)
   [xxx]\_MatchScores:
   Id, MatchId (FK → Matches), SetNumber (int), Team1Score (int), Team2Score (int), IsFinalSet
   (bool).
   Dùng để lưu chi tiết điểm từng set (phục vụ DUPR nâng cao).
   [xxx]\_Notifications:
   Id, MemberId (FK → Members, nullable) - null = thông báo chung cho tất cả, Title, Content, Type
   (Enum: Info/Warning/Success/Error), IsRead (bool), CreatedDate, LinkUrl (string, nullable).
   [xxx]\_ActivityLogs:
   Id, UserId (FK → AspNetUsers), Action (string), EntityType (string), EntityId (int?, nullable),
   Description (string), IpAddress (string, nullable), CreatedDate.
   PHẦN 3: YÊU CẦU CHỨC NĂNG & GIAO DIỆN (UI)
8. Dashboard & News
   Hiển thị tin tức ghim (IsPinned = true) và tin mới nhất.
   Hiển thị cảnh báo Quỹ âm (Chỉ Admin/Treasurer thấy) - Nếu tổng Thu - tổng Chi < 0 thì hiển thị cảnh
   báo màu đỏ.
   Khuyến nghị: Thống kê tổng quan (số dư quỹ, số kèo đang mở, số booking sắp tới, BXH Top 5).
9. Quản lý Tài chính Động (Dynamic Treasury)
   Admin có thể CRUD Danh mục Transaction (Thêm/Sửa/Xóa các loại thu chi).
   Ghi nhận giao dịch: Chọn Category, nhập Amount (dương cho Thu, âm cho Chi hoặc dùng Type),
   Description, Date.
   Xem báo cáo tổng quan: Tổng Thu, tổng Chi, số dư quỹ.
   Khi Challenge kết thúc: Khuyến nghị tự động tạo Transaction ghi nhận chi trả thưởng:
   Team Battle: Tạo Transaction Chi với mô tả "Chi thưởng Challenge [Tên]", Amount = PrizePool,
   chia đều cho team thắng.
   RoundRobin: Tạo các Transaction Chi cho top 1/2/3 theo tỷ lệ (VD: 50%/30%/20% PrizePool).
10. Quản lý Sân & Đặt sân (Booking)
    CRUD Courts (Admin): Tạo, sửa, xóa, vô hiệu hóa sân.
    Đặt sân (Member):
    Chọn Court từ dropdown, chọn StartTime và EndTime.
    Hệ thống chặn trùng lịch: Kiểm tra xem có booking nào trên cùng Court có thời gian giao nhau
    không.
    Ví dụ: Nếu đã có booking Sân 1 từ 8:00-9:00, thì không cho đặt Sân 1 từ 8:30-9:30 hoặc 7:30-8:
    (giao nhau).
    Validation bắt buộc:
    StartTime ≥ hiện tại (không đặt quá khứ).
    EndTime > StartTime.
    Không đặt trùng slot cùng sân (đã kiểm tra ở trên).
    Khuyến nghị:
    Giới hạn 2 slot/ngày/member.
    Mỗi slot tối đa 2 giờ.
    Hiển thị lịch theo dạng Calendar (tuần/tháng) với màu phân biệt đã đặt / còn trống / của mình.
11. Module Trọng tài & Ghi nhận Trận đấu (Referee)
    Đây là chức năng quan trọng nhất để đảm bảo dữ liệu đúng chuẩn Đơn/Đôi. Chỉ Referee hoặc Admin mới
    được ghi nhận kết quả trận đấu.

Giao diện Tạo/Nhập kết quả Trận đấu:

Bước 1: Chọn Thể thức (Format)
Đánh Đôi (2vs2).
Khi chọn Đơn → chỉ hiển thị 2 dropdown (Player 1 Team A, Player 1 Team B).
Khi chọn Đôi → hiển thị đủ 4 dropdown.
Bước 2: Chọn Người chơi (theo Thể thức)
Đơn: 2 Dropdown (Player 1 Team A vs Player 1 Team B); các ô Player 2 ẩn/disable.
Đôi: đủ 4 Dropdown (Team1_Player1, Team1_Player2, Team2_Player1, Team2_Player2).
Validation: Không được chọn cùng một người hai lần trong một trận (VD: Không thể chọn
"Nguyễn Văn A" làm cả Team1_Player1 và Team2_Player1).
Bước 3: Chọn Kết quả & Cấu hình
Team 1 Thắng / [ ] Team 2 Thắng.
IsRanked - Nếu tích thì trận này sẽ ảnh hưởng đến Rank.
Dropdown: Chọn Challenge (nếu trận này thuộc Challenge, có thể để trống nếu là giao hữu).
Logic Service (bắt buộc):

Nếu IsRanked = true:
Cập nhật Rank cho người thắng và người thua.
Có thể dùng công thức đơn giản: Thắng +0.1, Thua -0.1.
Khuyến nghị: Công thức kiểu Elo/DUPR (dựa trên chênh lệch rank, hệ số K):
ExpectedScore = 1 / (1 + 10^((OpponentRank - PlayerRank) / 400))
K = 32 (trận thường) hoặc 64 (trận Challenge)
NewRank = OldRank + K × (ActualScore - ExpectedScore)
Với ActualScore = 1 nếu thắng, 0 nếu thua.
Nếu trận thuộc Challenge (ChallengeId != null):
Nếu GameMode = TeamBattle: Cập nhật CurrentScore_TeamA hoặc CurrentScore_TeamB (+1 cho
team thắng).
Khi một team đạt Config_TargetWins → đánh dấu Challenge.Status = Finished.
Nếu GameMode = RoundRobin: Tích điểm cá nhân (VD: Thắng +3 điểm, Thua +1 điểm, tính
tổng).
Khuyến nghị: Khi Challenge Finished:
Tự động phân phối thưởng (tạo Transactions).
Lock các Match liên quan (không cho sửa/xóa).
Tạo notifications cho participants. 5. Sàn đấu Kèo (Challenge Board)
Member tạo và tham gia các Kèo.
Hiển thị rõ: Tổng Quỹ Thưởng (PrizePool) , Entry Fee , Danh sách phe A – phe B (nếu TeamBattle),
danh sách participants (nếu RoundRobin).
Khuyến nghị:
Khi tạo Mini-game Team Battle, hỗ trợ phân chia Team A/B tự động:
Sắp xếp participants theo Rank giảm dần.
Chia xen kẽ: Rank cao nhất → Team A, rank thứ 2 → Team B, rank thứ 3 → Team A...
Đảm bảo chênh lệch tổng rank giữa 2 team < 50 điểm (nếu có thể). 6. Phân quyền & Bảo mật
Roles đề xuất:
Admin: Toàn quyền (CRUD tất cả).
Treasurer: Quản lý tài chính (CRUD Transactions, Categories, xem báo cáo).
Referee: Ghi nhận kết quả trận đấu (Create Match), không được sửa/xóa tùy ý (chỉ Admin mới
sửa/xóa Match).
Member: Xem thông tin, đặt sân, tham gia challenge, xem/sửa thông tin của chính mình.
Có thể bổ sung NewsEditor (chỉ quản lý News).
Yêu cầu bắt buộc:
Member chỉ xem/sửa thông tin của chính mình (không sửa được thông tin member khác).
Member không thể tự sửa Rank của mình (chỉ hệ thống tự động cập nhật khi có Match).
Chỉ người tạo Challenge (CreatedBy) hoặc Admin mới được đóng/mở challenge (thay đổi Status).
Chỉ Admin/Treasurer mới thấy cảnh báo quỹ âm.
Sử dụng Authorization (Role-based hoặc Policy) trên các Razor Page / Action tương ứng.
PHẦN 4: YÊU CẦU KỸ THUẬT (MIGRATION & SEEDING)
Yêu cầu bắt buộc: Sinh viên thực hiện Data Seeding (trong OnModelCreating hoặc DbInitializer) để khi
chạy Update-Database, hệ thống có sẵn dữ liệu sau:

1. Identity:
   1 Admin (email: admin@pcm.com, password: tùy chọn, VD: "Admin@123").
   6–8 Member mẫu (tạo user Identity tương ứng).
   Members phải có UserId liên kết với user Identity (lấy UserId từ AspNetUsers sau khi tạo user).
2. Courts:
   Ít nhất 2 sân mẫu (VD: "Sân 1", "Sân 2", IsActive = true).
3. Tài chính:
   Categories mẫu: "Tiền sân" (Thu), "Quỹ tháng" (Thu), "Nước" (Chi), "Phạt" (Chi).
   Transactions mẫu: Tạo vài giao dịch Thu và Chi sao cho Quỹ đang dương (tổng Thu > tổng Chi).
   Ví dụ: Thu 1.000.000đ (Quỹ tháng), Chi 200.000đ (Nước) → Số dư = 800.000đ.
4. Hoạt động:
   1 Kèo Mini-game (Team Battle) Status = Ongoing , có:
   EntryFee (VD: 50.000đ).
   PrizePool (VD: 500.000đ = 10 người × 50.000đ).
   Participants: 10-12 người, chia đều Team A / Team B (Team = TeamA hoặc TeamB),
   EntryFeePaid = true, EntryFeeAmount = EntryFee, Status = Confirmed.
   Lịch sử Matches: 2–3 trận đã diễn ra:
   1-2 trận Đơn (Singles) với IsRanked = true, có kết quả (WinningSide = Team1 hoặc Team2).
   1 trận Đôi (Doubles) với IsRanked = true, có kết quả.
   Có thể gắn với Challenge (ChallengeId) hoặc để null (giao hữu).
   PHẦN 5: YÊU CẦU GIAO DIỆN & TRẢI NGHIỆM (UI/UX)
   Sử dụng Razor Pages kết hợp Bootstrap.
   Dashboard:
   Hiển thị các chỉ số quan trọng: Số dư quỹ, Số kèo đang mở, BXH Top 5 (5 người có Rank cao
   nhất).
   Khuyến nghị: Widget Top 5 Ranking bên phải trang chủ; thống kê nhanh (số trận hôm nay,
   booking sắp tới).
   Sàn đấu:
   Thiết kế dạng thẻ (Card), làm nổi bật PrizePool , Entry Fee và phần thưởng Mini-game.
   Hiển thị rõ Status (Open/Ongoing/Finished), danh sách participants.
   Khuyến nghị:
   Giao diện responsive (mobile-friendly).
   Pagination cho danh sách dài: Members, Matches, Transactions, Challenges.
   Loading spinner/feedback khi submit form.
   Toast notifications cho success/error.
   PHẦN 6: ĐIỂM CỘNG (BONUS FEATURES)
5. Báo cáo tài chính (Export): Xuất lịch sử Thu/Chi ra Excel hoặc PDF (có thể dùng thư viện như EPPlus,
   ClosedXML, iTextSharp).
6. Top Ranking: Widget Top 5 cao thủ (Rank cao nhất) bên phải trang chủ, có avatar, trend up/down.
7. Validate Tuổi: Khi nhập DateOfBirth → tự động tính tuổi. Nếu < 10 hoặc > 80 thì hiện cảnh báo: "Cẩn
   thận chấn thương nhé!".
8. DUPR nâng cao: Tính Rank theo công thức kiểu Elo/DUPR (ExpectedScore, hệ số K, chênh lệch rank đối
   thủ) thay vì +0.1/-0.1 đơn giản.
9. Calendar view: Lịch đặt sân theo tuần/tháng, màu phân biệt đã đặt / còn trống / của mình, click vào
   slot để đặt sân.
10. Notifications / ActivityLogs:
    Lưu thông báo (VD: "Rank của bạn đã tăng lên 4.5", "Challenge XYZ đã kết thúc") và hiển thị trên
    UI.
    Lưu nhật ký hoạt động (tạo Match, cập nhật Rank, v.v.) và hiển thị trên trang Activity Logs.
11. Tìm kiếm & lọc:
    Tìm kiếm Members (theo tên, email, rank).
    Lọc Matches (theo ngày, format, challenge).
    Lọc Transactions (theo khoảng thời gian, category).
    Lọc Challenges (theo status, type).
