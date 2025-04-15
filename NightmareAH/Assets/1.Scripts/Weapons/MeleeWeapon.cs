
public class MeleeWeapon : baseWeapon
{
    public float Range;
    public float Scale;

    public MeleeWeapon(float Range,float Damage,float frequency,float scale,int MaxLv, DataTableManager.WeaponIconData icon) : base(Damage, frequency,MaxLv,icon)
    {
        this.Range = Range;
        this.Scale = scale;
    }

    public override void Action()
    {

    }

    public override void LvUp()
    {
    }
}
