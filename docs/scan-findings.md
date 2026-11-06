# Automated scan findings (ZAP baseline) — crAPI

Passive scan only. Configuration-level issues; no auth/authorization flaws
(those come from manual testing in Phase 5).

| Severity | Finding | Affected |
| --- | --- | --- |
| Medium | Content Security Policy (CSP) header not set | All responses |
| Low | X-Content-Type-Options header missing | All responses |
| Low | Server leaks version via `Server` header | All responses |
| Low | Permissions-Policy header not set | All responses |
| Low | Cross-Origin-Embedder-Policy header missing | All responses |
