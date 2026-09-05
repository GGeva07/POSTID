# Arquitectura y Lineamientos del Backend POS

Este documento establece los principios de arquitectura, seguridad y mantenimiento para el desarrollo del backend del POS de servicios (POSTID) orientado al mercado de República Dominicana. Este sistema se diseña como un **monolito estructurado en N-Capas** utilizando ASP.NET Core, preparado para escalar y manejar concurrencia, cumplimiento fiscal (e-CF) y alta disponibilidad, superando las limitaciones de un sistema básico para unos pocos clientes.

## 1. Arquitectura de N-Capas (Monolito Estructurado)

Para garantizar la mantenibilidad y evitar que el monolito se degrade, se implementa una arquitectura en capas estrictamente desacoplada:

### 1.1. Capa de Presentación / API (`PosID.Api`)
- **Responsabilidad:** Exponer los endpoints RESTful/GraphQL, validación de entrada (Data Annotations/FluentValidation), autenticación y ruteo.
- **Regla estricta:** No debe contener lógica de negocio ni acceso a datos. Debe delegar toda la ejecución a la capa de Aplicación.

### 1.2. Capa de Aplicación (`PosID.Application`)
- **Responsabilidad:** Orquestación de casos de uso, coordinación de transacciones, DTOs (Data Transfer Objects), e interfaces de servicios externos (ej. puertos para el conector de DGII).
- **Regla estricta:** Conoce la capa de Dominio, pero ignora completamente la Infraestructura.

### 1.3. Capa de Dominio (`PosID.Core`)
- **Responsabilidad:** Entidades del núcleo (Orden, Cliente, Documento Fiscal, etc.), reglas de negocio puras, constantes fiscales (cálculos de ITBIS, reglas de redondeo).
- **Regla estricta:** Es el corazón del sistema. No tiene dependencias de ningún framework externo (excepto librerías base de .NET). Las validaciones fiscales críticas viven aquí.

### 1.4. Capa de Infraestructura (`PosID.Infrastructure`)
- **Responsabilidad:** Acceso a bases de datos (Entity Framework Core), implementaciones de interfaces externas (comunicación con DGII para e-CF, envíos de correo, proveedores de pago), y repositorios.
- **Regla estricta:** Es la única capa que conoce de tecnologías específicas de bases de datos o APIs de terceros.

---

## 2. Lineamientos de Seguridad

Dado que el sistema maneja información de pagos, datos fiscales y datos personales (Ley 172-13), la seguridad es un pilar fundamental:

- **Autenticación y Autorización:**
  - Uso de JWT (JSON Web Tokens) con tiempos de expiración cortos y Refresh Tokens.
  - Control de acceso basado en roles (RBAC) y políticas granulares (ej. permiso específico para aplicar descuentos mayores al límite o anular facturas).
- **Protección de Datos:**
  - Cifrado en tránsito (TLS 1.2+ obligatorio) y cifrado en reposo para datos sensibles (tokens, certificados de firma e-CF).
  - Los logs de la aplicación NUNCA deben registrar PII (Personal Identifiable Information) ni datos sensibles de tarjetas (PAN, CVV).
- **Auditoría e Inmutabilidad:**
  - Toda acción sensible (cierres de caja, anulación de comprobantes, modificación de catálogos) debe generar un log de auditoría inmutable indicando: quién, cuándo, desde qué sucursal y el estado previo/posterior.
  - Los documentos fiscales emitidos y las órdenes cobradas son **inmutables**. Cualquier corrección debe realizarse mediante notas de crédito o flujos compensatorios autorizados.
- **Protección de Endpoints:**
  - Implementación de Rate Limiting para prevenir abusos y ataques de fuerza bruta.
  - Validación estricta de todos los inputs (OWASP Top 10).

---

## 3. Lineamientos de Mantenimiento y Escalabilidad

Para que el monolito sea robusto y soporte crecimiento a múltiples sucursales y alto volumen:

- **Idempotencia en APIs Transaccionales:**
  - Los endpoints de cobro y emisión fiscal deben ser idempotentes para manejar caídas de red e interrupciones sin duplicar cargos ni comprobantes. Uso de `TrackId` y claves de idempotencia.
- **Desacoplamiento de Servicios Lentos:**
  - La comunicación con los servicios de la DGII (e-CF) debe realizarse preferentemente mediante colas o procesos en background si la normativa lo permite, para no bloquear el flujo de cobro rápido de la cajera (meta de < 60 segundos).
- **Inyección de Dependencias (DI):**
  - Uso estricto del contenedor DI de .NET. Las clases deben depender de abstracciones (interfaces), no de implementaciones concretas, facilitando el testing (Mocks).
- **Resiliencia:**
  - Implementación de patrones como *Retry* y *Circuit Breaker* (mediante librerías como Polly) para la comunicación con la DGII o pasarelas de pago, asegurando la continuidad operativa ante fallas externas temporales.
- **Observabilidad:**
  - Integración temprana de telemetría (logs estructurados, métricas de rendimiento de base de datos, trazas distribuidas) utilizando herramientas como Serilog u OpenTelemetry.
- **Manejo de Base de Datos:**
  - Uso de migraciones controladas (EF Core Migrations).
  - Índices optimizados para las consultas frecuentes (búsqueda de clientes por teléfono/cédula, listado de órdenes del día).
