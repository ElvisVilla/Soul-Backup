using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour, IDamageable {

    #region Properties 
    /// <summary>
    /// Retorna true si la vida es 0.
    /// </summary>
    public bool IsDead { get; private set; }

    public int CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = value;
        }
    }
    #endregion

    #region Variables
    [Header("Set in Inspector")]
    [SerializeField] int maxHealth = 50; //50 by default
    [SerializeField] Slider slider = null;
    [SerializeField] int _currentHealth;

    public GameObject floatingTextPrefab;
    public TextMeshProUGUI healBarTextAmount;
    Player player;
    #endregion

    // Use this for initialization
    void Awake ()
    {
        player = GetComponent<Player>();
        CurrentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = CurrentHealth;
        healBarTextAmount.text = HealthText();
    }

    public void TakeDamage (int damage, Vector2 point)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
            CurrentHealth = 0;

        slider.DOValue(CurrentHealth, 0.7f).SetEase(Ease.Linear);
        //player.Movement.KnockBackBehaviour();
        healBarTextAmount.text = HealthText();

        //if(player.Movement.movementState == Bissash.MovementState.PerformingAbility)
        //{
        //    int animHitParameter = Random.Range(1, 3);
        //    if (animHitParameter == 1)
        //        player.Anim.SetTrigger("TakeHit");
        //    else if (animHitParameter == 2)
        //        player.Anim.SetTrigger("TakeDamage");
        //}
        //else
        //{
            
        //}
        
        if (floatingTextPrefab != null)
            ShowDamage(damage);

        if (CurrentHealth <= 0 && !IsDead)
            Die();

    }

    private void ShowDamage(int damage)
    {
        //var instance = LeanPool.Spawn(floatingTextPrefab, transform.position + Vector3.up, Quaternion.identity);
        //instance.GetComponent<TextMeshPro>().text = damage.ToString();
    }

    public void TakeHeal(int effect, Vector2 point)
    {
        CurrentHealth += effect;
        if (CurrentHealth >= maxHealth)
            CurrentHealth = maxHealth;

        slider.DOValue(CurrentHealth, 0.7f).SetEase(Ease.Linear);
        healBarTextAmount.text = HealthText();
    }

    public void Die()
    {
        IsDead = true;
        //player.Anim.SetTrigger("Dying");
        Physics2D.IgnoreLayerCollision(12, 9);
        //Ejecutar escena GameOver.
    }

    string HealthText()
    {
        return $"{CurrentHealth} / {maxHealth}";
    }
}
