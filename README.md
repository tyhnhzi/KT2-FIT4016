# Pickleball Club Management System

Hệ thống quản lý câu lạc bộ Pickleball với các chức năng quản lý thành viên, đặt sân, tổ chức giải đấu, và quản lý tài chính.

## 🚀 Tính năng chính

- **Quản lý thành viên**: Đăng ký, cập nhật thông tin thành viên
- **Quản lý sân**: Đặt sân, xem lịch sân trống
- **Quản lý giải đấu**: Tạo thử thách, quản lý trận đấu
- **Quản lý tài chính**: Theo dõi giao dịch, thu chi
- **Tin tức**: Đăng và quản lý tin tức câu lạc bộ
- **API RESTful**: Đầy đủ endpoints với Swagger documentation
- **JWT Authentication**: Bảo mật API với token

## 📋 Yêu cầu hệ thống

- .NET 8.0 SDK
- PostgreSQL 14+ hoặc Docker
- Visual Studio 2022 / VS Code / Rider (tùy chọn)

## 🛠️ Cài đặt

### 1. Clone repository

```bash
git clone https://github.com/tyhnhzi/KT2-FIT4016.git
cd KT2-FIT4016
```

### 2. Cấu hình Database

#### Sử dụng Docker (Khuyến nghị)

```bash
# Windows
docker-manager.bat up

# Linux/Mac
chmod +x docker-manager.sh
./docker-manager.sh up
```

#### Hoặc cấu hình PostgreSQL thủ công

Chỉnh sửa file `PickleballClub/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=pickleballdb;Username=postgres;Password=your_password"
  }
}
```

### 3. Chạy Migration

```bash
cd PickleballClub
dotnet ef database update
```

### 4. Chạy ứng dụng

```bash
dotnet run
```

Ứng dụng sẽ chạy tại:
- Web: `https://localhost:5001` hoặc `http://localhost:5000`
- API: `https://localhost:5001/api`
- Swagger: `https://localhost:5001/swagger`

## 🔐 Đăng nhập

### Tài khoản mặc định

Hệ thống tạo sẵn 2 tài khoản:

**Admin:**
- Email: `admin@pickleballclub.com`
- Password: `Admin@123`

**User:**
- Email: `user@pickleballclub.com`  
- Password: `User@123`

### Đăng nhập qua API

**Endpoint:** `POST /api/auth/login`

**Request:**
```json
{
  "email": "admin@pcm.com",
  "password": "Admin@123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "admin@pcm.com",
  "roles": ["Admin"]
}
```

### Sử dụng token

Thêm header vào mọi request cần authentication:

```
Authorization: Bearer {your_token}
```

## 📚 API Documentation

### Swagger UI

Truy cập: `https://localhost:5001/swagger`

**Cách sử dụng Swagger:**

1. Đăng nhập để lấy token qua endpoint `/api/auth/login`
2. Click nút **Authorize** ở góc phải trên
3. Nhập: `Bearer {your_token}` (có khoảng trắng sau "Bearer")
4. Click **Authorize** → **Close**
5. Bây giờ bạn có thể test các API endpoints yêu cầu authentication

### API Endpoints

#### Authentication
- `POST /api/auth/register` - Đăng ký tài khoản mới
- `POST /api/auth/login` - Đăng nhập
- `GET /api/auth/me` - Lấy thông tin user hiện tại

#### Members (Thành viên)
- `GET /api/members` - Danh sách thành viên
- `GET /api/members/{id}` - Chi tiết thành viên
- `POST /api/members` - Tạo thành viên mới
- `PUT /api/members/{id}` - Cập nhật thành viên
- `DELETE /api/members/{id}` - Xóa thành viên

#### Courts (Sân)
- `GET /api/courts` - Danh sách sân
- `GET /api/courts/{id}` - Chi tiết sân
- `POST /api/courts` - Tạo sân mới
- `PUT /api/courts/{id}` - Cập nhật sân
- `DELETE /api/courts/{id}` - Xóa sân

#### Bookings (Đặt sân)
- `GET /api/bookings` - Danh sách đặt sân
- `GET /api/bookings/{id}` - Chi tiết đặt sân
- `POST /api/bookings` - Tạo đặt sân mới
- `PUT /api/bookings/{id}` - Cập nhật đặt sân
- `DELETE /api/bookings/{id}` - Hủy đặt sán

#### Challenges (Thử thách)
- `GET /api/challenges` - Danh sách thử thách
- `GET /api/challenges/{id}` - Chi tiết thử thách
- `POST /api/challenges` - Tạo thử thách mới
- `PUT /api/challenges/{id}` - Cập nhật thử thách
- `DELETE /api/challenges/{id}` - Xóa thử thách

#### Matches (Trận đấu)
- `GET /api/matches` - Danh sách trận đấu
- `GET /api/matches/{id}` - Chi tiết trận đấu
- `POST /api/matches` - Tạo trận đấu mới
- `PUT /api/matches/{id}` - Cập nhật trận đấu
- `DELETE /api/matches/{id}` - Xóa trận đấu

#### News (Tin tức)
- `GET /api/news` - Danh sách tin tức
- `GET /api/news/{id}` - Chi tiết tin tức
- `POST /api/news` - Tạo tin tức mới
- `PUT /api/news/{id}` - Cập nhật tin tức
- `DELETE /api/news/{id}` - Xóa tin tức

#### Transactions (Giao dịch)
- `GET /api/transactions` - Danh sách giao dịch
- `GET /api/transactions/{id}` - Chi tiết giao dịch
- `POST /api/transactions` - Tạo giao dịch mới
- `PUT /api/transactions/{id}` - Cập nhật giao dịch
- `DELETE /api/transactions/{id}` - Xóa giao dịch

## 🐳 Docker Deployment

### Chạy với Docker Compose

```bash
# Build và start services
docker-compose up -d

# Xem logs
docker-compose logs -f

# Stop services
docker-compose down
```

Services:
- **Web App**: http://localhost:8080
- **PostgreSQL**: localhost:5432

### Docker Manager Scripts

**Windows:**
```bash
# Start
docker-manager.bat up

# Stop
docker-manager.bat down

# Rebuild
docker-manager.bat rebuild

# View logs
docker-manager.bat logs
```

**Linux/Mac:**
```bash
# Start
./docker-manager.sh up

# Stop
./docker-manager.sh down

# Rebuild
./docker-manager.sh rebuild

# View logs
./docker-manager.sh logs
```

## 🗂️ Cấu trúc dự án

```
CPM/
├── PickleballClub/
│   ├── Controllers/          # API Controllers
│   ├── Data/                 # Database context & initializer
│   ├── Migrations/           # EF Core migrations
│   ├── Models/               # Data models
│   ├── Pages/                # Razor Pages (Web UI)
│   ├── wwwroot/              # Static files
│   ├── Program.cs            # App configuration
│   ├── appsettings.json      # App settings
│   └── Dockerfile            # Docker configuration
├── docker-compose.yml        # Docker compose config
├── CPM.sln                   # Solution file
└── README.md                 # Documentation
```

## 🔧 Cấu hình

### JWT Settings (appsettings.json)

```json
{
  "Jwt": {
    "Key": "your-super-secret-key-min-32-characters-long",
    "Issuer": "PickleballClubAPI",
    "Audience": "PickleballClubUsers",
    "ExpirationHours": 24
  }
}
```

### Database Connection

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=pickleballdb;Username=postgres;Password=postgres"
  }
}
```

## 📱 Ví dụ sử dụng API

### 1. Đăng ký tài khoản mới

```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "newuser@example.com",
    "password": "Password@123",
    "confirmPassword": "Password@123"
  }'
```

### 2. Đăng nhập

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@pickleballclub.com",
    "password": "Admin@123"
  }'
```

### 3. Lấy danh sách thành viên

```bash
curl -X GET https://localhost:5001/api/members \
  -H "Authorization: Bearer {your_token}"
```

### 4. Tạo đặt sân mới

```bash
curl -X POST https://localhost:5001/api/bookings \
  -H "Authorization: Bearer {your_token}" \
  -H "Content-Type: application/json" \
  -d '{
    "courtId": 1,
    "memberId": 1,
    "bookingDate": "2026-02-01T10:00:00",
    "durationHours": 2,
    "status": "Confirmed"
  }'
```

## 🧪 Testing

### Health Check

```bash
curl https://localhost:5001/health
```

Response:
```json
{
  "status": "Healthy",
  "timestamp": "2026-01-31T10:00:00Z"
}
```

## 🛡️ Security

- ✅ JWT Authentication với Bearer token
- ✅ Password hashing với Identity
- ✅ Role-based authorization (Admin, User)
- ✅ HTTPS enforcement
- ✅ CORS configuration
- ✅ SQL Injection protection (EF Core)

## 📝 Notes

- Development mode có Swagger UI enabled
- Production mode cần cấu hình HTTPS certificate
- Đối với aaPanel deployment, không cần file `nginx.conf` (đã exclude trong .gitignore)
- Migrations tự động chạy khi start ứng dụng
- Sample data được seed tự động

## 🤝 Contributing

1. Fork repository
2. Tạo feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Tạo Pull Request

## 📄 License

This project is for educational purposes.

## 👥 Contact

- Repository: https://github.com/tyhnhzi/KT2-FIT4016
- Issues: https://github.com/tyhnhzi/KT2-FIT4016/issues

---

**Happy Coding! 🎾**
