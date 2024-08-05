# Personal Diary

Personal Diary is a web application built using ASP.NET Core 8.0, Razor Pages, and SQLite. This application allows users to plan and manage their daily tasks efficiently. It includes a dynamic calendar with support for recurring tasks, task types, and statuses.

## Features

- Create, read, update, and delete (CRUD) tasks.
- Support for recurring tasks (daily, weekly, monthly, yearly).
- Tasks can have different types and statuses.
- Dynamic calendar view using FullCalendar.
- Modal forms for task creation and editing.
- Manage task types with colors.
- View activity statistics on a weekly basis.


## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Development](#development)

## Installation

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

### Clone the Repository

```bash
git clone https://github.com/anoddyne/Personal_Diary.git
cd Personal_Diary
```

### Setup
1. Open the solution file (`Personal_Diary.sln`) in Visual Studio 2022.
2. Restore the NuGet packages by building the solution.

3. Run the following commands to install necessary frontend dependencies:
```bash
cd wwwroot
npm install
```
### Running the Application
1. Set the startup project to `Personal_Diary`.
2. Press `F5` or click on the `Run` button in Visual Studio to start the application.


## Usage

### Development Version
To use the development version of the application:
1. Ensure the application is running by following the [Installation](#installation) steps.
2. Open your browser and navigate to `https://localhost:7056`.

### Release Version
For the release version, download the latest release from the [Releases](https://github.com/anoddyne/Personal_Diary/releases) page. Follow the instructions provided in the release notes to set up and run the application.

## Development 

### Code Structure
- `Pages/` - Contains the Razor Pages for the application.
- `DB_Models/` - Contains the data models used by Entity Framework.
- `wwwroot/` - Contains static files (CSS, JS, images).
- `Migrations/` - Contains database migrations.

### Creating a New Task
1. Click on the `Добавить задачу` button in the upper left corner.
2. Fill in the task details in the modal form.
3. Click `Сохранить` in the lower right corner of the modal window to create the task.
### Editing a Task
1. Click on a task in the calendar.
2. Edit the task details in the modal form.
3. Click `Сохранить` in the lower right corner of the modal window next to the `Удалить` button to update the task.
### Deleting a Task
1. Right-click on a task in the calendar.
2. Select `Удалить` (2nd option) from the context menu.
### Managing Task Types
1. Go to the `Настройка типов задач` tab.
2. Click on `Добавить тип задачи` to create a new task type or click `Редактировать` on an existing task type to edit or click `Удалить` to remove the task type.
3. Fill in the name and select a color for the task type.
4. Click `Сохранить` to save changes or `Закрыть` to close the modal window.

### Viewing Activity Statistics
1. Go to the `Активность` tab.
2. View the bar chart displaying activity hours for each task type over the past four weeks.
3. The Y-axis shows the number of hours, and the X-axis shows the weeks.
4. Each bar represents a task type with its corresponding color and displays the number of hours and task type name.

### Notes
- Tasks cannot span multiple days. Ensure the start and end dates are the same.
- Recurring tasks are displayed dynamically without creating multiple entries in the database.
- Tasks with an empty task type are not considered in the activity statistics.

