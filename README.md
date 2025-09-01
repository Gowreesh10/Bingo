# Bingo Game Application

A console-based Bingo game application built with .NET Core 8.0, featuring a client-server architecture with file-based message queuing for inter-process communication.

## 🎯 Project Overview

This Bingo game consists of three main components:
- **Bingo.Server**: Game server that manages matches, players, and game logic
- **Bingo.Client**: Console client for players to join games and play
- **Bingo.Shared**: Shared models and infrastructure for communication

## 🛠️ Technologies Used

### Core Technologies
- **.NET Core 8.0** - Target framework for all projects
- **C#** - Primary programming language
- **Console Application** - User interface for both server and client

### Key Libraries & Packages
- **Newtonsoft.Json (13.0.1)** - JSON serialization for data persistence
- **xUnit (2.9.3)** - Unit testing framework
- **Microsoft.NET.Test.Sdk (17.8.0)** - Test execution engine

### Architecture Patterns
- **Domain-Driven Design (DDD)** - Clean separation of domain logic
- **State Machine Pattern** - Game state management
- **Repository Pattern** - Data persistence abstraction
- **File-based Message Queue** - Inter-process communication

## 📁 Project Structure

```
Bingo/
├── Bingo.Server/                 # Game server application
│   ├── Domain/                   # Domain logic
│   │   ├── Entities/            # Core business entities
│   │   │   ├── Match.cs         # Game match entity
│   │   │   ├── Player.cs        # Player entity
│   │   │   ├── BingoCard.cs     # Bingo card entity
│   │   │   └── BingoNumber.cs   # Individual number entity
│   │   ├── Services/            # Business services
│   │   │   ├── CardGenerator.cs # Generates valid Bingo cards
│   │   │   ├── NumberDrawer.cs  # Draws random numbers
│   │   │   └── WinEvaluator.cs  # Evaluates winning conditions
│   │   ├── Abstractions/        # Interfaces
│   │   │   ├── IMatchRepository.cs
│   │   │   └── IWinRule.cs
│   │   └── Rules/               # Win condition rules
│   │       └── IWinRule.cs      # Row, Column, Diagonal win rules
│   ├── StateMachine/            # Game state management
│   │   ├── IGameState.cs        # State interface
│   │   ├── GameStateHandler.cs  # State coordinator
│   │   ├── LobbyState.cs        # Waiting for players
│   │   ├── TicketingState.cs    # Distributing cards
│   │   ├── DrawState.cs         # Drawing numbers
│   │   ├── WinCheckState.cs     # Checking for winners
│   │   └── ResultState.cs       # Game results
│   ├── Infrastructure/          # Data persistence
│   │   └── JsonMatchRepository.cs
│   ├── CardGeneratorTests.cs    # Unit tests
│   ├── WinEvaluatorTests.cs     # Unit tests
│   └── Program.cs               # Server entry point
├── Bingo.Client/                # Client application
│   ├── Program.cs               # Client entry point
│   └── Bingo.Client.csproj      # Client project file
├── Bingo.Shared/                # Shared components
│   ├── Models/                  # Shared data models
│   │   └── GameMessage.cs       # Message format
│   ├── Infrastructure/          # Shared infrastructure
│   │   └── FileMessageQueue.cs  # File-based messaging
│   └── Bingo.Shared.csproj      # Shared project file
└── Bingo.sln                    # Solution file
```

## 🎮 Game Features

### Core Gameplay
- **5x5 Bingo Cards** with traditional B-I-N-G-O column ranges
- **FREE Space** in the center (automatically marked)
- **Multiple Win Conditions**: Row, Column, or Diagonal completion
- **Real-time Number Drawing** with random selection
- **Multi-player Support** with individual cards per player

### Game States
1. **Lobby** - Waiting for players to join
2. **Ticketing** - Distributing Bingo cards to players
3. **Drawing** - Drawing and announcing numbers
4. **Win Check** - Evaluating winning conditions
5. **Results** - Displaying game results

## 🚀 How to Run


### Step-by-Step Instructions

#### 1. Clone and Navigate
```bash
git clone <repository-url>
cd Bingo
```

#### 2. Restore Dependencies
```bash
dotnet restore
```

#### 3. Build the Solution
```bash
dotnet build
```

#### 4. Run Tests (Optional)
```bash
dotnet test
```

#### 5. Start the Game Server
```bash
dotnet run --project Bingo.Server
```

The server will start and display:
```
Bingo Server Started
Waiting for players...
```

#### 6. Start Client Applications
Open additional terminal windows and run:
```bash
dotnet run --project Bingo.Client
```

Each client will prompt for a player name:
```
Enter your name: [Your Name]
```

#### 7. Game Flow
1. **Join Game**: Clients automatically join the server
2. **Receive Cards**: Server distributes unique Bingo cards
3. **Start Game**: Server begins drawing numbers
4. **Mark Numbers**: Clients mark numbers on their cards
5. **Win Detection**: Server checks for winning conditions
6. **Results**: Game ends when someone wins

### Alternative: Run Individual Projects

#### Server Only
```bash
cd Bingo.Server
dotnet run
```

#### Client Only
```bash
cd Bingo.Client
dotnet run
```

## 🧪 Testing

### Run All Tests
```bash
dotnet test
```

### Run Specific Test Project
```bash
dotnet test Bingo.Server
```


## 📋 Game Rules

### Bingo Card Layout
- **5x5 Grid** with B-I-N-G-O columns
- **Column Ranges**:
  - B: 1-15
  - I: 16-30
  - N: 31-45 (center is FREE)
  - G: 46-60
  - O: 61-75

### Winning Conditions
- **Row Win**: Complete any horizontal row
- **Column Win**: Complete any vertical column
- **Diagonal Win**: Complete either diagonal

### Number Drawing
- Numbers 1-75 are drawn randomly without replacement
- Each number is announced with its letter (B-15, I-23, etc.)
- FREE space is automatically marked

## 🔧 Configuration

### File Locations
- **Match Data**: Stored in JSON format in the server directory
- **Message Queue**: Uses file-based communication between server and clients
- **Logs**: Console output for game events


## 🐛 Troubleshooting

### Common Issues

#### Build Errors
```bash
# Clean and rebuild
dotnet clean
dotnet build
```



## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Add tests for new functionality
4. Ensure all tests pass
5. Submit a pull request

---

**Enjoy playing Bingo! 🎉**