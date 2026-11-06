# F-01 — BOLA fix (demonstrated in .NET)

The crAPI finding (BOLA on vehicle location) was reproduced and fixed in a
minimal .NET API to demonstrate the root cause and the remediation.

## Vulnerable code

    app.MapGet("/vehicle/{id}/location", (int id) =>
    {
        var vehicle = vehicles.FirstOrDefault(v => v.Id == id);   // no owner check
        return vehicle is null ? Results.NotFound() : Results.Ok(new { vehicle.Location });
    }).RequireAuthorization();

Behaviour: driver-a's token reading vehicle 2 (driver-b's) returned 200 + location.

## Fixed code

    app.MapGet("/vehicle/{id}/location", (int id, ClaimsPrincipal user) =>
    {
        var userEmail = user.FindFirstValue(ClaimTypes.Email);
        var vehicle = vehicles.FirstOrDefault(v => v.Id == id && v.OwnerEmail == userEmail);
        return vehicle is null ? Results.NotFound() : Results.Ok(new { vehicle.Location });
    }).RequireAuthorization();

Key points:
- Owner identity comes from the validated token (ClaimsPrincipal), never from the request.
- Ownership is enforced in the query: id AND owner must match.
- Returns 404 (not 403) so the response does not confirm the object exists.

## Retest (after fix)

- Own vehicle (1): 200 + location — legitimate use still works.
- Other's vehicle (2): 404 — the attack no longer leaks data.
