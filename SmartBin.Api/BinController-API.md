# SmartBin API Documentation

> **Base URL**
> - HTTP:  `http://localhost:5080`
> - HTTPS: `https://localhost:7074`
>
> All endpoints accept and return `application/json` unless stated otherwise.

---

## Table of Contents

- [Authentication](#authentication)
  - [POST /api/auth/register](#post-apiauthregister)
  - [POST /api/auth/login](#post-apiauthlogin)
  - [POST /api/auth/logout](#post-apiauthlogout)
- [Bins](#bins)
  - [GET /api/bin](#get-apibin)
  - [POST /api/bin](#post-apibin)
  - [PUT /api/bin](#put-apibin)
  - [GET /api/bin/section](#get-apibinsection)
  - [PUT /api/bin/section](#put-apibinsection)
- [Transactions](#transactions)
  - [GET /api/transaction](#get-apitransaction)
  - [POST /api/transaction](#post-apitransaction)
- [Data Models](#data-models)
- [Material Reference](#material-reference)
- [Error Responses](#error-responses)
- [Postman Testing Guide](#postman-testing-guide)

---

## Authentication

> No authorization header is required for register or login.

### POST /api/auth/register

Create a new user account.

**Request Body — `application/json`**

| Field      | Type   | Required | Constraints                  | Description            |
|------------|--------|----------|------------------------------|------------------------|
| `name`     | string | ✅ Yes   | —                            | Full name of the user  |
| `email`    | string | ✅ Yes   | Valid email format           | User's email address   |
| `password` | string | ✅ Yes   | Min 6 characters             | Plain-text password    |

**Example Request**

```http
POST /api/auth/register
Content-Type: application/json

{
  "name": "Ahmed Sayed",
  "email": "ahmed@example.com",
  "password": "secret123"
}
```

**Success Response — `200 OK`**

```json
{
  "message": "User registered successfully.",
  "userId": 4
}
```

**Error Responses**

| Status            | Condition                         | Body                               |
|-------------------|-----------------------------------|------------------------------------|
| `400 Bad Request` | Email is already registered       | `{ "message": "Email already in use." }` |
| `400 Bad Request` | Validation failed (missing fields, short password, bad email) | Validation error details |

---

### POST /api/auth/login

Authenticate an existing user and receive a JWT token.

**Request Body — `application/json`**

| Field      | Type   | Required | Description             |
|------------|--------|----------|-------------------------|
| `email`    | string | ✅ Yes   | Registered email address|
| `password` | string | ✅ Yes   | Account password        |

**Example Request**

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "ahmed@example.com",
  "password": "secret123"
}
```

**Success Response — `200 OK`**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": 4
}
```

> **Important:** Store the `token` in the frontend. All protected endpoints automatically identify the user from the token — **no `userId` needs to be passed as a parameter**. The token is valid for **5 days**.

**Error Responses**

| Status        | Condition                          | Body                                          |
|---------------|------------------------------------|-----------------------------------------------|
| `401 Unauthorized` | Wrong email or password       | `{ "message": "Invalid email or password." }` |

---

### POST /api/auth/logout

Invalidate the current session by clearing the stored auth token. Requires a valid JWT.

> 🔒 **Requires Authorization header:** `Bearer <token>`

**No request body required.**

**Example Request**

```http
POST /api/auth/logout
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Success Response — `200 OK`**

```json
{
  "message": "Logged out successfully."
}
```

**Error Responses**

| Status             | Condition                          |
|--------------------|------------------------------------|
| `401 Unauthorized` | Missing or invalid token           |

---

## Bins

### GET /api/bin

Retrieve all bins belonging to the authenticated user, including their fill-level sections.

> 🔒 **Requires Authorization header:** `Bearer <token>`

**No query parameters required.** The user is identified from the JWT token.

**Example Request**

```http
GET /api/bin
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Success Response — `200 OK`**

Returns an array of `Bin` objects, each with a nested `sections` array.

```json
[
  {
    "id": 1,
    "identificationToken": "a3f9e2b1-74dc-4c3e-9e2a-1234567890ab",
    "latitude": 30.0444,
    "longitude": 31.2357,
    "createdAt": "2026-03-05T10:23:53Z",
    "sections": [
      {
        "materialId": 1,
        "materialName": "Plastic",
        "levelPercentage": 45.5,
        "weight": 1.2
      },
      {
        "materialId": 2,
        "materialName": "Metal",
        "levelPercentage": 20.0,
        "weight": 0.8
      }
    ]
  }
]
```

---

### POST /api/bin

Create a new bin for the authenticated user. Automatically generates a unique identification token and initialises two bin sections (Plastic and Metal) with zero fill levels.

> 🔒 **Requires Authorization header:** `Bearer <token>`

**No query parameters required.** The user is identified from the JWT token.

**Example Request**

```http
POST /api/bin
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Success Response — `200 OK`**

Returns the newly created `Bin` object with its initialised sections.

```json
{
  "id": 5,
  "identificationToken": "f1e2d3c4-b5a6-7890-abcd-ef1234567890",
  "latitude": 0.0,
  "longitude": 0.0,
  "createdAt": "2026-03-05T14:00:00Z",
  "sections": [
    {
      "materialId": 1,
      "materialName": "Plastic",
      "levelPercentage": 0.0,
      "weight": 0.0
    },
    {
      "materialId": 2,
      "materialName": "Metal",
      "levelPercentage": 0.0,
      "weight": 0.0
    }
  ]
}
```

> **Notes**
> - The `identificationToken` is a GUID auto-generated by the server. Store it securely — it is required for all hardware-device operations (location updates, section updates, and transaction submissions).
> - Two `BinSection` records are created automatically upon bin creation:
>   - `materialId: 1` → Plastic
>   - `materialId: 2` → Metal

---

### PUT /api/bin

Update the GPS location of an existing bin. Intended for use by the physical IoT device.

**Query Parameters**

| Parameter   | Type    | Required | Description                                      |
|-------------|---------|----------|--------------------------------------------------|
| `binId`     | integer | ✅ Yes   | The ID of the bin to update                      |
| `Latitude`  | float   | ✅ Yes   | New latitude coordinate                          |
| `Longitude` | float   | ✅ Yes   | New longitude coordinate                         |
| `Token`     | string  | ✅ Yes   | The bin's `identificationToken` for verification |

**Example Request**

```http
PUT /api/bin?binId=5&Latitude=30.0444&Longitude=31.2357&Token=f1e2d3c4-b5a6-7890-abcd-ef1234567890
```

**Success Response — `200 OK`**

Returns the updated `Bin` object with sections.

```json
{
  "id": 5,
  "identificationToken": "f1e2d3c4-b5a6-7890-abcd-ef1234567890",
  "latitude": 30.0444,
  "longitude": 31.2357,
  "createdAt": "2026-03-05T14:00:00Z",
  "sections": [
    {
      "materialId": 1,
      "materialName": "Plastic",
      "levelPercentage": 0.0,
      "weight": 0.0
    },
    {
      "materialId": 2,
      "materialName": "Metal",
      "levelPercentage": 0.0,
      "weight": 0.0
    }
  ]
}
```

**Error Responses**

| Status          | Condition                                           |
|-----------------|-----------------------------------------------------|
| `404 Not Found` | No bin matches the provided `binId` + `Token` pair  |

> **Body:** `"Bin not found or token invalid."`

---

### GET /api/bin/section

Retrieve a specific bin along with all of its sections (fill levels, weights, and material types).

> 🔒 **Requires Authorization header:** `Bearer <token>`

**Query Parameters**

| Parameter | Type    | Required | Description       |
|-----------|---------|----------|-------------------|
| `binId`   | integer | ✅ Yes   | The ID of the bin |

**Example Request**

```http
GET /api/bin/section?binId=5
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Success Response — `200 OK`**

Returns the `Bin` object with its nested `sections` array.

```json
{
  "id": 5,
  "identificationToken": "f1e2d3c4-b5a6-7890-abcd-ef1234567890",
  "latitude": 30.0444,
  "longitude": 31.2357,
  "createdAt": "2026-03-05T14:00:00Z",
  "sections": [
    {
      "materialId": 1,
      "materialName": "Plastic",
      "levelPercentage": 45.5,
      "weight": 1.2
    },
    {
      "materialId": 2,
      "materialName": "Metal",
      "levelPercentage": 20.0,
      "weight": 0.8
    }
  ]
}
```

**Error Responses**

| Status          | Condition                                                    |
|-----------------|--------------------------------------------------------------|
| `404 Not Found` | No bin found with the provided `binId` for the current user  |

---

### PUT /api/bin/section

Update the fill level and weight of a specific bin section. Intended for use by the physical IoT device.

**Query Parameters**

| Parameter | Type    | Required | Description                                      |
|-----------|---------|----------|--------------------------------------------------|
| `binId`   | integer | ✅ Yes   | The ID of the bin                                |
| `token`   | string  | ✅ Yes   | The bin's `identificationToken` for verification |

**Request Body — `application/json`**

| Field             | Type    | Required | Description                                                          |
|-------------------|---------|----------|----------------------------------------------------------------------|
| `materialId`      | integer | ✅ Yes   | The section to update (`1` = Plastic, `2` = Metal)                   |
| `levelPercentage` | float   | ✅ Yes   | Updated fill level as a percentage (0.0 – 100.0)                     |
| `weight`          | float   | ✅ Yes   | Updated weight in kilograms                                          |
| `materialName`    | string  | ✅ Yes     | 'Plastic' or 'Metal' server                                     |

**Example Request**

```http
PUT /api/bin/section?binId=5&token=f1e2d3c4-b5a6-7890-abcd-ef1234567890
Content-Type: application/json

{
  "materialId": 1,
  "levelPercentage": 72.5,
  "weight": 1.8
}
```

**Success Response — `200 OK`**

Returns the updated `Bin` object with all sections.

```json
{
  "id": 5,
  "identificationToken": "f1e2d3c4-b5a6-7890-abcd-ef1234567890",
  "latitude": 30.0444,
  "longitude": 31.2357,
  "createdAt": "2026-03-05T14:00:00Z",
  "sections": [
    {
      "materialId": 1,
      "materialName": "Plastic",
      "levelPercentage": 72.5,
      "weight": 1.8
    },
    {
      "materialId": 2,
      "materialName": "Metal",
      "levelPercentage": 20.0,
      "weight": 0.8
    }
  ]
}
```

**Error Responses**

| Status          | Condition                                                             |
|-----------------|-----------------------------------------------------------------------|
| `404 Not Found` | No bin matches the `binId` + `token` pair, or the section was not found |

> **Body:** `"Bin or section not found, or token invalid."`

---

## Transactions

### GET /api/transaction

Retrieve a paginated list of transactions (waste deposit events) for a specific bin, ordered by most recent first.

> 🔒 **Requires Authorization header:** `Bearer <token>`

**Query Parameters**

| Parameter    | Type    | Required | Default | Description                          |
|--------------|---------|----------|---------|--------------------------------------|
| `binId`      | integer | ✅ Yes   | —       | The ID of the bin                    |
| `pageNumber` | integer | ❌ No    | `1`     | The page number to retrieve (min: 1) |

> **Note:** Page size is fixed at **10 records per page**.

**Example Request**

```http
GET /api/transaction?binId=5&pageNumber=2
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Success Response — `200 OK`**

```json
{
  "totalRecords": 42,
  "totalPages": 5,
  "currentPage": 2,
  "pageSize": 10,
  "data": [
    {
      "timestamp": "2026-03-05T15:45:00Z",
      "materialName": "Metal",
      "aiConfidence": 0.85
    },
    {
      "timestamp": "2026-03-05T14:10:00Z",
      "materialName": "Plastic",
      "aiConfidence": null
    }
  ]
}
```

**Response Fields**

| Field                  | Type              | Description                                                          |
|------------------------|-------------------|----------------------------------------------------------------------|
| `totalRecords`         | integer           | Total number of transaction records for this bin                     |
| `totalPages`           | integer           | Total number of pages                                                |
| `currentPage`          | integer           | The current page number returned                                     |
| `pageSize`             | integer           | Number of records per page (always `10`)                             |
| `data`                 | array             | The transaction records for this page                                |
| `data[].timestamp`     | string (ISO 8601) | UTC date/time the transaction was recorded                           |
| `data[].materialName`  | string            | Human-readable material name (e.g., `"Plastic"`, `"Metal"`)         |
| `data[].aiConfidence`  | float \| null     | AI classification confidence score (0.0 – 1.0); `null` if not used  |

---

### POST /api/transaction

Record a new waste deposit transaction for a bin. Supports two modes:

- **Without Image** — Provide the `materialId` directly from the sensor.
- **With Image** — Upload a photo; the server records the transaction with an AI confidence score (full AI classification coming in a future release).

**Form Parameters — `multipart/form-data`**

| Field        | Type    | Required | Description                                                               |
|--------------|---------|----------|---------------------------------------------------------------------------|
| `binId`      | integer | ✅ Yes   | The ID of the bin where the deposit occurred                              |
| `token`      | string  | ✅ Yes   | The bin's `identificationToken` for authentication                        |
| `image`      | file    | ❌ No    | Image of the deposited waste (triggers AI mode)                           |
| `materialId` | integer | ❌ No    | Material ID for direct sensor mode. Defaults to `0` (Plastic) if omitted |

**Behavior Details**

| Mode                      | Condition          | `materialId` used                        | `aiConfidence` stored |
|---------------------------|--------------------|------------------------------------------|-----------------------|
| Direct Sensor (no image)  | `image` is null    | From form field (default `0` = Plastic)  | `null`                |
| AI Mode (image uploaded)  | `image` is present | `1` (Metal)                              | `0.85`                |

> ⚠️ Full AI image classification is not yet implemented. When an image is provided, the material is currently hardcoded to Metal with a fixed confidence of `0.85`.

**Example Request — Direct Sensor Mode**

```http
POST /api/transaction
Content-Type: multipart/form-data

binId=5
token=f1e2d3c4-b5a6-7890-abcd-ef1234567890
materialId=0
```

**Example Request — With Image**

```http
POST /api/transaction
Content-Type: multipart/form-data

binId=5
token=f1e2d3c4-b5a6-7890-abcd-ef1234567890
image=<binary file>
```

**Success Response — `200 OK`**

```json
{
  "message": "Transaction added successfully.",
  "transactionId": 87
}
```

**Error Responses**

| Status          | Condition                                           |
|-----------------|-----------------------------------------------------|
| `404 Not Found` | No bin matches the provided `binId` + `token` pair  |

> **Body:** `"Bin not found or invalid token."`

---

## Data Models

### BinDto

| Field                 | Type              | Description                                                     |
|-----------------------|-------------------|-----------------------------------------------------------------|
| `id`                  | integer           | Unique identifier                                               |
| `identificationToken` | string (GUID)     | Unique token used to authenticate device requests               |
| `latitude`            | float             | GPS latitude of the bin                                         |
| `longitude`           | float             | GPS longitude of the bin                                        |
| `createdAt`           | string (ISO 8601) | UTC timestamp when the bin was registered                       |
| `sections`            | array             | Array of `BinSectionDto` objects for each material compartment  |

### BinSectionDto

| Field             | Type    | Description                                      |
|-------------------|---------|--------------------------------------------------|
| `materialId`      | integer | ID of the material this section holds            |
| `materialName`    | string  | Human-readable name of the material              |
| `levelPercentage` | float   | Current fill level as a percentage (0.0 – 100.0) |
| `weight`          | float   | Current weight in kilograms                      |

### TransactionDto (Response Object)

| Field           | Type              | Description                                             |
|-----------------|-------------------|---------------------------------------------------------|
| `timestamp`     | string (ISO 8601) | UTC date/time the deposit was recorded                  |
| `materialName`  | string            | Human-readable name of the detected/specified material  |
| `aiConfidence`  | float \| null     | AI confidence score (0.0 – 1.0), `null` if N/A         |

---

## Material Reference

| `materialId` | Name    |
|:------------:|---------|
| `1`          | Plastic |
| `2`          | Metal   |

---

## Error Responses

| Status Code                 | Meaning                                                        |
|-----------------------------|----------------------------------------------------------------|
| `200 OK`                    | Request succeeded                                              |
| `404 Not Found`             | The resource does not exist, or the token/ownership is invalid |
| `400 Bad Request`           | Missing or malformed query/body parameters                     |
| `500 Internal Server Error` | An unexpected server-side error occurred                       |

---

## Postman Testing Guide

> **API is running at:** `http://localhost:5080`
> Start the server with `dotnet run` inside the `SmartBin.Api` project folder.

---

### Step 1 — Register a new user

| Field  | Value |
|--------|-------|
| Method | `POST` |
| URL    | `http://localhost:5080/api/auth/register` |
| Header | `Content-Type: application/json` |

**Body** (raw → JSON):
```json
{
  "name": "Ahmed",
  "email": "ahmed@test.com",
  "password": "123456"
}
```

**Expected Response `200 OK`:**
```json
{
  "message": "User registered successfully.",
  "userId": 1
}
```

---

### Step 2 — Login and copy the token

| Field  | Value |
|--------|-------|
| Method | `POST` |
| URL    | `http://localhost:5080/api/auth/login` |
| Header | `Content-Type: application/json` |

**Body** (raw → JSON):
```json
{
  "email": "ahmed@test.com",
  "password": "123456"
}
```

**Expected Response `200 OK`:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": 1
}
```

> ✅ **Copy the full `token` value** — that's all you need for every protected request below. The `userId` in the response is informational only.

---

### Step 3 — Attach the token to protected requests

Every endpoint marked **🔒 Requires Auth** needs the JWT token sent as a Bearer token.

**Option A — Authorization tab (recommended):**
1. Open the request in Postman
2. Go to the **Authorization** tab
3. Set **Type** to `Bearer Token`
4. Paste your token in the **Token** field

**Option B — Headers tab (manual):**

| Key | Value |
|-----|-------|
| `Authorization` | `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...` |

> ⚠️ The word `Bearer` followed by a **space** must come before the token. Do not include quotes.

---

### Step 4 — Test the endpoints

#### 🔒 Protected endpoints (require `Authorization: Bearer <token>` header)

| # | What | Method | URL |
|---|------|--------|-----|
| 1 | Get all my bins | `GET` | `http://localhost:5080/api/bin` |
| 2 | Create a new bin | `POST` | `http://localhost:5080/api/bin` |
| 3 | Get bin with sections | `GET` | `http://localhost:5080/api/bin/section?binId=1` |
| 4 | Get transactions (paginated) | `GET` | `http://localhost:5080/api/transaction?binId=1&pageNumber=1` |
| 5 | Logout | `POST` | `http://localhost:5080/api/auth/logout` |

#### 🔓 Device endpoints (no JWT — use the bin's `identificationToken` instead)

| # | What | Method | URL / Body |
|---|------|--------|------------|
| 6 | Update bin GPS location | `PUT` | `http://localhost:5080/api/bin?binId=1&latitude=30.04&longitude=31.23&token=<identificationToken>` |
| 7 | Update bin section fill level | `PUT` | `http://localhost:5080/api/bin/section?binId=1&token=<identificationToken>` + JSON body |
| 8 | Add a transaction (sensor) | `POST` | `http://localhost:5080/api/transaction` — form-data: `binId`, `token`, `materialId` |

---

### Example: Create a bin then view its sections

**1. Create bin** — `POST http://localhost:5080/api/bin`
- Add `Authorization: Bearer <token>` header
- No body needed
- Copy the `identificationToken` from the response

**2. View bin sections** — `GET http://localhost:5080/api/bin/section?binId=1`
- Add `Authorization: Bearer <token>` header
- Replace `1` with the actual `id` from the create response

---

### Example: Add a transaction from a device

`POST http://localhost:5080/api/transaction`
- **Content-Type:** `multipart/form-data`
- No Authorization header needed

| Key | Value |
|-----|-------|
| `binId` | `1` |
| `token` | `<identificationToken from bin>` |
| `materialId` | `1` (Plastic) or `2` (Metal) |

---

*SmartBin API — Last updated March 6, 2026*
