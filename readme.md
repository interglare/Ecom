# Тестовое задание. Backend
## 1. Восстановление зависимостей

Из корневой папки проекта, где находится `.sln`, выполнить:

```bash
dotnet restore
```

## 2. Создание базы данных

Проект использует SQLite. Для создания базы данных нужно применить миграции:

```bash
dotnet ef database update \
  --project ./Ecom.Infrastructure/Ecom.Infrastructure.csproj \
  --startup-project ./Ecom.Api/Ecom.Api.csproj
```

После выполнения команды будет создан локальный файл базы данных `app.db`.

## 3. Запуск проекта

Из корневой папки проекта выполнить:

```bash
dotnet run --project ./Ecom.Api/Ecom.Api.csproj
```

После запуска приложение будет доступно по следующему адресу:

```text
http://localhost:5202
```

Swagger доступен по адресу:

```text
http://localhost:5202/swagger
```

## 3. Проверка через Swagger

### 1. Загрузить Excel-файл

Excel-файл уже есть в проекте под названием sample.xlsx

Endpoint:

```http
POST /api/products/upload
```

Файл передается через `multipart/form-data`.

Название поля:

```text
file
```

Пример ответа:

```json
{
  "message": "Файл успешно загружен",
  "importedCount": 4
}
```

### 2. Проверить список загруженных товаров

Endpoint:

```http
GET /api/products
```

Пример ответа:

```json
[
  {
    "id": 1,
    "name": "Ручка",
    "unit": "штука",
    "unitPrice": 1.5,
    "quantity": 150,
    "isProcessed": false
  }
]
```

### 3. Запустить группировку вручную

Endpoint:

```http
POST /api/product-groups/process
```

Пример ответа:

```json
{
  "message": "Группировка товаров выполнена",
  "createdGroupsCount": 4
}
```

### 4. Получить список групп

Endpoint:

```http
GET /api/product-groups
```

Пример ответа:

```json
[
  {
    "id": 1,
    "name": "Группа 1",
    "totalPrice": 199.8,
    "createdAt": "2026-05-08T10:00:00Z"
  }
]
```

### 5. Получить товары конкретной группы

Endpoint:

```http
GET /api/product-groups/{groupId}/products
```

Пример:

```http
GET /api/product-groups/1/products
```

Пример ответа:

```json
{
  "id": 1,
  "name": "Группа 1",
  "totalPrice": 199.8,
  "products": [
    {
      "productName": "Доска маркерная",
      "unit": "штука",
      "unitPrice": 32,
      "quantity": 6,
      "totalPrice": 192
    },
    {
      "productName": "Бумага А4",
      "unit": "упаковка",
      "unitPrice": 2.6,
      "quantity": 3,
      "totalPrice": 7.8
    }
  ]
}
```

## Фоновая обработка

В приложении также реализована фоновая обработка.

После запуска приложения сервис автоматически проверяет наличие необработанных товаров и запускает группировку каждые 5 минут.

Для ручной проверки можно не ждать 5 минут и использовать endpoint:

```http
POST /api/product-groups/process
```

## Полный сценарий проверки

1. Восстановить зависимости:

```bash
dotnet restore
```

2. Создать базу данных:

```bash
dotnet ef database update --project ./Ecom.Infrastructure/Ecom.Infrastructure.csproj --startup-project ./Ecom.Api/Ecom.Api.csproj
```

3. Запустить проект:

```bash
dotnet run --project ./Ecom.Api/Ecom.Api.csproj
```

4. Открыть Swagger:

```text
http://localhost:5202/swagger
```

5. Загрузить Excel-файл через:

```http
POST /api/products/upload
```

6. Проверить загруженные товары:

```http
GET /api/products
```

7. Запустить группировку:

```http
POST /api/product-groups/process
```

8. Проверить список групп:

```http
GET /api/product-groups
```

9. Проверить товары внутри группы:

```http
GET /api/product-groups/1/products
```

## База данных

Для упрощения локального запуска используется SQLite. Это позволяет проверить проект без установки отдельной СУБД или Docker.

Локальный файл базы данных `app.db` не добавляется в git.

Миграции EF Core находятся в проекте `Ecom.Infrastructure` и позволяют создать базу данных командой:

```bash
dotnet ef database update --project ./Ecom.Infrastructure/Ecom.Infrastructure.csproj --startup-project ./Ecom.Api/Ecom.Api.csproj
```
