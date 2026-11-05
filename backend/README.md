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
- `GET /api/integrations` to view the active provider per channel
- `POST /api/notifications` to record a notification request
- `GET` and `POST /api/rules` to manage event-triggered notification rules
- `POST /api/events/orders-created` to process an `OrderCreated` event

## Order-created flow

Create a rule with `eventType` set to `OrderCreated`, a template ID, delivery
channel, and category. When the order system calls `/api/events/orders-created`,
each matching active rule creates a notification request for that customer. This
first implementation records requests synchronously; asynchronous delivery through
the Worker and RabbitMQ comes in the next step.

## Development delivery providers

Email, SMS, and Push use the `Development` provider by default. Each adapter accepts
a delivery request, writes a safe log entry, and returns a normalized `Accepted`
result with a development message ID. Change `Integrations:{Channel}:Provider` in
environment configuration when a real adapter is added; provider selection is kept
inside the Integrations module.

Integrations, notifications, rules, outbox, inbox, scheduling, and provider delivery
through external vendors are deliberately deferred to their respective implementation phases.
