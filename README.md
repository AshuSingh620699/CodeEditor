# 🖥️ Code Editor (ASP.NET Web Forms)

An interactive **online code editor** built with ASP.NET and ADO.NET, enabling users to write, execute, and test code in real time using the **JDoodle API**. It mimics core features of online compilers with support for multiple languages, user dashboards, and real-time execution.

---

## ✨ Features

- 🧑‍💻 Execute code instantly via JDoodle API
- 🔐 User authentication (Sign up/Login/Forgot Password)
- 📊 Code output and error handling
- 💻 Clean UI built with Bootstrap and custom styles

---

## 🛠️ Tech Stack

- **Frontend**: ASP.NET Web Forms, HTML, CSS, JavaScript,Bootstrap
- **Backend**: C#, ADO.NET
- **Execution API**: [JDoodle](https://www.jdoodle.com/)
- **IDE**: Visual Studio
- **Database**: SQL Server

---

## 📂 Folder Structure

```
├── Projects.aspx # Main editor interface
├── LoginForm.aspx # User login page
├── signUp.aspx # Registration
├── forgot.aspx # Forgot password form
├── userDashboard.aspx # Dashboard for users
├── App_Data/Images # Editor/branding images
├── Scripts/ # JS and Bootstrap
├── Content/ # CSS files
├── CodeExecutorService.cs # JDoodle API handler
├── RealTimeExecutionService.cs # Output parsing
└── Web.config # App configuration
```
