import requests


class BaseRequest:
    def __init__(self):
        self.base_url = "https://petstore.swagger.io/v2"

    def get(self, path):
        return requests.get(self.base_url + path, timeout=10)

    def post(self, path, data):
        return requests.post(self.base_url + path, json=data, timeout=10)

    def put(self, path, data):
        return requests.put(self.base_url + path, json=data, timeout=10)

    def delete(self, path):
        return requests.delete(self.base_url + path, timeout=10)
