# Web & API Security Assessment

A hands-on web and API penetration test, delivered the way a client would
receive it: rules of engagement, reconnaissance, manual testing, a report
written for both executives and engineers, and a demonstrated code-level fix.

The target is **OWASP crAPI** (a deliberately vulnerable "connected-car" app),
run locally. The client "Carrolink" is fictional. Every test was run against my
own lab — no third-party system was touched.

## What's inside

| Deliverable | File |
| --- | --- |
| Rules of engagement (scope & authorization) | `docs/rules-of-engagement.md` |
| Reconnaissance & threat model | `docs/recon.md` |
| Full technical report | `report/security-assessment-report.md` |
| Test summary letter (for auditors) | `report/test-summary-letter.md` |
| Evidence (redacted) | `report/evidence/` |
| Code-level fix for the critical finding (.NET) | `fix-demo/` |

## Headline finding

**Critical — Broken Object Level Authorization (BOLA)** on vehicle location:
any authenticated user could retrieve the real-time location and owner details
of any other user's vehicle by changing the ID in the request. Found by manual
testing; no automated scanner detected it.

## Method

Manual testing with Burp Suite, backed by an OWASP ZAP baseline scan. The scan
found only configuration issues; the critical authorization flaw was found by
hand — which is the point: scanners cover breadth, a tester covers depth.

## Fix demonstrated, not just reported

The critical flaw was reproduced in a minimal .NET API and fixed in code, with a
before/after retest proving the exploit stops working while legitimate use
continues. See `fix-demo/` and section 4 of the report.

## Author

André Soler — Cloud & Application Security
[LinkedIn](https://linkedin.com/in/andresolereng) · [GitHub](https://github.com/andre-soler)
