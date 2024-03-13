# SHMUP
### 2024/02/22

It's a 2d schut game

I have a lot of fun doing boss fights and ive been playing some hard games lately so making a short game about fighting a big single boss was something i'd been looking forward to.
Similar games of that vein would be Alien Soldier on the genesis, the Power Fox flash game series and various other games by that developer.

Design-wise, Touhou Project is a HUGE inspiration, it has these beautiful and complex firing patters during their boss fights and mimicking that same kind of organized chaos would be perfect for this kind of boss-only game.
Creating diverse attack patterns to keep the player on his toes and to give some variety to this experience was crucial, originally there was a system where the player could switch between different types of weapons that would work better in certain situations but i scrapped it, the code is still there tho. A sort of layover from that system is the fact that the player can shoot sideways, letting him deal damage if the bottom part of the screen gets too hot for him.

fun times.

# PROTOTYPE 1
### 2024/03/??
i forgot to make it lmfao my b g

# PROTOTYPE 2 - SHELLSHOCK
### 2024/03/12
I've been wanting to make a boomer shooter type thing since forever but i either didnt have the tech skills to do it or the time to. As much as id like to make one for this class the scale of these games is just too big to be reasonably done. So theoretically this isnt a boomer shooter, we are taking a lot of cues design-wise and aesthetic-wise for this game. Considering im also working on a 3d early 3d aesthetic game on the side i'm also using this as practice or as a testing ground for certain scripts and whatnot that i could bring to that other game later.

Shellshock is a survival FPS with an early 3D aesthetic where you fight off waves of enemies with a pump-action shotgun.

The catch being, that you have to manually pump your shotgun, there are various types of ammunitions with different effects, and that you cannot jump - unless you use the backdraft of your shots to propel yourself upwards, like a sort of rocket-jumping.

The current ammo types we have are:
**buckshot:** Typical shotgun shell, shoots a code of hitscan(?) projectiles that do damage, the easiest ammo to find.
**slug:** A single large projectile, fires in a piercing straight line instantly, sorta like a railgun.
**dragonsbreath/flare**: IRL it's essentially incendiary buckshot, mechanically tho, i had the idea that you could use fire ammo to light up the level? I made the area too dark to see past a certain point so it's not as easy to spot and fire at enemies, and not as easy to see if there's anything above you vertically. Plus making a DoT system for fire rounds sounds like a pain.
**airblast:** Like the pyro's airblast in TF2, powerful gust of air that is most useful at propelling you off the ground. Coudl also potentially push back enemies or reflect projectiles back at attackers (like in tf2). But the question remains if i'll even add projectiles.
**infinite:** think the golden mushroom for mario kart except it's buckshot. There's only one of it in the level and it's at the very top vertically, you gotta look for it.

Enemy-wise right now i'm going to stick to having flying drone-like enmies that sort of beeline for the player? i think with navmeshes this should be easily doable since my experience with making pathfinding AI in 3d environments is pretty limited. we'll see how it goes.

As of 2024/03/12, the prototype has some basic features in, the PSX shader, character controller, and a system to pick up ammunition, cycle them out of your shotgun and eject them, and models/textures for all these ammo. still a lotta work to be done unfortunately.
