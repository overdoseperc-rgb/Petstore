using NUnit.Framework;

[TestFixture]
[NonParallelizable]
public class PetstoreTests
{
    private const string BaseUrl = "https://petstore.swagger.io/v2/";

    [Test]
    public async Task PetCreateAndGetTest()
    {
        using var api = new BaseRequest(BaseUrl);
        long id = Random.Shared.NextInt64(1000000000, 2000000000);
        string name = "Barsik-" + id;
        bool created = false;

        try
        {
            var pet = new { id, name, photoUrls = Array.Empty<string>(), status = "available" };
            var result = await api.Send("POST", "pet", pet);
            created = true;
            Assert.That(result.GetProperty("id").GetInt64(), Is.EqualTo(id));
            Assert.That(result.GetProperty("name").GetString(), Is.EqualTo(name));

            result = await api.Send("GET", $"pet/{id}");
            Assert.That(result.GetProperty("id").GetInt64(), Is.EqualTo(id));
            Assert.That(result.GetProperty("name").GetString(), Is.EqualTo(name));
        }
        finally
        {
            if (created)
                await api.Send("DELETE", $"pet/{id}");
        }
    }

    [Test]
    public async Task PetUpdateAndDeleteTest()
    {
        using var api = new BaseRequest(BaseUrl);
        long id = Random.Shared.NextInt64(1000000000, 2000000000);
        string name = "Barsik-" + id;
        bool created = false;

        try
        {
            await api.Send("POST", "pet", new { id, name, photoUrls = Array.Empty<string>(), status = "available" });
            created = true;

            var updatedPet = new { id, name = name + "-updated", photoUrls = Array.Empty<string>(), status = "sold" };
            var result = await api.Send("PUT", "pet", updatedPet);
            Assert.That(result.GetProperty("name").GetString(), Is.EqualTo(updatedPet.name));
            Assert.That(result.GetProperty("status").GetString(), Is.EqualTo("sold"));

            result = await api.Send("GET", $"pet/{id}");
            Assert.That(result.GetProperty("name").GetString(), Is.EqualTo(updatedPet.name));
            Assert.That(result.GetProperty("status").GetString(), Is.EqualTo("sold"));

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
    public async Task StoreInventoryTest()
    {
        using var api = new BaseRequest(BaseUrl);
        var result = await api.Send("GET", "store/inventory");

        Assert.That(result.ValueKind, Is.EqualTo(System.Text.Json.JsonValueKind.Object));
        foreach (var item in result.EnumerateObject())
            Assert.That(item.Value.GetInt32(), Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public async Task StoreOrderTest()
    {
        using var api = new BaseRequest(BaseUrl);
        int orderId = 0;
        long petId = Random.Shared.NextInt64(1000000000, 2000000000);
        bool created = false;

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
            }
        }

        Assert.That(orderId, Is.GreaterThan(0), "Все ID заказов от 1 до 10 заняты");

        try
        {
            var order = new { id = orderId, petId, quantity = 1, status = "placed", complete = false };
            var result = await api.Send("POST", "store/order", order);
            created = true;
            Assert.That(result.GetProperty("id").GetInt32(), Is.EqualTo(orderId));
            Assert.That(result.GetProperty("petId").GetInt64(), Is.EqualTo(petId));

            result = await api.Send("GET", $"store/order/{orderId}");
            Assert.That(result.GetProperty("petId").GetInt64(), Is.EqualTo(petId));
            Assert.That(result.GetProperty("quantity").GetInt32(), Is.EqualTo(1));
            Assert.That(result.GetProperty("status").GetString(), Is.EqualTo("placed"));

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
