# Hirondelle

Hirondelle est un outil libre et open-source permettant l'ouverture et l'étude d'un projet WINDEV hors de l'écosystème PC SOFT.

> **Disclaimer**  
> Le logiciel utilise la bibliothèque .NET et le travail de **reverse-engineering** du projet annexe **windev-re**, qui **a été réalisé par une IA**. 
> Elle a analysé les formats de fichier, concu une documentation lisible [ici](https://cacahuetesanssel.github.io/windev-re/) et a également concu la bibliothèque sous ma supervision et mes consignes de qualité de code. 
> 
> Vu la ridicule complexité (volontaire certainement !) de ces formats (on parle d'algorithmes de compression modifiés et propriétaires, des transformations bidons sur des sections entières d'octets, etc), l'utilisation de l'IA est de mon point de vue intéressante ici.
> 
> A noter que ce logiciel, lui, a été réalisé à la main.
> 
> **Tout est bien évidemment libre et open-source**

> Le logiciel et **windev-re** restent expérimentaux.
> 
> Comme le stipule la license, il est livré sans aucune garantie et l'utilisation que vous aurez de ce logiciel reste et demeure votre propre responsabilité.

Le logiciel est capable d'ouvrir une partie des formats propriétaires de WINDEV pour en rendre possible l'extraction de données :
- **Description du projet** (`.wdp`) : ouverture et export du *code du projet*, noms des configurations, liste des éléments et informations de version.
- **Collection de procédures globales** (`.wdg`) : liste des procédures et ouverture + extraction du code.
- **Fenêtre** (`.wdw`) : titre, champs (positions, types, libellés, etc.), ouverture et export du code.
- **Etats** (`.wde`) : *en attente d'implémentation*
- **Analyse** (`.wdd`) : liste des fichiers (tables) avec informations sur les rubriques (colonnes) comme le nom, libellé, type de données, nature de la clé, etc.