# ProductsApi

A RESTful API for managing products built with ASP.NET Core 8.0. This API provides CRUD operations for products with validation, exception handling, and soft delete functionality.

## Features

- ✅ **CRUD Operations**: Create, Read, Update, and Delete products
- ✅ **Soft Delete**: Products are marked as deleted instead of being permanently removed
- ✅ **Validation**: Input validation using FluentValidation
- ✅ **Exception Handling**: Centralized exception handling via middleware
- ✅ **Pagination**: List products with pagination support
- ✅ **Filtering**: Filter products by keyword and category
- ✅ **Swagger Documentation**: Interactive API documentation
- ✅ **In-Memory Storage**: Simple in-memory data store (no database required)

## Technologies

- **.NET 8.0**
- **ASP.NET Core Web API**
- **FluentValidation** - Input validation
- **Swashbuckle (Swagger)** - API documentation

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Any IDE that supports .NET (Visual Studio, VS Code, Rider, etc.)

## Getting Started

### Clone the Repository

```bash
git clone git@github.com:aboud426/ProductsApi.git
cd ProductsApi
```

### Run the Application

```bash
cd ProductsApi
dotnet run
```

The API will be available at:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`

### Swagger UI

Once the application is running, navigate to:
- **Swagger UI**: `https://localhost:5001/swagger` (or `http://localhost:5000/swagger`)

## API Endpoints

### Create Product
```http
POST /api/products
Content-Type: application/json

{
  "name": "Laptop",
  "category": "Electronics",
  "price": 999.99
}
```

### Get Product by ID
```http
GET /api/products/{id}
```

### Update Product
```http
PUT /api/products/{id}
Content-Type: application/json

{
  "name": "Gaming Laptop",
  "category": "Electronics",
  "price": 1299.99
}
```

### Delete Product (Soft Delete)
```http
DELETE /api/products/{id}
```

### List Products
```http
GET /api/products?keyword=laptop&category=Electronics&page=1&pageSize=10&includeDeleted=false
```

**Query Parameters:**
- `keyword` (optional): Filter by product name
- `category` (optional): Filter by category
- `page` (default: 1): Page number
- `pageSize` (default: 10): Items per page (max: 100)
- `includeDeleted` (default: false): Include soft-deleted products

## Project Structure

```
ProductsApi/
├── Controllers/
│   └── ProductController.cs      # API endpoints
├── Services/
│   ├── IProductService.cs         # Service interface
│   └── ProductService.cs          # Business logic
├── Repositories/
│   ├── IProductRepository.cs      # Repository interface
│   ├── ProductRepository.cs       # Data access
│   ├── IUnitOfWork.cs             # Unit of Work interface
│   ├── UnitOfWork.cs              # Unit of Work implementation
│   └── InMemoryDataStore.cs       # In-memory storage
├── Models/
│   └── Product.cs                 # Product entity
├── DTOs/
│   ├── CreateProductDTO.cs        # Create request DTO
│   ├── UpdateProductDTO.cs        # Update request DTO
│   ├── ProductDTO.cs              # Product response DTO
│   └── PaginatedResult.cs         # Pagination result
├── Exceptions/
│   ├── NotFoundException.cs       # Custom exception
│   └── BusinessException.cs       # Business exception
├── Middlewares/
│   ├── ExceptionHandlingMiddleware.cs  # Global exception handler
│   └── MiddlewareExtensions.cs    # Middleware extensions
└── Program.cs                     # Application entry point
```

## Architecture

### Clean Architecture Principles

- **Controllers**: Handle HTTP requests/responses only, no business logic
- **Services**: Contain business logic and validation
- **Repositories**: Handle data access operations
- **DTOs**: Data transfer objects for API contracts
- **Exceptions**: Custom exceptions for business rules

### Exception Handling

All exceptions are handled centrally by the `ExceptionHandlingMiddleware`:

- **ValidationException** → 400 Bad Request (with validation errors)
- **InvalidOperationException** → 400 Bad Request
- **NotFoundException** → 404 Not Found
- **Other exceptions** → 500 Internal Server Error

### Soft Delete

Products are not permanently deleted. Instead:
- The `IsDeleted` flag is set to `true`
- Deleted products are excluded from queries by default
- Use `includeDeleted=true` query parameter to include deleted products
- Products can be recovered if needed (by setting `IsDeleted = false`)

## Validation Rules

### Create Product
- `name`: Required, must be unique
- `category`: Required
- `price`: Required, must be greater than 0

### Update Product
- `name`: Required, must be unique (excluding current product)
- `category`: Required
- `price`: Required, must be greater than 0

## Example Requests

### Create a Product
```bash
curl -X POST "https://localhost:5001/api/products" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "iPhone 15",
    "category": "Electronics",
    "price": 999.99
  }'
```

### Get All Products
```bash
curl "https://localhost:5001/api/products?page=1&pageSize=10"
```

### Update a Product
```bash
curl -X PUT "https://localhost:5001/api/products/{id}" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "iPhone 15 Pro",
    "category": "Electronics",
    "price": 1199.99
  }'
```

### Delete a Product
```bash
curl -X DELETE "https://localhost:5001/api/products/{id}"
```

## Development

### Build the Project
```bash
dotnet build
```


## License

This project is open source and available under the MIT License.

## Author

Created by aboud426

