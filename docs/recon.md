# Reconnaissance — Carrolink (crAPI)

## Scope confirmed

In-scope host: `http://localhost:8888`
Out of scope: Google Maps (`maps.google.com`, `googleapis.com`), MailHog.

## Test accounts

| Account | Role | Vehicle ID |
| --- | --- | --- |
| driver-a@example.com | Regular user | b2e029b9-a139-4c6... |
| driver-b@example.com | Regular user | fea4d175-bae2-424... |

Passwords and PINs are stored in `.env` (not committed).

## Endpoint map

### identity
- POST /identity/api/auth/signup
- POST /identity/api/auth/login
- POST /identity/api/auth/verify
- GET  /identity/api/v2/user/dashboard
- POST /identity/api/v2/vehicle/add_vehicle
- GET  /identity/api/v2/vehicle/vehicles
- GET  /identity/api/v2/vehicle/{vehicleId}

### workshop
- GET  /workshop/api/mechanic/
- POST /workshop/api/merchant/contact_mechanic
- GET  /workshop/api/merchant/service_requests/{id}

### community (to map)
- posts and comments

## Threat model (short)

Most sensitive assets: real-time vehicle location, personal data (name, email,
phone), orders and coupons. Main trust boundary: any logged-in user must only
reach their own vehicle, profile and orders.

## Chosen focus areas for manual testing (Phase 5)

1. Vehicle & location — BOLA on `GET /identity/api/v2/vehicle/{id}`:
   as driver-a, request driver-b's vehicle.
2. Workshop & orders — BOLA on service requests; mass assignment on
   `contact_mechanic`. Investigate the 411 on `service_requests`.
3. Authentication — rate limiting on `login` and OTP `verify`
   (account takeover scenario).
