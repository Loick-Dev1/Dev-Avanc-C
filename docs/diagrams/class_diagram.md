# Class Diagram

This diagram shows the domain entities and their relationships.

```mermaid
classDiagram
    class Customer {
        +Guid Id
        +string FirstName
        +string LastName
        +string Email
    }

    class Provider {
        +Guid Id
        +string Name
        +string Email
    }

    class Product {
        +Guid Id
        +Guid ProviderId
        +decimal Price
        +bool IsActive
        +ChangePrice(decimal newPrice)
        +ApplyDiscount(decimal discount)
        +Activate()
        +Desactivate()
    }

    class Order {
        +Guid Id
        +Guid CustomerId
        +DateTime OrderDate
        +decimal TotalAmount
        +IReadOnlyCollection~OrderItem~ Items
        +AddItem(Product product, int quantity)
        -RecalculateTotal()
    }

    class OrderItem {
        +Guid ProductId
        +decimal Price
        +int Quantity
        +AddQuantity(int quantity)
    }

    Order *-- OrderItem : Contains
    OrderItem ..> Product : References
    Product --> Provider : Supplied By
    Order --> Customer : Placed By
```
