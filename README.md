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

Pour les fichiers C#, ne pas préciser *Script* dans le nom.  
Préfixe de script/classe au format `[PREFIX]_NomDuScript` :

- **PZL** : Puzzle  
- **UI** : User Interface  
- **PC** : Player Controller  
- **SFX** : Sound effect  
- **DLG** : Delegate  
- **SGL** : Singleton classes
- **OBJ** : Object Scripts  
- **ABS** : Abstract classes

## Nom de variable et fonction

Getter uniquement sous forme de methodes. Pas de Property (sauf Singletons).

### Variables

```cs
private m_nomDeVariable

public nomDeVariable
```

### Fonctions / Methodes

Nom des fonctions:

```cs
// Functions
void NomDeLaFonction(/*...*/)
// Coroutine
Coroutine CoroutineNomDeLaFontion(/*...*/)
// Getters
bool GetIsExemple() => m_isExemple;
// Setters
void SetIsExemple(bool value) => m_isExemple = value;
```
