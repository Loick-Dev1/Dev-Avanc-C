# Component Diagram

This diagram shows the main components of the system and their interactions.

```mermaid
graph TD
    subgraph ClientLayer [Client Layer]
        Client[Web Client/Postman]
    end

    subgraph APILayer [API Layer]
        PC[ProductsController]
        OC[OrdersController]
    end

    subgraph ApplicationLayer [Application Layer]
        PS[ProductService]
        OS[OrderService]
    end

    subgraph DomainLayer [Domain Layer]
        PE[Product Entity]
        OE[Order Entity]
    end

    subgraph InfrastructureLayer [Infrastructure Layer]
        EPR[EfProductRepository]
        EOR[EfOrderRepository]
        DS[Data Store]
    end

    Client -->|HTTP/JSON| PC
    Client -->|HTTP/JSON| OC

    PC -->|Uses| PS
    OC -->|Uses| OS

    PS -->|Uses| EPR
    OS -->|Uses| EOR
    OS -->|Uses| EPR

    EPR -->|Read/Write| DS
    EOR -->|Read/Write| DS

    PS -.->|Manipulates| PE
    OS -.->|Manipulates| OE
```
