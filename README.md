# Chess Companion
Chess Companion is a C# application that functions as a tool to improve users' chess skills on the website chess.com. The application offers various features, including an integrated chess engine that enables analysis and evaluation of the user's moves. The user has the option to choose up to five evaluation lines, showing the best moves in the current position. Additionally, the application provides feedback on the user's moves based on a comparison between the user's moves and the best possible moves in that particular position.

The application is designed following the Model-View-ViewModel (MVVM) architecture, allowing a clear separation of responsibilities and facilitating code maintenance and extension. The Model component includes logic for manipulating the game flow, fetching information from chess.com, and a DOM manipulator to modify the website's view. ChessViewModel and GameMediator act as view models and facilitate communication between the WPF interface and the logic.



# Features
Analyze move: See how your move compares to the best move.
* See whether the move is bad, good, best move or a blunder
* Identifies bad moves in real times so you can improve your chess skills.
![AnalyzeGif](https://github.com/martinkoch1/ChessCompanion/assets/71707790/c326ee73-efce-4b62-bc14-97465d5f0f93)


Evaluation bar: See how good your position is compared to your opponent.
* See if you are currently ahead or behind your opponent.
* Notifies you if there's a forced checkmate and how many moves it will take.
https://github.com/martinkoch1/ChessCompanion/assets/71707790/6d74260f-b2fb-4bc6-be78-f95a6c4c6680

AutoMove: Let the computer take over and play the best moves for you.
* Use with caution! You will get banned if you use the computer against real people.

# Technologies used:
* Selenium WebDriver: Used for scraping the website.
* .NET Framework.
* Web Technologies: CSS and javascript for manipulation the website.
* WPF.
* Threading.
* Linq.
* MVVM architecture
  
