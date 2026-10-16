from base_request import BaseRequest


api = BaseRequest()
order = {"id": 7, "petId": 987654321, "quantity": 1, "status": "placed", "complete": False}


def test_inventory():
    response = api.get("/store/inventory")
    assert response.status_code == 200
    assert isinstance(response.json(), dict)


def test_add_order():
    response = api.post("/store/order", order)
    assert response.status_code == 200
    assert response.json()["id"] == 7


def test_get_order():
    api.post("/store/order", order)
    response = api.get("/store/order/7")
    assert response.status_code == 200
    assert response.json()["id"] == 7


def test_delete_order():
    api.post("/store/order", order)
    response = api.delete("/store/order/7")
    assert response.status_code == 200
