# AskJiffy – Backend

This is the **backend API** for a fullstack ChatGPT-style web application, built with **ASP.NET Core**, and integrated with **Google Gemini** for generative AI to stream answers to the UI.

## 🛠️ Tech Stack && Tools used

- **ASP.NET Core Web API**
- **Entity Framework Core** with **SQL Server**
- **JWT Authentication** (Google ID Tokens)
- **Swagger/OpenAPI** for API documentation
- **Dependency Injection** (BL/DAL/DAO pattern)
- **Google Gemini API** integration

## 🔐 Authentication

- Uses **JWT Bearer authentication** via **Google Sign-In**.
- API endpoints are secured with middleware that validates **ID tokens** issued by Google.

## 🤖 AI Services

- **Gemini** (Google): Configured with Gemini Flash 2.0 model, via API key and custom domain.

### 🌐 Frontend
You can find the frontend repository here:  
👉 [Frontend Repo](https://github.com/AanshKot/askJiffy-UI)

<details>
  <summary>📁 Project Structure</summary>

  <br/>

  <ul>
    <li><code>askJiffy_service/</code>
      <ul>
        <li><code>Controllers/</code> – Handles incoming requests from the frontend</li>
        <li><code>Business/</code> – Business logic layer (BL, DAL)</li>
        <li><code>Models/</code> – Data models</li>
        <li><code>Repository/</code> – Data access layer (DAOs)</li>
        <li><code>Services/</code> – Extensions and service registration</li>
        <li><code>Program.cs</code> – Application entry and DI setup</li>
      </ul>
    </li>
  </ul>

</details>
