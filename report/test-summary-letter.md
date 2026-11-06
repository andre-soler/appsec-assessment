# Security Test Summary — Carrolink Platform

**Date:** November 6, 2026
**Prepared by:** André Soler, Security Consultant
**Prepared for:** Carrolink — to share with partners and auditors

---

This letter summarizes an independent security test of the Carrolink connected-car platform. It is intended to be shared with customers and auditors. It deliberately contains no exploitation details.

## Scope

The web application and its API were tested in a dedicated staging environment, with written authorization and a defined testing window. Production systems were not involved. Testing followed the OWASP Testing Guide, the OWASP Top 10 and the OWASP API Security Top 10.

## Result

The test identified **one critical issue**, related to server-side authorization, together with a small number of lower-risk configuration and session-management items. The critical issue was reproduced, and a concrete remediation was provided and validated.

| Severity | Count |
| --- | --- |
| Critical | 1 |
| Medium | 1 |
| Low | 2 |

## Status

A remediation plan was delivered with the full technical report. The critical issue has a clear, low-effort fix. A re-test is recommended once the fixes are deployed, after which an updated summary can be issued.

## Scope of this letter

This summary is provided for information to third parties. The detailed technical report, including evidence and reproduction steps, is confidential and shared only with Carrolink's engineering team.

---

André Soler
Security Consultant
