# Underwater-Riches

## MAIN MENU SCENE

here you have the option to choose wether or not you wish to have the speed boost item.
simply make the selection with you mouse

## MAINGAME

The submarine is controlled the following way:

Left Arrow key or A: move left
Right Arrow key or D: move right

Right Arrow key or W or Space: Force Upwards
LeftShift: Use Boost (if available)

OBJECTIVE:
Collect the gold pieces and bring them to the boat!
But watch out for the sharks, and octopuses, they will hurt you!
When pressing up, you will receive a small amount of force upwards. You must therefore press up repeatedly to gain substantial force and quick movement.
Collecting gold adds weight and makes moving more difficult, especially gold bags. They are however, particularly rewarding.
Horizontal movement does not work on this force system intentionally. This is to make avoiding the enemies easier since the game is hard enough as it is.
Also, if you gain enough speed and exit the water, you cannot add upwards force until you return to the water.

Levels increase on a timer (40seconds by default).
5 seconds before the next level begins, all enemies will gain a slight boost in speed. Every subsequent enemy spawned will also be faster.
after a few levels, the enemies' speed is substantial, making later levels an actual challenge.

(optional) boost mode
Noz cylinders will spawn on the floor alongside the gold in this mode. Once collected, you have access to 3 seconds of nitrous boost.
Nitrous is only consumed when holding shift, and resupplied when getting another cylinder. They do no stack, so getting 2 cylinders will still only award the player with 3 seconds of use.
This encourages the player to use it as soon as they get it.

## CODE
The code is split into six scripts.
- GameManager takes care of the bulk of the level.
	It spawns enemies, players, gold, boost, manipulates the score, respawns player on death, etc.
- Player handles the player's input as well as their interaction with collisions and triggers.
	It also creates the bubble particles during motion
	It also calls back important events to the gamemanager, such as death and score
- sharkController and OctopusController
	I grouped them together since they're almost identical.
	If I had time I would refactor them to inherit from the same class considering the amount of things they have in common.
	The only difference is that sharks teleport to the other side of the screen whereas octopuses change direction.
- goldController
	this one only determines if enough time has passed and gold needs to despawn itself
- StaticData 
	This is a static class used to pass data between the Menu Scene and the MainGame. It only contains the gamemode.