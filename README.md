# OpenAPI V3 Typed Parser (.NET)

A C# library that parses **OpenAPI v3** documents (JSON/YAML) into a **strongly-typed object model** suitable for **code generation** workflows.

This project focuses on:
- Loading OpenAPI v3 specs from files/streams/strings
- Resolving common `$ref` patterns (local + external)
- Producing a normalized, codegen-friendly in-memory model (paths, operations, schemas, parameters, request/response bodies, components)
- Providing deterministic output for repeatable generators

> **Target audience:** teams building generators (C#, TS, SDKs, API clients, tests, docs) that need a typed representation of OpenAPI v3.

---

## Features

- ✅ Parse OpenAPI v3 (JSON / YAML)
- ✅ Typed output model (no “raw” JSON tokens leaking into your generator layer)
- ✅ Extracts:
  - `Paths` → `Operations` (method, tags, summary, operationId, parameters, requestBody, responses)
  - `Components` → schemas, parameters, requestBodies, responses, headers, securitySchemes
  - Schema shapes: `object`, `array`, `enum`, primitives, `oneOf` / `anyOf` / `allOf`, nullable, format
- ✅ Reference resolution:
  - `#/components/...` (local)
  - external files (optional)
- ✅ Generator-friendly normalization:
  - canonical naming utilities
  - flattened/expanded schema accessors
  - stable ordering (ideal for deterministic codegen)

---

## Installation

### NuGet (recommended)
```bash
dotnet add package OpenApiV3.TypedParser
