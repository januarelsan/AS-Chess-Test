## Features
- Board Creator

Set the pieces type, team, and position. The default board set are the same as the original chess, but we can create custom board sets like adding the amount of Rook Piece.
- Choose a Team color for the Player.
- 3 Game Modes
  - Human vs Human
  - Human vs AI
  - AI vs AI

## Draw Conditions
- King vs King
- King & a Minor Piece vs King
- King & a Minor Piece vs King & a Minor Piece
- King & 2 Knights vs King
- Stalemate

> The Minor pieces are Bishop & Knight.

Draw Conditions reference : https://www.chess.com/article/view/how-chess-games-can-end-8-ways-explained#insufficient-material

## AI Engine
AI Agent runs on the following Cycle:
	> Sense → Think → Act → Sense (Cycled)

- Sense
In this step, The Agent will sense all legal/possible tiles that the agent can move. 

- Think
After that, The Agent will think (evaluate) all possible tiles score. The evaluation method can form The Agent's behavior. This is a list of the evaluation methods:
  - Check if the move can leave the king unprotected (threatened)
This method can form behavior to not leave the king unprotected.
  - Check if the king is under threat and if the agent can protect the king. If the agent can protect the king but its self not safe, then the king will move instead of the agent protecting him.
  - Check if the agent is under threat. If yes, then the agent has to move. But if the agent can capture the threatener and the threatener value greater than the agent, so the agent will capture the threatener
  - Check if the agent can give a threat to the enemy pieces. This evaluation can make the agent more aggressive in attack.
  - Check if the king can do castling. Since the castling is good to make sure the king is safe, then castling is evaluated very high. 
  - If the king is not under threat, the king will move passively. But, If a team piece amount is under 5, then the king tends to move aggressively to make sure the king helps other pieces attack.
  
- Act
After all agent possible moves are evaluated, the highest evaluated move will be taken.

Every Piece type has the value to help the agent decide if the agent wants to capture it, threatened it, or just stay to challenge it. The following are the values that are taken into use:
- Pawn : 10
- Knight : 30
- Bishop : 30
- Rook : 50
- Queen : 90
- King : 900

## Asset Source:

- Piece Icon:
https://commons.wikimedia.org/wiki/Category:SVG_chess_pieces#/media/File:Chess_Pieces_Sprite.svg
- Tile and Piece Texture:
https://www.freepik.com/free-photo/oak-wooden-textured-design-background_18835065.htm




