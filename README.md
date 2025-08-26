# Solar System Game


## About
A to-scale model of our solar system in Unity based off of data from NASA. Uses a 2 body solver and syncs planet position to a real world clock based on their keps at the J2000 epoch. This means that while playing this game, the planets in the game are where they are in real life.

Players control a spaceship (brick) and can travel between the planets of the solar system and various structures. I originally planed to have mining, building, and crafting features, but I decided to shelve this project and work on other things.

Uses floating origin technology to allow warping between planets and has a grid system. Grids are orbitals that be given a parent and orbit other things; entities that are on grids can move around in regular 3d space like any other video game.


## Videos

#### Warp Drive
Automatically scales warp speed and acceleration when warping into a new area.

https://github.com/user-attachments/assets/57e627b0-ae00-41d4-af09-822344d5eff0

https://github.com/user-attachments/assets/ecc48d90-ac37-4a10-896a-05b9073307dc

#### Dynamic Zooming Camera
Smoothly scales camera zoom-level out. Shows Earth, Terra Station, and the player ship which are all a different order of magnitude in size.

https://github.com/user-attachments/assets/cdf9f3fd-110c-42e2-a1fd-7b9673639125

