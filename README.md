## Battleship State Tracker
![BattleShipStateTracker ASP.NET Core Web API](https://github.com/nohabasiony85/BattleshipStateTracker/workflows/BattleShipStateTracker%20ASP.NET%20Core%20Web%20API/badge.svg)
### Background
Emulate the classic game "Battleship".
- Two players
- Each have a 10x10 board
- During setup, players can place an arbitrary number of “battleships” on their
board. The ships are 1-by-n sized, must fit entirely on the board, and must be
aligned either vertically or horizontally.
- During play, players take turn “attacking” a single position on the opponent’s
board, and the opponent must respond by either reporting a “hit” on one of
their battleships (if one occupies that position) or a “miss”
- A battleship is sunk if it has been hit on all the squares it occupies
- A player wins if all of their opponent’s battleships have been sunk. 



Implementation Details
 - ASP.NET Core 3.1 
 - Serilog
 - Unit tests Completed ( xUnit and Moq )
 - Integration Tests ( Not Completed )
 - Github Actions Build and Deployment Pipeline (Hosted in Azure Web Apps, URL: https://battleshipstatetrackerapi-m.azurewebsites.net/)
 - Postman collection provided.
