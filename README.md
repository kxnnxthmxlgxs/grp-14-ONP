<div align="center">

<br/>

```
███╗   ██╗███╗   ███╗██████╗       ██╗  ██╗██╗      █████╗ ██████╗ ███████╗██╗   ██╗███████╗
████╗  ██║████╗ ████║██╔══██╗      ██║  ██║██║     ██╔══██╗██╔══██╗██╔════╝╚██╗ ██╔╝██╔════╝
██╔██╗ ██║██╔████╔██║██████╔╝█████╗███████║██║     ███████║██████╔╝███████╗ ╚████╔╝ ███████╗
██║╚██╗██║██║╚██╔╝██║██╔══██╗╚════╝██╔══██║██║     ██╔══██║██╔══██╗╚════██║  ╚██╔╝  ╚════██║
██║ ╚████║██║ ╚═╝ ██║██████╔╝      ██║  ██║███████╗██║  ██║██████╔╝███████║   ██║   ███████║
╚═╝  ╚═══╝╚═╝     ╚═╝╚═════╝       ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚═════╝ ╚══════╝   ╚═╝   ╚══════╝
```

**Nelson Mandela Bay Haematology Laboratory System**

*Transforming haematology lab workflows — from paper to platform.*

<br/>

![Status](https://img.shields.io/badge/status-active%20development-brightgreen?style=for-the-badge)
![Platform](https://img.shields.io/badge/platform-ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Database](https://img.shields.io/badge/database-SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![SDG](https://img.shields.io/badge/UN%20SDG-Goal%203%20%7C%20Good%20Health-4C9F38?style=for-the-badge)
![Module](https://img.shields.io/badge/module-ONP400-0078D4?style=for-the-badge)

<br/>

</div>

---

## 📋 Overview

**NMB-HLabSys** is a web-based laboratory test request and management platform developed for the **ONP400 Applied Software Development** project. It replaces manual paper-based and email-driven workflows with a centralized digital solution designed for real-world haematology operations.

> Built in alignment with **UN SDG Goal 3 — Good Health and Well-Being**, this system supports more efficient healthcare services and lab processes across Nelson Mandela Bay.

---

## 🎯 Project Objectives

| Goal | Description |
|------|-------------|
| 🔄 **Streamline requests** | Centralize laboratory test request management |
| 📦 **Track processing** | Improve test tracking from submission to result |
| ✅ **Reduce errors** | Eliminate missing or incorrect information |
| 💬 **Improve communication** | Bridge the gap between healthcare providers and labs |
| 🔒 **Secure access** | Provide patients with secure, role-based access to results |
| 📦 **Manage stock** | Support consumables and inventory management |

---

## 👥 System Roles & Modules

<table>
<tr>
<td width="50%" valign="top">

### 🧪 Lab Manager
- Manage test categories and types
- Manage consumables and suppliers
- Monitor stock levels and reorder alerts
- Manage technician and doctor accounts
- Generate laboratory reports (PDF)

### 👨‍⚕️ Doctor
- Manage patient records
- Create and submit test requests
- Track request statuses
- View and release test results
- Receive abnormal result alerts
- Generate reports

</td>
<td width="50%" valign="top">

### 🔬 Lab Technician
- Receive and process samples
- Assign and perform tests
- Capture lab results
- Two-step result verification
- Workload dashboard
- Generate technician reports

### 🧑‍🦱 Patient
- Registration and profile management
- View lab requests and results
- Download PDF reports
- Manage medical history
- Grant/revoke doctor access permissions
- Track result trends over time

</td>
</tr>
</table>

### 🛡️ Admin Module
Manages supporting system data including medical conditions, allergies, medications, and other reference data.

---

## ✨ Core Features

### 🔐 Authentication & Security
- Secure login with password hashing and encryption
- Password reset via email
- Role-based access control (RBAC)
- Secure logout

### 📊 Reporting
- PDF report generation across all modules
- Abnormal result alerting
- Technician workload dashboards
- Patient result trend tracking

---

## 🛠 Technology Stack

> ⚠️ Final stack subject to change during development.

| Layer | Technology |
|-------|-----------|
| **Frontend** | HTML5, CSS3, JavaScript, Bootstrap / Tailwind *(TBD)* |
| **Backend** | C#, ASP.NET Core / ASP.NET MVC *(TBD)* |
| **Database** | Microsoft SQL Server |
| **Version Control** | Git & GitHub |
| **Other** | REST APIs, Email Integration, PDF Generation |

---

## 🏗 Software Engineering Concepts Applied

```
OOP  ·  Database Design & Normalization  ·  Referential Integrity
Authentication & Authorization  ·  CRUD Operations  ·  MVC Architecture
Input Validation  ·  Error Handling  ·  Reporting Systems
Team Collaboration (GitHub)  ·  Agile-style Development
```

---

## 📁 Repository Structure

```
/project-root
│
├── 📂 frontend/          # HTML, CSS, JS, UI components
├── 📂 backend/           # C# / ASP.NET Core application
├── 📂 database/          # SQL scripts and schema
├── 📂 documentation/     # Project docs and specs
├── 📂 reports/           # Generated or sample reports
└── 📄 README.md
```

---

## 🚀 Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Git](https://git-scm.com/)

### Setup

```bash
# 1. Clone the repository
git clone <repository-url>

# 2. Navigate into the project
cd project-folder

# 3. Set up the database
#    - Install Microsoft SQL Server
#    - Create the project database manually
#    - Apply SQL scripts from /database folder

# 4. Run the application
#    Instructions will be added once the stack is finalized
```

---

## 🔀 Git Workflow

```bash
# Pull latest changes
git pull origin main

# Create a feature branch
git checkout -b feature/your-feature-name

# Stage and commit
git add .
git commit -m "feat: describe what you added"

# Push your branch
git push origin feature/your-feature-name
```

> 📌 All pull requests **must be reviewed** before merging into `main`.

---

## 👨‍💻 Team & Subsystem Ownership

| Subsystem | Responsibility |
|-----------|---------------|
| 🧪 Laboratory Manager Module | Group Member 1 |
| 👨‍⚕️ Doctor Module | Group Member 2 |
| 🔬 Lab Technician Module | Group Member 3 |
| 🧑‍🦱 Patient & Admin Module | Group Member 4 |

---

## 📌 Project Status

> 🚧 **Active Development** — Features, architecture, and documentation will continue to evolve throughout the project lifecycle.

---

## 📜 License

This project is developed for **educational purposes** as part of the **ONP400 Applied Software Development** module.

---

<div align="center">

*Built with purpose. Designed for impact.*

</div>
