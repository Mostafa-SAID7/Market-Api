1. الخلاصة المعمارية أولًا

المشروع حاليًا مشروع Clean Architecture جيد كبداية، وفيه Feature-based CQRS داخل Application، لكن عندك مشكلة مهمة:

الـ structure شكله Clean Architecture أكثر مما هو Clean Architecture فعليًا.

يعني الـ folders والـ documentation بيقولوا:

API
 ↓
Application
 ↓
Domain
 ↑
Infrastructure

لكن التنفيذ الفعلي فيه boundaries مكسورة في أماكن مهمة.

وأهم شيء أنا شايفه:

Critical Architecture Issues
المشكلة	الخطورة
Duplicate IUnitOfWork بين Domain وInfrastructure	🔴 Critical
Repository بيعمل SaveChangesAsync() بنفسه	🔴 Critical
Unit of Work بالتالي مش ماسك transaction boundary حقيقية	🔴 Critical
Infrastructure يرجع ProductResponse من Application	🔴 Critical
IProductQueryRepository موجود داخل Infrastructure	🔴 Critical
Application CQRS مش مستفيد بالكامل من Pipeline Behaviors	🟠 High
Querying حاليًا ممكن يحمل collections كبيرة بدون pagination	🔴 Critical للـ scale
CreateProductCommand يستخدم MediatR داخل MediatR بشكل غير ضروري	🟠 High
Error handling وValidation middleware متداخلين	🟠 High
ExceptionHandlingMiddleware يعرض exception.Message للـ client	🔴 Security
Database migration/seeding عند startup	🟠 Production concern
Domain entities ما زالت Anemic جزئيًا وpublic setters واسعة	🟠 High
لا يوجد concurrency strategy واضح	🔴 Critical للـ Cart/Order/Inventory
لا يوجد explicit transaction strategy للـ business operations المركبة	🔴 Critical
Tests الموجودة حاليًا placeholders عمليًا	🔴 Critical
.NET 9 أصبح قريب جدًا من نهاية الدعم	🟠 Strategic

والأخير مهم جدًا: المشروع مستهدف .NET 9، وحاليًا في سبتمبر 2026، و.NET 9 في maintenance وينتهي دعمه 10 نوفمبر 2026، بينما .NET 10 LTS مدعوم حتى 14 نوفمبر 2028.

2. أولًا: إيه الـ Architecture المناسبة للمشروع؟

أنا مش أنصحك تحول المشروع Microservices الآن.

الأنسب للـ Market API حاليًا:

Modular Monolith
+
Clean Architecture
+
Vertical Slice / CQRS
+
DDD-lite
+
EF Core

يعني:

                    Market API
                        │
             ┌──────────┴──────────┐
             │                     │
       Presentation           Application
             │                     │
             └──────────┬──────────┘
                        │
                     Domain
                        ↑
                        │
                 Infrastructure

لكن داخليًا Application تكون:

Features
├── Catalog
│   ├── Products
│   └── Categories
│
├── Ordering
│   ├── Orders
│   └── Cart
│
├── Identity
│   └── Users
│
├── Vendors
│
└── Reviews

وده أهم من مجرد:

Products
Categories
Orders
...

لأنك بتبني business boundaries وليس folders فقط.

3. أهم مشكلة وجدتها: Unit of Work

عندك:

Market.Domain
└── Repositories
    └── IUnitOfWork.cs

وفي نفس الوقت:

Market.Infrastructure
└── Persistence
    └── UnitOfWork
        ├── IUnitOfWork.cs
        └── UnitOfWork.cs

دي architectural smell قوية جدًا.

لأن عندك abstraction باسم واحد في مكانين مختلفين.

والأسوأ إن Infrastructure.DependencyInjection يسجل IUnitOfWork الموجود في Infrastructure namespace، بينما Application handlers تعتمد على Market.Domain.Repositories.IUnitOfWork.

يعني عندك potential DI boundary mismatch.

الحل

احتفظ بـ abstraction واحدة فقط.

وأنا في المشروع ده أميل إلى:

Application
└── Abstractions
    └── Persistence
        ├── IUnitOfWork.cs
        ├── IProductRepository.cs
        └── ...

والـ Domain لا يعرف EF ولا يحتاج Repository abstractions لكل شيء.

ثم:

Application
      ↓
IProductRepository
      ↑
Infrastructure
ProductRepository
4. والأهم: Repository عندك بيعمل SaveChanges

دي عندك في:

Repository<T>

مثلًا:

_dbSet.Add(entity);
await _context.SaveChangesAsync(cancellationToken);

و:

_dbSet.Update(entity);
await _context.SaveChangesAsync(cancellationToken);

وده ضد فكرة الـ Unit of Work.

لأن عندك Handler بيعمل:

await _unitOfWork.Products.CreateAsync(product);
await _unitOfWork.SaveAsync();

فالعملية أصبحت تقريبًا:

CreateAsync
   ↓
SaveChanges

ثم

SaveAsync
   ↓
SaveChanges مرة ثانية

وده يكسر transaction boundary.

5. الـ Repository المفروض يعمل إيه؟

Repository:

Load
Add
Update
Remove
Exists

لكن:

❌ SaveChanges
❌ BeginTransaction
❌ Commit
❌ Rollback

الـ Application operation هي اللي تحدد:

Business Operation
       ↓
Modify Aggregate(s)
       ↓
Unit of Work
       ↓
SaveChanges

مثال:

var order = await _orders.GetByIdAsync(...);

order.AddItem(...);
order.CalculateTotal();

await _unitOfWork.SaveChangesAsync(cancellationToken);

ولو العملية كبيرة:

Create Order
   ↓
Reserve Inventory
   ↓
Create Order Items
   ↓
Update Cart
   ↓
SaveChanges

كلها transaction واحدة لو داخل نفس database boundary.

6. عندك مشكلة تانية أعمق: Query Repository

عندك:

Infrastructure
└── Persistence
    └── Repositories
        └── IProductQueryRepository.cs

والـ interface نفسه يستخدم:

ProductResponse

اللي موجود في:

Application
└── Features
    └── Products
        └── ProductResponse.cs

يعني:

Infrastructure
      ↓
Application DTO

وده عكس اتجاه dependency المطلوب.

الـ Infrastructure لا يجب أن يعرف:

ProductResponse
CreateProductCommand
GetProductQuery
Controller
HTTP
الأفضل

الـ contract يكون في Application:

Application
└── Abstractions
    └── Persistence
        └── IProductReadRepository.cs

والـ implementation:

Infrastructure
└── Persistence
    └── ProductReadRepository.cs

وبالتالي:

Application
   │
   │ interface
   ↓
Infrastructure
   │
   │ EF Core
   ↓
SQL Server
7. لكن عندي Recommendation أقوى للـ Queries

بما إنك عامل CQRS، أنا لا أريدك تعمل:

Query
 ↓
Generic Repository
 ↓
Entity
 ↓
Map DTO

في الـ read-heavy endpoints.

الأفضل:

Query
 ↓
IProductReadService
 ↓
SQL projection
 ↓
ProductListItemDto

مثال:

GET /products

يكون:

GetProductsQuery
       ↓
GetProductsQueryHandler
       ↓
IProductReadRepository
       ↓
EF Core IQueryable internally
       ↓
Select(...)
       ↓
Pagination
       ↓
DTO

وبالتالي:

SQL Server
   ↓
only required columns
   ↓
DTO

وليس:

SQL Server
   ↓
Product entities
   ↓
navigation properties
   ↓
memory
   ↓
DTO

وده مهم جدًا للـ scalability.

Microsoft نفسها توصي بتجنب إعادة collections كبيرة عبر HTTP وتقليل allocations وتحسين data access/I/O واستخدام pagination.

8. GetAllProductsQuery عندك حاليًا مشكلة scalability

عندك:

var products = await _unitOfWork.Products.GetAllAsync(cancellationToken);

ثم:

products.Select(...)

يعني:

Database
   ↓
كل Products
   ↓
Memory
   ↓
Mapping

لو عندك:

1,000 products

مش كارثة.

لكن:

100,000
500,000
1,000,000

هتبقى مشكلة.

لازم endpoint يكون:

GET /api/products?page=1&pageSize=20

والـ database تعمل:

OFFSET ...
FETCH ...

أو equivalent EF:

.Skip(...)
.Take(...)

والأفضل مع ordering ثابت.

9. لازم تعمل Cursor Pagination للـ hot endpoints لاحقًا

مش كل endpoints تحتاج Cursor Pagination.

استخدم:

Normal pagination
Admin
Category
Search
Backoffice
Cursor pagination

لـ:

Product feeds
Popular products
Infinite scrolling
High traffic catalog
Orders timeline

مثل:

GET /products?cursor=eyJpZCI6...

وده يديك scalability أفضل من page numbers لما dataset يكبر جدًا.

10. Product Entity محتاجة Boundary أقوى

حاليًا:

public string Name { get; set; }
public decimal Price { get; set; }
public int Quantity { get; set; }
public ProductStatus Status { get; set; }

أي consumer يقدر يعمل:

product.Quantity = -100;
product.Price = -500;
product.Status = ...

أنت عندك بعض behavior:

AddTags()
RemoveTag()
CalculateProfit()
IsInStock

وده جيد.

لكن محتاج تنقل النموذج أكثر إلى:

public class Product : AggregateRoot
{
    private Product() { }

    public string Name { get; private set; }
    public Money Price { get; private set; }
    public int Quantity { get; private set; }

    public void ChangePrice(...)
    public void IncreaseStock(...)
    public void DecreaseStock(...)
    public void Activate()
    public void Deactivate()
}

مش لازم تعمل DDD كامل، لكن business invariants لازم تكون protected.

11. Order هو أهم Aggregate عندك

ده أهم جزء في الـ ecommerce.

أنا أعتبر:

Order
 ├── OrderItems
 ├── Pricing
 ├── Payment state
 └── Shipping state

Aggregate boundary.

يعني ما ينفعش Application تعمل:

order.OrderStatus = ...
order.TotalPrice = ...

في كل مكان.

الأفضل:

order.Confirm();
order.Cancel();
order.MarkAsPaid();
order.MarkAsShipped();

وده يخلي state transitions controlled.

12. عندك مشكلة Domain Modeling في Cart/Order

مثلًا:

public string ProductName
public decimal Price

موجودين في:

CartItem
OrderItem

وده مش غلط بالضرورة.

لكن لازم تفرق:

CartItem

السعر الحالي ممكن يتغير.

OrderItem

السعر يجب أن يكون snapshot وقت الشراء.

يعني:

Product.Price
     ↓
OrderItem.UnitPrice

ولا تعتمد على Product بعد إنشاء Order.

وده مهم جدًا business-wise.

13. Inventory هو أكبر concurrency risk

عندك:

Product.Quantity
Product.Sold

لكن مفيش:

RowVersion

ولا optimistic concurrency strategy.

تخيل:

Stock = 1

User A:

Read = 1

User B:

Read = 1

A:

Buy

B:

Buy

ممكن الاثنين يعدوا.

لازم تدخل:

Optimistic Concurrency

مثل:

public byte[] RowVersion { get; private set; }

مع SQL Server:

rowversion

ثم التعامل مع:

DbUpdateConcurrencyException

وده لازم يدخل في الـ architecture prompt.

14. Cart concurrency برضه محتاجة strategy

مثلاً:

User has cart
        ↓
Request A adds product
Request B updates quantity

لازم تحدد:

Who owns Cart?
What is concurrency token?
How conflicts resolved?

مش مجرد CRUD.

15. Middleware عندك فيه duplication

عندك:

CorrelationIdMiddleware
RequestLoggingMiddleware
ValidationMiddleware
ExceptionHandlingMiddleware

وجودهم مش غلط.

لكن الـ responsibilities مش واضحة كفاية.

أنا أريد:

Middleware
├── Exception handling
├── Correlation / Trace
└── HTTP logging

بينما:

Validation

تروح:

MediatR Pipeline Behavior

لأنها business/application pipeline وليست HTTP concern.

يعني:

HTTP
 ↓
Middleware
 ↓
Controller
 ↓
MediatR
 ↓
ValidationBehavior
 ↓
AuthorizationBehavior
 ↓
TransactionBehavior
 ↓
Handler

وده أنظف بكثير.

16. Pipeline Behaviors ناقصة

بحثت عن:

IPipelineBehavior
AddBehavior

ومفيش implementation واضحة في المشروع الحالي.

وأنا أعتبر ده gap مهم.

ممكن تعمل:

Application
└── Behaviors
    ├── ValidationBehavior
    ├── LoggingBehavior
    ├── PerformanceBehavior
    ├── TransactionBehavior
    └── AuthorizationBehavior

لكن مش لازم تحط كل حاجة.

القاعدة:

Validation
Behavior
Transaction
Behavior

للـ commands التي تحتاج transaction.

Logging

ممكن:

Serilog + Activity

بدل duplication.

Authorization

حسب requirement:

ASP.NET Authorization

أو application authorization behavior لو عندك use-case policies.

17. Exception handling محتاج يتغير

حاليًا عندك:

response.Details = exception.Message;

وده خطر.

لأن exception ممكن يحتوي:

SQL error
Connection details
Internal implementation
Stack-related information
Sensitive data

الأفضل production API يستخدم:

ProblemDetails

ويوجد correlation/trace id.

ASP.NET Core يوفر built-in ProblemDetails وUseExceptionHandler، وMicrosoft تحذر من إرسال معلومات exception الحساسة للـ clients.

مثلاً:

{
  "type": "https://api.market.com/errors/internal",
  "title": "Unexpected error",
  "status": 500,
  "traceId": "..."
}

والـ log الداخلي يحتوي التفاصيل.

18. ValidationMiddleware عندك مش مكانها المثالي

FluentValidation موجودة في:

Application

لكن validation middleware في:

API

والـ middleware نفسها تنتظر exception.

الأفضل:

ValidationBehavior

مثلاً:

MediatR Request
       ↓
ValidationBehavior
       ↓
Validator
       ↓
Handler

وبالتالي لا تحتاج:

ValidationMiddleware

إلا لو عندك HTTP-specific validation خارج application use cases.

19. Program.cs محتاج يكون Composition Root فقط

حاليًا عندك:

await Market.Infrastructure.DependencyInjection
    .InitializeDatabaseAsync(...)

أنا لا أحب أن الـ API تعرف method داخل Infrastructure بهذا الشكل.

الأفضل:

Infrastructure
└── DatabaseInitializer

لكن execution orchestration ممكن يكون في API:

await app.Services.InitializeDatabaseAsync();

والأفضل production:

Application startup
      ❌ migrations

Deployment pipeline
      ↓
Database migration
      ↓
Application

لأن لو عندك:

10 API replicas

كل instance ممكن يحاول يعمل migrations.

20. Scaling model اللي أقترحه

لو بدأ المشروع:

1,000 users

architecture الحالية بعد الإصلاح كافية جدًا.

لكن لو:

10k users
100k users
1M users

لا تقفز مباشرة Microservices.

امشِ كده:

Phase 1
Modular Monolith
+
SQL Server
+
Redis
+
CQRS

ثم:

Phase 2

API
 ↓
Load Balancer
 ↓
3-10 API instances
 ↓
Redis
 ↓
SQL Server

ثم:

Phase 3

Catalog
Ordering
Identity
Vendor
Payment
Notification

تبدأ تتحول إلى services فقط لما يكون عندك business/organizational scaling reason.

21. الـ Modules اللي شايفها للمشروع

أنا هقسم الـ domain إلى:

Catalog
├── Product
├── Category
└── ProductTag

Ordering
├── Cart
├── Order
└── OrderItem

Identity
└── User

Marketplace
└── Vendor

Reviewing
├── Review
└── ReviewImage

وده يديك مستقبلًا boundaries واضحة:

Catalog Service
Ordering Service
Identity Service
Vendor Service
Review Service

لكن الآن كلها داخل نفس process.

ده اسمه:

Modular Monolith as a stepping stone to Microservices

22. Dependency structure اللي أريدها

بدل الوضع الحالي:

API
 ├── Application
 └── Infrastructure

Infrastructure
 ├── Application
 └── Domain

أريد:

                 API
                  │
                  ▼
             Application
                  │
                  ▼
                Domain

Infrastructure
      │
      ├──────────► Application abstractions
      │
      └──────────► Domain

والـ key rule:

Domain
   ↓
depends on NOTHING

Application
   ↓
Domain

Infrastructure
   ↓
Application + Domain

API
   ↓
Application + Infrastructure

لكن:

Infrastructure
❌ MUST NOT depend on Application Features DTOs
❌ MUST NOT depend on Controllers
❌ MUST NOT depend on HTTP
23. Folder structure المقترحة

أنا لا أنصحك تعمل 50 project.

ابدأ بـ:

src/
├── Market.Domain/
│   ├── Common/
│   │   ├── Entity.cs
│   │   ├── AggregateRoot.cs
│   │   └── DomainEvent.cs
│   │
│   ├── Catalog/
│   │   ├── Products/
│   │   ├── Categories/
│   │   └── Tags/
│   │
│   ├── Ordering/
│   │   ├── Orders/
│   │   └── Carts/
│   │
│   ├── Identity/
│   │   └── Users/
│   │
│   ├── Marketplace/
│   │   └── Vendors/
│   │
│   └── Reviews/
│
├── Market.Application/
│   ├── Abstractions/
│   │   ├── Persistence/
│   │   ├── Identity/
│   │   ├── Caching/
│   │   └── Messaging/
│   │
│   ├── Behaviors/
│   │   ├── ValidationBehavior.cs
│   │   ├── TransactionBehavior.cs
│   │   └── PerformanceBehavior.cs
│   │
│   └── Features/
│       ├── Catalog/
│       ├── Ordering/
│       ├── Identity/
│       ├── Vendors/
│       └── Reviews/
│
├── Market.Infrastructure/
│   ├── Persistence/
│   │   ├── DbContext/
│   │   ├── Configurations/
│   │   ├── Repositories/
│   │   └── Migrations/
│   │
│   ├── Identity/
│   ├── Caching/
│   └── Messaging/
│
└── Market.API/
    ├── Controllers/
    ├── Middleware/
    ├── Contracts/
    └── Program.cs
24. والأهم: لا تعمل Generic Repository لكل حاجة

حاليًا عندك:

IRepository<T>

وده مش necessarily غلط، لكن مع CQRS وEF Core ممكن يتحول إلى abstraction noise.

خصوصًا:

GetAll
GetById
Create
Update
Delete
Exists

ثم فوقه:

IProductRepository
IOrderRepository
ICartRepository
...

أنا أفضل:

Write side

Aggregate-specific repositories:

IOrderRepository
ICartRepository
IProductRepository
Read side

Dedicated read abstractions:

IProductReadRepository
IOrderReadRepository
ICatalogReadService

وبالتالي architecture تعكس business use cases بدل generic CRUD.

25. Tests عندك Gap كبير

الـ solution عنده:

Market.Domain.Tests
Market.Application.Tests
Market.Infrastructure.Tests
Market.API.Tests

لكن الموجود حاليًا فيه UnitTest1.cs placeholders.

فالمشكلة ليست وجود test projects؛ المشكلة:

الـ architecture غير محمية architectural tests.

لازم تضيف:

Domain tests
Application handler tests
Repository integration tests
API integration tests
Architecture tests

وأنا أريد architecture tests تمنع:

Domain → Infrastructure
Domain → API

Application → Infrastructure

Infrastructure → API

دي مهمة جدًا.

ممكن تستخدم NetArchTest أو architecture testing approach مشابه.

26. Database boundary

الـ SQL model الحالي مناسب كبداية، لكن لازم تضيف:

Indexes
Unique constraints
Foreign keys
Check constraints
Concurrency
Soft-delete strategy
Audit columns

خصوصًا:

Product.SKU
User.Email
Order.OrderNumber
Vendor.UserId
Cart.UserId

لازم تحدد uniqueness حسب business rules.

27. Soft Delete محتاج central strategy

حاليًا repository يعمل:

.Where(x => !x.IsDeleted)

في كل query.

ده ممكن يعمل duplication.

الأفضل EF Core global query filters:

builder.Entity<Product>()
    .HasQueryFilter(x => !x.IsDeleted);

لكن لازم يكون عندك strategy للـ admin / restore:

IgnoreQueryFilters()

بحذر.

28. Observability

أنت عندك:

Serilog
Correlation ID
Request logging
Health checks

كويس.

لكن architecture المستقبلية محتاجة:

Logs
+
Metrics
+
Distributed Tracing

يعني:

OpenTelemetry
      ↓
Trace
      ↓
API
 ↓
EF Core
 ↓
Redis
 ↓
Message Broker

والـ correlation ID يفضل يكون متكامل مع Activity.TraceId بدل اختراع tracing system منفصل.

29. Caching

للـ ecommerce:

Product catalog
Category
Vendor public profile
Popular products

مرشحين للـ Redis.

لكن:

Cart
Order
Payment
Inventory

ما تعملش caching لها بدون consistency strategy واضحة.

30. Event-driven architecture

مش لازم RabbitMQ من أول يوم.

لكن architecture لازم تكون ready.

مثلاً:

OrderPlaced
ProductCreated
ProductStockChanged
OrderPaid
OrderCancelled
ReviewCreated
VendorApproved

وبعدها:

Order
 ↓
Domain Event
 ↓
Outbox
 ↓
Message Broker
 ↓
Notification
Analytics
Search
Inventory

وده الطريق الصحيح لما تكبر بدل إنك تربط services ببعض مباشرة.

31. أهم Business boundaries مستقبلًا

لو المشروع وصل enterprise scale، أنا شايف boundaries محتملة:

Identity
   │
   └── Users/Auth

Catalog
   │
   ├── Products
   ├── Categories
   └── Search

Marketplace
   │
   └── Vendors

Cart
   │
   └── Shopping Cart

Ordering
   │
   ├── Orders
   └── Order Items

Inventory
   │
   └── Stock

Payment
   │
   └── Payments

Review
   │
   └── Reviews

Notification

لكن لا تفصلهم إلى microservices لمجرد وجود boundaries.

الفصل يكون عندما يكون عندك:

independent scaling
independent deployment
independent ownership
different data requirements
different availability requirements
different release cadence
32. نقطة مهمة جدًا في الـ .NET version

المشروع حاليًا .NET 9.

وبما إن التاريخ الحالي سبتمبر 2026:

.NET 9
STS
EOL: Nov 10, 2026

.NET 10
LTS
EOL: Nov 14, 2028

فأنا هضيف في refactoring prompt:

Evaluate migration to .NET 10 LTS

وليس مجرد:

upgrade packages blindly

لازم تعمل:

TargetFramework
Packages
EF Core
MediatR
FluentValidation
Docker image
CI
Tests
Swagger
Deployment

كلهم aligned.

33. القرار النهائي للـ Market-Api

لو أنا الـ architect المسؤول عن المشروع، القرار يكون:

Architecture:
Modular Monolith

Foundation:
Clean Architecture

Application organization:
Vertical Slice + CQRS

Domain:
DDD-lite / Aggregate-oriented

Persistence:
EF Core + SQL Server

Read side:
Dedicated read repositories/projections

Write side:
Aggregate repositories

Transactions:
Unit of Work / DbContext boundary

Validation:
MediatR Pipeline Behavior

Errors:
ProblemDetails

Observability:
Serilog + OpenTelemetry-ready

Caching:
Redis-ready

Async:
Outbox-ready

Messaging:
Broker-ready

Scaling:
Horizontal API scaling

Future:
Selective Microservices extraction

وده في رأيي أنسب evolution path للمشروع الحالي.
