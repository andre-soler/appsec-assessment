# Web & API Security Assessment — Carrolink

**Client:** Carrolink (fictional) · **Assessor:** André Soler
**Testing window:** November 2026 · **Report date:** November 6, 2026
**Standards:** OWASP Top 10, OWASP API Security Top 10, OWASP WSTG

---

## 1. Executive summary

Carrolink's connected-car platform was assessed on its staging environment, covering the web application and the API used by the mobile app. Testing was authorized, time-boxed, and read-and-reproduce only; no production system was touched.

The assessment found **one critical issue**: any authenticated user can retrieve the real-time location and owner details of any other user's vehicle. This is both a privacy breach and a physical-safety risk for drivers. The remaining findings are lower-risk configuration and session-management issues.

| Severity | Count |
| --- | --- |
| Critical | 1 |
| Medium | 1 |
| Low | 2 |

**The critical issue should be fixed before onboarding the fleet customer.** It is a server-side authorization gap with a small, well-understood fix (demonstrated in section 4). Once resolved, the platform has no other high-risk exposure in the tested scope.

---

## 2. Scope and method

### Scope

- **In scope:** staging web application and API (`identity`, `community`, `workshop` services).
- **Out of scope:** production, email infrastructure, third-party services, underlying servers and network.

### Method

1. Reconnaissance: mapped the application and API, identified user roles and the most sensitive assets (vehicle location, personal data, orders).
2. Automated scanning (OWASP ZAP) for configuration-level issues.
3. Manual testing of authorization, authentication, injection and session management, using two test accounts to verify cross-user access.
4. Each finding validated by reproduction and recorded with evidence (tokens and personal data redacted).

### Severity scale

| Severity | Criteria |
| --- | --- |
| Critical | Easy to exploit; exposes personal data or allows account/asset takeover |
| Medium | Weakens defenses or exposes the platform |
| Low | Minor weakness; depends on another flaw to cause harm |

---

## 3. Findings

### F-01 — Broken Object Level Authorization on vehicle location

**Severity:** Critical
**Category:** OWASP API Security Top 10 — API1:2023 (BOLA)
**Endpoint:** `GET /identity/api/v2/vehicle/{vehicleId}/location`

**Description.** The endpoint returns a vehicle's real-time location and owner details based only on the vehicle ID in the URL. The server validates that the caller is authenticated, but never checks that the vehicle belongs to the caller. Any logged-in user can read any vehicle by changing the ID.

**Evidence.** Logged in as test account A, a request for account B's vehicle returned HTTP 200 with:
- the vehicle's latitude and longitude;
- the owner's full name and email.

The request used account A's own token throughout. (Full request/response in `report/evidence/F-01-bola-location.md`, redacted.)

**Impact.** Vehicle IDs are UUIDs, but they are exposed to users and can be collected (for example, the `/vehicle/vehicles` endpoint lists them). An attacker with any account can track where other customers' vehicles are, in real time, and tie each to a named person. For a fleet of 800 vehicles this is a serious privacy and physical-safety risk.

**Recommendation.** Enforce object ownership on the server for every object request. Load the vehicle by ID **and** by the owner taken from the authenticated token; return 404 (not 403) when they do not match, so the response does not confirm the object exists. See section 4 for a working fix.

---

### F-02 — Stored XSS in community posts (not vulnerable in tested fields)

**Severity:** Informational (tested, not vulnerable)
**Category:** OWASP Top 10 — A03:2021 (Injection)
**Endpoint:** `POST /community/api/v2/community/posts`

**Description.** Community posts are shown to other users, making them a candidate for stored cross-site scripting. The post title was tested with HTML markup.

**Result.** Input was HTML-encoded on output (`<b>test</b>` rendered as literal text, not bold). The title field is **not vulnerable**. This is recorded as evidence that the area was tested.

**Recommendation.** Keep output encoding on all user-generated content, and add a Content Security Policy (see F-04) as a second layer.

---

### F-03 — Long-lived access token

**Severity:** Low
**Category:** OWASP API Security Top 10 — API2:2023 (Broken Authentication)

**Description.** The JWT access token is valid for 7 days (`exp − iat = 604800s`). A stolen token remains usable for a full week.

**Positive notes.** The token does expire, carries no sensitive data (no password or hash), and is signed with RS256 (resistant to algorithm-confusion attacks).

**Recommendation.** Shorten the access-token lifetime to 15–60 minutes and use a separate refresh-token mechanism.

---

### F-04 — Missing security headers

**Severity:** Medium (CSP) / Low (others)
**Category:** OWASP Top 10 — A05:2021 (Security Misconfiguration)
**Source:** OWASP ZAP baseline scan

| Header | Severity | Effect |
| --- | --- | --- |
| Content-Security-Policy | Medium | No defense-in-depth against XSS |
| X-Content-Type-Options | Low | Browser may MIME-sniff responses |
| Permissions-Policy | Low | No control over browser features |
| Server (version disclosed) | Low | Reveals server software version |

**Recommendation.** Add these headers at the gateway for all responses. A strict `Content-Security-Policy` is the most valuable.

---

## 4. Remediation walkthrough (F-01)

To make the fix concrete, the vulnerability was reproduced and fixed in a minimal .NET API. The same pattern applies to the platform's own code.

**Vulnerable — looks up the object by ID
