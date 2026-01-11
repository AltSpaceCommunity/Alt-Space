<div align="center">

# 🌌 Alt-Space
**✨ Далёкий тёмный космос... ✨**

[![GitHub License](https://img.shields.io/github/license/pe4henika/alt-space?style=for-the-badge)](./LEGAL_RU.md)  [![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?&style=for-the-badge)](https://dotnet.microsoft.com/)  [![Discord](https://img.shields.io/badge/Discord-Join%20Server-7289DA?style=for-the-badge&logo=discord&logoColor=white)](https://discord.gg/JpFnEZ99WG)

[📄 Лицензия](./LEGAL-RU.md) • [💬 Discord](https://discord.gg/JpFnEZ99WG) • [English](./README-EN.md)

</div>

---

## ⚙️ О Alt-Space

**Alt-Space** — это форк оригинального англоязычного проекта [HULLROT](https://github.com/Sector-Crescent/Hullrot),  
многопользовательского космического симулятора, работающего на движке **Robust Toolbox**.  

Проект создаётся как эксперимент **русскоязычного сообщества энтузиастов**.  
Alt-Space не стремится повторять ванильный опыт, а предлагает **своё уникальное видение событий в космосе**.  

> ⚠️ **Важно:** проект в первую очередь экспериментальный!

---

## 🚀 Быстрый старт

### 🔧 Требования

- **Git** — <https://git-scm.com/downloads>  
- **.NET SDK 9.0.101 или выше** — <https://dotnet.microsoft.com/download/dotnet/9.0>  

---

### 🪟 Windows

```bash
# 1️⃣ Клонируйте репозиторий
git clone https://github.com/Pe4henika/Alt-Space.git
cd Alt-Space

# 2️⃣ Загрузите подмодули движка
git submodule update --init --recursive

# 3️⃣ Соберите проект
Scripts\bat\buildAllRelease.bat

# 4️⃣ Запустите клиент и сервер
Scripts\bat\runQuickAll.bat
```
### 🐧 Linux

```bash
# 1️⃣ Клонируйте репозиторий
git clone https://github.com/Pe4henika/Alt-Space.git
cd Alt-Space

# 2️⃣ Загрузите подмодули движка
git submodule update --init --recursive

# 3️⃣ Соберите проект
chmod +x Scripts/sh/buildAllRelease.sh
Scripts/sh/buildAllRelease.sh

# 4️⃣ Запустите клиент и сервер
chmod +x Scripts/sh/runQuickAll.sh
Scripts/sh/runQuickAll.sh
```
> 💡 **Совет:** после запуска клиента подключайтесь к localhost, чтобы начать игру.

---
## 📜 Лицензия

Информация о лицензиях кода и ассетов содержится в файле:  
[LEGAL_RU.md](./LEGAL_RU.md)
[LEGAL.md](./LEGAL.md) 

> ⚖️ Код распространяется под **GNU AGPLv3**, ассеты — в основном **CC-BY-SA 3.0**.
