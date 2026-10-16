from base_request import BaseRequest


api = BaseRequest()
pet = {"id": 987654321, "name": "Barsik", "photoUrls": [], "status": "available"}


def test_add_pet():
    response = api.post("/pet", pet)
    assert response.status_code == 200
    assert response.json()["name"] == "Barsik"


def test_get_pet():
    api.post("/pet", pet)
    response = api.get("/pet/987654321")
    assert response.status_code == 200
    assert response.json()["id"] == 987654321


def test_update_pet():
    api.post("/pet", pet)
    new_pet = {"id": 987654321, "name": "Murzik", "photoUrls": [], "status": "sold"}
    response = api.put("/pet", new_pet)
    assert response.status_code == 200
    assert response.json()["name"] == "Murzik"


def test_delete_pet():
    api.post("/pet", pet)
    response = api.delete("/pet/987654321")
    assert response.status_code == 200
