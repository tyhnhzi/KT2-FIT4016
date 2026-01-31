# Pickleball Club Management System (CPM)

Hệ thống quản lý câu lạc bộ Pickleball, bao gồm quản lý thành viên, đặt sân, tài chính, tin tức và các giải đấu/kèo đấu.

## 🚀 Tính năng Chính

### Web Application (Razor Pages)

#### 1. Quản lý Tin tức (News Management)

- **Người dùng**: Xem danh sách tin tức.
- **Admin**: Thêm, Sửa, Xóa tin tức.

#### 2. Quản lý Đặt sân (Booking Management)

- **Hội viên**: Đặt sân, xem lịch sử đặt sân của mình.
- **Admin**:
  - Xem toàn bộ lịch đặt sân.
  - Đặt sân hộ hội viên.
  - Duyệt (Confirm) hoặc Hủy (Cancel) yêu cầu đặt sân.
  - Chỉnh sửa thông tin, xóa đặt sân.

#### 3. Sàn đấu Kèo & Giải (Challenges)

- **Hội viên**:
  - Xem danh sách các kèo đấu (Open, Ongoing, Finished).
  - Tham gia kèo đấu (trạng thái Open).
- **Admin**:
  - Tạo kèo mới (Giải đấu, Kèo giao lưu...).
  - Duyệt kèo (nếu thành viên tạo - tính năng mở rộng).
  - Bắt đầu kèo (chuyển sang Ongoing).
  - Kết thúc kèo.

#### 4. Quản lý Kết quả Trận đấu (Matches)

- **Referee/Admin**:
  - Ghi nhận kết quả trận đấu (Singles/Doubles).
  - Chọn kèo đấu liên quan (để tính điểm cho kèo).
  - Chọn người thắng cuộc.
- **Hệ thống**:
  - Tự động cập nhật Rank (trình độ) cho người chơi nếu trận đấu có tính điểm (Ranked).
  - Tự động thống kê số trận thắng/thua.

#### 5. Quản lý Tài chính (Treasury) - Admin Only

- Theo dõi tổng quỹ CLB.
- Ghi nhận Thu/Chi (Income/Expense).
- Quản lý danh mục thu chi.
- Cảnh báo khi quỹ âm.

---

## 🔌 REST API Endpoints

Hệ thống cung cấp REST API đầy đủ cho các tài nguyên chính:

### Members API
- `GET /api/members` - Lấy danh sách tất cả members
- `GET /api/members/{id}` - Lấy thông tin member theo ID
- `POST /api/members` - Tạo member mới
- `PUT /api/members/{id}` - Cập nhật member
- `DELETE /api/members/{id}` - Xóa member

### Courts API
- `GET /api/courts` - Lấy danh sách tất cả sân
- `GET /api/courts/{id}` - Lấy thông tin sân theo ID
- `POST /api/courts` - Tạo sân mới
- `PUT /api/courts/{id}` - Cập nhật sân
- `DELETE /api/courts/{id}` - Xóa sân

### Bookings API
- `GET /api/bookings` - Lấy danh sách tất cả bookings
- `GET /api/bookings/{id}` - Lấy thông tin booking theo ID
- `POST /api/bookings` - Tạo booking mới
- `PUT /api/bookings/{id}` - Cập nhật booking
- `DELETE /api/bookings/{id}` - Xóa booking

### Challenges API
- `GET /api/challenges` - Lấy danh sách tất cả challenges
- `GET /api/challenges/{id}` - Lấy thông tin challenge theo ID
- `POST /api/challenges` - Tạo challenge mới
- `PUT /api/challenges/{id}` - Cập nhật challenge
- `DELETE /api/challenges/{id}` - Xóa challenge

### Participants API
- `GET /api/participants` - Lấy danh sách tất cả participants
- `GET /api/participants/{id}` - Lấy thông tin participant theo ID
- `POST /api/participants` - Tạo participant mới
- `PUT /api/participants/{id}` - Cập nhật participant
- `DELETE /api/participants/{id}` - Xóa participant

### Matches API
- `GET /api/matches` - Lấy danh sách tất cả matches
- `GET /api/matches/{id}` - Lấy thông tin match theo ID
- `POST /api/matches` - Tạo match mới
- `PUT /api/matches/{id}` - Cập nhật match
- `DELETE /api/matches/{id}` - Xóa match

### News API
- `GET /api/news` - Lấy danh sách tất cả tin tức
- `GET /api/news/{id}` - Lấy thông tin tin tức theo ID
- `POST /api/news` - Tạo tin tức mới
- `PUT /api/news/{id}` - Cập nhật tin tức
- `DELETE /api/news/{id}` - Xóa tin tức

### Transactions API
- `GET /api/transactions` - Lấy danh sách tất cả transactions
- `GET /api/transactions/{id}` - Lấy thông tin transaction theo ID
- `POST /api/transactions` - Tạo transaction mới
- `PUT /api/transactions/{id}` - Cập nhật transaction
- `DELETE /api/transactions/{id}` - Xóa transaction

### Transaction Categories API
- `GET /api/transaction-categories` - Lấy danh sách tất cả categories
- `GET /api/transaction-categories/{id}` - Lấy thông tin category theo ID
- `POST /api/transaction-categories` - Tạo category mới
- `PUT /api/transaction-categories/{id}` - Cập nhật category
- `DELETE /api/transaction-categories/{id}` - Xóa category

### API Testing với cURL

```bash
# Lấy danh sách members
curl -X GET http://localhost:5003/api/members

# Lấy member theo ID
curl -X GET http://localhost:5003/api/members/1

# Tạo member mới
curl -X POST http://localhost:5003/api/members \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Nguyen Van A",
    "dob": "1990-01-01",
    "phoneNumber": "0123456789",
    "identityUserId": "user-id-here"
  }'

# Cập nhật member
curl -X PUT http://localhost:5003/api/members/1 \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "fullName": "Nguyen Van B",
    "dob": "1990-01-01",
    "phoneNumber": "0123456789",
    "identityUserId": "user-id-here"
  }'

# Xóa member
curl -X DELETE http://localhost:5003/api/members/1
```

---

## 🛠️ Cài đặt & Chạy ứng dụng

### Yêu cầu hệ thống

- .NET 8.0 SDK trở lên.
- SQL Server (hoặc LocalDB).

### Các bước cài đặt

1. **Clone Repository**

   ```bash
   git clone https://github.com/tyhnhzi/KT2-FIT4016.git
   cd KT2-FIT4016/CPM/PickleballClub
   ```

2. **Cấu hình Database**
   - Mở file `appsettings.json`.
   - Cập nhật chuỗi kết nối `DefaultConnection` phù hợp với SQL Server của bạn.

3. **Cập nhật Database (Migrations)**

   ```bash
   dotnet ef database update
   ```

4. **Chạy ứng dụng**
   ```bash
   dotnet run
   ```
   - Web Application: `http://localhost:5003`
   - Swagger UI: `http://localhost:5003/swagger`

---

## 🔐 API Authentication & Security

### JWT Authentication

Tất cả API endpoints đều yêu cầu JWT token để truy cập (trừ `/api/auth/login` và `/api/auth/register`).

### Authentication Endpoints

#### 1. Login (Lấy JWT Token)
```bash
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@pickleballclub.com",
  "password": "Admin@123"
}

Response:
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "email": "admin@pickleballclub.com",
  "userId": "user-id",
  "roles": ["Admin"],
  "expiresIn": 1440
}
```

#### 2. Register (Đăng ký tài khoản mới)
```bash
POST /api/auth/register
Content-Type: application/json

{
  "email": "newuser@example.com",
  "password": "Password123"
}
```

### Sử dụng JWT Token

Sau khi login, sử dụng token trong header `Authorization`:

```bash
# Cách 1: Bearer token
curl -X GET http://localhost:5003/api/members \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE"

# Cách 2: Postman/Swagger
# Add header: Authorization: Bearer YOUR_JWT_TOKEN_HERE
```

### Cấu hình JWT (appsettings.json)

```json
"Jwt": {
  "Key": "YourSuperSecretKeyForJWTTokenGenerationMustBeAtLeast32Characters",
  "Issuer": "PickleballClubAPI",
  "Audience": "PickleballClubClients",
  "ExpireMinutes": 1440
}
```

**⚠️ QUAN TRỌNG**: Đổi `Jwt:Key` trong production để bảo mật!

---

## 📖 Swagger API Documentation

Khi chạy ở môi trường Development, truy cập Swagger UI tại:

```
http://localhost:5003/swagger
```

### Sử dụng Swagger với JWT:

1. Đăng nhập qua `/api/auth/login` để lấy token
2. Click nút **"Authorize"** ở góc phải trên
3. Nhập: `Bearer YOUR_TOKEN_HERE`
4. Click **"Authorize"** và **"Close"**
5. Giờ có thể test tất cả API endpoints

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

---

## 🚀 Deploy lên VPS (Ubuntu/Linux)

### 1. Chuẩn bị VPS

#### Cài đặt .NET Runtime trên Ubuntu

```bash
# Cập nhật hệ thống
sudo apt update && sudo apt upgrade -y

# Cài đặt .NET 8.0 Runtime
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0 --runtime aspnetcore

# Thêm dotnet vào PATH
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools' >> ~/.bashrc
source ~/.bashrc

# Kiểm tra
dotnet --version
```

#### Cài đặt Nginx (Reverse Proxy)

```bash
sudo apt install nginx -y
sudo systemctl start nginx
sudo systemctl enable nginx
```

#### Cài đặt SQL Server trên Ubuntu (hoặc dùng SQL Server riêng)

```bash
# Tham khảo: https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-ubuntu
```

### 2. Upload Code lên VPS

```bash
# Trên máy local, build ứng dụng
dotnet publish -c Release -o ./publish

# Upload lên VPS qua SCP
scp -r ./publish/* username@your-vps-ip:/var/www/pickleballclub/

# Hoặc dùng Git
ssh username@your-vps-ip
cd /var/www/pickleballclub
git clone https://github.com/tyhnhzi/KT2-FIT4016.git .
git checkout main
```

### 3. Cấu hình appsettings.json trên VPS

```bash
nano /var/www/pickleballclub/appsettings.json
```

**Cập nhật**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_VPS_IP_OR_DB_SERVER;Database=PickleBallDB;User Id=sa;Password=YOUR_STRONG_PASSWORD;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "GENERATE_NEW_SECURE_KEY_AT_LEAST_32_CHARS_LONG_FOR_PRODUCTION",
    "Issuer": "PickleballClubAPI",
    "Audience": "PickleballClubClients",
    "ExpireMinutes": 1440
  },
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5000"
      }
    }
  }
}
```

### 4. Chạy Migrations

```bash
cd /var/www/pickleballclub
dotnet ef database update
```

### 5. Tạo Service cho ASP.NET Core

```bash
sudo nano /etc/systemd/system/pickleballclub.service
```

**Nội dung file**:
```ini
[Unit]
Description=Pickleball Club Management System
After=network.target

[Service]
WorkingDirectory=/var/www/pickleballclub
ExecStart=/home/USERNAME/.dotnet/dotnet /var/www/pickleballclub/PickleballClub.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=pickleballclub
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

**Khởi động service**:
```bash
sudo systemctl daemon-reload
sudo systemctl start pickleballclub
sudo systemctl enable pickleballclub
sudo systemctl status pickleballclub
```

### 6. Cấu hình Nginx Reverse Proxy

```bash
sudo nano /etc/nginx/sites-available/pickleballclub
```

**Nội dung**:
```nginx
server {
    listen 80;
    server_name your-domain.com www.your-domain.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

**Enable site**:
```bash
sudo ln -s /etc/nginx/sites-available/pickleballclub /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl restart nginx
```

### 7. Cài đặt SSL với Let's Encrypt (HTTPS)

```bash
sudo apt install certbot python3-certbot-nginx -y
sudo certbot --nginx -d your-domain.com -d www.your-domain.com
```

### 8. Cấu hình Firewall

```bash
sudo ufw allow 'Nginx Full'
sudo ufw allow OpenSSH
sudo ufw enable
```

### 9. Kiểm tra & Logs

```bash
# Kiểm tra status
sudo systemctl status pickleballclub

# Xem logs
sudo journalctl -u pickleballclub -f

# Restart khi cần
sudo systemctl restart pickleballclub
```

### 10. Truy cập

- **Web App**: `http://your-domain.com` hoặc `https://your-domain.com`
- **API Docs**: `https://your-domain.com/swagger` (nếu enable trong Production)

---

## 🔧 Troubleshooting

### Lỗi kết nối Database
- Kiểm tra connection string trong `appsettings.json`
- Đảm bảo SQL Server đang chạy và cho phép remote connections
- Kiểm tra firewall mở port 1433

### App không start
```bash
# Xem logs chi tiết
sudo journalctl -u pickleballclub --no-pager

# Kiểm tra dotnet runtime
dotnet --info
```

### Swagger không hiện trong Production
- Mở file `Program.cs`
- Sửa `if (app.Environment.IsDevelopment())` thành `if (true)` (tạm thời để test)

---

**FIT4016 - Bài Kiểm Tra 2**
