# 🗂️ Oblivion_Game Directory

This folder is intended to store the **compiled build** of the game.

---

## 🏗️ What goes here?

After building the project using the `dotnet publish` command, this folder will contain the **compiled executable**, game assets, and necessary dependencies.

### 📁 Expected Structure After Build

```
Oblivion_Game/
└── Release/
    └── net8.0-windows/
        ├── Oblivion.exe         # The compiled game
        ├── Content/             # .xnb game content
        └── Dependencies/        # DLLs (MonoGame, SharpDX, etc.)
```

---

## 🛠️ How to Build

First, make sure this folder exists (if it doesn't yet):

```bash
mkdir Oblivion_Game
```

Then build the project into this folder using:

```bash
dotnet publish -c Release -o "../Oblivion_Game/Release/net8.0-windows
```

This command generates all the runtime files in the correct subdirectory.

---

## 📌 Notes

- This folder is **not version-controlled by default** because Git ignores build output.
- This `README.md` ensures the folder remains visible in repositories.
- Avoid manually editing or placing files here — always build using `dotnet publish`.

---

> _“This folder contains the heart of the game — once built.”_
# 🗂️ Oblivion_Game Directory

This folder is intended to store the **compiled build** of the game.

---

## 🏗️ What goes here?

After building the project using the `dotnet publish` command, this folder will contain the **compiled executable**, game assets, and necessary dependencies.

### 📁 Expected Structure After Build

```
Oblivion_Game/
└── Release/
    └── net8.0-windows/
        ├── Oblivion.exe         # The compiled game
        ├── Content/             # .xnb game content
        └── Dependencies/        # DLLs (MonoGame, SharpDX, etc.)
```

---

## 🛠️ How to Build

First, make sure this folder exists (if it doesn't yet):

```bash
mkdir Oblivion_Game
```

Then build the project into this folder using:

```bash
dotnet publish -c Release -o "Oblivion_Game/Release/net8.0-windows"
```

This command generates all the runtime files in the correct subdirectory.

---

## 📌 Notes

- This folder is **not version-controlled by default** because Git ignores build output.
- This `README.md` ensures the folder remains visible in repositories.
- Avoid manually editing or placing files here — always build using `dotnet publish`.

---

> _“This folder contains the heart of the game — once built.”_
