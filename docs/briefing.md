# Briefing — Carrolink

> Fictional client, created for project 3 of a security portfolio.
> System under test: OWASP crAPI, running locally. Discovery call notes (45 min, video) — November 5, 2026.

## 1. The client

- **Company:** Carrolink — connected-car platform for private drivers.
- **Product:** mobile app, web dashboard and an API used by both.
- **Team:** 12 people, 4 developers. No one dedicated to security.
- **Contacts:** CTO (technical decision maker) + 1 backend developer (point of contact during testing).

## 2. What the platform does

- Vehicle registration by VIN.
- Real-time vehicle location.
- Service booking with partner workshops (mechanic role).
- Parts shop with discount coupons.
- Community forum with posts and comments between drivers.

## 3. Why test now

- A rental company with 800 vehicles wants to use the platform across its whole fleet.
- The contract requires a **third-party security test report of the application and API** before signing.
- The CTO suspects that "one user can see another user's stuff", but it has never been tested.

## 4. Data at stake

- Real-time vehicle location (a physical safety risk for drivers).
- Personal data: name, email, phone number.
- Purchase history and coupons.
- Conversations with workshops about vehicle condition.

## 5. What they want to receive

- A **technical report** for the development team, with reproduction steps and a fix for every finding.
- A **test summary letter** to send to the rental company, with no exploitable details.
- A retest of critical findings after they are fixed.

## 6. Constraints

- Testing on **staging only**. Production is out of scope.
- Test accounts created for this engagement; no real customer accounts.
- Timeline: 5 to 7 business days.
- No technique that could take the environment down: the team keeps using staging during the test.

## 7. Open questions

- Is there API documentation (OpenAPI/Swagger)?
- Which user roles exist besides driver and mechanic?
- Do the coupons have real value, or are they test-only?
- Who should be notified if a critical finding comes up during testing?
