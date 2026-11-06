# Training Notes — Phase 2 (OWASP Juice Shop)

Warm-up before testing the client system (crAPI). Goal: get fluent with Burp
and ZAP on known challenges, one per OWASP category. None of this goes in the
client report.

## Challenges solved

| # | Challenge | OWASP category | How I found it | How I would fix it |
| --- | --- | --- | --- | --- |
| 1 | Score Board | Reconnaissance / A05 Security Misconfiguration | Searched the bundled `main.js` for route definitions and found `routerLink` pointing to `/score-board` | Hidden routes are not access control. Remove unused routes from production builds, and protect sensitive pages with server-side authorization |
| 2 | Login Admin | A03 Injection (SQLi) | A single quote in the email field caused a SQLITE_ERROR, showing the input was concatenated into the SQL query. `' OR 1=1--` made the WHERE clause always true and commented out the password check | Use parameterized queries (`@email` parameters or Entity Framework in .NET) so input is always treated as data, never as SQL |
| 2b | Error Handling | A05 Security Misconfiguration | The login API returned the raw SQLite error in the response body | Handle exceptions globally; log details server-side only and return a generic message |
| 3 | DOM XSS | A03 Injection (XSS) | The search term is reflected in the results title. `<b>test</b>` rendered as bold, so input was inserted as HTML. An `<iframe>` with a `javascript:` URL executed script | In Angular, render user input with interpolation (`{{ }}`), which escapes HTML by default. Never use `bypassSecurityTrustHtml` on user input. Add a Content Security Policy as a second layer |
| 4 | View Basket | A01 Broken Access Control (API1 BOLA) | Captured `GET /rest/basket/1`, sent it to Repeater and changed the ID to 2. The response returned another user's basket (`UserId: 2`) with 200 OK. Also reproduced through the UI by changing `bid` in Session Storage | Check ownership on the server for every object request: load the basket by ID AND by the user ID taken from the token; return 404 if it does not belong to the caller. Never trust IDs sent by the client |
| 5 | Confidential Document | A01 Broken Access Control / A05 Misconfiguration | The legal document lived under `/ftp/`. Removing the filename exposed a directory listing; `acquisitions.md` opened directly | Disable directory listing on the server. Store non-public files outside the web root, behind authentication. Use an allow-list of extensions, not a block-list |

## Side observations

- The JWT in the `Authorization` header and cookies carries user data; tokens
  should never hold sensitive fields.
- `Access-Control-Allow-Origin: *` on API responses — permissive CORS, worth
  flagging.

## Manual vs automated (ZAP baseline)

ZAP baseline found: 0 High, 2 Medium, 4 Low, 4 Informational — all configuration
issues (missing security headers, cookie flags, server version disclosure).

It found NONE of the five issues found by hand, including two critical ones
(SQL injection login bypass and BOLA on the basket).

Takeaway: automated scanners cover mechanical, configuration-level issues well,
but miss business-logic and authorization flaws — which are usually the most
severe. The value of a manual test is exactly what the scanner cannot see.
This is why the report must combine both: the scanner for breadth, the tester
for depth.
