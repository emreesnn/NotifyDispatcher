# NotifyDispatcher

**NotifyDispatcher**, .NET Core ile geliştirilmiş, event-driven mimariye sahip bir arka plan uygulamasıdır. Amazon üzerinden ürün URL'si kullanılarak belirlenen ürünlerin fiyatlarını periyodik olarak kontrol eder ve fiyat değişimi algılandığında Telegram üzerinden bildirim gönderir.

---

## Özellikler

* Amazon gibi sitelerden HTML scraping ile fiyat alma *(Şu anda yalnızca Amazon desteklenmektedir)*
* Fiyat değişimi durumunda event oluşturma
* Event dispatching mantığı ile loosely coupled mimari
* Hangfire ile zamanlanmış job çalıştırma
* Telegram Bot API ile bildirim gönderme

---

## Kullanılan Teknolojiler

* **ASP.NET Core Web API**
* **Entity Framework Core** + **MSSQL**
* **HtmlAgilityPack** (Web scraping)
* **Hangfire** (Scheduled Background Jobs)
* **Telegram.Bot API**
* **Clean Architecture** mantığı (Dispatcher / Notifier / Service ayrımı)

---

## Mimarinin Genel Akışı

```text
[Hangfire] --(periyodik olarak)-->
  [ProductCheckJob] --(her ürün için)-->
    [ProductWatcherService] --(fiyat değişti mi?)-->
      [PriceChangedEvent] -->
        [EventDispatcher] -->
          [NotificationDispatcher] -->
            [TelegramNotifier] --> Telegram
```

---

## Kurulum

1. Bu repoyu klonlayın:

   ```bash
   git clone https://github.com/emreesnn/NotifyDispatcher.git
   ```

2. `appsettings.json` içeriğine MSSQL bağlantı bilgilerini ve Telegram bot bilgilerini ekleyin:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=HangfireDb;User Id=sa;Password=1234;"
   },
   "Telegram": {
     "BotToken": "123456:ABC-DEF...",
     "ChatId": "123456789"
   }
   ```

3. Veritabanını oluşturun:

   ```bash
   dotnet ef database update
   ```

4. Uygulamayı başlatın:

   ```bash
   dotnet run
   ```

5. Hangfire dashboard'a erişin:

   ```text
   http://localhost:5000/hangfire
   ```

---

## Örnek Bildirim (Telegram)

```text
Fiyat değişti!
Ürün Adı: Ürün
Eski fiyat: 1020,98
Yeni fiyat: 920,98
```

---

## Öğrendiklerim

* Event-driven backend yapısı kurma
* Dispatcher / Notifier gibi soyutlamalarla mimari temizliği
* Hangfire ile zamanlanmış arka plan işleri
* Telegram API ile bildirim gönderme
* Fiyat takibi için scraping mantığı

---

## İleriye Dönük Fikirler

* Email notifier entegrasyonu
* Farklı siteler için web scraping desteği
* Event loglama ve arayüz eklenmesi
* Gerçek event queue (RabbitMQ, Kafka vb.) kullanımı
* Web UI ile ürün ekleme
* Telegram Botu üzerinden ürün ekleyebilme

---

> Bu proje, portföy amaçlı hazırlanmıştır ve yayına açık bir servis değildir.
> Web scraping yasal olmayabilir. Bu projeyi kullanmadan önce hedef site kurallarını inceleyiniz. Kullanımdan doğacak herhangi bir sorumluluk geliştiriciye ait değildir.
