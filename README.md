# ConferenceApp.API

## Добавление заявки

Для того, чтобы добавить заявку, необходимо передать следующий JSON:

```

POST https://localhost:5001/api/request/create
Content-Type: application/json

{
  "user": {
    "firstName": "Петя",
    "middleName": "Иванович",
    "lastName": "Васечкин",
    "degree": 0,
    "organization": "ЮЗГУ",
    "address": "ул. Пушкина, дом Колотушкина",
    "phone": "88005553535",
    "fax": "88005553535",
    "email": "petya@swsu.ru"
  },
  "reports": [
    {
      "title": "Исследование зеленого слоника",
      "reportType": 0,
      "file": /* Здесь должен быть файл */,
      "Collaborators": "Петр Иванович Васечкин"
    }
  ]
}
```

## Приложить файл к заявке

Чтобы приложить файл к заявке требуется передать следующий JSON

```
POST https://localhost:5001/api/request/{requestId}/attach-report
Content-Type: application/json

{
  "title": "Исследование зеленого слоника",
  "reportType": 0,
  "file": /* Здесь должен быть файл */,
  "Collaborators": "Петр Иванович Васечкин"
}
```


## Одобрить заявку
Запрос для одобрения заявки

```
GET https://localhost:5001/api/request/{requestId}/approve
```

## Отклонить заявку
Запрос для отклонения заявки

```
GET https://localhost:5001/api/request/{requestId}/reject
```
