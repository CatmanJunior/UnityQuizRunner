Here's a table outlining the methods for each game state in your design pattern, providing a clear overview of the functionalities required for each state:

| Game State     | Enter Method                                         | Exit Method                                        | HandleInput Method                                  | Update Method                                       |
|----------------|------------------------------------------------------|----------------------------------------------------|-----------------------------------------------------|----------------------------------------------------|
| **MainMenu**   | Initialize/display main menu UI. <br>Prepare for player check-ins. | Hide/clean up main menu UI.                        | Handle main menu navigation and player check-ins.   | Update any main menu animations or timers.         |
| **CategoryVote** | Display category voting UI. <br>Start countdown for voting. | Hide/clean up category voting UI.                  | Handle inputs for category voting.                  | Update UI and handle voting countdown timer.       |
| **StartQuiz**  | Initialize the quiz. <br>Load questions and start quiz timer. | Clean up before moving to question state.          | May not be needed unless pre-quiz interaction exists. | Update any pre-quiz animations or timers.         |
| **Question**   | Display the current question. <br>Start countdown for answering. | Clean up after the question is answered or times out. | Handle player inputs for answering questions.       | Update question UI and handle countdown timer.     |
| **PostQuestion** | Display correct answers and update scores. <br>Start timer before next question. | Clean up post-question UI.                         | Typically no input handling needed.                 | Update any post-question animations or timers.     |
| **FinalScore** | Display final scores and rankings. <br>Options for replay or exit to main menu. | Clean up final score UI.                           | Handle navigation in the final score screen.        | Update final score screen animations/UI elements.  |

Each state encapsulates the behavior necessary for its respective part of the game, ensuring that the code remains organized and each class's responsibilities are clearly defined. The `StateManager` or `GameStateHandler` would manage transitions between these states and delegate responsibilities like input handling and updates to the active state.

