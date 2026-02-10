# API Reference

The API is documented using standard REST principles.

## Swagger / OpenAPI

For interactive documentation, please visit:
`https://localhost:<port>/swagger`

## Endpoints

### Authentication

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/login` | Login to get JWT Token |

**Note**: Secure endpoints require `Authorization: Bearer <token>` header.

### Products

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create a new product (Requires Auth) |
| PUT | `/api/products/{id}` | Update a product (Requires Auth) |
| DELETE | `/api/products/{id}` | Delete a product (Requires Auth) |
| PATCH | `/api/products/{id}/price` | Change product price (Requires Auth) |

### Orders

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/orders` | Get all orders |
| GET | `/api/orders/{id}` | Get order by ID |
| POST | `/api/orders` | Create a new order |
