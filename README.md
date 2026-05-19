# 🌊 FloodBuster v2.0
### Flood Evacuation Path Planning System

![SDG 11](https://img.shields.io/badge/UN%20SDG-11%20Sustainable%20Cities-orange?style=for-the-badge)
![VB.NET](https://img.shields.io/badge/VB.NET-Windows%20Forms-blue?style=for-the-badge&logo=dotnet)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-red?style=for-the-badge&logo=microsoftsqlserver)
![Status](https://img.shields.io/badge/Status-In%20Development-yellow?style=for-the-badge)

> A professional-grade VB.NET Windows Forms Application that supports disaster preparedness by enabling barangay officials and residents to monitor flood-affected areas, manage emergency alerts, and find the safest evacuation routes — built for **SDG 11: Sustainable Cities and Communities**.

---

## Table of Contents

- [About the Project](#about-the-project)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Installation & Setup](#installation--setup)
- [Database Setup](#database-setup)
- [Team Members](#team-members)
- [Academic Context](#academic-context)

---

## About the Project

FloodBuster v2.0 is the evolution of the original FloodBuster C++ console application, now rebuilt as a full **N-Tier VB.NET Windows Forms Application** with persistent SQL Server database integration, role-based login, and Crystal Reports reporting.

Floods are among the most destructive disasters in the Philippines, often leaving communities without clear evacuation guidance. FloodBuster addresses this by providing:

- Real-time flood status tracking per barangay
- Shortest-path evacuation route recommendations
- Emergency alert management
- Evacuation center capacity monitoring
- Summary reports for data-driven decision making

This project directly contributes to **SDG 11, Target 11.5** — reducing deaths and economic losses caused by water-related disasters.

---

## Features

| Feature | Description | Role Access |
|---|---|---|
| Login System | Role-based authentication (Admin / Standard User) | All |
| Flood Status Tracking | Mark and reset flooded barangays in real time | Admin |
| Emergency Alerts | Issue, view, and clear flood alerts per barangay | Admin / View |
| Evacuation Routing | Recommends nearest safe evacuation center via shortest-path logic | All |
| Evacuation Center Management | Track center capacity and occupancy | Admin |
| Report Generation | Monthly Flood Incident & Evacuation Summary via Crystal Reports | Admin |
| User Management | Create and manage Admin / Standard User accounts | Admin |


---

## Tech Stack

- **Language:** Visual Basic .NET (VB.NET)
- **Framework:** .NET Framework 4.7.2+
- **UI:** Windows Forms
- **Database:** Microsoft SQL Server
- **ORM / Data Access:** ADO.NET (SqlClient)
- **Reporting:** SAP Crystal Reports for Visual Studio
- **Version Control:** Git / GitHub

---

## Prerequisites

Before running the project, make sure you have the following installed:

- [ ] [Visual Studio 2019 or 2022](https://visualstudio.microsoft.com/) (with `.NET Desktop Development` workload)
- [ ] [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Developer or Express edition)
- [ ] [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- [ ] [SAP Crystal Reports Runtime for Visual Studio](https://www.sap.com/products/technology-platform/crystal-reports.html)
- [ ] Git (for cloning the repository)

---

## Installation & Setup

### 1. Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/FloodBuster_v.2.0.git
cd FloodBuster_v.2.0
```

### 2. Open the Solution

1. Open **Visual Studio**
2. Go to `File → Open → Project/Solution`
3. Navigate to the `CODE/` folder and open `FloodBuster.sln`

### 3. Restore NuGet Packages

Visual Studio should restore packages automatically. If not:

```
Tools → NuGet Package Manager → Manage NuGet Packages for Solution → Restore
```

### 4. Configure the Database Connection

Open `DAL/DatabaseConnection.vb` and update the connection string to match your SQL Server setup:

```vb
Private Const CONNECTION_STRING As String =
    "Server=YOUR_SERVER_NAME;Database=FloodBusterDB;Integrated Security=True;"
```

Replace `YOUR_SERVER_NAME` with your SQL Server instance name (e.g., `localhost`, `.\SQLEXPRESS`, or `(localdb)\MSSQLLocalDB`).

---

##  Database Setup

### 1. Open SSMS

Connect to your SQL Server instance using **SQL Server Management Studio**.

### 2. Run the Database Script

1. In SSMS, go to `File → Open → File`
2. Open `DATABASE/Database_Script.sql`
3. Click **Execute (F5)**

This will:
- Create the `FloodBusterDB` database
- Create all tables (`Barangays`, `EvacuationCenters`, `FloodAlerts`, `Users`, `BarangayConnections`)
- Insert seed data (sample barangays, evacuation centers, and a default Admin account)

### 3. Default Admin/User Credentials

After running the script, you can log in with:

```
Admin
Username: Maria Victoria
Password: hash_v82n291x

User
Username: Kurt
Password: hash_c33m82q2
```

> **Important:** Change the admin password after first login.

---

## Team Contributions

| Name | Role | Primary Responsibilities |
| :--- | :--- | :--- |
| **Esteban, Maria Victoria N.** | **UI/UX & Documentation Lead** | SDAD authoring, Crystal Reports design, UI Design, and Presentation Layer lead. |
| **Caliguiran, Arjane** | **Business Logic Specialist** | Development of the BLL (Business Logic Layer) and core system algorithms. |
| **Riza, Christina Alexandra** | **Presentation Developer** | Implementation of Windows Forms and UI logic in the Presentation Layer. |
| **Lagrimas, Don Christian** | **Lead Database Engineer** | Database schema design, SQL scripting, and Data Access Layer (DAL) architecture. |
| **Lamayo, Justine Kurt** | **Data Access Developer** | Implementation of repositories and data mapping within the Data Access Layer (DAL). |

---

## Project Architecture Overview

* **Presentation Layer:** Managed by Esteban & Riza (WinForms, Crystal Reports).
* **Business Logic Layer (BLL):** Managed by Caliguiran (Services & Logic).
* **Data Access Layer (DAL):** Managed by Lagrimas & Lamayo (Repositories & SQL).
* **Project Oversight:** Managed by [Your Name] (Integration & Architecture).

##  Academic Context

| Detail | Info |
|---|---|
| **Course** | ITELEC1 — IT Elective 1 (.NET/C#) |
| **Program** | BSIT, 2nd Year |
| **Term** | 2nd Semester, Cycle 2 |
| **Professor** | Prof. Justin Neypes |
| **Institution** | National Teacher's College |
| **Submission Deadline** | May 20, 2026 |
| **UN SDG** | SDG 11 — Sustainable Cities and Communities |

---

> *"By 2030, significantly reduce the number of deaths and the number of people affected... caused by disasters, including water-related disasters."*
> — UN SDG 11, Target 11.5
