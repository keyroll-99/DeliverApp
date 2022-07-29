# Backend
Backend has a layer model

* Controllers is of top of layer, it place where all reqest from frontend come
* Service is a layer where we should keep business logic 
* Integrations is a layer where we keep all integrations logic, like sending e-mail
* Utils is a layer response for connect two or more business part. Two services shouldn't know about themeselves
* Repository is a layer response for connect with database, but we can write linq query in higher layer. Only we shouldn't write query in Controller layer.

Layer can know and call layer which is below, but can't call layer which is higher.

Oczywiście w folderze test mamy wszystkie testy, każda metoda realizująca logikę biznesową powinna mieć własny test.




