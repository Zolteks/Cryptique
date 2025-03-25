# Cryptique

## Architecture de base et nommage des dossiers
### Exemple
Root:
- Script
- - Puzzle 1
- FBX
- - Taverne
- - Village
- - Foret
- - Grotte
- - Taniere
- - Generic asset
- VFX
- - Puzzle 1
- UI
- Sound
- Scenes

## Nom des fichiers
Pour les fichiers C#, ne pas preciser "Script" dans le nom.
Prefixe de script/classe au format "\[PREFIX\]_NomDuScript" :
- PZL : Puzzle
- UI : User Interface
- PC : Player Controler
- SFX : Sound effect
- DLG : Delegate

## Nom de variable et fonction

Getter uniquement sous forme de methodes. Pas de Property.

### Variables
Nom des variables:
\[Initial du type en minuscule\]NomDeVariable

### Fonctions / Methodes
Nom des fonctions:
Standard : NomDeLaFonction
Coroutine : CoroutineNomDeLaFontion