# Simple 2D Clicker — Save & Load System Demo (XOR / AES256 Encryption)

A minimal 2D clicker game created to **demonstrate a flexible save/load system** with **switchable encryption** — choose between **XOR** or **AES-256** for secure data serialization.

## Overview

This project showcases a lightweight save system for Unity that supports:

- ✅ Local data persistence
    
- 🔁 Auto-save with configurable delay
    
- 🔐 Two encryption methods:
    
    - **XOR cipher** — simple and lightweight
        
    - **AES-256 cipher** — strong symmetric encryption using `System.Security.Cryptography`


### `ICipherProvider`

Defines the interface for encryption/decryption providers:

`string Encrypt(string text);`
`string Decrypt(string text);`

### `XORCipher`

Implements a simple symmetric XOR cipher with a repeating key.  
Used for lightweight or debug encryption.

### `AESCipher`

Implements secure AES-256 encryption with IV initialization and key storage in Unity `PlayerPrefs`.  
For real production use, it’s recommended to store keys securely (e.g. KeyStore, Keychain, DPAPI).

### `SaveManager`

Main controller responsible for:

- Initializing the save system
    
- Handling save slots
    
- Triggering manual and automatic saves
    
- Raising `GameEvent`s when data is loaded or saved
    

The manager supports **three save slots** and an **auto-save routine** running every few seconds (adjustable via Inspector).
