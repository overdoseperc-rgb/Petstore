# КТ №4 — Petstore, вариант 1

Работа на C# (.NET 8), тесты NUnit.

## Файлы

- `BaseRequest.cs` — общий класс для отправки запросов и проверки HTTP-статуса.
- `PetstoreTests.cs` — два теста: для питомца и для заказа.
- `Petstore.Tests.csproj` — настройки проекта и библиотеки NUnit.
- `NuGet.Config` — источник библиотек.
- `TEST_RESULTS.md` — результаты тестирования и инструкция по получению актуального отчёта.
- `.github/workflows/tests.yml` — автоматическое выполнение тестов в GitHub Actions с отчётом TRX.

## Запуск

Установить .NET SDK 8, открыть терминал в папке проекта:

```sh
dotnet test
```

Для сохранения отчёта:

```sh
dotnet test Petstore.Tests.csproj --logger "trx;LogFileName=PetstoreResults.trx" --results-directory TestResults
```

Нужен интернет. Команда выполняет реальные запросы к `https://petstore.swagger.io/v2/`.

## Запросы

Pet: POST `/pet`, GET `/pet/{id}`, PUT `/pet`, DELETE `/pet/{id}`.

Store: GET `/store/inventory`, POST `/store/order`, GET `/store/order/{id}`, DELETE `/store/order/{id}`.

Проверяются HTTP-статусы и поля ответов. После удаления выполняется GET с ожиданием 404. При ошибке созданные данные удаляются в finally. Базовый класс можно использовать и для путей `/user`; отдельный тест user заданием не требуется.

Питомцу назначается случайный ID. Заказу выбирается свободный ID от 1 до 10, как указано в документации. Если все заняты, тест выдаёт ошибку. На общем сервере возможны изменения данных другими пользователями.

## Результаты

В ранее опубликованном README указан запуск от 9 октября 2026 года: **2 passed, 0 failed**. Исходный машинный отчёт этого запуска не был сохранён. Подробности приведены в [TEST_RESULTS.md](TEST_RESULTS.md).

Новые запуски автоматически выполняются в [GitHub Actions](../../actions/workflows/tests.yml) при push, pull request, а также вручную. Для каждого запуска публикуется таблица результатов и доступен для скачивания файл `PetstoreResults.trx` (артефакт хранится 30 дней). Статус проверки зависит от реальных ответов API.

Документация: https://petstore.swagger.io/

Учебный пример: https://github.com/sadboy2001/api_testing
