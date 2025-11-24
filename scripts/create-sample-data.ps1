<#
.SYNOPSIS
    Genera datos de muestra para los ejercicios del taller MCP.

.DESCRIPTION
    Este script crea archivos JSON con datos de muestra (clientes, productos, pedidos, sesiones)
    que se utilizan en los ejercicios prácticos del taller.

.PARAMETER OutputPath
    Ruta de salida para los archivos de datos. Por defecto: ./Data

.EXAMPLE
    .\create-sample-data.ps1
    .\create-sample-data.ps1 -OutputPath "C:\Data"
#>

[CmdletBinding()]
param(
    [Parameter()]
    [string]$OutputPath = "./data"
)

# Crear directorio de salida si no existe
if (-not (Test-Path $OutputPath)) {
    New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
    Write-Host "✓ Directorio creado: $OutputPath" -ForegroundColor Green
}

# Datos de clientes
$customers = @(
    @{
        id      = 1
        name    = "Ana García"
        email   = "ana.garcia@example.com"
        country = "España"
        created = "2024-01-15T10:30:00Z"
    },
    @{
        id      = 2
        name    = "Carlos Méndez"
        email   = "carlos.mendez@example.com"
        country = "México"
        created = "2024-02-20T14:45:00Z"
    },
    @{
        id      = 3
        name    = "Laura Torres"
        email   = "laura.torres@example.com"
        country = "Argentina"
        created = "2024-03-10T09:15:00Z"
    },
    @{
        id      = 4
        name    = "Miguel Rodríguez"
        email   = "miguel.rodriguez@example.com"
        country = "España"
        created = "2024-04-05T16:20:00Z"
    },
    @{
        id      = 5
        name    = "Sofia Fernández"
        email   = "sofia.fernandez@example.com"
        country = "Chile"
        created = "2024-05-12T11:00:00Z"
    }
)

# Datos de productos
$products = @(
    @{
        id       = 101
        name     = "Laptop Pro 15"
        price    = 1299.99
        category = "Informática"
        inStock  = $true
    },
    @{
        id       = 102
        name     = "Mouse Inalámbrico"
        price    = 29.99
        category = "Accesorios"
        inStock  = $true
    },
    @{
        id       = 103
        name     = "Teclado Mecánico"
        price    = 89.99
        category = "Accesorios"
        inStock  = $false
    },
    @{
        id       = 104
        name     = "Monitor 27 pulgadas"
        price    = 349.99
        category = "Informática"
        inStock  = $true
    },
    @{
        id       = 105
        name     = "Webcam HD"
        price    = 79.99
        category = "Electrónica"
        inStock  = $true
    }
)

# Datos de pedidos
$orders = @(
    @{
        id          = 1001
        customerId  = 1
        productId   = 101
        quantity    = 1
        totalAmount = 1299.99
        status      = "completed"
        orderDate   = "2024-06-01T10:00:00Z"
    },
    @{
        id          = 1002
        customerId  = 2
        productId   = 102
        quantity    = 2
        totalAmount = 59.98
        status      = "completed"
        orderDate   = "2024-06-02T14:30:00Z"
    },
    @{
        id          = 1003
        customerId  = 1
        productId   = 104
        quantity    = 1
        totalAmount = 349.99
        status      = "pending"
        orderDate   = "2024-06-03T09:15:00Z"
    },
    @{
        id          = 1004
        customerId  = 4
        productId   = 105
        quantity    = 1
        totalAmount = 79.99
        status      = "shipped"
        orderDate   = "2024-06-04T16:45:00Z"
    },
    @{
        id          = 1005
        customerId  = 5
        productId   = 101
        quantity    = 1
        totalAmount = 1299.99
        status      = "completed"
        orderDate   = "2024-06-05T11:20:00Z"
    }
)

# Datos de sesiones de usuario
$sessions = @(
    @{
        id          = "sess_001"
        userId      = "user_1"
        startTime   = "2024-06-10T08:00:00Z"
        endTime     = "2024-06-10T08:45:00Z"
        pagesViewed = 12
        actions     = @("search", "view_product", "add_to_cart", "checkout")
    },
    @{
        id          = "sess_002"
        userId      = "user_2"
        startTime   = "2024-06-10T09:30:00Z"
        endTime     = "2024-06-10T10:00:00Z"
        pagesViewed = 5
        actions     = @("search", "view_product")
    },
    @{
        id          = "sess_003"
        userId      = "user_1"
        startTime   = "2024-06-11T14:15:00Z"
        endTime     = "2024-06-11T15:00:00Z"
        pagesViewed = 8
        actions     = @("view_product", "add_to_cart")
    }
)

# Datos de carritos abandonados (8, 12, 24, 48, 72 horas)
$currentDate = Get-Date
$abandonedCarts = @(
    @{
        id          = "cart_001"
        userId      = "user_1"
        items       = @(
            @{ productId = 102; quantity = 1; price = 249.99 }
        )
        totalValue  = 249.99
        abandonedAt = $currentDate.AddHours(-8).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        hoursAgo    = 8
    },
    @{
        id          = "cart_002"
        userId      = "user_2"
        items       = @(
            @{ productId = 103; quantity = 1; price = 89.99 }
        )
        totalValue  = 89.99
        abandonedAt = $currentDate.AddHours(-12).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        hoursAgo    = 12
    },
    @{
        id          = "cart_003"
        userId      = "user_3"
        items       = @(
            @{ productId = 101; quantity = 1; price = 1299.99 },
            @{ productId = 104; quantity = 1; price = 349.99 }
        )
        totalValue  = 1649.98
        abandonedAt = $currentDate.AddHours(-24).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        hoursAgo    = 24
    },
    @{
        id          = "cart_004"
        userId      = "user_4"
        items       = @(
            @{ productId = 105; quantity = 2; price = 199.99 },
            @{ productId = 106; quantity = 1; price = 149.99 }
        )
        totalValue  = 549.98
        abandonedAt = $currentDate.AddHours(-48).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        hoursAgo    = 48
    },
    @{
        id          = "cart_005"
        userId      = "user_5"
        items       = @(
            @{ productId = 107; quantity = 3; price = 79.99 },
            @{ productId = 108; quantity = 1; price = 159.99 }
        )
        totalValue  = 399.97
        abandonedAt = $currentDate.AddHours(-72).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        hoursAgo    = 72
    }
)

# Datos de eventos del carrito (variados con diferentes cantidades y acciones)
$cartEvents = @(
    # User 1 - Compra exitosa de laptop
    @{
        userId      = "user_1"
        productId   = 102
        action      = "addToCart"
        timestamp   = $currentDate.AddDays(-1).AddHours(-12).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        quantity    = 1
    },
    # User 2 - Abandona teclado
    @{
        userId      = "user_2"
        productId   = 103
        action      = "addToCart"
        timestamp   = $currentDate.AddHours(-18).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        quantity    = 1
    },
    # User 3 - Compra múltiple (laptop + monitor)
    @{
        userId      = "user_3"
        productId   = 101
        action      = "addToCart"
        timestamp   = $currentDate.AddDays(-1).AddHours(-12).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        quantity    = 1
    },
    @{
        userId      = "user_3"
        productId   = 104
        action      = "addToCart"
        timestamp   = $currentDate.AddDays(-1).AddHours(-6).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        quantity    = 1
    },
    # User 4 - Múltiples webcams + auriculares
    @{
        userId      = "user_4"
        productId   = 105
        action      = "addToCart"
        timestamp   = $currentDate.AddDays(-2).AddHours(-12).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        quantity    = 2
    },
    @{
        userId      = "user_4"
        productId   = 106
        action      = "addToCart"
        timestamp   = $currentDate.AddDays(-2).AddHours(-6).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        quantity    = 1
    },
    # User 5 - Compra múltiple de accesorios
    @{
        userId      = "user_5"
        productId   = 107
        action      = "addToCart"
        timestamp   = $currentDate.AddDays(-3).AddHours(-12).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        quantity    = 3
    },
    @{
        userId      = "user_5"
        productId   = 108
        action      = "addToCart"
        timestamp   = $currentDate.AddDays(-3).AddHours(-6).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        quantity    = 1
    },
    # User 6 - Compra exitosa con checkout
    @{
        userId      = "user_6"
        productId   = 101
        action      = "addToCart"
        timestamp   = $currentDate.AddDays(-4).AddHours(-18).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        quantity    = 1
    },
    @{
        userId      = "user_6"
        productId   = 101
        action      = "checkout"
        timestamp   = $currentDate.AddDays(-4).AddHours(-12).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        quantity    = 1
    },
    # User 7 - Agrega y luego remueve mouse
    @{
        userId      = "user_7"
        productId   = 102
        action      = "addToCart"
        timestamp   = $currentDate.AddDays(-5).AddHours(-12).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\Z")
        quantity    = 1
    }
)

# Guardar archivos JSON
try {
    $customersPath = Join-Path $OutputPath "customers.json"
    $customers | ConvertTo-Json -Depth 10 | Out-File -FilePath $customersPath -Encoding UTF8
    Write-Host "✓ Generado: $customersPath ($($customers.Count) clientes)" -ForegroundColor Green

    $productsPath = Join-Path $OutputPath "products.json"
    $products | ConvertTo-Json -Depth 10 | Out-File -FilePath $productsPath -Encoding UTF8
    Write-Host "✓ Generado: $productsPath ($($products.Count) productos)" -ForegroundColor Green

    $ordersPath = Join-Path $OutputPath "orders.json"
    $orders | ConvertTo-Json -Depth 10 | Out-File -FilePath $ordersPath -Encoding UTF8
    Write-Host "✓ Generado: $ordersPath ($($orders.Count) pedidos)" -ForegroundColor Green

    $sessionsPath = Join-Path $OutputPath "sessions.json"
    $sessions | ConvertTo-Json -Depth 10 | Out-File -FilePath $sessionsPath -Encoding UTF8
    Write-Host "✓ Generado: $sessionsPath ($($sessions.Count) sesiones)" -ForegroundColor Green

    $abandonedCartsPath = Join-Path $OutputPath "abandoned-carts.json"
    $abandonedCarts | ConvertTo-Json -Depth 10 | Out-File -FilePath $abandonedCartsPath -Encoding UTF8
    Write-Host "✓ Generado: $abandonedCartsPath ($($abandonedCarts.Count) carritos abandonados)" -ForegroundColor Green

    $cartEventsPath = Join-Path $OutputPath "cart-events.json"
    $cartEvents | ConvertTo-Json -Depth 10 | Out-File -FilePath $cartEventsPath -Encoding UTF8
    Write-Host "✓ Generado: $cartEventsPath ($($cartEvents.Count) eventos de carrito)" -ForegroundColor Green

    Write-Host "`n✅ Todos los datos de muestra generados exitosamente" -ForegroundColor Green
    Write-Host "Ubicación: $(Resolve-Path $OutputPath)" -ForegroundColor Cyan
}
catch {
    Write-Host "❌ Error al generar datos: $_" -ForegroundColor Red
    exit 1
}
