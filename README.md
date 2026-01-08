# Portal Backend - N-Katmanlı .NET 8 API

Modern, performanslı ve ölçeklenebilir N-katmanlı mimari ile geliştirilmiş .NET 8 API template projesi.

## 🏗️ Mimari Yapı

Bu proje Clean Architecture ve N-Layered Architecture prensiplerine göre tasarlanmıştır.

```
portal-backend/
├── src/
│   ├── Portal.API/              # Web API katmanı (Presentation)
│   ├── Portal.Application/      # İş mantığı katmanı (Business Logic)
│   ├── Portal.Domain/           # Domain modelleri ve entity'ler
│   ├── Portal.Infrastructure/   # Altyapı katmanı (Data Access, External Services)
│   └── Portal.Core/             # Ortak kullanılan temel yapılar
├── Portal.sln                   # Solution dosyası
└── README.md
```

## 📦 Katmanlar

### Portal.Core
Tüm katmanlar tarafından kullanılan temel yapıları içerir:
- **BaseEntity**: Tüm entity'ler için base class (Id, CreatedAt, UpdatedAt, IsDeleted)
- **IRepository**: Generic repository interface
- **Result**: Standart API response yapısı (Success/Failure pattern)

### Portal.Domain
Domain modelleri ve iş entity'lerini içerir:
- **Entities**: User, LogEntry
- **Attributes**: BsonCollectionAttribute (MongoDB collection mapping)

### Portal.Infrastructure
Altyapı implementasyonlarını içerir:
- **Data**: MongoDbContext, MongoDbSettings
- **Repositories**: MongoRepository (Generic repository implementation)
- **Logging**: MongoDbSink (Serilog custom sink)
- **Caching**: CacheService (Memory cache)

### Portal.Application
İş mantığı ve servis implementasyonlarını içerir:
- **Services**: UserService (CRUD operations with logging)

### Portal.API
Web API katmanı:
- **Controllers**: UsersController (RESTful API endpoints)
- **Configuration**: Serilog, MongoDB, Dependency Injection

## 🚀 Özellikler

- ✅ **N-Katmanlı Mimari**: Clean Architecture prensipleri
- ✅ **MongoDB Entegrasyonu**: NoSQL veritabanı desteği
- ✅ **Serilog**: Yapılandırılmış loglama (Console, File, MongoDB)
- ✅ **Generic Repository Pattern**: CRUD operasyonları
- ✅ **Result Pattern**: Standartlaştırılmış API yanıtları
- ✅ **Memory Cache**: Performans optimizasyonu
- ✅ **Soft Delete**: Veri güvenliği
- ✅ **Swagger/OpenAPI**: API dokümantasyonu
- ✅ **Dependency Injection**: Gevşek bağlılık

## 🛠️ Teknolojiler

- .NET 8.0
- MongoDB 2.25.0
- Serilog 4.0.0
- ASP.NET Core Web API
- Swagger/OpenAPI

## 📋 Gereksinimler

- .NET 8.0 SDK veya üzeri
- MongoDB 4.4 veya üzeri

## ⚙️ Kurulum

1. Repository'yi klonlayın:
```bash
git clone https://github.com/ayssoft/portal-backend.git
cd portal-backend
```

2. MongoDB'nin çalıştığından emin olun (varsayılan: mongodb://localhost:27017)

3. Bağımlılıkları yükleyin:
```bash
dotnet restore
```

4. Projeyi derleyin:
```bash
dotnet build
```

5. Uygulamayı çalıştırın:
```bash
dotnet run --project src/Portal.API/Portal.API.csproj
```

## 🔧 Yapılandırma

`src/Portal.API/appsettings.json` dosyasından MongoDB bağlantı ayarlarını düzenleyebilirsiniz:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "PortalDB"
  }
}
```

## 📡 API Endpoints

### Users

- **GET** `/api/users` - Tüm kullanıcıları listele
- **GET** `/api/users/{id}` - Belirli bir kullanıcıyı getir
- **POST** `/api/users` - Yeni kullanıcı oluştur
- **PUT** `/api/users/{id}` - Kullanıcı güncelle
- **DELETE** `/api/users/{id}` - Kullanıcı sil (soft delete)

### Swagger UI

Uygulama çalıştırıldığında Swagger UI'a şu adresten erişilebilir:
```
http://localhost:5000/swagger
```

## 📝 Loglama

Serilog ile yapılandırılmış loglama:
- **Console**: Geliştirme ortamı için
- **File**: `logs/portal-YYYYMMDD.txt` formatında günlük dosyalar
- **MongoDB**: Logs collection'da kalıcı log kayıtları

## 🎯 Kullanım Örneği

### Yeni Kullanıcı Oluşturma

```bash
curl -X POST http://localhost:5000/api/users \
  -H "Content-Type: application/json" \
  -d '{
    "username": "johndoe",
    "email": "john@example.com",
    "passwordHash": "hashedpassword123",
    "isActive": true
  }'
```

### Response Format

```json
{
  "isSuccess": true,
  "data": {
    "id": "507f1f77bcf86cd799439011",
    "username": "johndoe",
    "email": "john@example.com",
    "isActive": true,
    "createdAt": "2024-01-08T10:00:00Z",
    "updatedAt": null,
    "isDeleted": false
  },
  "message": "User created successfully",
  "errors": null
}
```

## 🏛️ Mimari Kararlar

### Repository Pattern
Veri erişim katmanını soyutlamak ve test edilebilirliği artırmak için generic repository pattern kullanılmıştır.

### Result Pattern
API yanıtlarını standardize etmek ve hata yönetimini kolaylaştırmak için Result pattern uygulanmıştır.

### Soft Delete
Veri bütünlüğünü korumak için soft delete (IsDeleted flag) kullanılmıştır.

### Dependency Injection
Gevşek bağlılık (loose coupling) ve test edilebilirlik için DI container kullanılmıştır.

## 🧪 Test

```bash
# Birim testleri çalıştırma (test projeleri eklendiğinde)
dotnet test
```

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

## 👥 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'feat: Add amazing feature'`)
4. Branch'inizi push edin (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 📞 İletişim

Sorularınız için issue açabilirsiniz.
