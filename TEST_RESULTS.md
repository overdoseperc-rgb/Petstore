# Результаты тестирования Petstore API

Проект: C# (.NET 8), NUnit. API: https://petstore.swagger.io/v2/

## Четыре автоматизированных теста

| Тест | Проверка |
| --- | --- |
| `PetCreateAndGetTest` | POST /pet, GET /pet/{id}, проверка ID и имени |
| `PetUpdateAndDeleteTest` | POST /pet, PUT /pet, GET /pet/{id}, DELETE /pet, проверка изменённых полей и HTTP 404 |
| `StoreInventoryTest` | GET /store/inventory, проверка количества товаров |
| `StoreOrderTest` | POST /store/order, GET /store/order/{id}, DELETE /store/order/{id}, проверка полей и HTTP 404 |

## Актуальный результат

Проверяйте последний запуск в [GitHub Actions](https://github.com/overdoseperc-rgb/Petstore/actions/workflows/tests.yml). Количество и исход каждого теста появляются в разделе Summary; файл `PetstoreResults.trx` доступен как артефакт `petstore-test-results`.

## Предыдущие результаты

В исходном README был указан результат от **09.10.2026: 2 passed, 0 failed**. Этот результат относился к старым двум объединённым тестам. Исходный TRX того запуска отсутствует, поэтому историческая запись не подтверждена машинным отчётом.

## Локальный запуск

```bash
dotnet test Petstore.Tests.csproj --logger "trx;LogFileName=PetstoreResults.trx" --results-directory TestResults
```

API публичный и общий, поэтому тесты могут зависеть от доступности сервера и действий других пользователей.
