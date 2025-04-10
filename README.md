# ShortURLDTE

API RESTful desarrollada en .NET 8 para cargar, validar y consultar facturas electrónicas dispuestas y generadas en SII (DTE), aplicando arquitectura HEXAGONAL y autenticación con JWT.
---

## Arquitectura Hexagonal

La arquitectura hexagonal, permite separar el dominio del sistema de los detalles de infraestructura (como bases de datos, controladores web, validaciones externas, etc.).

### Ventajas de usar arquitectura hexagonal

- **Independencia de frameworks**: la lógica de negocio no depende de .NET, bases de datos ni protocolos.
- **Facilidad de pruebas**: los casos de uso y entidades pueden probarse de forma aislada con pruebas unitarias.
- **Alta mantenibilidad**: los cambios en tecnologías externas no afectan el dominio ni los casos de uso.
- **Mocks sencillos**: se pueden usar adaptadores simulados para testear sin dependencia externa.

### Estructura aplicada

```
ShortURLDTE/
Domain/          - Entidades y lógica de negocio
Application/     - Casos de uso y puertos
Infrastructure/  - Adaptadores secundarios (DB, XML, Validadores)
Api/             - Adaptador primario (HTTP Controllers)
```

---

## Seguridad con Autenticación JWT

La API implementa autenticación basada en "JSON Web Tokens (JWT)" para proteger endpoints que cargan o consultan DTEs.

### Flujo resumido

1. El usuario inicia sesión y recibe un token JWT.
2. Este token debe incluirse en la cabecera de cada petición:

```
Authorization: Bearer {token}
```

3. La API valida el token usando claves secretas configuradas en `appsettings.json`.
4. Si el token es válido, el usuario puede interactuar con los servicios. "usuario: Iconstruye, Password: MVARGAS1"

### Ventajas

- Seguridad sin mantener sesiones en el servidor
- Tokens autocontenidos con claims (rol, usuario, vencimiento(1 hora))
- Fácil integración con Postman, Swagger

---

## Tecnologías utilizadas

- .NET 8
- C#
- JWT Bearer Authentication
- Arquitectura Hexagonal 
- xUnit + Moq (tests)

---

## Endpoints principales

- 'POST /api/facturas' - Subir DTE en XML
- 'GET /api/facturas/{id}' - Consultar por ID (3 accesos)
- 'GET /f/{shortUrl}' - Redirección desde URL corta
