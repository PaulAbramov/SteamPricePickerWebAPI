# SteamPricePickerWebAPI

The data ingestion and processing hub for the Steam Trading Suite. This repository handles the heavy lifting of gathering and normalizing market data.

### 🔗 Part of a Suite
This repository is the **Data Tier** of the Steam Trading Platform. It works in tandem with the [TradingSiteBackend](https://github.com/PaulAbramov/TradingSiteBackend) to provide a complete market-tracking solution.

### 🧩 Role in the Suite
This module functions as a specialized scraper and data processor. It fetches item details (price, wear, availability) from multiple marketplaces and prepares them for consumption by the main trading backend.

### ✨ Key Features
* **Multi-Market Scraping:** Built-in support for BitSkins and OpSkins API integration.
* **Data Consolidation:** Normalizes inconsistent structures from different providers into a unified database format.
* **High-Volume Processing:** Designed to handle large datasets of skins and items across multiple games (CS:GO, Dota 2, etc.).

### 🛠 Tech Stack
* **Language:** C#
* **Framework:** .NET Core / Web API
* **Integration:** REST APIs, JSON Serialization/Deserialization
