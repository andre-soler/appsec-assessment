# JWT review — session token

**Date:** 2026-11-06

## Decoded payload
{"sub":"<redacted>","iat":1793979817,"exp":1794584617,"role":"user"}

## Findings
- Token DOES expire (exp present) — good.
- Lifetime: exp - iat = 604800s = 7 days — too long for an access token.
- No sensitive data in payload (no password/hash) — good.
- Header uses RS256 (asymmetric) — good, resistant to alg-confusion attacks.

## Conclusion
Low severity: access-token lifetime of 7 days is excessive. Recommend short-lived
access tokens (15–60 min) with a separate refresh mechanism.
