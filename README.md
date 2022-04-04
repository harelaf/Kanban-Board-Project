# Kanban Board

A Kanban Board is a project management tool designed to help visualize work and maximize efficiency.

## About The Project

This project uses a 3-tier architecture. The 'Data Layer' uses DB Browser for the RDBMS. The 'Business Layer is written in C#. The 'Presentation Layer' uses XAML to display the GUI.

## Usage

There is a problem with one of the dll files. Follow these steps in order to get the project to work:
1. Open the .sln file and run the Presentation project.
2. If a message about the security debugging option pops up click 'Ok' and ignore it.
3. You will recieve an exception, stop the run.
4. In the source files, go to Presentation/bin/Debug and delete 'kanbanDB.db'.
5. Run the project again, everything should work now.

## Contributors
- Harel Afriat

- Avishay Mamrud

- Yoad Ohayon
