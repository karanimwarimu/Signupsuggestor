

# 🛡️ C# Login & Signup Application with AI-Powered Username Suggestions

## 📌 Overview

This is a **C# Windows Forms Application** connected to **SQL Server** that provides a simple **Login & Signup system**.
What makes it **unique** is the integration of an **AI-based username suggestion feature**:

* When a user enters their **full name** during signup, the system (CONNECTED TO OPEN API ) generates a list of **cool, creative, and unique usernames**.
* The app checks availability in the database to ensure **no duplicates**.

---

## ✨ Features

* 🔐 **User Authentication**

  * Secure Login with username & password.
  * New user registration with validation.

* 🧠 **AI Username Suggestions**

  * Suggests multiple **unique usernames** based on the entered full name.
  * Example: If you type `John Smith`, suggestions might be:

    * `john_s`
    * `smithyJ`
    * `theRealJohn`
    * `johnx2025`

* 💾 **SQL Server Integration**

  * Stores user details (FullName, Username, Password, Role).
  * Validates username uniqueness before saving.

* ⚡ **CRUD Support for Users**

  * Create new users.
  * Read/Display user list (for admin).
  * Update user details.
  * Delete user accounts.

---

## 🛠️ Tech Stack

* **Frontend/UI**: C# WinForms
* **Backend**: .NET Framework 4.7.2+
* **Database**: SQL Server
* **AI/Logic**: Simple name transformation rules + uniqueness check , USE open ai (GET AN OPEN API KEY)
* **Configuration**: `App.config` connection string

---

## ⚙️ Setup Instructions

### 1️⃣ Database Setup


```

### 2️⃣ Update Connection String (`App.config`)

```xml
   <connectionStrings>
			<add name="MyDbConnection"
					 connectionString="Server=MACHINENAME/INSTANCENAME;Database=DATABASENAME;Trusted_Connection=True;TrustServerCertificate=True"
					 providerName="System.Data.SqlClient" />
		</connectionStrings>
```
### SETUP API KEY 

```
	<appSettings>
		<add key="OpenAI_API_Key" value="PUT OPEN API KEY HERE " />
	</appSettings>
```

### 3️⃣ Run the Application

* Open in **Visual Studio**.
* Restore dependencies.
* Press **F5** to run.

---

## 🚀 Usage Flow

1. Open the app.
2. Click **Sign Up**.
3. Enter **Full Name + Password**.
4. System suggests **cool usernames** → pick one.
5. Account is saved in SQL Server.
6. Use chosen username & password to **Login**.

---

## 🧩 Future Improvements
* Add **role-based access control** (Admin vs User).

---

## 📸 Demo (optional)

👉 You can add screenshots of:

* Signup form with AI suggestions.
* Login screen.
* Database table view.

---

