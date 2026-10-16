# КТ №4 — Petstore, вариант 1

Работа на C# (.NET 8), NUnit. **4 отдельных автоматизированных теста**.

## Состав проекта

- `BaseRequest.cs` — HTTP-запросы и проверка кодов ответа.
- `PetstoreTests.cs` — четыре теста: создание и получение питомца; изменение и удаление питомца; остатки магазина; работа с заказом.
- `Petstore.Tests.csproj` — проект NUnit.
- `TEST_RESULTS.md` — перечень тестов и результаты.
- `.github/workflows/tests.yml` — запуск тестов на GitHub Actions и публикация TRX-отчёта.

## Запуск

Для работы необходим .NET SDK 8 и интернет.

```bash
dotnet test
```

Запуск с сохранением отчёта:

```bash
dotnet test Petstore.Tests.csproj --logger "trx;LogFileName=PetstoreResults.trx" --results-directory TestResults
```

## Проверяемый API

Адрес: https://petstore.swagger.io/v2/

- Pet: POST `/pet`, GET `/pet/{id}`, PUT `/pet`, DELETE `/pet/{id}`.
- Store: GET `/store/inventory`, POST `/store/order`, GET `/store/order/{id}`, DELETE `/store/order/{id}`.

Проверяются коды ответа, поля объектов и отсутствие удалённых записей (HTTP 404). Для питомцев используются случайные ID. Для заказов выбирается свободный ID от 1 до 10. Поскольку Petstore — публичный сервис, тесты зависят от его состояния.

## Тестовые результаты

Смотрите [TEST_RESULTS.md](TEST_RESULTS.md) и [актуальные запуски GitHub Actions](../../actions/workflows/tests.yml). После каждого запуска сохраняется TRX-отчёт и отображается таблица с результатами четырёх тестов. Прошлый результат `2 passed` относится к прежней версии тестов.

Документация: https://petstore.swagger.io/

Пример: https://github.com/sadboy2001/api_testing
