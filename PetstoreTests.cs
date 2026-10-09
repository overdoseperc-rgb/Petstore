using NUnit.Framework;

[TestFixture]
[NonParallelizable]
public class PetstoreTests
{
    private const string BaseUrl = "https://petstore.swagger.io/v2/";

    [Test]
    public async Task PetTest()
    {
        using var api = new BaseRequest(BaseUrl);
        long id = Random.Shared.NextInt64(1000000000, 2000000000);
        string name = "Barsik-" + id;
        bool created = false;

        try
        {
            // 1. Создание питомца.
            var pet = new { id, name, photoUrls = new string[] { }, status = "available" };
            var result = await api.Send("POST", "pet", pet);
            created = true;
            Assert.That(result.GetProperty("id").GetInt64(), Is.EqualTo(id));
            Assert.That(result.GetProperty("name").GetString(), Is.EqualTo(name));

            // 2. Получение питомца по ID.
            result = await api.Send("GET", $"pet/{id}");
            Assert.That(result.GetProperty("name").GetString(), Is.EqualTo(name));

            // 3. Изменение имени и статуса.
            var updatedPet = new { id, name = name + "-updated", photoUrls = new string[] { }, status = "sold" };
            result = await api.Send("PUT", "pet", updatedPet);
            Assert.That(result.GetProperty("name").GetString(), Is.EqualTo(name + "-updated"));
            Assert.That(result.GetProperty("status").GetString(), Is.EqualTo("sold"));
            result = await api.Send("GET", $"pet/{id}");
            Assert.That(result.GetProperty("name").GetString(), Is.EqualTo(name + "-updated"));
            Assert.That(result.GetProperty("status").GetString(), Is.EqualTo("sold"));

            // 4. Удаление питомца.
            await api.Send("DELETE", $"pet/{id}");
            created = false;
            await api.Send("GET", $"pet/{id}", expectedStatus: 404);
        }
        finally
        {
            if (created)
                await api.Send("DELETE", $"pet/{id}");
        }
    }

    [Test]
    public async Task StoreTest()
    {
        using var api = new BaseRequest(BaseUrl);
        int orderId = 0;
        long petId = Random.Shared.NextInt64(1000000000, 2000000000);
        bool created = false;

        // Для GET заказа документация задаёт ID от 1 до 10.
        // Выбираем свободный ID, чтобы не заменять существующий заказ.
        for (int i = 1; i <= 10; i++)
        {
            try
            {
                await api.Send("GET", $"store/order/{i}", expectedStatus: 404);
                orderId = i;
                break;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Этот ID уже занят.
            }
        }
        Assert.That(orderId, Is.GreaterThan(0), "Все ID заказов от 1 до 10 заняты");

        try
        {
            // 1. Получение остатков магазина.
            var result = await api.Send("GET", "store/inventory");
            foreach (var item in result.EnumerateObject())
                Assert.That(item.Value.GetInt32(), Is.GreaterThanOrEqualTo(0));

            // 2. Создание заказа.
            var order = new { id = orderId, petId, quantity = 1, status = "placed", complete = false };
            result = await api.Send("POST", "store/order", order);
            created = true;
            Assert.That(result.GetProperty("id").GetInt32(), Is.EqualTo(orderId));
            Assert.That(result.GetProperty("petId").GetInt64(), Is.EqualTo(petId));

            // 3. Получение заказа по ID.
            result = await api.Send("GET", $"store/order/{orderId}");
            Assert.That(result.GetProperty("petId").GetInt64(), Is.EqualTo(petId));
            Assert.That(result.GetProperty("quantity").GetInt32(), Is.EqualTo(1));
            Assert.That(result.GetProperty("status").GetString(), Is.EqualTo("placed"));

            // 4. Удаление заказа.
            await api.Send("DELETE", $"store/order/{orderId}");
            created = false;
            await api.Send("GET", $"store/order/{orderId}", expectedStatus: 404);
        }
        finally
        {
            if (created)
                await api.Send("DELETE", $"store/order/{orderId}");
        }
    }
}
