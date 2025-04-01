# Store & Upgrade System (feature/store)

## Overview
This branch handles the in-game store or upgrade system. Players can spend collected currency (coins, energy, etc.) to unlock new abilities, power-ups, skins, or continue runs after losing.

## Tasks & Milestones
1. **Currency & Economy**
   - Define how currency is earned (distance traveled, defeating enemies, picking up collectibles).
   - Determine pricing for items, skins, and upgrades.

2. **UI for Store**
   - Create a store screen accessible from the main menu or game-over screen (Keep in touch with UI-HUD developer).
   - Display items and their prices, handle purchase confirmation.

3. **Upgrade Implementation**
   - Implement actual gameplay impact (faster movement, extra health, special attacks, etc.).
   - Store data (e.g., in PlayerPrefs or a JSON file) to persist purchases between sessions.

4. **Skins or Cosmetics (Optional)**
   - Offer different astronaut suits or rocket designs for variety (Keep in touch with Rustam).
   - Integrate purchased items with the main gameplay scene.

5. **Testing & Balancing**
   - Ensure the economy feels fair and doesn’t disrupt game flow.
   - Check for any potential exploits (e.g., infinite currency loop).

## Additional Notes
- Work closely with **UI/HUD** to ensure a consistent interface design.
- Keep potential in-app purchase logic in mind if we want to monetize in the future. (Don't think so)

---

