# Component Diagram

This diagram shows the main components of the system and their interactions.

```mermaid
```markdown
componentDiagram
```
    package "Client Layer" {
        [Web Client/Postman] as Client
    }

    package "API Layer" {
        [ProductsController] as PC
        [OrdersController] as OC
    }

    package "Application Layer" {
        [ProductService] as PS
        [OrderService] as OS
    }

    package "Domain Layer" {
        [Product Entity] as PE
        [Order Entity] as OE
    }

    package "Infrastructure Layer" {
        [EfProductRepository] as EPR
        [EfOrderRepository] as EOR
        [Data Store] as DS
    }

    Client --> PC : HTTP/JSON
    Client --> OC : HTTP/JSON

    PC --> PS : Uses
    OC --> OS : Uses

    PS --> EPR : Uses
    OS --> EOR : Uses
    OS --> EPR : Uses (Check Product)

    EPR --> DS : Read/Write
    EOR --> DS : Read/Write

    PS ..> PE : Manipulates
    OS ..> OE : Manipulates
```
