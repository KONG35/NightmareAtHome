
public abstract class baseWeapon
{
    public PlayerCharacter my;
    public int lv;
    public int MaxLv;
    public float Damage;
    public float Frequency;
    public DataTableManager.WeaponIconData Icon;

    protected baseWeapon(float Damage,float frequency, int MaxLv, DataTableManager.WeaponIconData icon)
    {
        lv = 0;
        this.MaxLv = MaxLv;
        this.Damage = Damage;
        this.Frequency = frequency;
        Icon = icon;
    }

    public abstract void Action();

    public void EquipWeapon(PlayerCharacter my)
    {
        this.my = my;
    }
    public abstract void LvUp();
}
