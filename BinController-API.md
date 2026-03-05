# SmartBin API Documentation

This document outlines the API endpoints, parameters, and expected responses for the SmartBin application based on the current controllers.

---

## 1. Bin Controller API

**Base Route:** Custom defined per endpoint

### 1.1 Get Bins by User
Retrieves a list of all bins owned by a specific user.

* **Endpoint:** `GET /api/bin`
* **Query Parameters:**
    * `UserId` (integer, required): The ID of the user.
* **Expected Response:** `200 OK`
    * **Body:** Array of `Bin` objects.

### 1.2 Create Bin
Creates a new bin for a user and automatically initializes two empty bin sections (Material 0 and Material 1).

* **Endpoint:** `POST /api/bin`
* **Query Parameters:**
    * `UserId` (integer, required): The ID of the user to assign the bin to.
* **Expected Response:** `200 OK`
    * **Body:** The newly created `Bin` object (includes generated `IdentificationToken`, default coordinates [0,0], and `CreatedAt` timestamp in UTC).

### 1.3 Update Bin Location
Updates the GPS coordinates of a specific bin. Requires hardware authentication via token.

* **Endpoint:** `PUT /api/bin`
* **Query Parameters:**
    * `BinId` (integer, required): The ID of the bin to update.
    * `Latitude` (float, required): The new latitude.
    * `Longitude` (float, required): The new longitude.
    * `token` (string, required): The identification token belonging to the bin.
* **Expected Responses:**
    * `200 OK`: Returns the updated `Bin` object.
    * `404 Not Found`: If the `BinId` and `token` combination does not exist.

### 1.4 Get Bin Sections
Retrieves a specific bin along with its attached sections (e.g., Plastic and Metal fill levels).

* **Endpoint:** `GET /api/bin/section/`
* **Query Parameters:**
    * `BinId` (integer, required): The ID of the requested bin.
    * `UserId` (integer, required): The ID of the user who owns the bin (for authorization).
* **Expected Responses:**
    * `200 OK`: Returns the `Bin` object containing an array of `BinSections`.
    * `404 Not Found`: If the bin does not exist or does not belong to the provided `UserId`.

---

## 2. Transaction Controller API

**Base Route:** Custom defined per endpoint

### 2.1 Get Transactions (Paginated)
Retrieves a paginated list of waste disposal transactions for a specific bin, sorted from newest to oldest.

* **Endpoint:** `GET /api/transection`
* **Query Parameters:**
    * `BinId` (integer, required): The ID of the bin.
    * `UserId` (integer, required): The ID of the user who owns the bin.
    * `pageNumber` (integer, optional, default: 1): The requested page (minimum 1).
    * `pageSize` (integer, optional, default: 10): Records per page (maximum 100).
* **Expected Response:** `200 OK`
    * **Body:** ```json
    {
      "totalRecords": 0,
      "totalPages": 0,
      "currentPage": 1,
      "pageSize": 10,
      "data": [
        {
          "timestamp": "2026-03-05T14:30:00Z",
          "materialName": "string",
          "aiConfidence": 0.0
        }
      ]
    }
    ```

### 2.2 Add Transaction
Records a new waste disposal event. This endpoint accepts either direct material data from sensors or an image for AI processing. Requires hardware authentication.

* **Endpoint:** `POST /api/transection`
* **Content-Type:** `multipart/form-data` (Supports file uploads)
* **Form/Query Parameters:**
    * `BinId` (integer, required): The ID of the hardware bin.
    * `Token` (string, required): The bin's identification token.
    * `image` (file, optional): An image file from the bin's camera.
    * `MaterialId` (integer, optional): The ID of the detected material (used if no image is provided. Defaults to 1).
* **Behavior Details:**
    * **Direct Sensor Mode (No Image):** Sets the transaction time to UTC, records `MaterialId` (falls back to 1 if null).
    * **AI Mode (Image Uploaded):** Sets the transaction time to UTC, sets a simulated `AiConfidence` of 0.85f, and sets `MaterialId` to 1. 
* **Expected Responses:**
    * `200 OK`:
    ```json
    {
      "message": "Transection added successfully.",
      "transectionId": 123
    }
    ```
    * `404 Not Found`: If the `BinId` and `Token` combination is invalid.