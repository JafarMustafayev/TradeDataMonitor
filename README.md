# **Trade Data Monitoring Application**

## **Overview**

The Trade Data Monitoring Application is a desktop-based tool designed to monitor a specified directory for trade data files in various formats (XML, CSV, TXT). The application is built using the **MVVM (Model-View-ViewModel)** architecture and supports configurable directory paths and refresh frequencies. The application processes trade data files and displays the parsed trade information in a user-friendly graphical interface.

## **Features**

- **Real-time Monitoring**: Monitors a specified directory for new or modified trade data files.
- **File Format Support**: Handles XML, CSV, and TXT file formats with the ability to extend support for additional formats.
- **Configurable Settings**: Users can set and update the input directory path and refresh frequency directly from the GUI.
- **Parallel Processing**: Processes files in parallel to ensure the application remains responsive even with large data sets.
- **User Notifications**: Alerts users when changes are made to the input directory or refresh frequency settings.
- **Extensible Architecture**: The application is designed to allow easy integration of additional file loaders or processing logic without requiring recompilation.

## **Technology Stack**

- **Frontend**: WPF (Windows Presentation Foundation)
- **Backend**: C#
- **Architecture**: MVVM (Model-View-ViewModel)
- **Data Handling**: ObservableCollection for real-time UI updates

## **Project Structure**

The project is structured as follows:

- **`TradeDataMonitoringApp`**: The main application containing the `MainWindow.xaml` and the associated `MainViewModel`.
- **`Models`**: Contains the data models, such as `Trade`, that represent the trade data.
- **`ViewModels`**: Contains the `MainViewModel` class that connects the UI with the backend logic.
- **`Services`**: Contains the `FileMonitoringService` class responsible for monitoring the specified directory and loading new trade data.
- **`Views`**: Contains the WPF views (`.xaml` files) that define the user interface.

## **Installation**

1. **Prerequisites**:
   - .NET 8.0 SDK 
   - Visual Studio 2022 or any compatible IDE

2. **Clone the Repository**:
   ```bash
   git clone https://github.com/JafarMustafayev/TradeDataMonitor.git
   cd TradeDataMonitor
   ```

3. **Build the Project**:
   - Open the project in Visual Studio.
   - Build the solution (`Ctrl+Shift+B`) to restore the dependencies and compile the application.

4. **Run the Application**:
   - Set `TradeDataMonitor` as the startup project.
   - Start the application (`F5`).

## **Usage**

1. **Setting the Input Directory**:
   - Upon starting the application, specify the directory to monitor for trade data files.
   - You can change this directory at any time using the "Change Directory" button.

2. **Setting the Refresh Frequency**:
   - Define how frequently (in seconds) the application should check the input directory for new files.
   - Adjust this setting using the "Set Refresh Frequency" button.

3. **Viewing Trade Data**:
   - As files are detected and processed, the parsed trade data will be displayed in the application's main window.
   - The data is shown in a table format, with columns corresponding to the properties of the `Trade` model.

## **Extending the Application**

The architecture allows easy extension of file processing capabilities:

- **Adding New File Formats**:
  - Implement a new file loader by inheriting from a common interface or base class (e.g., `IFileLoader`).
  - Register the new loader in the `FileMonitoringService` so it can be used when new files are detected.

- **Customizing Data Processing**:
  - Modify or extend the `Trade` model to include additional fields as needed.
  - Update the corresponding ViewModel and View to handle and display these fields.
