# Sandbox code writing - .NET Developer Test Assignment

This repository contains the solution for the .NET Developer Test Assignment. The goal of the task was to create a sandbox program that solves programming problems using the C# language, the .NET platform, and UWP/WPF/WinUI3 technologies.

## Features Implemented

- **MVVM Architecture**: The application follows the MVVM pattern to separate concerns between the UI and the business logic, making it scalable and easy to maintain.
  
- **Code Editor**: 
  - A functional code editor where users can input their code.
  - Support for switching between multiple programming languages (C#, Java, JavaScript, Pascal, PHP, Python, Ruby, etc.).
  - Memory limit and runtime limit settings.
  - Display of progress during code compilation and execution, showing the status and result (output or error).

- **UI Design**: 
  - Aesthetic UI with modern controls and appropriate styling, avoiding overly simplistic design.
  - Responsive layout and user-friendly interface.
  
- **Additional Features**:
  - **Light/Dark Theme**: Users can switch between light and dark themes for better user experience.
  - **Localization Support**: Multiple language support for UI text.
  - **Syntax Highlighting**: Added syntax highlighting for supported languages using an external library.

- **Task List** (Bonus Feature):
  - Implemented the task list feature, fetching problems from the Leetcode public API.
  - Users can view problem descriptions, input, and expected output from the platform.

## Architecture

The application uses the following patterns:
- **MVVM**: Ensures separation of concerns and easy maintainability.
- **Dependency Injection**: For better testability and decoupling of classes.
- **Async Programming**: For non-blocking code execution, particularly during HTTP requests and long-running operations.

## Technologies Used

- **C#**: Core language for the application logic.
- **.NET (WPF)**: UI framework.
- **Hackerearth Open API**: For fetching programming problems.
- **Leetcode Public API**: For fetching task list and task details.
- **External Libraries**: 
  - Libraries for syntax highlighting and working with JSON.

## Additional Information

- The application uses the standard `HttpClient` to make API requests and handles responses.
- The source code is stored in a public Git repository with a commit history.
- API keys have been excluded from the repository for security reasons.

## Conclusion

This solution demonstrates the implementation of a sandbox program that adheres to the requirements of the assignment. The application provides an intuitive interface for solving programming problems with additional useful features to enhance the user experience.

