\# E-Commerce API



A full-featured E-Commerce REST API built with \*\*ASP.NET Core 10\*\*, \*\*C#\*\*, \*\*Entity Framework Core\*\*, and \*\*SQL Server\*\*.



The API provides user authentication, JWT-based authorization, role-based access control, product and category management, shopping cart functionality, checkout, order management, inventory control, validation, database transactions, and concurrency protection.



\---



\## 🚀 Features



\### 🔐 Authentication \& Authorization



\- User registration

\- User login

\- Secure password hashing

\- JWT Bearer authentication

\- Role-based authorization

\- Customer and Admin roles

\- Admin account seeding

\- Protected API endpoints

\- User-specific resource access



\### 📦 Product Management



\- Create products

\- Get all products

\- Get product by ID

\- Update products

\- Delete products

\- Product/category relationship

\- Product stock management

\- Pagination

\- Filtering/search functionality



\### 🗂️ Category Management



\- Create categories

\- Get categories

\- Update categories

\- Delete categories

\- Product-category relationship



\### 🛒 Shopping Cart



\- Add products to cart

\- View current user's cart

\- Update cart quantity

\- Remove products from cart

\- JWT-based cart ownership

\- Stock validation



\### 📋 Checkout \& Orders



\- Checkout directly from the user's cart

\- Automatic order creation

\- Automatic order item creation

\- Automatic total calculation

\- Inventory deduction

\- Cart clearing after checkout

\- Database transaction during checkout

\- Customer order history

\- Admin access to all orders

\- Order status management



\### 🛡️ Reliability \& Validation



\- DTO-based request/response models

\- DTO-level validation

\- Controller-level validation

\- Global exception handling

\- Consistent API error responses

\- Appropriate HTTP status codes

\- Database transactions

\- EF Core concurrency protection

\- RowVersion-based concurrency handling

\- Service-layer business logic



\---



\# 🛠️ Tech Stack



| Technology | Purpose |

|---|---|

| C# | Backend programming language |

| ASP.NET Core 10 | REST API framework |

| Entity Framework Core | ORM and database access |

| SQL Server | Relational database |

| JWT | Authentication and authorization |

| PasswordHasher | Secure password hashing |

| REST | API architecture |

| Postman | API testing |

| Git | Version control |

| GitHub | Source code hosting |



\---



\# 🏗️ Architecture



The application follows a layered architecture where responsibilities are separated between controllers, services, data access, and models.



```text

&#x20;                   Client

&#x20;                     │

&#x20;                     │ HTTP / JSON

&#x20;                     ▼

&#x20;              ┌───────────────┐

&#x20;              │  Controllers  │

&#x20;              └───────┬───────┘

&#x20;                      │

&#x20;                      ▼

&#x20;              ┌───────────────┐

&#x20;              │   Services    │

&#x20;              └───────┬───────┘

&#x20;                      │

&#x20;                      ▼

&#x20;              ┌───────────────┐

&#x20;              │   EF Core     │

&#x20;              │  AppDbContext │

&#x20;              └───────┬───────┘

&#x20;                      │

&#x20;                      ▼

&#x20;              ┌───────────────┐

&#x20;              │   SQL Server  │

&#x20;              └───────────────┘

```



Controllers are responsible for handling HTTP requests and responses, while services contain the application's business logic.



\---



\# 📁 Project Structure



```text

ECommerceApi/

│

├── Controllers/

│   ├── AuthController

│   ├── ProductController

│   ├── CategoryController

│   ├── CartController

│   └── OrderController

│

├── Data/

│   └── AppDbContext

│

├── DTOs/

│   ├── Authentication DTOs

│   ├── Product DTOs

│   ├── Cart DTOs

│   ├── Order DTOs

│   └── Response DTOs

│

├── Exceptions/

│

├── MiddleWares/

│

├── Migrations/

│

├── Models/

│   ├── User

│   ├── Product

│   ├── Category

│   ├── CartItem

│   ├── Order

│   └── OrderItem

│

├── Services/

│   ├── AuthService

│   ├── ProductService

│   ├── CartService

│   └── OrderService

│

├── Properties/

│

├── Program.cs

├── ECommerceApi.csproj

├── ECommerceApi.http

├── appsettings.json

├── .gitignore

└── README.md

```



\---



\# 🗄️ Database Structure



The application uses \*\*SQL Server\*\* with \*\*Entity Framework Core\*\*.



\## Main Entities



\### User



```text

User

├── UserId

├── Name

├── Email

├── PasswordHash

├── Role

└── CreatedAt

```



\### Category



```text

Category

├── CategoryId

├── Name

└── Description

```



\### Product



```text

Product

├── ProductId

├── Name

├── Description

├── Price

├── StockQuantity

├── ImageUrl

├── CategoryId

├── CreatedAt

└── RowVersion

```



\### CartItem



```text

CartItem

├── CartItemId

├── UserId

├── ProductId

└── Quantity

```



\### Order



```text

Order

├── OrderId

├── UserId

├── TotalAmount

├── Status

└── CreatedAt

```



\### OrderItem



```text

OrderItem

├── OrderItemId

├── OrderId

├── ProductId

├── Quantity

└── UnitPrice

```



\## Entity Relationships



```text

User

&#x20;│

&#x20;├──────────────► Orders

&#x20;│

&#x20;└──────────────► CartItems



Category

&#x20;│

&#x20;└──────────────► Products

&#x20;                      │

&#x20;                      ├────────► CartItems

&#x20;                      │

&#x20;                      └────────► OrderItems



Order

&#x20;│

&#x20;└──────────────► OrderItems

&#x20;                      │

&#x20;                      └────────► Product

```



\---



\# 🔐 Authentication Flow



The API uses \*\*JWT Bearer Authentication\*\*.



```text

&#x20;               Register

&#x20;                  │

&#x20;                  ▼

&#x20;         Password is hashed

&#x20;                  │

&#x20;                  ▼

&#x20;            User created

&#x20;                  │

&#x20;                  ▼

&#x20;                Login

&#x20;                  │

&#x20;                  ▼

&#x20;           Credentials checked

&#x20;                  │

&#x20;                  ▼

&#x20;            JWT generated

&#x20;                  │

&#x20;                  ▼

&#x20;     Authorization: Bearer <token>

&#x20;                  │

&#x20;                  ▼

&#x20;         Protected API endpoint

&#x20;                  │

&#x20;                  ▼

&#x20;         JWT token validated

```



The application supports:



```text

Customer

Admin

```



Admin-only operations are protected using role-based authorization.



Example:



```csharp

\[Authorize(Roles = "Admin")]

```



\---



\# 🔑 JWT Configuration



Sensitive JWT credentials are not stored directly in the GitHub repository.



The JWT signing key is stored locally using \*\*.NET User Secrets\*\*.



Example:



```bash

dotnet user-secrets set "Jwt:Key" "YOUR\_SECRET\_KEY"

```



The application uses:



```text

Jwt:Key

Jwt:Issuer

Jwt:Audience

```



The issuer and audience are stored in normal application configuration, while the secret signing key is kept outside source control.



\---



\# 👨‍💼 Admin Account



The application includes admin account seeding.



Admin credentials are stored using \*\*.NET User Secrets\*\* rather than being committed to source control.



Example:



```bash

dotnet user-secrets set "Admin:Email" "admin@example.com"

dotnet user-secrets set "Admin:Password" "YOUR\_ADMIN\_PASSWORD"

```



This prevents administrator credentials from being exposed in the GitHub repository.



\---



\# 🛒 Shopping Cart Flow



A user's cart is associated with their authenticated account.



```text

Login

&#x20; │

&#x20; ▼

JWT Token

&#x20; │

&#x20; ▼

Add Product to Cart

&#x20; │

&#x20; ▼

Cart belongs to authenticated User

&#x20; │

&#x20; ├── View Cart

&#x20; ├── Update Quantity

&#x20; └── Remove Item

```



Cart operations use the authenticated user's identity from the JWT rather than trusting a user ID supplied by the client.



\---



\# 💳 Checkout Flow



Checkout is handled using a database transaction.



```text

&#x20;                User Cart

&#x20;                   │

&#x20;                   ▼

&#x20;             Validate Cart

&#x20;                   │

&#x20;                   ▼

&#x20;             Validate Stock

&#x20;                   │

&#x20;                   ▼

&#x20;           Calculate Total

&#x20;                   │

&#x20;                   ▼

&#x20;             Create Order

&#x20;                   │

&#x20;                   ▼

&#x20;         Create Order Items

&#x20;                   │

&#x20;                   ▼

&#x20;          Decrease Stock

&#x20;                   │

&#x20;                   ▼

&#x20;             Clear Cart

&#x20;                   │

&#x20;                   ▼

&#x20;            Save Changes

&#x20;                   │

&#x20;                   ▼

&#x20;          Commit Transaction

```



If an unexpected error occurs during checkout, the transaction is rolled back.



This prevents situations where an order is created but stock or cart changes are only partially completed.



\---



\# 🔄 Concurrency Protection



Product inventory uses \*\*EF Core concurrency protection\*\* through a `RowVersion` property.



This helps protect stock updates when multiple requests attempt to modify the same product simultaneously.



Conceptually:



```text

Request A ───────► Product Stock

&#x20;                      │

Request B ───────►     │

&#x20;                      ▼

&#x20;               RowVersion check

&#x20;                      │

&#x20;                ┌─────┴─────┐

&#x20;                │           │

&#x20;             Success     Conflict

```



This helps reduce the risk of incorrect inventory updates caused by simultaneous requests.



\---



\# 📡 API Overview



\## Authentication



```text

POST   /api/auth/register

POST   /api/auth/login

GET    /api/auth/users

GET    /api/auth/users/{id}

```



\## Products



```text

GET    /api/products

GET    /api/products/{id}

POST   /api/products

PUT    /api/products/{id}

DELETE /api/products/{id}

```



\## Categories



```text

GET    /api/categories

GET    /api/categories/{id}

POST   /api/categories

PUT    /api/categories/{id}

DELETE /api/categories/{id}

```



\## Cart



```text

GET    /api/cart

POST   /api/cart

PUT    /api/cart/{id}

DELETE /api/cart/{id}

```



\## Orders



```text

POST   /api/orders/checkout

GET    /api/orders

GET    /api/orders/{id}

```



Admin-protected endpoints are available for administrative operations such as managing products, categories, and viewing/managing orders.



> Endpoint routes may vary slightly depending on the current controller route configuration.



\---



\# 🧪 API Testing



The API has been tested using \*\*Postman\*\*.



Typical customer flow:



```text

Register

&#x20;  │

&#x20;  ▼

Login

&#x20;  │

&#x20;  ▼

Receive JWT

&#x20;  │

&#x20;  ▼

Add Bearer Token

&#x20;  │

&#x20;  ▼

Products

&#x20;  │

&#x20;  ▼

Cart

&#x20;  │

&#x20;  ▼

Checkout

&#x20;  │

&#x20;  ▼

Orders

```



Admin flow:



```text

Admin Login

&#x20;    │

&#x20;    ▼

Receive Admin JWT

&#x20;    │

&#x20;    ▼

Bearer Authentication

&#x20;    │

&#x20;    ▼

Admin Protected Endpoints

```



\---



\# ⚙️ Getting Started



\## Prerequisites



Install the following:



\- .NET 10 SDK

\- SQL Server

\- Git

\- Visual Studio or VS Code

\- Entity Framework Core CLI



\---



\## 1. Clone the Repository



```bash

git clone https://github.com/samritpaudel62/ECommerce-ASP.NET-Core.git

```



```bash

cd ECommerce-ASP.NET-Core

```



\---



\## 2. Configure User Secrets



Initialize User Secrets:



```bash

dotnet user-secrets init

```



Set the JWT secret:



```bash

dotnet user-secrets set "Jwt:Key" "YOUR\_SECRET\_KEY"

```



Set the admin credentials:



```bash

dotnet user-secrets set "Admin:Email" "admin@example.com"

dotnet user-secrets set "Admin:Password" "YOUR\_ADMIN\_PASSWORD"

```



Verify:



```bash

dotnet user-secrets list

```



\---



\## 3. Configure SQL Server



The application uses SQL Server.



Example local connection string:



```text

Server=localhost;Database=ECommerceDb;Trusted\_Connection=True;TrustServerCertificate=True;Encrypt=False;

```



For production environments, database credentials should be managed through secure environment configuration or a secrets manager.



\---



\## 4. Apply EF Core Migrations



Run:



```bash

dotnet ef database update

```



If the Entity Framework CLI is not installed:



```bash

dotnet tool install --global dotnet-ef

```



Then run:



```bash

dotnet ef database update

```



\---



\## 5. Run the API



```bash

dotnet run

```



The API will start using the configured HTTP/HTTPS endpoints.



\---



\# 🔒 Security



The project follows several security practices:



\- Passwords are hashed before storage

\- JWT authentication protects secured endpoints

\- Role-based authorization protects admin operations

\- Users can only access their own cart and orders where applicable

\- Sensitive JWT secrets are stored using User Secrets

\- Admin credentials are stored outside source control

\- Database operations use EF Core

\- Checkout uses database transactions

\- Product inventory uses concurrency protection

\- DTOs are used to control request and response data



\### Never commit secrets



The following should never be committed to GitHub:



```text

JWT signing keys

Admin passwords

Production database passwords

API keys

Environment secrets

```



\---



\# 📱 Frontend Integration



The API is designed to be consumed by a frontend application such as \*\*React\*\*.



Planned architecture:



```text

┌──────────────────────┐

│    React Frontend    │

└──────────┬───────────┘

&#x20;          │

&#x20;          │ HTTP / JSON

&#x20;          ▼

┌──────────────────────┐

│ ASP.NET Core Web API │

└──────────┬───────────┘

&#x20;          │

&#x20;          ▼

┌──────────────────────┐

│    Entity Framework  │

│        Core          │

└──────────┬───────────┘

&#x20;          │

&#x20;          ▼

┌──────────────────────┐

│      SQL Server      │

└──────────────────────┘

```



JWT authentication allows the React frontend to authenticate users and access protected API resources.



\---



\# 🔮 Future Improvements



The following features can be added in future development:



\- React frontend

\- Refresh token authentication

\- Email verification

\- Password reset

\- Payment gateway integration

\- Product image upload/storage

\- Automated unit tests

\- Integration tests

\- Docker support

\- CI/CD pipeline

\- Cloud deployment

\- Redis caching

\- Rate limiting

\- API versioning

\- Advanced logging and monitoring



\---



\# 📌 Project Status



\### Backend Status: Stable and ready for frontend integration



The core backend functionality has been implemented and tested using Postman, including:



\- Authentication

\- JWT authorization

\- Admin authorization

\- Product CRUD

\- Category CRUD

\- Cart management

\- Checkout

\- Order creation

\- Order retrieval

\- Admin order management

\- DTO validation

\- Exception handling

\- Transactions

\- Inventory management

\- Concurrency protection



The next major development stage is integration with a React frontend.



\---



\# 👨‍💻 Author



\*\*Samrit Paudel\*\*



GitHub:



https://github.com/samritpaudel62



\---



\# 📄 License



This project is currently intended as a learning and portfolio project.

