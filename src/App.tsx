import { FormEvent, ReactNode, useMemo, useState } from "react"

type Page = "Dashboard" | "Customers" | "Notifications" | "Templates" | "Rules" | "Scheduled" | "Integrations" | "Settings"

type IconName = "grid" | "users" | "bell" | "file" | "branch" | "clock" | "plug" | "settings" | "search" | "plus" | "chevron" | "more" | "mail" | "phone" | "push" | "check" | "alert" | "menu" | "panel" | "x" | "arrow" | "copy" | "key" | "bolt" | "calendar" | "external"

const paths: Record<IconName, ReactNode> = {
  grid: (
    <>
      <rect x="3" y="3" width="7" height="7" rx="1" />
      <rect x="14" y="3" width="7" height="7" rx="1" />
      <rect x="3" y="14" width="7" height="7" rx="1" />
      <rect x="14" y="14" width="7" height="7" rx="1" />
    </>
  ),
  users: (
    <>
      <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2" />
      <circle cx="9" cy="7" r="4" />
      <path d="M22 21v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75" />
    </>
  ),
  bell: (
    <>
      <path d="M18 8a6 6 0 0 0-12 0c0 7-3 7-3 9h18c0-2-3-2-3-9" />
      <path d="M10 21h4" />
    </>
  ),
  file: (
    <>
      <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z" />
      <path d="M14 2v6h6M8 13h8M8 17h5" />
    </>
  ),
  branch: (
    <>
      <circle cx="6" cy="5" r="3" />
      <circle cx="18" cy="6" r="3" />
      <circle cx="18" cy="18" r="3" />
      <path d="M6 8v5a5 5 0 0 0 5 5h4M9 5h6" />
    </>
  ),
  clock: (
    <>
      <circle cx="12" cy="12" r="9" />
      <path d="M12 7v5l3 2" />
    </>
  ),
  plug: (
    <>
      <path d="M12 22v-5M9 8V2M15 8V2M18 8v3a6 6 0 0 1-12 0V8z" />
    </>
  ),
  settings: (
    <>
      <circle cx="12" cy="12" r="3" />
      <path d="M19.4 15a1.7 1.7 0 0 0 .34 1.88l.06.06-2.83 2.83-.06-.06a1.7 1.7 0 0 0-1.88-.34 1.7 1.7 0 0 0-1.03 1.56V21h-4v-.08A1.7 1.7 0 0 0 8.94 19.4a1.7 1.7 0 0 0-1.88.34l-.06.06-2.83-2.83.06-.06A1.7 1.7 0 0 0 4.6 15 1.7 1.7 0 0 0 3.08 14H3v-4h.08A1.7 1.7 0 0 0 4.6 9a1.7 1.7 0 0 0-.34-1.88L4.2 7.06l2.83-2.83.06.06A1.7 1.7 0 0 0 9 4.6 1.7 1.7 0 0 0 10 3.08V3h4v.08A1.7 1.7 0 0 0 15 4.6a1.7 1.7 0 0 0 1.88-.34l.06-.06 2.83 2.83-.06.06A1.7 1.7 0 0 0 19.4 9c.12.62.68 1.07 1.32 1.08H21v4h-.08A1.7 1.7 0 0 0 19.4 15z" />
    </>
  ),
  search: (
    <>
      <circle cx="11" cy="11" r="7" />
      <path d="m20 20-4-4" />
    </>
  ),
  plus: <path d="M12 5v14M5 12h14" />,
  chevron: <path d="m9 18 6-6-6-6" />,
  more: (
    <>
      <circle cx="5" cy="12" r="1" fill="currentColor" />
      <circle cx="12" cy="12" r="1" fill="currentColor" />
      <circle cx="19" cy="12" r="1" fill="currentColor" />
    </>
  ),
  mail: (
    <>
      <rect x="3" y="5" width="18" height="14" rx="2" />
      <path d="m3 7 9 6 9-6" />
    </>
  ),
  phone: (
    <>
      <path d="M22 16.9v3a2 2 0 0 1-2.18 2 19.8 19.8 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6A19.8 19.8 0 0 1 2.12 4.2 2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72c.12.9.33 1.78.62 2.63a2 2 0 0 1-.45 2.11L8 9.73a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45c.85.3 1.73.5 2.63.62A2 2 0 0 1 22 16.9z" />
    </>
  ),
  push: (
    <>
      <path d="M6 8a6 6 0 0 1 12 0c0 7 3 7 3 9H3c0-2 3-2 3-9" />
      <path d="M10 21h4M9 4h6" />
    </>
  ),
  check: <path d="m5 12 4 4L19 6" />,
  alert: (
    <>
      <path d="M10.3 3.6 1.8 18a2 2 0 0 0 1.7 3h17a2 2 0 0 0 1.7-3L13.7 3.6a2 2 0 0 0-3.4 0z" />
      <path d="M12 9v4M12 17h.01" />
    </>
  ),
  menu: <path d="M4 6h16M4 12h16M4 18h16" />,
  panel: (
    <>
      <rect x="3" y="4" width="18" height="16" rx="2" />
      <path d="M9 4v16" />
    </>
  ),
  x: <path d="M18 6 6 18M6 6l12 12" />,
  arrow: <path d="M5 12h14m-5-5 5 5-5 5" />,
  copy: (
    <>
      <rect x="8" y="8" width="12" height="12" rx="2" />
      <path d="M16 8V6a2 2 0 0 0-2-2H6a2 2 0 0 0-2 2v8a2 2 0 0 0 2 2h2" />
    </>
  ),
  key: (
    <>
      <circle cx="8" cy="15" r="4" />
      <path d="m11 12 9-9M17 6l3 3M14 9l3 3" />
    </>
  ),
  bolt: <path d="m13 2-9 12h8l-1 8 9-12h-8z" />,
  calendar: (
    <>
      <rect x="3" y="5" width="18" height="16" rx="2" />
      <path d="M16 3v4M8 3v4M3 10h18" />
    </>
  ),
  external: (
    <>
      <path d="M15 3h6v6M10 14 21 3" />
      <path d="M18 13v6a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h6" />
    </>
  ),
}

function Icon({ name, size = 18 }: { name: IconName size?: number }) {
  return (
    <svg
      aria-hidden="true"
      width={size}
      height={size}
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="1.8"
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      {paths[name]}
    </svg>
  )
}

const navPrimary: { name: Page icon: IconName }[] = [
  { name: "Dashboard", icon: "grid" },
  { name: "Customers", icon: "users" },
  { name: "Notifications", icon: "bell" },
  { name: "Templates", icon: "file" },
  { name: "Rules", icon: "branch" },
  { name: "Scheduled", icon: "clock" },
]
const navConfig: { name: Page icon: IconName }[] = [
  { name: "Integrations", icon: "plug" },
  { name: "Settings", icon: "settings" },
]

const notifications = [
  {
    id: "ntf_8F4K2M",
    name: "Olivia Martin",
    contact: "olivia@acme.co",
    channel: "Email",
    template: "Order confirmed",
    status: "Delivered",
    created: "Today, 10:42 AM",
  },
  {
    id: "ntf_2P9L7A",
    name: "Ethan Williams",
    contact: "+1 415 820 4412",
    channel: "SMS",
    template: "Delivery update",
    status: "Processing",
    created: "Today, 10:39 AM",
  },
  {
    id: "ntf_6Q1B3R",
    name: "Sophia Chen",
    contact: "sophia@northstar.io",
    channel: "Email",
    template: "Welcome aboard",
    status: "Delivered",
    created: "Today, 10:31 AM",
  },
  {
    id: "ntf_4D8J5C",
    name: "Noah Johnson",
    contact: "Device •••• 926",
    channel: "Push",
    template: "Payment received",
    status: "Failed",
    created: "Today, 10:18 AM",
  },
  {
    id: "ntf_1H7N9T",
    name: "Emma Davis",
    contact: "emma@linear.co",
    channel: "Email",
    template: "Password reset",
    status: "Pending",
    created: "Today, 10:12 AM",
  },
]

const customers = [
  {
    initials: "OM",
    name: "Olivia Martin",
    email: "olivia@acme.co",
    phone: "+1 415 821 0934",
    id: "cus_8F4K2M",
    created: "May 24, 2025",
  },
  {
    initials: "EW",
    name: "Ethan Williams",
    email: "ethan@studio.com",
    phone: "+1 415 820 4412",
    id: "cus_2P9L7A",
    created: "May 22, 2025",
  },
  {
    initials: "SC",
    name: "Sophia Chen",
    email: "sophia@northstar.io",
    phone: "—",
    id: "cus_6Q1B3R",
    created: "May 18, 2025",
  },
  {
    initials: "NJ",
    name: "Noah Johnson",
    email: "noah@basecamp.dev",
    phone: "+44 20 7946 0921",
    id: "cus_4D8J5C",
    created: "May 16, 2025",
  },
  {
    initials: "ED",
    name: "Emma Davis",
    email: "emma@linear.co",
    phone: "+1 646 555 0128",
    id: "cus_1H7N9T",
    created: "May 12, 2025",
  },
]

function Channel({ name }: { name: string }) {
  const icon: IconName =
    name === "Email" ? "mail" : name === "SMS" ? "phone" : "push"
  return (
    <span className={`channel channel-${name.toLowerCase()}`}>
      <Icon name={icon} size={14} />
      {name}
    </span>
  )
}

function Status({ value }: { value: string }) {
  return (
    <span className={`status status-${value.toLowerCase()}`}>
      <span />
      {value}
    </span>
  )
}

function PageHeader({
  title,
  subtitle,
  action,
  onAction,
}: {
  title: string
  subtitle: string
  action?: string
  onAction?: () => void
}) {
  return (
    <header className="page-header">
      <div>
        <h1>{title}</h1>
        <p>{subtitle}</p>
      </div>
      {action && (
        <button className="btn btn-primary" onClick={onAction}>
          <Icon name="plus" />
          {action}
        </button>
      )}
    </header>
  )
}

function TableShell({
  children,
  count = "1–5 of 48",
}: {
  children: ReactNode
  count?: string
}) {
  return (
    <div className="table-card">
      <div className="table-scroll">{children}</div>
      <div className="pagination">
        <span>Showing {count}</span>
        <div>
          <button className="icon-btn" disabled aria-label="Previous page">
            <Icon name="chevron" size={16} />
          </button>
          <button className="page-num active">1</button>
          <button className="page-num">2</button>
          <button className="page-num">3</button>
          <button className="icon-btn next" aria-label="Next page">
            <Icon name="chevron" size={16} />
          </button>
        </div>
      </div>
    </div>
  )
}

function Filters({
  placeholder = "Search…",
  children,
}: {
  placeholder?: string
  children?: ReactNode
}) {
  return (
    <div className="filters">
      <label className="search">
        <Icon name="search" size={17} />
        <input aria-label={placeholder} placeholder={placeholder} />
      </label>
      <div className="filter-group">{children}</div>
    </div>
  )
}

function SelectFilter({ children }: { children: ReactNode }) {
  return (
    <button className="filter-btn">
      {children}
      <span className="down">⌄</span>
    </button>
  )
}

function Dashboard({
  navigate,
  createNotification,
}: {
  navigate: (p: Page) => void
  createNotification: () => void
}) {
  const metrics = [
    {
      label: "Total notifications",
      value: "24,892",
      change: "+12.5%",
      tone: "violet",
      icon: "bell" as IconName,
    },
    {
      label: "Pending",
      value: "128",
      change: "8 processing",
      tone: "blue",
      icon: "clock" as IconName,
    },
    {
      label: "Successfully delivered",
      value: "23,946",
      change: "96.2% rate",
      tone: "green",
      icon: "check" as IconName,
    },
    {
      label: "Failed",
      value: "818",
      change: "3.2% rate",
      tone: "red",
      icon: "alert" as IconName,
    },
  ]
  return (
    <>
      <PageHeader
        title="Dashboard"
        subtitle="Overview of your notification activity."
        action="Create notification"
        onAction={createNotification}
      />
      <section className="metric-grid">
        {metrics.map((m) => (
          <article className="metric-card" key={m.label}>
            <div className={`metric-icon ${m.tone}`}>
              <Icon name={m.icon} size={20} />
            </div>
            <p>{m.label}</p>
            <div>
              <strong>{m.value}</strong>
              <span className={m.tone === "red" ? "muted-change" : "positive"}>
                {m.change}
              </span>
            </div>
          </article>
        ))}
      </section>
      <section className="dashboard-grid">
        <div className="content-card recent-card">
          <div className="card-heading">
            <div>
              <h2>Recent activity</h2>
              <p>Your latest notification events</p>
            </div>
            <button
              className="text-btn"
              onClick={() => navigate("Notifications")}
            >
              View all <Icon name="arrow" size={15} />
            </button>
          </div>
          <div className="table-scroll">
            <table>
              <thead>
                <tr>
                  <th>Recipient</th>
                  <th>Channel</th>
                  <th>Template</th>
                  <th>Status</th>
                  <th>Created</th>
                </tr>
              </thead>
              <tbody>
                {notifications.slice(0, 4).map((n) => (
                  <tr key={n.id}>
                    <td>
                      <strong>{n.name}</strong>
                      <small>{n.contact}</small>
                    </td>
                    <td>
                      <Channel name={n.channel} />
                    </td>
                    <td>{n.template}</td>
                    <td>
                      <Status value={n.status} />
                    </td>
                    <td className="muted-cell">{n.created}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
        <div className="content-card quick-card">
          <div className="card-heading">
            <div>
              <h2>Quick actions</h2>
              <p>Get things done faster</p>
            </div>
          </div>
          {[
            [
              "bell",
              "Create notification",
              "Send a message",
              createNotification,
            ],
            [
              "file",
              "Create template",
              "Build reusable content",
              () => navigate("Templates"),
            ],
            [
              "users",
              "Add customer",
              "Create a new profile",
              () => navigate("Customers"),
            ],
            [
              "plug",
              "Configure integration",
              "Connect a provider",
              () => navigate("Integrations"),
            ],
          ].map(([icon, title, sub, fn]) => (
            <button
              className="quick-action"
              key={title as string}
              onClick={fn as () => void}
            >
              <span>
                <Icon name={icon as IconName} />
              </span>
              <div>
                <strong>{title as string}</strong>
                <small>{sub as string}</small>
              </div>
              <Icon name="chevron" size={16} />
            </button>
          ))}
        </div>
      </section>
    </>
  )
}

function Customers({
  openAdd,
  setDetail,
}: {
  openAdd: () => void
  setDetail: (v: boolean) => void
}) {
  return (
    <>
      <PageHeader
        title="Customers"
        subtitle="Manage customer profiles and communication preferences."
        action="Add customer"
        onAction={openAdd}
      />
      <Filters placeholder="Search name, email or external ID…">
        <SelectFilter>All channels</SelectFilter>
        <SelectFilter>Created date</SelectFilter>
      </Filters>
      <TableShell count="1–5 of 2,481">
        <table>
          <thead>
            <tr>
              <th>Customer</th>
              <th>Email</th>
              <th>Phone</th>
              <th>External ID</th>
              <th>Created</th>
              <th>
                <span className="sr-only">Actions</span>
              </th>
            </tr>
          </thead>
          <tbody>
            {customers.map((c, i) => (
              <tr
                key={c.id}
                className="clickable-row"
                onClick={() => i === 0 && setDetail(true)}
              >
                <td>
                  <div className="person">
                    <span className="avatar-sm">{c.initials}</span>
                    <strong>{c.name}</strong>
                  </div>
                </td>
                <td>{c.email}</td>
                <td className="muted-cell">{c.phone}</td>
                <td>
                  <code>{c.id}</code>
                </td>
                <td className="muted-cell">{c.created}</td>
                <td>
                  <button
                    className="icon-btn"
                    aria-label={`Actions for ${c.name}`}
                  >
                    <Icon name="more" />
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </TableShell>
    </>
  )
}

function Notifications({
  setDetail,
  createNotification,
}: {
  setDetail: (v: boolean) => void
  createNotification: () => void
}) {
  return (
    <>
      <PageHeader
        title="Notifications"
        subtitle="Monitor every message sent through your applications."
        action="Create notification"
        onAction={createNotification}
      />
      <div className="tab-row">
        <button className="tab active">
          All <span>48</span>
        </button>
        <button className="tab">
          Pending <span>8</span>
        </button>
        <button className="tab">
          Delivered <span>36</span>
        </button>
        <button className="tab">
          Failed <span>4</span>
        </button>
      </div>
      <Filters placeholder="Search by ID or recipient…">
        <SelectFilter>All channels</SelectFilter>
        <SelectFilter>All statuses</SelectFilter>
        <SelectFilter>Date range</SelectFilter>
      </Filters>
      <TableShell>
        <table>
          <thead>
            <tr>
              <th>Notification ID</th>
              <th>Recipient</th>
              <th>Channel</th>
              <th>Template</th>
              <th>Status</th>
              <th>Created at</th>
              <th>
                <span className="sr-only">Actions</span>
              </th>
            </tr>
          </thead>
          <tbody>
            {notifications.map((n, i) => (
              <tr
                key={n.id}
                className="clickable-row"
                onClick={() => i === 3 && setDetail(true)}
              >
                <td>
                  <code>{n.id}</code>
                </td>
                <td>
                  <strong>{n.name}</strong>
                  <small>{n.contact}</small>
                </td>
                <td>
                  <Channel name={n.channel} />
                </td>
                <td>{n.template}</td>
                <td>
                  <Status value={n.status} />
                </td>
                <td className="muted-cell">{n.created}</td>
                <td>
                  <button className="icon-btn">
                    <Icon name="more" />
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </TableShell>
    </>
  )
}

function Templates({ openEditor }: { openEditor: () => void }) {
  const rows = [
    [
      "Order confirmation",
      "Sent after a successful order",
      "Email",
      "May 28, 2025",
    ],
    [
      "Delivery update",
      "Shipment status and tracking link",
      "SMS",
      "May 26, 2025",
    ],
    ["Welcome aboard", "New customer welcome message", "Email", "May 22, 2025"],
    [
      "Payment received",
      "Payment success confirmation",
      "Push",
      "May 18, 2025",
    ],
    ["Password reset", "Secure account recovery link", "Email", "May 12, 2025"],
  ]
  return (
    <>
      <PageHeader
        title="Templates"
        subtitle="Create and manage reusable notification content."
        action="Create template"
        onAction={openEditor}
      />
      <Filters placeholder="Search templates…">
        <SelectFilter>All channels</SelectFilter>
        <SelectFilter>Last updated</SelectFilter>
      </Filters>
      <TableShell count="1–5 of 12">
        <table>
          <thead>
            <tr>
              <th>Template name</th>
              <th>Description</th>
              <th>Channel</th>
              <th>Last updated</th>
              <th>
                <span className="sr-only">Actions</span>
              </th>
            </tr>
          </thead>
          <tbody>
            {rows.map((r) => (
              <tr key={r[0]}>
                <td>
                  <strong>{r[0]}</strong>
                </td>
                <td className="muted-cell">{r[1]}</td>
                <td>
                  <Channel name={r[2]} />
                </td>
                <td className="muted-cell">{r[3]}</td>
                <td>
                  <button className="icon-btn">
                    <Icon name="more" />
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </TableShell>
    </>
  )
}

function Rules({ openEditor }: { openEditor: () => void }) {
  const rules = [
    [
      "Order confirmation",
      "OrderCreated",
      "2 actions",
      "Active",
      "May 28, 2025",
    ],
    [
      "New user onboarding",
      "UserRegistered",
      "3 actions",
      "Active",
      "May 26, 2025",
    ],
    [
      "Payment follow-up",
      "PaymentFailed",
      "2 actions",
      "Inactive",
      "May 21, 2025",
    ],
    [
      "Subscription renewal",
      "SubscriptionRenewed",
      "1 action",
      "Active",
      "May 18, 2025",
    ],
  ]
  return (
    <>
      <PageHeader
        title="Rules"
        subtitle="Automate notifications when events happen."
        action="Create rule"
        onAction={openEditor}
      />
      <div className="info-banner">
        <span>
          <Icon name="bolt" />
        </span>
        <div>
          <strong>Rules turn events into notifications</strong>
          <p>
            Connect your application and define what should happen when an event
            is received.
          </p>
        </div>
        <button>
          Read docs <Icon name="external" size={14} />
        </button>
      </div>
      <Filters placeholder="Search rules…">
        <SelectFilter>All statuses</SelectFilter>
      </Filters>
      <TableShell count="1–4 of 4">
        <table>
          <thead>
            <tr>
              <th>Rule name</th>
              <th>Event type</th>
              <th>Actions</th>
              <th>Status</th>
              <th>Last updated</th>
              <th>
                <span className="sr-only">Actions</span>
              </th>
            </tr>
          </thead>
          <tbody>
            {rules.map((r) => (
              <tr key={r[0]}>
                <td>
                  <strong>{r[0]}</strong>
                </td>
                <td>
                  <code>{r[1]}</code>
                </td>
                <td>{r[2]}</td>
                <td>
                  <Status value={r[3]} />
                </td>
                <td className="muted-cell">{r[4]}</td>
                <td>
                  <button className="icon-btn">
                    <Icon name="more" />
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </TableShell>
    </>
  )
}

function Scheduled({ setConfirm }: { setConfirm: (v: boolean) => void }) {
  return (
    <>
      <PageHeader
        title="Scheduled"
        subtitle="View and manage notifications waiting to be sent."
      />
      <Filters placeholder="Search recipient…">
        <SelectFilter>All channels</SelectFilter>
        <SelectFilter>Scheduled date</SelectFilter>
      </Filters>
      <TableShell count="1–4 of 4">
        <table>
          <thead>
            <tr>
              <th>Recipient</th>
              <th>Channel</th>
              <th>Template</th>
              <th>Scheduled time</th>
              <th>Status</th>
              <th>
                <span className="sr-only">Actions</span>
              </th>
            </tr>
          </thead>
          <tbody>
            {notifications.slice(0, 4).map((n, i) => (
              <tr key={n.id}>
                <td>
                  <strong>{n.name}</strong>
                  <small>{n.contact}</small>
                </td>
                <td>
                  <Channel name={n.channel} />
                </td>
                <td>{n.template}</td>
                <td>
                  <div className="date-cell">
                    <Icon name="calendar" size={15} />
                    Jun {2 + i}, 2025 · {9 + i}:00 AM
                  </div>
                </td>
                <td>
                  <Status value="Scheduled" />
                </td>
                <td>
                  <button
                    className="link-danger"
                    onClick={() => setConfirm(true)}
                  >
                    Cancel
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </TableShell>
    </>
  )
}

function Integrations({ toast }: { toast: (m: string) => void }) {
  const integrations = [
    {
      type: "Email",
      provider: "Resend",
      detail: "Transactional email",
      icon: "mail" as IconName,
      connected: true,
      color: "purple",
    },
    {
      type: "SMS",
      provider: "Twilio",
      detail: "Text messaging",
      icon: "phone" as IconName,
      connected: true,
      color: "red",
    },
    {
      type: "Push",
      provider: "Firebase Cloud Messaging",
      detail: "Mobile push",
      icon: "push" as IconName,
      connected: false,
      color: "amber",
    },
  ]
  return (
    <>
      <PageHeader
        title="Integrations"
        subtitle="Connect providers to deliver notifications across every channel."
      />
      <section className="integration-grid">
        {integrations.map((i) => (
          <article className="integration-card" key={i.type}>
            <div className="integration-top">
              <span className={`provider-icon ${i.color}`}>
                <Icon name={i.icon} size={23} />
              </span>
              <Status value={i.connected ? "Connected" : "Not connected"} />
            </div>
            <p className="eyebrow">{i.type}</p>
            <h2>{i.provider}</h2>
            <p>{i.detail}</p>
            <div className="integration-meta">
              {i.connected ? (
                <>
                  <span>API key</span>
                  <code>•••• •••• •••• 7a2f</code>
                </>
              ) : (
                <span>Set up a provider to start sending</span>
              )}
            </div>
            <button
              className={`btn ${i.connected ? "btn-secondary" : "btn-primary"}`}
              onClick={() =>
                toast(
                  i.connected
                    ? `${i.provider} connection verified`
                    : "Configuration panel opened",
                )
              }
            >
              {i.connected ? "Manage connection" : "Configure provider"}
            </button>
          </article>
        ))}
      </section>
      <div className="docs-card">
        <span>
          <Icon name="file" />
        </span>
        <div>
          <strong>Looking for implementation guides?</strong>
          <p>
            Explore our SDK documentation and API reference to connect your
            application.
          </p>
        </div>
        <button className="btn btn-secondary">
          View developer docs <Icon name="external" size={15} />
        </button>
      </div>
    </>
  )
}

function Settings({ toast }: { toast: (m: string) => void }) {
  return (
    <>
      <PageHeader
        title="Settings"
        subtitle="Manage workspace defaults, API access, and your profile."
      />
      <div className="settings-layout">
        <aside>
          <button className="active">General</button>
          <button>API keys</button>
          <button>Profile</button>
        </aside>
        <div className="settings-content">
          <section className="settings-section">
            <div>
              <h2>General settings</h2>
              <p>Configure defaults for your notification workspace.</p>
            </div>
            <form
              onSubmit={(e) => {
                e.preventDefault()
                toast("Settings saved successfully")
              }}
            >
              <div className="field">
                <label>Application name</label>
                <input defaultValue="Notify Production" />
              </div>
              <div className="form-grid">
                <div className="field">
                  <label>Default timezone</label>
                  <select defaultValue="America/New_York">
                    <option>America/New_York</option>
                    <option>Europe/Paris</option>
                    <option>UTC</option>
                  </select>
                </div>
                <div className="field">
                  <label>Default language</label>
                  <select defaultValue="English">
                    <option>English</option>
                    <option>French</option>
                    <option>Spanish</option>
                  </select>
                </div>
              </div>
              <div className="form-actions">
                <button className="btn btn-primary">Save changes</button>
              </div>
            </form>
          </section>
          <section className="settings-section api-preview">
            <div>
              <h2>API access</h2>
              <p>
                Use API keys to authenticate requests from your applications.
              </p>
            </div>
            <div className="api-row">
              <span className="provider-icon purple">
                <Icon name="key" />
              </span>
              <div>
                <strong>Production API key</strong>
                <code>ntf_live_••••••••••••c9x2</code>
                <small>Created May 14, 2025</small>
              </div>
              <button className="btn btn-secondary">Manage keys</button>
            </div>
          </section>
        </div>
      </div>
    </>
  )
}

function Modal({
  title,
  subtitle,
  onClose,
  children,
  wide = false,
}: {
  title: string
  subtitle?: string
  onClose: () => void
  children: ReactNode
  wide?: boolean
}) {
  return (
    <div
      className="modal-backdrop"
      onMouseDown={(e) => e.target === e.currentTarget && onClose()}
    >
      <div
        className={`modal ${wide ? "modal-wide" : ""}`}
        role="dialog"
        aria-modal="true"
        aria-label={title}
      >
        <div className="modal-head">
          <div>
            <h2>{title}</h2>
            {subtitle && <p>{subtitle}</p>}
          </div>
          <button className="icon-btn" onClick={onClose} aria-label="Close">
            <Icon name="x" />
          </button>
        </div>
        {children}
      </div>
    </div>
  )
}

function AddCustomer({
  onClose,
  onSave,
}: {
  onClose: () => void
  onSave: () => void
}) {
  const [attrs, setAttrs] = useState([{ key: "plan", value: "premium" }])
  const submit = (e: FormEvent) => {
    e.preventDefault()
    onSave()
  }
  return (
    <Modal
      title="Add customer"
      subtitle="Create a profile and set communication preferences."
      onClose={onClose}
    >
      <form onSubmit={submit}>
        <div className="modal-body">
          <p className="section-label">Basic information</p>
          <div className="field">
            <label>Full name</label>
            <input required placeholder="e.g. Maya Patel" />
          </div>
          <div className="field">
            <label>
              External ID <span>Optional</span>
            </label>
            <input placeholder="cus_12345" />
          </div>
          <div className="form-grid">
            <div className="field">
              <label>Email address</label>
              <input required type="email" placeholder="maya@company.com" />
            </div>
            <div className="field">
              <label>Phone number</label>
              <input placeholder="+1 555 000 0000" />
            </div>
          </div>
          <p className="section-label spaced">Custom attributes</p>
          {attrs.map((a, i) => (
            <div className="attribute-row" key={i}>
              <input
                aria-label="Attribute key"
                defaultValue={a.key}
                placeholder="Key"
              />
              <input
                aria-label="Attribute value"
                defaultValue={a.value}
                placeholder="Value"
              />
              <button
                type="button"
                className="icon-btn"
                onClick={() => setAttrs(attrs.filter((_, x) => x !== i))}
              >
                <Icon name="x" size={16} />
              </button>
            </div>
          ))}
          <button
            type="button"
            className="add-link"
            onClick={() => setAttrs([...attrs, { key: "", value: "" }])}
          >
            <Icon name="plus" size={15} /> Add attribute
          </button>
          <p className="section-label spaced">Communication preferences</p>
          <div className="preference-row">
            {["Email", "SMS", "Push"].map((p, i) => (
              <label key={p}>
                <span>
                  <Icon name={i === 0 ? "mail" : i === 1 ? "phone" : "push"} />
                  <b>{p}</b>
                </span>
                <input type="checkbox" defaultChecked={i !== 2} />
                <i />
              </label>
            ))}
          </div>
        </div>
        <div className="modal-actions">
          <button type="button" className="btn btn-secondary" onClick={onClose}>
            Cancel
          </button>
          <button className="btn btn-primary">Add customer</button>
        </div>
      </form>
    </Modal>
  )
}

function CreateNotification({
  onClose,
  onDone,
}: {
  onClose: () => void
  onDone: () => void
}) {
  const [step, setStep] = useState(1)
  const [channel, setChannel] = useState("Email")
  const labels = ["Recipient", "Channel", "Content", "Scheduling"]
  return (
    <Modal
      title="Create notification"
      subtitle={`Step ${step} of 4 — ${labels[step - 1]}`}
      onClose={onClose}
      wide
    >
      <div className="steps">
        {labels.map((l, i) => (
          <div className={i + 1 <= step ? "active" : ""} key={l}>
            <span>
              {i + 1 < step ? <Icon name="check" size={14} /> : i + 1}
            </span>
            <b>{l}</b>
          </div>
        ))}
      </div>
      <div className="modal-body flow-body">
        {step === 1 && (
          <>
            <p className="section-label">Choose a recipient</p>
            <label className="search large">
              <Icon name="search" />
              <input
                autoFocus
                placeholder="Search customers by name or email…"
              />
            </label>
            <div className="select-list">
              {customers.slice(0, 3).map((c, i) => (
                <label key={c.id} className={i === 0 ? "selected" : ""}>
                  <span className="avatar-sm">{c.initials}</span>
                  <div>
                    <strong>{c.name}</strong>
                    <small>{c.email}</small>
                  </div>
                  <input
                    type="radio"
                    name="customer"
                    defaultChecked={i === 0}
                  />
                </label>
              ))}
            </div>
            <div className="or">
              <span>or</span>
            </div>
            <button className="btn btn-secondary full">
              Enter recipient manually
            </button>
          </>
        )}
        {step === 2 && (
          <>
            <p className="section-label">Select a delivery channel</p>
            <div className="channel-options">
              {["Email", "SMS", "Push"].map((c, i) => (
                <button
                  className={channel === c ? "selected" : ""}
                  onClick={() => setChannel(c)}
                  key={c}
                >
                  <span
                    className={`provider-icon ${
                      i === 0 ? "purple" : i === 1 ? "green" : "amber"
                    }`}
                  >
                    <Icon
                      name={i === 0 ? "mail" : i === 1 ? "phone" : "push"}
                    />
                  </span>
                  <strong>{c}</strong>
                  <small>
                    {i === 0
                      ? "Rich content and HTML"
                      : i === 1
                        ? "Short text message"
                        : "Mobile device alert"}
                  </small>
                  <i>{channel === c && <Icon name="check" size={14} />}</i>
                </button>
              ))}
            </div>
          </>
        )}
        {step === 3 && (
          <>
            <div className="field">
              <label>Template</label>
              <select>
                <option>Order confirmation</option>
                <option>Welcome aboard</option>
                <option>Password reset</option>
              </select>
            </div>
            {channel === "Email" && (
              <div className="field">
                <label>Subject</label>
                <input defaultValue="Your order has been confirmed" />
              </div>
            )}{" "}
            {channel === "Push" && (
              <div className="field">
                <label>Title</label>
                <input defaultValue="Order confirmed" />
              </div>
            )}
            <div className="field">
              <label>Message</label>
              <textarea
                rows={5}
                defaultValue={`Hi {{firstName}},\n\nGreat news — your order {{orderId}} has been confirmed and is being prepared.`}
              />
              <div className="field-hint">
                <span>
                  Variables: <code>{"{{firstName}}"}</code>{" "}
                  <code>{"{{orderId}}"}</code>
                </span>
                {channel === "SMS" && <span>118 / 160</span>}
              </div>
            </div>
          </>
        )}
        {step === 4 && (
          <>
            <p className="section-label">When should this be sent?</p>
            <div className="schedule-options">
              <label className="selected">
                <input type="radio" name="schedule" defaultChecked />
                <span>
                  <Icon name="bolt" />
                </span>
                <div>
                  <strong>Send immediately</strong>
                  <small>Deliver as soon as you confirm</small>
                </div>
              </label>
              <label>
                <input type="radio" name="schedule" />
                <span>
                  <Icon name="calendar" />
                </span>
                <div>
                  <strong>Schedule for later</strong>
                  <small>Choose a specific date and time</small>
                </div>
              </label>
            </div>
            <div className="summary-box">
              <p>Notification summary</p>
              <div>
                <span>Recipient</span>
                <b>Olivia Martin</b>
              </div>
              <div>
                <span>Channel</span>
                <Channel name={channel} />
              </div>
              <div>
                <span>Template</span>
                <b>Order confirmation</b>
              </div>
            </div>
          </>
        )}
      </div>
      <div className="modal-actions">
        <button
          className="btn btn-secondary"
          onClick={step === 1 ? onClose : () => setStep(step - 1)}
        >
          {step === 1 ? "Cancel" : "Back"}
        </button>
        <button
          className="btn btn-primary"
          onClick={() => (step < 4 ? setStep(step + 1) : onDone())}
        >
          {step < 4 ? (
            <>
              Continue <Icon name="arrow" />
            </>
          ) : (
            "Send notification"
          )}
        </button>
      </div>
    </Modal>
  )
}

function CustomerDetail({ onClose }: { onClose: () => void }) {
  return (
    <Modal
      title="Olivia Martin"
      subtitle="Customer since May 24, 2025"
      onClose={onClose}
      wide
    >
      <div className="detail-hero">
        <span className="avatar-lg">OM</span>
        <div>
          <h3>Olivia Martin</h3>
          <p>olivia@acme.co · +1 415 821 0934</p>
        </div>
        <button className="btn btn-secondary">Edit customer</button>
      </div>
      <div className="modal-body">
        <div className="detail-grid">
          <section>
            <p className="section-label">Customer information</p>
            <dl>
              <div>
                <dt>External ID</dt>
                <dd>
                  <code>cus_8F4K2M</code>
                </dd>
              </div>
              <div>
                <dt>Email</dt>
                <dd>olivia@acme.co</dd>
              </div>
              <div>
                <dt>Phone</dt>
                <dd>+1 415 821 0934</dd>
              </div>
              <div>
                <dt>Plan</dt>
                <dd>Premium</dd>
              </div>
            </dl>
          </section>
          <section>
            <p className="section-label">Communication preferences</p>
            <div className="preference-status">
              <div>
                <span>
                  <Icon name="mail" />
                </span>
                <b>Email</b>
                <Status value="Enabled" />
              </div>
              <div>
                <span>
                  <Icon name="phone" />
                </span>
                <b>SMS</b>
                <Status value="Enabled" />
              </div>
              <div>
                <span>
                  <Icon name="push" />
                </span>
                <b>Push</b>
                <Status value="Disabled" />
              </div>
            </div>
          </section>
        </div>
        <div className="card-heading history-heading">
          <div>
            <h2>Notification history</h2>
            <p>Latest messages sent to this customer</p>
          </div>
        </div>
        <div className="table-card inset">
          <table>
            <thead>
              <tr>
                <th>Date</th>
                <th>Channel</th>
                <th>Template</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              {notifications.slice(0, 3).map((n) => (
                <tr key={n.id}>
                  <td className="muted-cell">{n.created}</td>
                  <td>
                    <Channel name={n.channel} />
                  </td>
                  <td>{n.template}</td>
                  <td>
                    <Status value={n.status} />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </Modal>
  )
}

function NotificationDetail({ onClose }: { onClose: () => void }) {
  return (
    <Modal
      title="Notification details"
      subtitle="ntf_4D8J5C"
      onClose={onClose}
      wide
    >
      <div className="failure-banner">
        <Icon name="alert" />
        <div>
          <strong>Delivery failed</strong>
          <p>
            Device token is no longer registered. Remove the token or ask the
            customer to open the app again.
          </p>
        </div>
      </div>
      <div className="modal-body">
        <div className="detail-grid">
          <section>
            <p className="section-label">Notification</p>
            <dl>
              <div>
                <dt>Recipient</dt>
                <dd>Noah Johnson</dd>
              </div>
              <div>
                <dt>Channel</dt>
                <dd>
                  <Channel name="Push" />
                </dd>
              </div>
              <div>
                <dt>Template</dt>
                <dd>Payment received</dd>
              </div>
              <div>
                <dt>Created</dt>
                <dd>Today, 10:18 AM</dd>
              </div>
            </dl>
          </section>
          <section>
            <p className="section-label">Delivery information</p>
            <dl>
              <div>
                <dt>Provider</dt>
                <dd>Firebase Cloud Messaging</dd>
              </div>
              <div>
                <dt>Message ID</dt>
                <dd>
                  <code>fcm_921kda</code>
                </dd>
              </div>
              <div>
                <dt>Error code</dt>
                <dd>
                  <code>UNREGISTERED</code>
                </dd>
              </div>
              <div>
                <dt>Retry</dt>
                <dd>Not eligible</dd>
              </div>
            </dl>
          </section>
        </div>
        <p className="section-label spaced">Delivery timeline</p>
        <div className="timeline">
          {[
            ["Created", "10:18:02 AM", true],
            ["Pending", "10:18:03 AM", true],
            ["Processing", "10:18:04 AM", true],
            ["Failed", "10:18:06 AM", false],
          ].map(([name, time, ok]) => (
            <div className={ok ? "done" : "failed"} key={name as string}>
              <span>
                {ok ? (
                  <Icon name="check" size={13} />
                ) : (
                  <Icon name="x" size={13} />
                )}
              </span>
              <b>{name as string}</b>
              <small>{time as string}</small>
            </div>
          ))}
        </div>
      </div>
      <div className="modal-actions">
        <button className="btn btn-secondary" onClick={onClose}>
          Close
        </button>
        <button className="btn btn-primary">Retry notification</button>
      </div>
    </Modal>
  )
}

function SimpleEditor({
  type,
  onClose,
  onDone,
}: {
  type: "template" | "rule"
  onClose: () => void
  onDone: () => void
}) {
  return (
    <Modal
      title={`Create ${type}`}
      subtitle={
        type === "template"
          ? "Build reusable content for your notifications."
          : "Automate notifications when an event occurs."
      }
      onClose={onClose}
      wide
    >
      <div className="modal-body editor-body">
        {type === "template" ? (
          <>
            <div className="form-grid">
              <div className="field">
                <label>Template name</label>
                <input placeholder="e.g. Order confirmation" />
              </div>
              <div className="field">
                <label>Channel</label>
                <select>
                  <option>Email</option>
                  <option>SMS</option>
                  <option>Push</option>
                </select>
              </div>
            </div>
            <div className="field">
              <label>Description</label>
              <input placeholder="What is this template used for?" />
            </div>
            <div className="field">
              <label>Subject</label>
              <input placeholder="Your order {{orderId}} is confirmed" />
            </div>
            <div className="editor-grid">
              <div className="field">
                <label>Message content</label>
                <textarea
                  rows={8}
                  defaultValue={
                    "Hi {{firstName}},\n\nYour order {{orderId}} is confirmed."
                  }
                />
              </div>
              <aside className="variables">
                <strong>Available variables</strong>
                <p>Click to copy and insert into your message.</p>
                {[
                  "{{firstName}}",
                  "{{lastName}}",
                  "{{email}}",
                  "{{orderId}}",
                  "{{orderAmount}}",
                ].map((v) => (
                  <button key={v}>
                    <code>{v}</code>
                    <Icon name="copy" size={14} />
                  </button>
                ))}
              </aside>
            </div>
          </>
        ) : (
          <>
            <div className="field">
              <label>Rule name</label>
              <input placeholder="e.g. Order confirmation flow" />
            </div>
            <div className="field">
              <label>Trigger event</label>
              <select>
                <option>OrderCreated</option>
                <option>UserRegistered</option>
                <option>PaymentCompleted</option>
              </select>
            </div>
            <p className="section-label spaced">Actions</p>
            <div className="rule-action">
              <span>1</span>
              <div className="form-grid">
                <div className="field">
                  <label>Channel</label>
                  <select>
                    <option>Email</option>
                    <option>SMS</option>
                    <option>Push</option>
                  </select>
                </div>
                <div className="field">
                  <label>Template</label>
                  <select>
                    <option>Order confirmation</option>
                  </select>
                </div>
              </div>
              <button className="icon-btn">
                <Icon name="x" />
              </button>
            </div>
            <button className="add-link">
              <Icon name="plus" size={15} /> Add notification action
            </button>
          </>
        )}
      </div>
      <div className="modal-actions">
        <button className="btn btn-secondary" onClick={onClose}>
          Cancel
        </button>
        <button className="btn btn-primary" onClick={onDone}>
          {type === "template" ? "Create template" : "Activate rule"}
        </button>
      </div>
    </Modal>
  )
}

export default function App() {
  const [page, setPage] = useState<Page>("Dashboard")
  const [collapsed, setCollapsed] = useState(false)
  const [mobileOpen, setMobileOpen] = useState(false)
  const [overlay, setOverlay] = useState<string | null>(null)
  const [toast, setToast] = useState("")
  const showToast = (message: string) => {
    setToast(message)
    window.setTimeout(() => setToast(""), 2800)
  }
  const navigate = (p: Page) => {
    setPage(p)
    setMobileOpen(false)
    setOverlay(null)
  }
  const content = useMemo(() => {
    if (page === "Dashboard")
      return (
        <Dashboard
          navigate={navigate}
          createNotification={() => setOverlay("create-notification")}
        />
      )
    if (page === "Customers")
      return (
        <Customers
          openAdd={() => setOverlay("add-customer")}
          setDetail={() => setOverlay("customer-detail")}
        />
      )
    if (page === "Notifications")
      return (
        <Notifications
          setDetail={() => setOverlay("notification-detail")}
          createNotification={() => setOverlay("create-notification")}
        />
      )
    if (page === "Templates")
      return <Templates openEditor={() => setOverlay("template-editor")} />
    if (page === "Rules")
      return <Rules openEditor={() => setOverlay("rule-editor")} />
    if (page === "Scheduled")
      return <Scheduled setConfirm={() => setOverlay("confirm-cancel")} />
    if (page === "Integrations") return <Integrations toast={showToast} />
    return <Settings toast={showToast} />
  }, [page])
  const save = (message: string) => {
    setOverlay(null)
    showToast(message)
  }
  return (
    <div className={`app ${collapsed ? "is-collapsed" : ""}`}>
      {mobileOpen && (
        <button
          className="mobile-scrim"
          aria-label="Close menu"
          onClick={() => setMobileOpen(false)}
        />
      )}
      <aside className={`sidebar ${mobileOpen ? "mobile-open" : ""}`}>
        <div className="brand">
          <span className="brand-mark">
            <span />
            <span />
            <span />
          </span>
          {!collapsed && (
            <strong>
              notify<span>.</span>
            </strong>
          )}
          <button
            className="collapse-btn"
            onClick={() => setCollapsed(!collapsed)}
            aria-label="Collapse sidebar"
          >
            <Icon name="panel" size={17} />
          </button>
        </div>
        <nav>
          <p>{!collapsed ? "Workspace" : "···"}</p>
          {navPrimary.map((n) => (
            <button
              key={n.name}
              title={n.name}
              className={page === n.name ? "active" : ""}
              onClick={() => navigate(n.name)}
            >
              <Icon name={n.icon} />
              <span>{n.name}</span>
              {n.name === "Notifications" && !collapsed && <em>8</em>}
            </button>
          ))}
          <p>{!collapsed ? "Configuration" : "···"}</p>
          {navConfig.map((n) => (
            <button
              key={n.name}
              title={n.name}
              className={page === n.name ? "active" : ""}
              onClick={() => navigate(n.name)}
            >
              <Icon name={n.icon} />
              <span>{n.name}</span>
            </button>
          ))}
        </nav>
        <div className="sidebar-footer">
          <div className="help-card">
            <span>
              <Icon name="bolt" size={16} />
            </span>
            <strong>Quick start</strong>
            <small>Complete your setup</small>
            <div>
              <i style={{ width: "75%" }} />
            </div>
            <small>3 of 4 completed</small>
          </div>
          <button className="profile">
            <span className="avatar-sm">MK</span>
            <span>
              <strong>Maya Kim</strong>
              <small>maya@acme.co</small>
            </span>
            <Icon name="more" />
          </button>
        </div>
      </aside>
      <main>
        <div className="mobile-top">
          <button className="icon-btn" onClick={() => setMobileOpen(true)}>
            <Icon name="menu" />
          </button>
          <div className="brand compact">
            <span className="brand-mark">
              <span />
              <span />
              <span />
            </span>
            <strong>
              notify<span>.</span>
            </strong>
          </div>
          <button className="avatar-sm">MK</button>
        </div>
        <div className="topbar">
          <div className="environment">
            <span /> Production <b>⌄</b>
          </div>
          <div>
            <button className="icon-btn" aria-label="Search">
              <Icon name="search" />
            </button>
            <button
              className="icon-btn notification-button"
              aria-label="Notifications"
            >
              <Icon name="bell" />
              <span />
            </button>
            <span className="divider" />
            <button className="docs-link">
              Documentation <Icon name="external" size={14} />
            </button>
          </div>
        </div>
        <div className="content">{content}</div>
      </main>
      {overlay === "add-customer" && (
        <AddCustomer
          onClose={() => setOverlay(null)}
          onSave={() => save("Customer added successfully")}
        />
      )}
      {overlay === "create-notification" && (
        <CreateNotification
          onClose={() => setOverlay(null)}
          onDone={() => save("Notification sent successfully")}
        />
      )}
      {overlay === "customer-detail" && (
        <CustomerDetail onClose={() => setOverlay(null)} />
      )}
      {overlay === "notification-detail" && (
        <NotificationDetail onClose={() => setOverlay(null)} />
      )}
      {overlay === "template-editor" && (
        <SimpleEditor
          type="template"
          onClose={() => setOverlay(null)}
          onDone={() => save("Template created successfully")}
        />
      )}
      {overlay === "rule-editor" && (
        <SimpleEditor
          type="rule"
          onClose={() => setOverlay(null)}
          onDone={() => save("Rule activated successfully")}
        />
      )}
      {overlay === "confirm-cancel" && (
        <Modal
          title="Cancel scheduled notification?"
          subtitle="This action cannot be undone."
          onClose={() => setOverlay(null)}
        >
          <div className="modal-body">
            <div className="confirm-copy">
              <span>
                <Icon name="alert" />
              </span>
              <p>
                The notification for <strong>Olivia Martin</strong> scheduled
                for Jun 2, 2025 will be permanently cancelled.
              </p>
            </div>
          </div>
          <div className="modal-actions">
            <button
              className="btn btn-secondary"
              onClick={() => setOverlay(null)}
            >
              Keep notification
            </button>
            <button
              className="btn btn-danger"
              onClick={() => save("Scheduled notification cancelled")}
            >
              Cancel notification
            </button>
          </div>
        </Modal>
      )}
      {toast && (
        <div className="toast">
          <span>
            <Icon name="check" size={15} />
          </span>
          <strong>{toast}</strong>
          <button onClick={() => setToast("")}>
            <Icon name="x" size={15} />
          </button>
        </div>
      )}
    </div>
  )
}
