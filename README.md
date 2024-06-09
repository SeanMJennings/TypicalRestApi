# Crud API
Implement CRUD (Create, Read, Update and Delete) functionality and the corresponding endpoints related to a user.

## Guidelines
- Include testing.
- Demonstrate clean, reuseable code and best practises.

## Approach taken
- Used TDD working from domain outwards to API.
- Used domain driven design to build user entity comprised of primitives to build in validation.

## Notes
- The Repository could have been mocked in the API testing but it seemed equal effort to using the real repository.

## Ideas for expansion
- Add more details to user: password (hashed) address and mobile.
- Consider alternative ORM to entity framework like Dapper for simplicity.
- Add logging.
- Connect to database outside application.
- SPA frontend application over UI.
- CQRS: Separate write and read models for user.