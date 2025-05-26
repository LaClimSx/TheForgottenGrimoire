# TheForgottenGrimoire
* Produced [scripts](https://github.com/LaClimSx/TheForgottenGrimoire/tree/main/TheForgottenGrimoire/Assets/Scripts) and assets: Created entirely by your team.
    * Staff asset
    * Grimoire pages
    * Grimoire script to follow player
    * Grimoire script to turn pages
    * Spell detection based on the drawing
    * All the spells and their interactions (interactor + interactable) :
        * Fire interactors (fire ball + flame thrower) can burn and destroy fire interactables, and light up designated items (candles...)
        * Electric interactors (projectile + zone) can power up electric interactables (batteries, fans...)
        * Wind can push some items, or serve as handjets
        * Earth can create cubes that are able to activate pressure plates, or handles to climb some designated walls
        * Space allows to teleport back to the hub, or boost the range of your TP and grab
    * Puzzle (managers) for all the levels : level design and handler scripts
    * Level 3 (wind) assets : fan, batteries, cubes, materials...
    * Teleporter to level and back to hub.
    * Particles: wind particle, fire particle and all particles used in spells animations
* Adapted scripts/assets: Modified from external sources.
    * Most puzzle assets : torch, candles, lights... have extra scripts or elements for the puzzles, mostly to interact with the spells.
    * Pressure plates : based on [FistFullOfShrimp's buttons](https://www.youtube.com/watch?v=_pApJDiFxV4)
    * Drawing at a given position : based on [https://dineshpunni.notion.site/Drawing-in-XR-2a2b46869e6f46c589092045a86e8a0a](https://dineshpunni.notion.site/Drawing-in-XR-2a2b46869e6f46c589092045a86e8a0a)
    * Grimoire asset : from [grimoire-style-book](https://assetstore.unity.com/packages/3d/props/grimoire-style-book-3996)
    * Visual assets : 
        * Level 4 : [https://assetstore.unity.com/packages/3d/props/industrial/abandoned-factory-lite-62597](https://assetstore.unity.com/packages/3d/props/industrial/abandoned-factory-lite-62597)
* Unmodified scripts/assets: Used as-is.
    *  Climbing: from unity XR toolkit 
    *  Buttons : from XRIT
    *  Sliding door : from [FistFullOfShrimp](https://github.com/Fist-Full-of-Shrimp/Unity-VR-Basics-2022/blob/main/Assets/Scripts/10%20Buttons/SlidingDoor.cs)
    * Tube Renderer : from [https://dineshpunni.notion.site/Drawing-in-XR-2a2b46869e6f46c589092045a86e8a0a](https://dineshpunni.notion.site/Drawing-in-XR-2a2b46869e6f46c589092045a86e8a0a)
    *  Sounds : [https://pixabay.com/fr/sound-effects/machine-working-sound-244256/](https://pixabay.com/fr/sound-effects/machine-working-sound-244256/) & [https://pixabay.com/fr/sound-effects/electricity-106510/](https://pixabay.com/fr/sound-effects/electricity-106510/) 
    *  Visual assets :
        * Level 1 : [https://assetstore.unity.com/packages/3d/environments/dungeons/dungeon-modular-pack-295430 ](https://assetstore.unity.com/packages/3d/environments/dungeons/dungeon-modular-pack-295430) & [https://assetstore.unity.com/packages/3d/environments/dungeons/ultimate-low-poly-dungeon-143535](https://assetstore.unity.com/packages/3d/environments/dungeons/ultimate-low-poly-dungeon-143535)
        * Level 2 : [https://assetstore.unity.com/packages/3d/props/industrial/wooden-planks-various-308365](https://assetstore.unity.com/packages/3d/props/industrial/wooden-planks-various-308365) & [https://assetstore.unity.com/packages/3d/props/interior/free-pbr-lamps-70181](https://assetstore.unity.com/packages/3d/props/interior/free-pbr-lamps-70181) & [https://assetstore.unity.com/packages/3d/environments/industrial/rpg-fps-game-assets-for-pc-mobile-industrial-set-v3-0-101429](https://assetstore.unity.com/packages/3d/environments/industrial/rpg-fps-game-assets-for-pc-mobile-industrial-set-v3-0-101429)
        * Level 3 : [https://www.pngall.com/flammable-sign-png/](https://www.pngall.com/flammable-sign-png/)
        * Wooden boxes for level 4 : [https://assetstore.unity.com/packages/3d/props/industrial/wood-box-pack-15-objects-105811](https://assetstore.unity.com/packages/3d/props/industrial/wood-box-pack-15-objects-105811)
* Relevant content (scripts, assets, scenes) for each custom feature.
    * Advanced Movement Mechanics : Hand jets, done with the wind spell. Scripts : https://github.com/LaClimSx/TheForgottenGrimoire/blob/main/TheForgottenGrimoire/Assets/Scripts/spell/HandJets.cs & https://github.com/LaClimSx/TheForgottenGrimoire/blob/main/TheForgottenGrimoire/Assets/Scripts/spell/HandJetsProjectile.cs
    * Non-Movement Interaction Mechanics : Spellcasting, with the staff. Scripts : [Spell related scripts](https://github.com/LaClimSx/TheForgottenGrimoire/tree/main/TheForgottenGrimoire/Assets/Scripts/spell)
    * Non-Movement Interaction Mechanics : Puzzle-solving, in the different levels. [Level 1 manager](https://github.com/LaClimSx/TheForgottenGrimoire/blob/main/TheForgottenGrimoire/Assets/Scripts/lvl1/Managerlvl1candles2.cs) & [Level 2 manager](https://github.com/LaClimSx/TheForgottenGrimoire/blob/main/TheForgottenGrimoire/Assets/Scripts/lvl2/level2Manager.cs) & [Level 3 scripts](https://github.com/LaClimSx/TheForgottenGrimoire/tree/main/TheForgottenGrimoire/Assets/Scripts/windLevel) & [Level 4 scripts](https://github.com/LaClimSx/TheForgottenGrimoire/tree/main/TheForgottenGrimoire/Assets/Scripts/lvl4)
