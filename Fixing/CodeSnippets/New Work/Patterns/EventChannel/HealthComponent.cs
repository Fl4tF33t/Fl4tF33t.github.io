using System;
using UnityEngine;

public interface IHealth {
    public int Health { get; }
    public void Damage(int amount);
    public void Heal(int amount);
}
[Serializable]
public class HealthComponent : IHealth {
    private readonly int maxHealth;
    private readonly IntEventChannel healthChannel;
    private readonly EmptyChannel deathChannel;
    private int health;
    
    public int Health {
        get => health;
        private set {
            int clamped = Mathf.Clamp(value, 0, maxHealth);
            if (clamped == health) return;
            health = clamped;
            healthChannel.RaiseEvent(health);
        }
    }
    public HealthComponent(int  maxHealth, IntEventChannel healthChannel, EmptyChannel deathChannel) {
        this.maxHealth = maxHealth;
        Health = maxHealth;
        this.healthChannel = healthChannel;
        this.deathChannel = deathChannel;
    }

    public void Damage(int amount) {
        Health -= amount;
    }

    public void Heal(int amount) {
        Health += amount;
    }
}