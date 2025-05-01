# 🗂️ Folder Sync Tool

Simple C# console app that keeps one folder (`replica`) in sync with another (`source`).  
Runs periodically, compares files via MD5, and logs all actions.

---

## 🔧 Features

- One-way sync: `replica = source`
- Checks every N seconds (interval is configurable)
- Detects:
  - new or updated files
  - deleted files and empty folders
- Logs to console and file
- No third-party sync libs used
