# Rules of Engagement — Carrolink

> Scope and authorization document. No testing starts before both parties sign.

## 1. In-scope targets

| Target | Address |
| --- | --- |
| Staging web application | `http://localhost:8888` |
| Staging API (used by the web app and the mobile app) | `http://localhost:8888/identity/*`, `/community/*`, `/workshop/*` |

**Anything not listed in this table is out of scope**, even if it seems related to the system.

## 2. Out of scope

- Production environment.
- Staging mailbox (MailHog, `http://localhost:8025`): may be **read** to receive codes for the test accounts, but is not a test target.
- Infrastructure (servers, network, direct database access).
- Third-party services integrated with the platform.
- Any other application on the same machine.

## 3. Test accounts

| Account | Role | Purpose |
| --- | --- | --- |
| `driver-a@example.com` | Regular user | General testing |
| `driver-b@example.com` | Regular user | Check whether one account can access the other's data |
| Workshop account, if available | Mechanic | Check separation between roles |

Account passwords are stored in a `.env` file, outside the repository.

## 4. Testing window

- **Period:** November 9–13, 2026.
- **Hours:** 9 a.m. to 6 p.m., Brasília time (UTC−3).

## 5. Permitted techniques

- Browsing and traffic interception through a proxy.
- Automated vulnerability scanning with rate limiting.
- Manual testing of authorization, authentication, injection and business logic.
- Repeated authentication attempts, **up to 100 requests per test**, to verify rate limiting.

## 6. Prohibited techniques

- Denial of service, or any test aimed at taking the environment down.
- Social engineering against Carrolink staff.
- Modifying or deleting data from accounts other than the test accounts.
- Using discovered vulnerabilities to access out-of-scope systems.
- Retaining access after the testing window ends.

## 7. Critical findings

If a finding allows access to other users' data, account takeover or code execution on the server:

1. Testing of that area stops.
2. The technical contact is notified **within 2 hours**, through the agreed channel, with a summary that does not include exploitation steps.
3. The finding is included in the report as usual.

## 8. Evidence handling

- Raw traffic captures stay only on the tester's machine, outside any repository.
- Evidence included in the report has tokens, passwords and personal data redacted.
- All raw captures are deleted 30 days after the report is delivered.

## 9. Authorization

Carrolink authorizes André Soler to perform the tests described in this document, on the targets, within the period and using the techniques defined above.

| | Name | Title | Signature | Date |
| --- | --- | --- | --- | --- |
| Carrolink | | CTO | | |
| Tester | André Soler | Security Consultant | | |
