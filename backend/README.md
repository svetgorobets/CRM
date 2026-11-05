# Notification Management backend

The backend is a modular monolith with separate API and Worker hosts. Modules own
their domain, application, and infrastructure projects. The hosts compose those
projects without exposing module internals to each other.

## Local infrastructure

Run `docker compose up -d` from this directory to start PostgreSQL and RabbitMQ.
The development connection settings live in the API and Worker `appsettings.Development.json`
files. They are suitable only for local development; production secrets belong in a
secret store or environment configuration.

## Current phase

The foundation, Customers, and Templates modules are implemented. In Development,
the API applies their migrations on startup after PostgreSQL is available.

Available endpoints:

- `GET`, `POST`, and `PUT /api/customers`; `PUT /api/customers/{id}/preferences`
- `GET`, `POST`, `PUT`, and `DELETE /api/templates`
- `POST /api/templates/{id}/render` with a JSON `values` object

Integrations, notifications, rules, outbox, inbox, scheduling, and provider delivery
are deliberately deferred to their respective implementation phases.
