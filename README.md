# User Authentication App

## Project Setup

### Prerequisites


- **.NET 6 or later**: Required for running the backend application. 
- **Node.js 16 or later**: Required for running the frontend. 
- **MongoDB**: A NoSQL database. 
- **Visual Studio** (for backend development) and **Visual Studio Code** (for frontend development). 

### Cloning the Repository

To get started with the project:

1. Open a terminal/command prompt.
2. Clone the repository using the following command:
   bash
   `git clone https://github.com/your-repo/user-authentication-system.git`
3. Navigate to the project directory:
   bash
   `cd user-authentication-system`

### Backend Setup

1.  Navigate to the `backend` folder:
    
    bash
    `cd backend` 
    
2.  Configure the MongoDB
    *   Open `appsettings.json` in the `backend` directory.
    *   Update the following fields:
    json
    `"ConnectionStrings": {
    "MongoDb": "your-mongodb-connection-string"
    }`

3.  Configure Email Service
This project uses Ethereal Mail for email functionality. You can either use your own credentials or the default one already provided in the `appsettings.json` file.

json
    `"EmailSettings": {
        "Host": "smtp.ethereal.email",
        "Port": 587,
        "Username": "your-ethereal-username",
        "Password": "your-ethereal-password"
    }` 
      
  
4.  Restore dependencies and start the backend server:
    
    bash
    `dotnet restore
    dotnet run` 
    
5.  By default, the backend will run at `https://localhost:7104`. If you need to run it on a different URL, update the `authService.js` file in the frontend to match this URL.
    

### Frontend Setup

1.  Navigate to the `frontend` folder:
    
    bash
    `cd ../frontend` 
    
2.  Install the necessary dependencies:
    
    bash
    `npm install` 
    
3.  Configure the API URL:
    
    *   Open `authService.js` in the `frontend/src/services` directory.
    *   Ensure the `API_URL` matches your backend server (default is `https://localhost:7104`).
    javascript
      `const API_URL = "https://your-backend-url";`
  
4.  Start the frontend development server:
    
    bash
    `npm run dev` 
    
    By default, this will serve the frontend on `http://localhost:5173`.
> **Note**: Sometimes the page needs refresh to show navigation bar after login. If navigation bar is not seen on the page, refresh the page.   

### Using the System

1.  **Register**:
    
    *   Navigate to the registration page.
    *   Enter your name, email, and password to create an account.
2.  **Verify Email**:
    
    *   Check your email for a verification code.
    *   Enter the code on the verification page to activate your account.
3.  **Login**:
    
    *   Use your email and password to log in.
4.  **Forgot Password**:
    
    *   Enter your email on the forgot password page to receive a reset link.
    *   Follow the instructions to reset your password.
5.  **Admin Reports**:
    
    *   Admins can access reports by navigating to the admin dashboard.

> **Note**: The front-end assumes the backend is running at `https://localhost:7104`. If your backend is on a different URL, update the `API_URL` in the `authService.js` file accordingly.

Features
--------

*   User registration with email verification.
*   Login with secure authentication.
*   Password reset with a verification code.
*   Admin dashboard with user reports.