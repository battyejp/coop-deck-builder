# Cooperative Deck Builder

A Unity-based cooperative deck-building game where players work together to overcome challenges using strategic card play and deck management.

## Project Overview

This project provides a foundational framework for creating cooperative deck-building games in Unity. Players collaborate by sharing resources, coordinating strategies, and combining their deck abilities to achieve common objectives.

### Key Features
- **Cooperative Gameplay**: Players work together towards shared goals
- **Dynamic Deck Building**: Cards can be acquired, modified, and shared between players
- **Modular Card System**: Extensible card framework supporting various card types and effects
- **Flexible Game Flow**: Adaptable turn structure supporting different cooperative mechanics

## Setup Instructions

### Prerequisites
- Unity 2022.3 LTS or later
- Git for version control

### Getting Started
1. Clone this repository:
   ```bash
   git clone https://github.com/battyejp/coop-deck-builder.git
   cd coop-deck-builder
   ```

2. Open Unity Hub and create a new Unity project

3. Copy the contents of this repository into your Unity project:
   - Copy `Assets/Scripts/` to your project's `Assets/Scripts/` folder
   - Copy `docs/` folder to your project root for reference

4. Review the documentation in the `docs/` folder for project structure guidelines

5. Import the scripts into Unity and follow the setup instructions in `docs/SceneSetup.md`

## Contributing

We welcome contributions to improve and expand this cooperative deck-builder framework!

### How to Contribute
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes following our coding standards
4. Test your changes thoroughly
5. Commit your changes (`git commit -m 'Add amazing feature'`)
6. Push to the branch (`git push origin feature/amazing-feature`)
7. Open a Pull Request

### Development Guidelines
- Follow Unity C# coding conventions
- Add XML documentation to public methods and classes
- Include unit tests for new functionality when applicable
- Update documentation for any architectural changes

### Reporting Issues
Please use the GitHub Issues page to report bugs or request features. Include:
- Unity version
- Steps to reproduce (for bugs)
- Expected vs actual behavior
- Screenshots or logs when applicable

## Project Roadmap

### Phase 1: Core Foundation ✓
- [x] Basic project structure
- [x] Core Card and Deck classes
- [x] Basic game flow management
- [x] Documentation framework

### Phase 2: Cooperative Mechanics (Planned)
- [ ] Player interaction system
- [ ] Shared resource management
- [ ] Turn coordination mechanics
- [ ] Victory/defeat conditions

### Phase 3: Advanced Features (Future)
- [ ] Card effect system
- [ ] Deck modification mechanics
- [ ] Save/load functionality
- [ ] Multiplayer networking support

### Phase 4: Polish & Extensions (Future)
- [ ] UI/UX improvements
- [ ] Audio system integration
- [ ] Achievement system
- [ ] Modding support

## Architecture

This project follows a modular architecture designed for extensibility:

- **Card System**: Scriptable Object-based cards with flexible effect systems
- **Deck Management**: Centralized deck operations with event-driven updates
- **Game Flow**: State machine-based game progression
- **Player Coordination**: Event-based communication between players

For detailed architectural information, see `docs/UnityProjectStructure.md`.

## License

This project is open source and available under the [MIT License](LICENSE).

## Contact

For questions or discussions about this project, please:
- Open an issue on GitHub
- Start a discussion in the GitHub Discussions tab
- Reach out to the maintainers

---

**Note**: This is a foundational framework. Actual game content, art assets, and specific game rules need to be implemented based on your cooperative deck-building game design.